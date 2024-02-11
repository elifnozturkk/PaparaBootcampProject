using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users.Tenants;

namespace PaparaApp.Project.API.Models.Payments
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid? FlatId { get; set; }
        public PaymentType? PaymentType { get; set; } 
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentMethod? PaymentMethod { get; set; } 
        public int Year { get; set; }
        public int Month { get; set; }
        public Guid? TenantId { get; set; } 
        public PaymentStatus PaymentStatus { get; set; }
        public  Flat? Flat { get; set; }
        public  TenantUser? Tenant { get; set; }
    }

}
