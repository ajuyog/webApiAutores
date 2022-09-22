using System.ComponentModel.DataAnnotations;

namespace webApiAutores.Dtos
{
    public class EditarAdminDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
