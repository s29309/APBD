using Cw7.DTO;
using Microsoft.Data.SqlClient;

namespace Cw7;

public interface IDb
{
    Task<int> NoOrder(ProductWarehouse warehouse);
    Task<bool> NoWarehouse(int id);
    Task<bool> NoProduct(int id);
    Task UpdateFulfilledAt(Order order);
    Task<int> InsertProductWarehouse(ProductWarehouse warehouse, Order order, Product product);
}

public class Db : IDb
{
    private readonly IConfiguration _configuration;

    public Db(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> NoOrder(ProductWarehouse productWarehouse)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = "select IdOrder from [Order] where IdProduct = @1 and Amount = @2 and FulfilledAt IS NULL and CreatedAt<@3";
            command.Parameters.AddWithValue("@1", productWarehouse.IdProduct);
            command.Parameters.AddWithValue("@2", productWarehouse.Amount);
            command.Parameters.AddWithValue("@3", productWarehouse.CreatedAt);
            await connection.OpenAsync();

            object? order = await command.ExecuteScalarAsync();
            return order is not null ? (int)order : -1;
        }
    }

    public async Task<bool> NoWarehouse(int id)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = "select * from Warehouse where IdWarehouse = @1";
            command.Parameters.AddWithValue("@1", id);
            await connection.OpenAsync();

            return await command.ExecuteScalarAsync() is null;
        }
    }

    public async Task<bool> NoProduct(int id)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = "select * from Product where IdProduct = @1";
            command.Parameters.AddWithValue("@1", id);
            await connection.OpenAsync();

            return await command.ExecuteScalarAsync() is null;
        }
    }

    public async Task UpdateFulfilledAt(Order order)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = "update [Order] set FulfilledAt = @1 where IdOrder = @2";
            command.Parameters.AddWithValue("@1", DateTime.UtcNow);
            command.Parameters.AddWithValue("@2", order.IdOrder);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<decimal> GetPrice(Product product)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = "select Price from Product where IdProduct = @1";
            command.Parameters.AddWithValue("@1", product.IdProduct);

            await connection.OpenAsync();
            object? price = await command.ExecuteScalarAsync();
            return price is not null ? (decimal)price : throw new Exception();
        }
    }



    public async Task<int> InsertProductWarehouse(ProductWarehouse productWarehouse, Order order, Product product)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = "insert into Product_Warehouse(idwarehouse, idproduct, idorder, amount, price, createdat) values (@1,@2,@3,@4,@5,@6); SELECT SCOPE_IDENTITY();";
            decimal price = await GetPrice(product);
            command.Parameters.AddWithValue("@1", productWarehouse.IdWarehouse);
            command.Parameters.AddWithValue("@2", productWarehouse.IdProduct);
            command.Parameters.AddWithValue("@3", order.IdOrder);
            command.Parameters.AddWithValue("@4", productWarehouse.Amount);
            command.Parameters.AddWithValue("@5", productWarehouse.Amount * price);
            command.Parameters.AddWithValue("@6", DateTime.UtcNow);

            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
    }

}