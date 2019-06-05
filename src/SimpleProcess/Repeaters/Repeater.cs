using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleProcess.Repeaters
{
    class Repeater : IRepeater
    {
        private readonly Func<Task> func;
        private readonly int repeatCount;
        private readonly Action whenFailed;

        public Repeater(Func<Task> func, int repeatCount, Action whenFailed)
        {
            this.func = func;
            this.repeatCount = repeatCount;
            this.whenFailed = whenFailed;
        }

        public async Task Wait()
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
        }
    }
}