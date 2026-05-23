using ERP_Stock_Movement.Products.Models;
using ERP_Stock_Movement.Products.Repositories;

namespace ERP_Stock_Movement.Products.Services;

public class ProductService
{
    private readonly ProductRepository _repository;

    public ProductService(ProductRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Product>> GetAll() => _repository.GetAll();

    public Task<Product?> GetById(int id) => _repository.GetById(id);

    public async Task<(bool Success, Product? Product, string? Error)> CreateAsync(CreateProductRequest request)
    {
        if (request.ProductId <= 0)
        {
            return (false, null, "Product id must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return (false, null, "Name is required.");
        }

        if (request.Price < 0)
        {
            return (false, null, "Price cannot be negative.");
        }

        if (await _repository.ExistsAsync(request.ProductId))
        {
            return (false, null, $"Product id {request.ProductId} already exists. Use a different id.");
        }

        var product = new Product
        {
            Id = request.ProductId,
            Name = request.Name.Trim(),
            Price = request.Price
        };

        var created = await _repository.Create(product);
        return (true, created, null);
    }
}
