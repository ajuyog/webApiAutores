using System.ComponentModel.DataAnnotations;
using webApiAutores.Validaciones;

namespace webApiAutores.Dtos
{
    public class LibroPatchDto
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        
    }
}
