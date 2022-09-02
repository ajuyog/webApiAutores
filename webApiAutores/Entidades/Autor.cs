using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using webApiAutores.Validaciones;

namespace webApiAutores.Entidades
{
    public class Autor: IValidatableObject
    {
        public static Task<ActionResult<List<Autor>>> AnyAsync { get; internal set; }
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La priemra letra debe ser mayúscula", 
                        new string[] {nameof(Nombre)});
                }
            }
        }
    }
}
