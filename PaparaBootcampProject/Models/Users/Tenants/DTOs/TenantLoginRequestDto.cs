namespace PaparaApp.Project.API.Models.Users.Tenants.DTOs
{
    public class TenantLoginRequestDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string NationalityId { get; set; } = default!;
    }
}
