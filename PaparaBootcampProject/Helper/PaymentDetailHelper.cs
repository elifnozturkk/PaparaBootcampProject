using PaparaApp.Project.API.Models.Payments;
using PaparaApp.Project.API.Models.Shared;

namespace PaparaApp.Project.API.Helper
{
    public class PaymentDetailHelper
    {
        public static List<PaymentDetailDto> FormatPayments(List<Payment> payments)
        {
            return payments.Select(payment => new PaymentDetailDto
            {
                PaymentTypeDescription = payment.PaymentType.HasValue ? payment.PaymentType.Value.ToString() : "Payment type is not specified.",
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentMethodDescription = payment.PaymentMethod.HasValue ? payment.PaymentMethod.Value.ToString() : "Payment method is not specified",
                TenantId = payment.TenantId ?? Guid.Empty
            }).ToList();
        }
    }
}
