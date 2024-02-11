using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users.Tenants;
using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Models.Payments.DTOs
{
    public class TenantPaymentAddRequestDto
    {
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentType PaymentType { get; set; }
        public Guid FlatId { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int DebtMonth { get; set; } 
        public int DebtYear { get; set; } 
    }
}
