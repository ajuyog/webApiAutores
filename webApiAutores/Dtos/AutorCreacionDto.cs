using System.ComponentModel.DataAnnotations;
using webApiAutores.Validaciones;

namespace webApiAutores.Dtos
{
    public class AutorCreacionDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
