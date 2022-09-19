
namespace webApiAutores.Dtos
{
    public class LibroDto
    {
        public int id { get; set; }
        public string Titulo { get; set; }
        public List<AutorDto> Autores { get; set; }
        public List<ComentarioDto> Comentarios { get; set; }
    }
}
