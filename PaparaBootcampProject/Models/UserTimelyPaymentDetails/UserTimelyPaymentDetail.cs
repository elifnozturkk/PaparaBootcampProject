using PaparaApp.Project.API.Enums;

namespace PaparaApp.Project.API.Models.UserTimelyPaymentDetails
{
    public class UserTimelyPaymentDetail
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid FlatId { get; set; }
        public int TimelyPaidWater { get; set; } = 0;
        public int TimelyPaidElectricity { get; set;}  = 0;
        public int TimelyPaidGas { get; set; } = 0;
        public int TimelyPaidDue { get; set; } = 0;
      


    }
}
