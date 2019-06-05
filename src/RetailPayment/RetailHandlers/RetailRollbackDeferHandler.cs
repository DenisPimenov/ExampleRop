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
    ///     Откат транзакции в случае исключительной ситуации (TransactionFailedException)
    /// </summary>
    public class RetailRollbackDeferHandler : IRequestHandler<RetailPaymentCommand, Unit>
    {
        private readonly IRequestHandler<RetailPaymentCommand, Unit> next;
        private readonly IRepeaterFactory repeaterFactory;
        private IRetailTransactionStore transactionStore;
        private readonly IEcomClient ecomClient;
        private readonly RetailAuthorizeOptions retailAuthorizeOptions;

        public RetailRollbackDeferHandler(IRequestHandler<RetailPaymentCommand, Unit> next,
            IEcomClient ecomClient,
            IRepeaterFactory repeaterFactory,
            RetailAuthorizeOptions retailAuthorizeOptions,
            IRetailTransactionStore transactionStore)
        {
            this.next = next;
            this.ecomClient = ecomClient;
            this.retailAuthorizeOptions = retailAuthorizeOptions;
            this.transactionStore = transactionStore;
            this.repeaterFactory = repeaterFactory;
        }

        public async Task<Unit> Handle(RetailPaymentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                return await next.Handle(command, cancellationToken);
            }
            catch (TransactionFailedException)
            {
                var transaction = await transactionStore.Get(command.OperationId);
                await Rollback(transaction);
                throw;
            }
        }

        private async Task<bool> Rollback(RetailTransaction transaction)
        {
            var repeater = await repeaterFactory.GetRepeater(
                transaction.OperationId,
                () => ecomClient.Rollback(transaction.OrderId),
                retailAuthorizeOptions.TimeoutCount,
                () => throw new TransactionFailedException());
            return await repeater.Wait();
        }
    }
}