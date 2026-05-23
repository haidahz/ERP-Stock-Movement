using ERP_Stock_Movement.Products.Data;

using ERP_Stock_Movement.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP_Stock_Movement.Products.Repositories
{
    public class ProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
