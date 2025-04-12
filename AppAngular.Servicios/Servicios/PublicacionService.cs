using AppAngular.Domain.Interfaces;
using AppAngular.Domain.Models;
using AppAngular.DTOS.DTOS;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class PublicacionService : IPublicacionService
{
    private readonly IPublicacionRepository _publicacionRepository;
    private readonly IMapper _mapper;

    public PublicacionService(IPublicacionRepository publicacionRepository, IMapper mapper)
    {
        _publicacionRepository = publicacionRepository ?? throw new ArgumentNullException(nameof(publicacionRepository));
        _mapper = mapper;
    }

    public async Task<IEnumerable<PublicacionDTO>> GetAllAsync()
    {
        var publicacion = await _publicacionRepository.GetAllAsync();

        return publicacion.Select(publicacion => new PublicacionDTO
        {
            Titulo = publicacion.Titulo
        });
    }

    public Task<PublicacionDTO> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(PublicacionDTO publicacion) //Crear DTO para el ADD
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PublicacionDTO publicacion) //Crear DTO para el Update
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}