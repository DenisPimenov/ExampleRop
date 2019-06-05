using System.Collections.Concurrent;
using System.Threading.Tasks;
using MediatR;

namespace SimpleProcess
{
    /// <summary>
    ///     Примитивный раннер команд 
    /// </summary>
    public class SimpleRunner
    {
        private readonly ConcurrentDictionary<string, Task> tasks;
        private readonly IMediator mediator;

        public SimpleRunner(IMediator mediator)
        {
            this.mediator = mediator;
            tasks = new ConcurrentDictionary<string, Task>();
        }

        public Task<TResult> Run<T, TResult>(T request) where T : IBackgroundCommand<TResult>
        {
            var task = mediator.Send(request)
                .ContinueWith(ContinuationAction, TaskContinuationOptions.OnlyOnFaulted);
            if (!tasks.TryAdd(request.OperationId, task))
            {
                return Task.FromResult(default(TResult)); //todo обернуть в Result
            }

            {}
            return request.CompletionSource.Task;
        }

        private void ContinuationAction(Task obj)
        {
            if (obj.Exception != null)
                throw obj.Exception;
        }
    }
}