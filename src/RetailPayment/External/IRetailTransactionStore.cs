using System.Threading.Tasks;

namespace RetailPayment.External
{
    public interface IRetailTransactionStore
    {
        Task Create(string operationId, int orderId);

        Task Save(RetailTransaction transaction);

        Task<RetailTransaction> Get(string operationId);
    }
}