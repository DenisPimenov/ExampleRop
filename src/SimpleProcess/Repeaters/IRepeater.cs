using System.Threading.Tasks;

namespace SimpleProcess.Repeaters
{
    public interface IRepeater
    {
        Task Wait();
    }
}