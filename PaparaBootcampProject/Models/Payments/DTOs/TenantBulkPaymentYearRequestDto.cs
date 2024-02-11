using PaparaApp.Project.API.Enums;

namespace PaparaApp.Project.API.Models.Payments.DTOs
{
    public class TenantBulkPaymentYearRequestDto
    {
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int Year { get; set; }
    }
}
