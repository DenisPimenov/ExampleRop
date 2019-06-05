using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailPayment.Commands;

namespace RetailPayment.CommandHandlers
{
    /// <summary>
    ///     Обработчик команды продолжения транзакции
    /// </summary>
    public class ResumeRetailPaymentCommandHandler : IRequestHandler<ResumeRetailPaymentCommand>
    {
        private readonly IRequestHandler<RetailPaymentCommand> inner;

        public ResumeRetailPaymentCommandHandler(IRequestHandler<RetailPaymentCommand> inner)
        {
            this.inner = inner;
        }

        public async Task<Unit> Handle(ResumeRetailPaymentCommand command, CancellationToken cancellationToken)
        {
            //todo находим транзакцию и запускаем pipeline заного
            // можем посчитать когда транзакция была создана
            return await inner.Handle(new RetailPaymentCommand(command.OperationId), cancellationToken);
        }
    }
}