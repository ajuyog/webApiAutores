using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiAutores.Dtos;
using webApiAutores.Entidades;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using webApiAutores.Filtros;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace webApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration )
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<AutorDto>> Get()
        {
            
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDto>>(autores);
        }


        [HttpGet("{id:int}", Name ="obtenerAutor")]

        public async Task<ActionResult<AutorDtoConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autorDB=> autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDtoConLibros>(autor);

        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDto>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDto>>(autores);
  
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromBody]AutorCreacionDto autorCreacionDto)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDto.Nombre);

            if (existeAutor)
            {
                return BadRequest($"Ya existe el autor con nombre {autorCreacionDto.Nombre}");
            }

            var autor = mapper.Map<Autor>(autorCreacionDto);

            context.Autores.Add(autor);
            await context.SaveChangesAsync();

            var autorDto = mapper.Map<AutorDto>(autor);

            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDto);

        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(AutorCreacionDto autorCreacionDto, int id)
        {
            
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDto);
            autor.Id = id;

            context.Autores.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        
    }
}
