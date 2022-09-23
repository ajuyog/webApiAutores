

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webApiAutores.Dtos;
using webApiAutores.Servicios;

namespace webApiAutores.Utilidades
{
    public class HATEOASAutorFilterAttribute: HATEOASFiltroAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HATEOASAutorFilterAttribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var debeIncluir = DebeIncluirHATEOAS(context);

            if (!debeIncluir)
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;
            
            var autorDto =  resultado.Value as AutorDto;

            if(autorDto == null)
            {
                var autoresDto = resultado.Value as List<AutorDto> ??
                    throw new ArgumentException("Se Esperaba una instancia de autorDto o listado de autorDTo");

                autoresDto.ForEach(async autor => await generadorEnlaces.GenerarEnlaces(autor));

                resultado.Value = autoresDto;
            }
            else
            {
                await generadorEnlaces.GenerarEnlaces(autorDto);
            }

            
            await next();

        }
    }
}
