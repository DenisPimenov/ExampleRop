using System.Threading.Tasks;
using MediatR;
using SimpleProcess;

namespace RetailPayment.Commands
{
    public class ConfirmRetailPaymentCommand :  IBackgroundCommand<Unit>
    {
        public ConfirmRetailPaymentCommand(string operationId, string paRes)
        {
            OperationId = operationId;
            PaRes = paRes;
            CompletionSource = new TaskCompletionSource<Unit>();
            CompletionSource.SetResult(Unit.Value);
        }

        public TaskCompletionSource<Unit> CompletionSource { get; }

        public string OperationId { get; } 
        
        public string PaRes { get;  }
    }
}