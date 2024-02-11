using PaparaApp.Project.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Models.Payments.DTOs
{
    public class TenantBulkPaymentMonthAddRequestDto
    {
            public DateTime PaymentDate { get; set; } 
            public PaymentMethod PaymentMethod { get; set; }

            [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
            public int Month { get; set; }
            public int Year { get; set; }

    }
}
