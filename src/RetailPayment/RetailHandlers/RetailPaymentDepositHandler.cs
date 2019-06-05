using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailPayment.Commands;
using RetailPayment.External;
using RetailPayment.External.Payment;
using SimpleProcess.Repeaters;

namespace RetailPayment.RetailHandlers
{
    /// <summary>
    ///     Зачисление денег
    /// </summary>
    public class RetailPaymentDepositHandler : IRequestHandler<RetailPaymentCommand, Unit>
    {
        private readonly IRepeaterFactory repeaterFactory;
        private readonly IRequestHandler<RetailPaymentCommand, Unit> inner;
        private readonly IRetailTransactionStore transactionStore;
        private readonly IPaymentClient paymentClient;

        public RetailPaymentDepositHandler(IRequestHandler<RetailPaymentCommand, Unit> inner,
            IRetailTransactionStore transactionStore, IPaymentClient paymentClient, IRepeaterFactory repeaterFactory)
        {
            this.inner = inner;
            this.transactionStore = transactionStore;
            this.paymentClient = paymentClient;
            this.repeaterFactory = repeaterFactory;
        }

        public async Task<Unit> Handle(RetailPaymentCommand command, CancellationToken cancellationToken)
        {
            var id = command.OperationId;
            if (await IsTransactionDeposited(id))
                return await inner.Handle(command, cancellationToken);
            await DoDeposit(id);
            await UpdateTransactionStatus(id);
            return await inner.Handle(command, cancellationToken);
        }

        private async Task DoDeposit(string operationId)
        {
            var repeater = await repeaterFactory.GetRepeater(
                operationId,
                () => paymentClient.Deposit(operationId),
                int.MaxValue,
                () => throw new TransactionFailedException());
            await repeater.Wait();
        }

        private async Task UpdateTransactionStatus(string operationId)
        {
            var transaction = await transactionStore.Get(operationId);
            transaction.Status = TransactionStatus.Deposited;
            await transactionStore.Save(transaction);
        }

        private async Task<bool> IsTransactionDeposited(string operationId)
        {
            var transaction = await transactionStore.Get(operationId);
            return transaction != null && transaction.Status >= TransactionStatus.Deposited;
        }
    }
}