using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using PaparaApp.Project.API.Models.Users.Tenants;

namespace PaparaApp.Project.API.Models.Flats
{
    public class Flat
    {
        public Guid Id { get; set; } 
        public string BlockInfo { get; set; } = default!;
        public bool Status { get; set; } = false;
        public string Type { get; set; } = default!;
        public int Floor { get; set; }
        public int FlatNumber { get; set; }
        public TenantUser? Tenant { get; set; } 
        public Guid? TenantId { get; set; } 

    }
}
