using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailPayment.Commands;

namespace RetailPayment.CommandHandlers
{
    /// <summary>
    ///     Обработчик команды инициализация начала транзакции 
    /// </summary>
    public class StartRetailPaymentCommandHandler : IRequestHandler<StartRetailPaymentCommand, Unit>
    {
        private CancellationTokenSource tokenSource;
        private readonly IRequestHandler<StartRetailPaymentCommand, Unit> inner;

        public StartRetailPaymentCommandHandler(IRequestHandler<StartRetailPaymentCommand, Unit> inner)
        {
            this.inner = inner;
        }

        public async Task<Unit> Handle(StartRetailPaymentCommand command, CancellationToken cancellationToken)
        {
            using (tokenSource = new CancellationTokenSource(TimeSpan.FromDays(5)))
            {
                return await inner.Handle(command, tokenSource.Token);
            }
        }
    }
}