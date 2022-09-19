using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiAutores.Dtos;
using webApiAutores.Entidades;

namespace webApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDto>> Get(int id)
        {
            var libro = await context.Libros.Include(libroBD => libroBD.Comentarios).FirstOrDefaultAsync(x => x.id == id);
            return mapper.Map<LibroDto>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDto libroCreacionDto)
        {
            if (libroCreacionDto.AutoresIds == null)
            {
                return BadRequest("Falta autor");
            }

            var autoresIds = await context.Autores.Where(autorBD => libroCreacionDto.AutoresIds.Contains(autorBD.Id)).Select(x => x.Id).ToListAsync();

            if(libroCreacionDto.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores");
            }

            var libro = mapper.Map<Libro>(libroCreacionDto);

            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;

                }
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
