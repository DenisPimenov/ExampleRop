using System.Threading.Tasks;
using RetailPayment.Responses;
using SimpleProcess;

namespace RetailPayment.Commands
{
    /// <summary>
    ///     Входные данные для запуска процесса
    /// </summary>
    public class StartRetailPaymentCommand : IBackgroundCommand<IRetailPaymentResponse>
    {
        public StartRetailPaymentCommand(string operationId)
        {
            OperationId = operationId;
            CompletionSource = new TaskCompletionSource<IRetailPaymentResponse>();
        }

        public TaskCompletionSource<IRetailPaymentResponse> CompletionSource { get; }

        public string OperationId { get; }

        public int OrderId { get; set; }

        public CardData CardData { get; set; }
    }

    public class CardData
    {
        public string CardNumber { get; set; }

        /// <summary>
        /// Месяц срока окончания действия карты 
        /// </summary>
        public int ExpMonth { get; set; }

        /// <summary>
        /// Год срока окончания действия карты
        /// </summary> 
        public int ExpYear { get; set; }

        /// <summary>
        /// CVC/CVV
        /// </summary>
        public string Cvc2 { get; set; }
    }
   
}