using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users.Tenants;

namespace PaparaApp.Project.API.Mapping.TenantFlat.Dtos
{
    public class TenantFlatDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid FlatId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
