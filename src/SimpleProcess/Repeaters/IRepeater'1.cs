using System.Threading.Tasks;

namespace SimpleProcess.Repeaters
{
    public interface IRepeater<T>
    {
        Task<T> Wait();
    }
}