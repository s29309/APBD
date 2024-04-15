using Cw5;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Cw5
{
    public interface IDb
    {
        Task<bool> Exist(int id);
        Task CreateAnimal(Animal animal);

        Task<bool> UpdateAnimal(string id, Animal animal);

        Task<bool> DeleteAnimal(string id);
        Task<List<Animal>> GetAnimalList();


    }




    public class Db : IDb
    {
        private readonly IConfiguration _configuration;
        public Db(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Exist(int id)
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Animal where ID = @1";
                command.Parameters.AddWithValue("@1", id);
                await connection.OpenAsync();
                if (await command.ExecuteScalarAsync() is not null)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task CreateAnimal(Animal animal)
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = "insert into Animal (ID, Name, Description, Category, Area) values (@1,@2,@3,@4,@5)";
                command.Parameters.AddWithValue("@1", animal.ID);
                command.Parameters.AddWithValue("@2", animal.Weight);
                command.Parameters.AddWithValue("@3", animal.Name);
                command.Parameters.AddWithValue("@4", animal.Colour);
                command.Parameters.AddWithValue("@5", animal.Category);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> UpdateAnimal(string id, Animal animal)
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"update Animal set Name = @2, Description = @3, Category = @4, Area = @5 where ID = {id}";
                command.Parameters.AddWithValue("@2", animal.Weight);
                command.Parameters.AddWithValue("@3", animal.Name);
                command.Parameters.AddWithValue("@4", animal.Colour);
                command.Parameters.AddWithValue("@5", animal.Category);
                await connection.OpenAsync();
                var numRows = await command.ExecuteNonQueryAsync();
                if (numRows > 0) { return true; }
                return false;
            }
        }


        public async Task<bool> DeleteAnimal(string id)
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"delete from Animal where ID = {id}";
                await connection.OpenAsync();
                var numRows = await command.ExecuteNonQueryAsync();
                if (numRows > 0) { return true; }
                return false;
            }
        }

        public async Task<List<Animal>> GetAnimalList()
        {
            var animalList = new List<Animal>();
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"select * from animal order by Name asc";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    animalList.Add(new Animal
                    {
                        ID = reader.GetInt32(0),
                        Weight = reader.GetDouble(1),
                        Name = reader.GetString(2),
                        Colour = reader.GetString(3),
                        Category = reader.GetString(4),
                    });
                }
            }
            return animalList;
        }


        







    }
}
