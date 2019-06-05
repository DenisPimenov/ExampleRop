using System.Threading.Tasks;

namespace RetailPayment.External.Ecom
{
    public interface IEcomClient
    {
        Task<AuthorizeStatus> Authorize(EcomAuthorizeRequest data);
        
        Task<bool> Commit(int orderId);
        
        Task<bool> Rollback(int orderId);
    }
}