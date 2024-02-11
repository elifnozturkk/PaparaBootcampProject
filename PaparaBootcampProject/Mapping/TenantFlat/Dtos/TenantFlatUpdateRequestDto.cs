using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Mapping.TenantFlat.Dtos
{
    public class TenantFlatUpdateRequestDto
    {
        public Guid Id { get; set; } // ID of the TenantFlat mapping
        public DateTime EndDate { get; set; }
    }
}
