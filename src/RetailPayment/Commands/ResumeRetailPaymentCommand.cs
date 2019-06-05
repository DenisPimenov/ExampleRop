using MediatR;

namespace RetailPayment.Commands
{
    public class ResumeRetailPaymentCommand : IRequest<Unit>
    {
        public string OperationId { get; set; }
    }
}