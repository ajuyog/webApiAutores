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


        [HttpGet(Name ="obtenerAutores")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] bool incluirHATEOAS = true)
        {
            
            var autores = await context.Autores.ToListAsync();
            var dtos = mapper.Map<List<AutorDto>>(autores);
            

            if (incluirHATEOAS)
            {
                var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

                dtos.ForEach(dto => GenerarEnlaces(dto, esAdmin.Succeeded));

                var resultado = new ColeccionDeRecursos<AutorDto> { Valores = dtos };
                resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new { }),
                    descripcion: "self",
                    metodo: "GET"
                    ));

                if (esAdmin.Succeeded)
                {
                    resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("crearAutor", new { }),
                        descripcion: "crear-autor",
                        metodo: "POST"
                        ));
                }


                return Ok(resultado);
            }



            return Ok(dtos);
        }


        [HttpGet("{id:int}", Name ="obtenerAutor")]
        [AllowAnonymous]
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
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            GenerarEnlaces(dto, esAdmin.Succeeded);

            return dto;

        }

        private void GenerarEnlaces(AutorDto autorDto, bool esAdmin)
        {
            autorDto.Enlaces.Add(new DatoHATEOAS(
                
                enlace: Url.Link("obtenerAutor", new { id = autorDto.Id }),
                descripcion: "self", metodo: "GET"));

            if (esAdmin)
            {

                autorDto.Enlaces.Add(new DatoHATEOAS(

                    enlace: Url.Link("actualizarAutor", new { id = autorDto.Id }),
                    descripcion: "autor-actulizar", metodo: "PUT"));

                autorDto.Enlaces.Add(new DatoHATEOAS(

                    enlace: Url.Link("borrarAutor", new { id = autorDto.Id }),
                    descripcion: "self", metodo: "DELETE"));
            }
        }

        [HttpGet("{nombre}", Name ="obtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDto>>> Get([FromRoute] string nombre)
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

        [HttpPut("{id:int}", Name ="actualizarAutor")]

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

        [HttpDelete("{id:int}", Name ="borrarAutor")]
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
