namespace AppAngular.Domain.IRepository
{
    public interface IRepository<AspNetUsers> where AspNetUsers : class
    {
        // HAY QUE LLEVARLO A LA CAPA CORE PERO NOSE PORQUE NO ME LA TOMA

        // Obtener todos los registros de la entidad
        Task<IEnumerable<AspNetUsers>> GetAllAsync();

        // Obtener un registro por su identificador único
        Task<AspNetUsers> GetByIdAsync(object id);

        // Agregar un nuevo registro
        Task AddAsync(AspNetUsers entity);

        // Actualizar un registro existente
        Task UpdateAsync(AspNetUsers entity);

        // Eliminar un registro por su entidad
        Task DeleteAsync(AspNetUsers entity);

        // Eliminar un registro por su identificador
        Task DeleteByIdAsync(object id);

        // Guardar cambios (opcional si usas un Unit of Work)
        Task<int> SaveChangesAsync();
        Task DeleteAsync(int id);
    }
}
