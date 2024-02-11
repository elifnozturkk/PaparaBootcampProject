using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Models.Flats.DTOs
{
    public class FlatUpdateRequestDto
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }

    }
}
