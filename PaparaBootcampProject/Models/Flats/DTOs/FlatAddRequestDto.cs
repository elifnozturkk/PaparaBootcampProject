using System.ComponentModel.DataAnnotations;

namespace PaparaApp.Project.API.Models.Flats.DTOs
{
    public class FlatAddRequestDto
    {

            [Required]
            [StringLength(50, ErrorMessage = "Block information cannot be longer than 50 characters.")]
            public string BlockInfo { get; set; } = default!;

            public bool Status { get; set; } = false;

            [Required]
            [RegularExpression(@"^\d+\+\d+$", ErrorMessage = "Type must be in the format of 'number+number'.")]
            public string Type { get; set; } = default!;

            [Range(1, int.MaxValue, ErrorMessage = "Floor must be greater than 0.")]
            public int Floor { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Flat number must be greater than 0.")]
            public int FlatNumber { get; set; }

    }
}
