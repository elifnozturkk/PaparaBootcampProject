using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users.Tenants;

namespace PaparaApp.Project.API.Mapping.TenantFlat
{
    public class TenantFlat
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid FlatId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TenantUser? Tenant { get; set; }
        public Flat? Flat { get; set; }
    }
}
