using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailPayment.Commands;
using RetailPayment.Responses;

namespace Runner
{
    class RunnerHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public RunnerHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var provider = serviceProvider.CreateScope())
            {
                var runner = provider.ServiceProvider.GetRequiredService<SimpleProcess.SimpleRunner>();
                //Как пример запускаем на старте приложения ( может быть http/queue запрос)
                var result =
                    await runner.Run<StartRetailPaymentCommand, IRetailPaymentResponse>(
                        new StartRetailPaymentCommand("Operation"));
                Console.WriteLine("Процесс вернул результат"+ result);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}