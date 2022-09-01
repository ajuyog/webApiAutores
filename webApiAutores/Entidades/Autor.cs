using Microsoft.AspNetCore.Mvc;

namespace webApiAutores.Entidades
{
    public class Autor
    {
        public static Task<ActionResult<List<Autor>>> AnyAsync { get; internal set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
