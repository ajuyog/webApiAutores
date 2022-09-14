using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using webApiAutores.Validaciones;

namespace webApiAutores.Entidades
{
    public class Autor
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe superar {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        
    }
}
