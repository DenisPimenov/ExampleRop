using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailPayment.Commands;
using RetailPayment.Configuration;
using RetailPayment.External;
using RetailPayment.External.Ecom;
using SimpleProcess.Repeaters;

namespace RetailPayment.RetailHandlers
{
    /// <summary>
    ///     Фиксируем транзакцию списанием
    /// </summary>
    public class RetailPaymentCommit : IRequestHandler<RetailPaymentCommand, Unit>
    {
        private readonly IEcomClient ecomClient;
        private readonly IRepeaterFactory repeaterFactory;
        private readonly IRetailTransactionStore transactionStore;
        private readonly RetailAuthorizeOptions retailAuthorizeOptions;

        public RetailPaymentCommit(IEcomClient ecomClient,
            IRepeaterFactory repeaterFactory,
            IRetailTransactionStore transactionStore,
            RetailAuthorizeOptions retailAuthorizeOptions)
        {
            this.ecomClient = ecomClient;
            this.repeaterFactory = repeaterFactory;
            this.transactionStore = transactionStore;
            this.retailAuthorizeOptions = retailAuthorizeOptions;
        }

        public async Task<Unit> Handle(RetailPaymentCommand command, CancellationToken cancellationToken)
        {
            var transaction = await transactionStore.Get(command.OperationId);
            await CommitTransaction(transaction);
            await UpdateTransactionStatus(transaction);
            return Unit.Value;
        }

        private async Task CommitTransaction(RetailTransaction command)
        {
            var repeater = await repeaterFactory.GetRepeater(
                command.OperationId,
                () => ecomClient.Commit(command.OrderId),
                retailAuthorizeOptions.TimeoutCount,
                () => throw new TransactionFailedException());
            await repeater.Wait();
        }

        private async Task UpdateTransactionStatus(RetailTransaction transaction)
        {
            transaction.Status = TransactionStatus.Complited;
            await transactionStore.Save(transaction);
        }
    }
}