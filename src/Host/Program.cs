using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailPayment.Configuration;
using SimpleProcess.Repeaters;

namespace Runner
{
    class Program
    {
        static void Main()
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ServiceFactory>(p => p.GetRequiredService);
                    services.AddSingleton<IMediator, Mediator>();
                    services.AddRetailPayment();
                    services.AddSingleton<SimpleProcess.SimpleRunner>();
                    services.AddSingleton<IRepeaterFactory, InMemoryRepeaterFactory>();
                    services.AddHostedService<RunnerHostedService>();
                })
                .Build();
            host.Run();
        }
    }
}