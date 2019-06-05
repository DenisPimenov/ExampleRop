using MediatR;

namespace RetailPayment.Commands
{
    public class RetailPaymentCommand : IRequest
    {
        public RetailPaymentCommand(string operationId)
        {
            OperationId = operationId;
        }

        public string OperationId { get;}
    }
}