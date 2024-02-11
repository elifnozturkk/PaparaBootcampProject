using PaparaApp.Project.API.Enums;

namespace PaparaApp.Project.API.Models.Shared
{
    public class PaymentDetailDto
    {
            public string PaymentTypeDescription { get; set; }
            public decimal Amount { get; set; }
            public PaymentStatus PaymentStatus { get; set; }
            public string PaymentMethodDescription { get; set; }
            public Guid TenantId { get; set; }

    }
}
