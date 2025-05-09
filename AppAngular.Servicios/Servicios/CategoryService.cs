using AppAngular.Domain.Enums;
using AppAngular.Domain.Interfaces;
using AppAngular.Domain.IRepository;
using AppAngular.Domain.Models;
using AppAngular.DTOS.DTOS;
using AutoMapper;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        var category = await _categoryRepository.GetAllAsync();

        return category.Select(category => new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Active = category.Active,
        });
    }

    public Task<CategoryDTO> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(CreateCategoryDTO categoryDto)
    {
        var categoryEntity = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            Active = categoryDto.Active,
            
        };

        await _categoryRepository.AddAsync(categoryEntity);
    }

    public async Task UpdateAsync(UpdateCategoryDTO categoryDto)
    {
        var categoryEntity = await _categoryRepository.GetByIdAsync(categoryDto.Id);

        if (categoryEntity == null)
        {
            throw new Exception("Publicación no encontrada.");
        }

        categoryEntity.Name = categoryDto.Name;
        categoryEntity.Description = categoryDto.Description;
        categoryEntity.Active = categoryDto.Active;
            
        await _categoryRepository.UpdateAsync(categoryDto.Id, categoryEntity);
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}