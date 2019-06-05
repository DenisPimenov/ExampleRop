using System.Threading.Tasks;

namespace RetailPayment.External.Ecom
{
    internal class EcomClient : IEcomClient
    {
        public async Task<AuthorizeStatus> Authorize(EcomAuthorizeRequest data)
        {
            return AuthorizeStatus.Ok;
        }

        public async Task<bool> Commit(int orderId)
        {
            await Task.Delay(2000);
            return true;
        }

        public Task<bool> Rollback(int orderId)
        {
            return Task.FromResult(true);

        }
    }
}