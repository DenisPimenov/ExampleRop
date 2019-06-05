using System.Threading.Tasks;

namespace RetailPayment.External
{
    public class InMemoryRetailTransactionStore : IRetailTransactionStore
    {
        private RetailTransaction current;

        public Task Create(string operationId, int orderId)
        {
            current = new RetailTransaction(operationId, orderId, "md");
            return Task.CompletedTask;
        }

        public Task Save(RetailTransaction transaction)
        {
            return Task.CompletedTask;
        }

        public async Task<RetailTransaction> Get(string operationId)
        {
            if (current ==null)
            {
                return null;
            }
            
            return current.OperationId == operationId 
                ? current 
                : null;
        }
    }
}