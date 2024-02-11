using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Mapping.TenantFlat.Dtos
{
    public class TenantFlatAddRequestDto
    {
        [Required]
        public Guid TenantId { get; set; } = default!;

        [Required]
        public Guid FlatId { get; set; } = default!;

        [Required]
        public DateTime StartDate { get; set; }  

        public DateTime? EndDate { get; set; }
    }
}
