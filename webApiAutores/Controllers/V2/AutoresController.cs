using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiAutores.Dtos;
using webApiAutores.Entidades;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using webApiAutores.Filtros;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using webApiAutores.Utilidades;

namespace webApiAutores.Controllers.V2
{
    [ApiController]
    //[Route("api/v2/autores")]
    [Route("api/autores")]
    [CabeceraPresente("x-version", "2")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="EsAdmin")]

    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }


        [HttpGet(Name ="obtenerAutoresv2")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDto>>> Get()
        {
            
            var autores = await context.Autores.ToListAsync();
            
            autores.ForEach(autor => autor.Nombre = autor.Nombre.ToUpper());

            return mapper.Map<List<AutorDto>>(autores);
        }


        [HttpGet("{id:int}", Name ="obtenerAutorv2")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
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

            var dto = mapper.Map<AutorDtoConLibros>(autor);
            
            return dto;

        }


        [HttpGet("{nombre}", Name ="obtenerAutorPorNombrev2")]
        public async Task<ActionResult<List<AutorDto>>> GetByName([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDto>>(autores);
  
        }

        [HttpPost(Name ="crearAutor")]

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

        [HttpPut("{id:int}", Name ="actualizarAutorv2")]

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

        [HttpDelete("{id:int}", Name ="borrarAutorv2")]
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
