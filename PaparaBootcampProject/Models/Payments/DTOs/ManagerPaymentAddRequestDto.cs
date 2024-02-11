using PaparaApp.Project.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Models.Payments.DTOs
{
    public class ManagerPaymentAddRequestDto
    {

        public decimal Amount { get; set; } 
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public int Year { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }                             
    }
}
