using System.Threading.Tasks;
using MediatR;

namespace SimpleProcess
{
    /// <summary>
    ///     Интерфейс команд которые могут продолжать работать в фоне
    /// </summary>
    /// <typeparam name="TResult"> тип результата когда мы отпускаем инициатора запроса</typeparam>
    public interface IBackgroundCommand<TResult> : IRequest
    {
        TaskCompletionSource<TResult> CompletionSource { get; }

        string OperationId { get; }
    }
}