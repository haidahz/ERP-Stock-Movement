using ERP_Stock_Movement.Products.Repositories;
using ERP_Stock_Movement.Products.Models;
using ERP_Stock_Movement.Products.Repositories;

namespace ERP_Stock_Movement.Products.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Product> Create(Product product)
        {
            return await _repository.Create(product);
        }
    }
}
