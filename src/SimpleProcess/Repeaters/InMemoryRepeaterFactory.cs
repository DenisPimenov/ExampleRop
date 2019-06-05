using System;
using System.Threading.Tasks;

namespace SimpleProcess.Repeaters
{
    public class InMemoryRepeaterFactory : IRepeaterFactory
    {
        public async Task<IRepeater<T>> GetRepeater<T>(string id, Func<Task<T>> func, int repeatCount, Action whenFailed)
        {
            return new Repeater<T>(func,repeatCount,whenFailed);
        }

        public async Task<IRepeater> GetRepeater(string id, Func<Task> func, int repeatCount, Action whenFailed)
        {
            return new Repeater(func,repeatCount,whenFailed);
        }
    }
}