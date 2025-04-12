using AppAngular.Domain.Interfaces;
using AppAngular.Domain.Models;
using AppAngular.DTOS.DTOS;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AppAngular.Data.Repository
{
    public class PublicacionRepository : IPublicacionRepository
    {
        private readonly string _connectionString;

        public PublicacionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Publicacion>> GetAllAsync()
        {
            var publicaciones = new List<Publicacion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Publicacion", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            publicaciones.Add(new Publicacion
                            {
                                Id = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                            });
                        }
                    }
                }
            }

            return publicaciones;
        }

        public async Task<Publicacion> GetByIdAsync(int id)
        {
            Publicacion publicacion = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Publicacion WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            publicacion = new Publicacion
                            {
                                Id = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                            };
                        }
                    }
                }
            }

            return publicacion;
        }

        public async Task<Publicacion> AddAsync(Publicacion publicacion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("INSERT INTO Publicacion (Titulo, Descripcion) OUTPUT INSERTED.Id VALUES (@Titulo, @Descripcion)", connection))
                {
                    command.Parameters.AddWithValue("@Titulo", publicacion.Titulo);

                    var id = (int)await command.ExecuteScalarAsync();
                    publicacion.Id = id;
                }
            }

            return publicacion;
        }

        public async Task<Publicacion> UpdateAsync(int id, Publicacion publicacion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("UPDATE Publicacion SET Titulo = @Titulo, Descripcion = @Descripcion WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Titulo", publicacion.Titulo);
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return publicacion;
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM Publicaciones WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
