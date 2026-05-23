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

    public Task<Product> Create(Product product) => _repository.Create(product);
}
