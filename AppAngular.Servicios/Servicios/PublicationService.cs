using AppAngular.Domain.Enums;
using AppAngular.Domain.Interfaces;
using AppAngular.Domain.IRepository;
using AppAngular.Domain.Models;
using AppAngular.DTOS;
using AppAngular.DTOS.DTOS;
using AutoMapper;
using System.Data.SqlClient;

public class PublicationService : IPublicationService
{
    private readonly IPublicationRepository _publicacionRepository;
    private readonly IAspNetUserService _userService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public PublicationService(IPublicationRepository publicacionRepository, IAspNetUserService userService, IMapper mapper)
    {
        _publicacionRepository = publicacionRepository ?? throw new ArgumentNullException(nameof(publicacionRepository));
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PublicationDTO>> GetAllAsync()
    {
        var publicacion = await _publicacionRepository.GetAllAsync();

        return publicacion.Select(publicacion => new PublicationDTO
        {
            Title = publicacion.Title,
            Description = publicacion.Description,
            Price = publicacion.Price,
            StockAvailable = publicacion.StockAvailable,
            PublicationDate = publicacion.PublicationDate,
            Status = publicacion.StatusEnums.ToString(),

            AspNetUsers = new AspNetUserDTO
            {
                Id = publicacion.AspNetUsers.Id,
            },

            Category = new CategoryDTO
            {
                Id = publicacion.Category.Id, //Aclaracion que aca me falta declararlo por eso me llegaba 0 al front
                Name = publicacion.Category.Name,
                Description = publicacion.Category.Description,
                Active = publicacion.Category.Active
            }
        });
    }

    public Task<PublicationDTO> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(CreatePublicationDTO publicacionDto)
    {

        // Tener en cuenta posibles validaciones futuras
       
        var publicationEntity = new Publication
        {
            Title = publicacionDto.Title,
            Description = publicacionDto.Description,
            Price = publicacionDto.Price,
            StockAvailable = publicacionDto.StockAvailable,
            PublicationDate = publicacionDto.PublicationDate,
            StatusEnums = Enum.Parse<StatusEnums>(publicacionDto.Status, true),
            UserId = publicacionDto.UserId,
            CategoryId = publicacionDto.CategoryId
        };

        await _publicacionRepository.AddAsync(publicationEntity);
        
    }

    public async Task UpdateAsync(UpdatePublicationDTO publicationDto) //Crear DTO para el Update
    {
        var publicationEntity = await _publicacionRepository.GetByIdAsync(publicationDto.Id);

        if (publicationEntity == null)
        {
            throw new Exception("Publicación no encontrada.");
        }

        publicationEntity.Title = publicationDto.Title;
        publicationEntity.Description = publicationDto.Description;
        publicationEntity.Price = publicationDto.Price;
        publicationEntity.StockAvailable = publicationDto.StockAvailable;
        publicationEntity.PublicationDate = publicationDto.PublicationDate;
        publicationEntity.StatusEnums = Enum.Parse<StatusEnums>(publicationDto.Status, true);
        publicationEntity.UserId = publicationDto.UserId;
        publicationEntity.CategoryId = publicationDto.CategoryId;

        await _publicacionRepository.UpdateAsync(publicationDto.Id, publicationEntity);
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

}