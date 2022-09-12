using System.ComponentModel.DataAnnotations;
using webApiAutores.Validaciones;

namespace webApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }
        public List<Comentario> Comentarios { get; set; }
        //public int AutorId { get; set; }
        //public Autor Autor { get; set; }
    }
}
