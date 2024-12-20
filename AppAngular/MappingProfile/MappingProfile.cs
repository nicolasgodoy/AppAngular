using AppAngular.Domain.Models;
using AppAngular.DTOS;
using AppAngular.Service.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppAngular.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AspNetUsers, AspNetUserDTO>();

            CreateMap<CrearUsuarioDTO, AspNetUsers>();

            CreateMap<AplicacionUserDTO, IdentityUser>();
        }
    }
}
