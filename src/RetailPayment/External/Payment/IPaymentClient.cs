using System.Threading.Tasks;

namespace RetailPayment.External.Payment
{
    public interface IPaymentClient
    {
        Task<bool> Deposit(string operationId);
    }
}