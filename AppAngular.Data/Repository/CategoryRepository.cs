using AppAngular.Domain.IRepository;
using AppAngular.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAngular.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string? _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Categories", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Active = reader.GetBoolean(3)
                            });
                        }
                    }
                }
            }

            return categories;
        }

        public async Task<Category> AddAsync(Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "INSERT INTO Categories (Name, Description, Active) OUTPUT INSERTED.Id VALUES (@Name, @Description, @Active)", connection))
                {
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.Parameters.AddWithValue("@Description", category.Description);
                    command.Parameters.AddWithValue("@Active", category.Active);

                    var id = (int)await command.ExecuteScalarAsync();
                    category.Id = id;
                }
            }

            return category;
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "UPDATE Categories SET Name = @Name, Description = @Description, Active = @Active WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.Parameters.AddWithValue("@Description", category.Description);
                    command.Parameters.AddWithValue("@Active", category.Active);
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return category;
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM Categories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public Task<Category> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
