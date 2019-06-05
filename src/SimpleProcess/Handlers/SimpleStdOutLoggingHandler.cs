using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SimpleProcess.Handlers
{
    public class SimpleStdOutLoggingHandler<T> : IRequestHandler<T,Unit> where T : IRequest<Unit>
    {
        private readonly IRequestHandler<T, Unit> inner;

        public SimpleStdOutLoggingHandler(IRequestHandler<T,Unit> inner)
        {
            this.inner = inner;
        }
        public async Task<Unit> Handle(T request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Start " + inner.GetType().Name);
            var result = await  inner.Handle(request, cancellationToken);
            Console.WriteLine("Complete " + inner.GetType().Name);
            return result;
        }
    }
}