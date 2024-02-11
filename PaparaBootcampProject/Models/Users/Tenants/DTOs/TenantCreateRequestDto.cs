using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Models.Users.Tenants.DTOs
{
    public class TenantCreateRequestDto
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        [StringLength(11, MinimumLength = 11, ErrorMessage = "The Nationality ID must contain exactly 11 characters.")]
        public string NationalityId { get; set; } = default!;
    }
}
