using System.ComponentModel.DataAnnotations;

namespace webApiAutores.Dtos
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Passaword { get; set; }
    }
}
