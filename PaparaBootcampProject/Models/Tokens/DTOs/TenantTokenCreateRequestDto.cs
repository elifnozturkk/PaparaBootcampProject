namespace PaparaApp.Project.API.Models.Tokens.DTOs
{
    public class TenantTokenCreateRequestDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string NationalityId { get; set; } = default!;
    }
}
