using AppAngular.Domain.Enums;
using AppAngular.Domain.IRepository;
using AppAngular.Domain.Models;
using AppAngular.DTOS.DTOS;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AppAngular.Data.Repository
{
    public class PublicacionRepository : IPublicationRepository
    {
        private readonly string? _connectionString;

        public PublicacionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Publication>> GetAllAsync()
        {
            var publicaciones = new List<Publication>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                SELECT
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Price,
                    p.StockAvailable,
                    p.PublicationDate,
                    p.StatusEnums,
                    p.UserId,
                    p.CategoryId,
                    c.Id AS CatId,  
                    c.Name AS CatName,
                    u.Id AS UserId,
                    u.Email AS UserEmail
                FROM Publications p
                INNER JOIN Categories c ON p.CategoryId = c.Id
                INNER JOIN AspNetUsers u ON p.UserId = u.Id";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var publication = new Publication
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                StockAvailable = reader.GetInt32(reader.GetOrdinal("StockAvailable")),
                                PublicationDate = reader.GetDateTime(reader.GetOrdinal("PublicationDate")),
                                StatusEnums = (StatusEnums)reader.GetInt32(reader.GetOrdinal("StatusEnums")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                UserId = reader.GetString(reader.GetOrdinal("UserId")),
                                Category = new Category
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CatId")),
                                    Name = reader.GetString(reader.GetOrdinal("CatName"))
                                },
                                AspNetUsers = new AspNetUsers
                                {
                                    Id = reader.GetString(reader.GetOrdinal("UserId")),
                                    Email = reader.GetString(reader.GetOrdinal("UserEmail"))
                                }
                            };

                            publicaciones.Add(publication);
                        }
                    }
                }
            }

            return publicaciones;
        }

        public async Task<Publication> GetByIdAsync(int id)
        {
            Publication publicacion = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Publications WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            publicacion = new Publication
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                            };
                        }
                    }
                }
            }

            return publicacion;
        }

        public async Task<Publication> AddAsync(Publication publicacion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // 1. Insertar la publicación y obtener el ID generado
                var insertQuery = @"
                INSERT INTO Publications 
                    (Title, Description, Price, StockAvailable, PublicationDate, StatusEnums, CategoryId, UserId)
                OUTPUT INSERTED.Id
                VALUES 
                    (@Title, @Description, @Price, @StockAvailable, @PublicationDate, @StatusEnums, @CategoryId, @UserId)";

                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Title", publicacion.Title);
                    insertCommand.Parameters.AddWithValue("@Description", publicacion.Description);
                    insertCommand.Parameters.AddWithValue("@Price", publicacion.Price);
                    insertCommand.Parameters.AddWithValue("@StockAvailable", publicacion.StockAvailable);
                    insertCommand.Parameters.AddWithValue("@PublicationDate", publicacion.PublicationDate);
                    insertCommand.Parameters.AddWithValue("@StatusEnums", (int)publicacion.StatusEnums);
                    insertCommand.Parameters.AddWithValue("@CategoryId", publicacion.CategoryId);
                    insertCommand.Parameters.AddWithValue("@UserId", publicacion.UserId);

                    publicacion.Id = (int)await insertCommand.ExecuteScalarAsync();
                }
            }

            return publicacion;
        }

        public async Task<Publication> UpdateAsync(int id, Publication publicacion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("UPDATE Publications SET Title = @Title, Description = @Description WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Title", publicacion.Title);
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

                using (var command = new SqlCommand("DELETE FROM Publications WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
