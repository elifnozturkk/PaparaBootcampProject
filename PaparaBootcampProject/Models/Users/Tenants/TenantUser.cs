using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users;

namespace PaparaApp.Project.API.Models.Users.Tenants
{
    public class TenantUser : AppUser
    {

        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string NationalityId { get; set; } = default!;
        public Flat? Flat { get; set; }
    }
}
