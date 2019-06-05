using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleProcess.Repeaters
{
    class Repeater<T> : IRepeater<T>
    {
        private readonly Func<Task<T>> func;
        private readonly int repeatCount;
        private readonly Action whenFailed;

        public Repeater(Func<Task<T>> func, int repeatCount, Action whenFailed)
        {
            this.func = func;
            this.repeatCount = repeatCount;
            this.whenFailed = whenFailed;
        }

        public async Task<T> Wait()
        {
            var count = repeatCount;
            while (count-- > 0)
            {
                try
                {
                    await func();
                }
                catch (Exception e) when (e is TimeoutException || e is HttpRequestException)
                {
                    if (count < 0)
                    {
                        whenFailed();
                    }
                }
                catch (Exception)
                {
                    whenFailed();
                }
            }

            whenFailed();
            return default;
        }
    }
}