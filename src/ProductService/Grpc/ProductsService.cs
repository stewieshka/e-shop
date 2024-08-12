using System.Text;
using Grpc.Core;
using Product;
using ProductService.Persistence;
using ProductService.Validators;

namespace ProductService.Grpc;

public class ProductsService(ProductsRepository productsRepository) : Products.ProductsBase
{
    public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var commonValidator = new CommonValidator<CreateProductRequest, CreateProductValidator>();

        await commonValidator.ValidateOrThrowAsync(request);
        
        var product = new Product.Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Price = request.Price,
            Stock = request.Stock
        };
        
        await productsRepository.CreateAsync(product);

        return new CreateProductResponse
        {
            Product = product
        };
    }

    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var commonValidator = new CommonValidator<GetProductRequest, GetProductValidator>();

        await commonValidator.ValidateOrThrowAsync(request);
        
        var product = await productsRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product was not found"));
        }

        return new GetProductResponse
        {
            Product = product
        };
    }

    public override async Task<SearchProductsResponse> SearchProducts(SearchProductsRequest request, ServerCallContext context)
    {
        var products = await productsRepository.Get(request.Page, request.Size);

        return new SearchProductsResponse
        {
            Products = { products },
            Total = products.Length
        };

    }

    public override Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        return base.UpdateProduct(request, context);
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        var commonValidator = new CommonValidator<DeleteProductRequest, DeleteProductValidator>();

        await commonValidator.ValidateOrThrowAsync(request);
        
        var deleted = await productsRepository.DeleteAsync(request.Id);

        if (!deleted)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Product was not deleted"));
        }

        return new DeleteProductResponse
        {
            Success = true
        };
    }
}