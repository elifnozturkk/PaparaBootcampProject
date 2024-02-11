namespace PaparaApp.Project.API.Models.UserDiscountStatuses.DTOs
{
    public class UserDiscountStatusAddRequestDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid FlatId { get; set; }
        public bool IsDiscountActive { get; set; } = true;
        public int DiscountStartYear { get; set; }
        public int DiscountEndYear { get; set; }
    }
}
