using AutoMapper;
using webApiAutores.Dtos;
using webApiAutores.Entidades;

namespace webApiAutores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDto, Autor>();
            CreateMap<Autor, AutorDto>();
            CreateMap<LibroCreacionDto, Libro>();
            CreateMap<Libro, LibroDto>();
            CreateMap<ComentarioCreacionDto, Comentario>();
;        }
    }
}
