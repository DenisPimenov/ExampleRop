using System;
using System.Threading.Tasks;

namespace SimpleProcess.Repeaters
{
    public interface IRepeaterFactory
    {
        Task<IRepeater<T>> GetRepeater<T>(string id, Func<Task<T>> func, int repeatCount, Action whenFailed);
        Task<IRepeater> GetRepeater(string id, Func<Task> func, int repeatCount, Action whenFailed);
    }
}