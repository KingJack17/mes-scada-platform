using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.DataAccess.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        // GenericRepository'deki metotları override ederek özelleştiriyoruz.
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                                 .Where(p => !p.IsDeleted)
                                 .Include(p => p.StockingUnitOfMeasure) // İlişkili veriyi dahil et
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.StockingUnitOfMeasure) // İlişkili veriyi dahil et
                                 .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        // Bu metot soft-delete (mantıksal silme) yapar.
        public override void Remove(Product entity)
        {
            entity.IsDeleted = true;
            _context.Products.Update(entity);
        }

        // Bu arayüzden gelen özel bir metottu, şimdilik boş bırakabiliriz veya silebiliriz.
        public Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            // Gelecekte bu metot doldurulabilir.
            throw new System.NotImplementedException();
        }
    }
}