namespace RetailPayment
{
    /// <summary>
    ///     Данные транзакции ( то что персистим )
    /// </summary>
    public class RetailTransaction
    {
        public RetailTransaction(string operationId, int orderId, string md)
        {
            OperationId = operationId;
            OrderId = orderId;
            Md = md;
        }

        public string OperationId { get; }

        public int OrderId { get; }

        public string Md { get; }
        
        public TransactionStatus Status { get; set; }
    }

    public enum TransactionStatus
    {
        Authorized,
        Deposited,
        Complited
    }
}