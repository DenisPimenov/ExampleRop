using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailPayment.Commands;

namespace RetailPayment.CommandHandlers
{
    /// <summary>
    ///     Обработчик команды подтверждения транзакции
    /// </summary>
    public class ConfirmRetailPaymentCommandHandler : IRequestHandler<ConfirmRetailPaymentCommand>
    {
        private readonly IRequestHandler<RetailPaymentCommand> inner;

        public ConfirmRetailPaymentCommandHandler(IRequestHandler<RetailPaymentCommand> inner)
        {
            this.inner = inner;
        }

        public async Task<Unit> Handle(ConfirmRetailPaymentCommand command, CancellationToken cancellationToken)
        {
            //todo нахожим по md транзакцию и подтверждаем ее в ecom и продолжаем работу
            return await inner.Handle(new RetailPaymentCommand("operationId from md"), cancellationToken);
        }
    }
}