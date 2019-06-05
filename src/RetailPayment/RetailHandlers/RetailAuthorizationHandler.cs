using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailPayment.Commands;
using RetailPayment.Configuration;
using RetailPayment.External;
using RetailPayment.External.Ecom;
using RetailPayment.Responses;
using SimpleProcess.Repeaters;

namespace RetailPayment.RetailHandlers
{
    /// <summary>
    ///     Авторизация денег
    /// </summary>
    public class RetailAuthorizationHandler : IRequestHandler<StartRetailPaymentCommand, Unit>
    {
        private readonly IRequestHandler<RetailPaymentCommand, Unit> next;
        private readonly IRepeaterFactory repeaterFactory;
        private readonly IEcomClient ecomClient;
        private readonly IRetailTransactionStore transactionStore;
        private readonly RetailAuthorizeOptions retailAuthorizeOptions;

        public RetailAuthorizationHandler(IRequestHandler<RetailPaymentCommand, Unit> next,
            IEcomClient ecomClient,
            IRepeaterFactory repeaterFactory,
            IRetailTransactionStore transactionStore,
            RetailAuthorizeOptions retailAuthorizeOptions)
        {
            this.next = next;
            this.ecomClient = ecomClient;
            this.repeaterFactory = repeaterFactory;
            this.transactionStore = transactionStore;
            this.retailAuthorizeOptions = retailAuthorizeOptions;
        }

        public async Task<Unit> Handle(StartRetailPaymentCommand command, CancellationToken cancellationToken)
        {
            if (await IsTransactionAuthorized(command.OperationId))
                return await next.Handle(new RetailPaymentCommand(command.OperationId), cancellationToken);
            
            return await AuthorizeTransaction(command, cancellationToken);
        }

        private async Task<Unit> AuthorizeTransaction(StartRetailPaymentCommand command,
            CancellationToken cancellationToken)
        {
            var authorizeResult = await AuthorizeInEcom(command);
            switch (authorizeResult)
            {
                // тут посути решаем как отпускать и продолжать транзакцию
                case AuthorizeStatus.Ok:
                    command.CompletionSource.SetResult(new InWork());
                    await transactionStore.Create(command.OperationId, command.OrderId);
                    return await next.Handle(new RetailPaymentCommand(command.OperationId), cancellationToken);
                case AuthorizeStatus.NoMoney:
                    command.CompletionSource.SetResult(new BalanceOut());
                    return Unit.Value;
                case AuthorizeStatus.NeedConfirm:
                    command.CompletionSource.SetResult(new NeedConfirm());
                    return Unit.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<AuthorizeStatus> AuthorizeInEcom(StartRetailPaymentCommand command)
        {
            var authorizeRequest = new EcomAuthorizeRequest();
            var repeater = await repeaterFactory.GetRepeater(
                command.OperationId,
                () => ecomClient.Authorize(authorizeRequest),
                retailAuthorizeOptions.TimeoutCount,
                () => throw new TransactionFailedException());
            return await repeater.Wait();
        }

        private async Task<bool> IsTransactionAuthorized(string operationId)
        {
            var transaction = await transactionStore.Get(operationId);
            return transaction != null && transaction.Status >= TransactionStatus.Authorized;
        }
    }
}