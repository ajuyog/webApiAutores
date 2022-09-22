using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
        
        [HttpGet("{id:int}", Name = "obtenerLibro")]
        public async Task<ActionResult<LibroDtoConAutores>> Get(int id)
        {
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x=> x.id == id);

            if (libro == null)
            {
                return NotFound();
            }


            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x=>x.Orden).ToList();

            return mapper.Map<LibroDtoConAutores>(libro);
        }

        [HttpPost(Name = "crearLibro")]
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

            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDto  = mapper.Map<LibroDto>(libro);

            return CreatedAtRoute("obtenerLibro", new { id = libro.id }, libroDto);

        }

        [HttpPut("{id:int}", Name = "actulizarLibro")]
        public async Task<ActionResult> Put(int id, LibroCreacionDto libroCreacionDto)
        {
            var libroDB = await context.Libros.Include(x => x.AutoresLibros).FirstOrDefaultAsync(x => x.id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDto, libroDB);

            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;

                }
            }
        }

        [HttpPatch("{id:int}", Name = "patchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDto> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            var libroDto = mapper.Map<LibroPatchDto>(libroDB);

            patchDocument.ApplyTo(libroDto, ModelState);

            var esValido = TryValidateModel(libroDB);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDto, libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "borrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Libros.Remove(new Libro() { id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
