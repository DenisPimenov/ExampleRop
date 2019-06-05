using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RetailPayment.CommandHandlers;
using RetailPayment.Commands;
using RetailPayment.External;
using RetailPayment.External.Ecom;
using RetailPayment.External.Payment;
using RetailPayment.RetailHandlers;
using SimpleProcess.Handlers;

namespace RetailPayment.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRetailPayment(this IServiceCollection serviceCollection)
        {
            // конфигурим pipeline (читаем с конца)
            // Реализации могут грейнами акторами объектами и тд
            serviceCollection.AddTransient<IRequestHandler<RetailPaymentCommand, Unit>, RetailPaymentCommit>();
            serviceCollection
                .Decorate<IRequestHandler<RetailPaymentCommand, Unit>, SimpleStdOutLoggingHandler<RetailPaymentCommand>
                >();
            serviceCollection.Decorate<IRequestHandler<RetailPaymentCommand, Unit>, RetailPaymentDepositHandler>();
            serviceCollection
                .Decorate<IRequestHandler<RetailPaymentCommand, Unit>, SimpleStdOutLoggingHandler<RetailPaymentCommand>
                >();
            serviceCollection.Decorate<IRequestHandler<RetailPaymentCommand, Unit>, RetailRollbackDeferHandler>();
            serviceCollection
                .Decorate<IRequestHandler<RetailPaymentCommand, Unit>, SimpleStdOutLoggingHandler<RetailPaymentCommand>
                >();

            //конфигурим pipeline старта транзакции
            serviceCollection
                .AddTransient<IRequestHandler<StartRetailPaymentCommand, Unit>, RetailAuthorizationHandler>();
            serviceCollection
                .Decorate<IRequestHandler<StartRetailPaymentCommand, Unit>, StartRetailPaymentCommandHandler>();

            //конфигурим pipeline ее продолжения транзакции
            serviceCollection
                .AddTransient<IRequestHandler<ResumeRetailPaymentCommand, Unit>, ResumeRetailPaymentCommandHandler>();

            //конфигурим pipeline ее подтверждение транзакции
            serviceCollection
                .AddTransient<IRequestHandler<ConfirmRetailPaymentCommand, Unit>, ConfirmRetailPaymentCommandHandler>();
            serviceCollection.AddScoped<IRetailTransactionStore, InMemoryRetailTransactionStore>();
            serviceCollection.AddTransient<IEcomClient, EcomClient>();
            serviceCollection.AddTransient<IPaymentClient, PaymentClient>();
            serviceCollection.AddTransient(p => new RetailAuthorizeOptions
            {
                TimeoutCount = 3
            });
        }
    }
}