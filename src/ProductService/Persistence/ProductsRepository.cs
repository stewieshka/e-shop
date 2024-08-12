using System.Data;
using Dapper;
using Grpc.Core;
using ProductService.Persistence.Database;

namespace ProductService.Persistence;

public class ProductsRepository(IDbConnectionFactory dbConnectionFactory)
{
    public async Task CreateAsync(Product.Product product)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             INSERT INTO products (id, name, description, category, price, stock) 
                             VALUES (@Id, @Name, @Description, @Category, @Price, @Stock);
                             """;

        var result = await connection.ExecuteAsync(query,
            new
            {
                Id = Guid.Parse(product.Id),
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock
            });

        if (result < 1)
        {
            throw new Exception("Error while creating a product");
        }
    }

    public async Task<Product.Product?> GetByIdAsync(string id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             SELECT id::text AS id, name, description, category, price, stock
                             FROM products
                             WHERE id = @Id::uuid;
                             """;
        var product = await connection.QueryFirstOrDefaultAsync<Product.Product>(query, new { Id = id });

        return product;
    }

    public async Task<Product.Product[]> Get(int page, int size)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             SELECT id::text AS id, name, description, category, price, stock 
                             FROM products
                             LIMIT @Limit
                             OFFSET @Offset
                             """;

        var products =
            await connection.QueryAsync<Product.Product>(query, new { Limit = size, Offset = (page - 1) * size });

        return products.ToArray();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             DELETE FROM products
                             WHERE id = @Id::uuid;
                             """;

        var result = await connection.ExecuteAsync(query, new { Id = id });

        return result == 1;
    }
}