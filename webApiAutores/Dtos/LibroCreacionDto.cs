using System.ComponentModel.DataAnnotations;
using webApiAutores.Validaciones;

namespace webApiAutores.Dtos
{
    public class LibroCreacionDto
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
    }
}
