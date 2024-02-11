namespace PaparaApp.Project.API.Models.Tokens.DTOs
{
    public class ApartmanManagerTokenCreateRequestDto
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
