namespace webApiAutores.Dtos
{
    public class AutorDtoConLibros: AutorDto
    {
        public List<LibroDto> Libros { get; set; }
    }
}
