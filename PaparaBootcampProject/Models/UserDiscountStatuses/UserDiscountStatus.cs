namespace PaparaApp.Project.API.Models.UserDiscountStatuses
{
    public class UserDiscountStatus
    {
        public Guid Id { get; set; }
        public Guid TenantId {get; set;}
        public Guid FlatId { get; set;}
        public bool IsDiscountActive { get; set; } 
        public int DiscountStartYear { get; set; }
        public int DiscountEndYear { get; set; }

    }
}
