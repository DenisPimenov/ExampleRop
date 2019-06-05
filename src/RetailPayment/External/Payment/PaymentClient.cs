using System.Threading.Tasks;

namespace RetailPayment.External.Payment
{
    class PaymentClient : IPaymentClient
    {
        public async Task<bool> Deposit(string operationId)
        {
            await Task.Delay(2000);
            return true;
        }
    }
}