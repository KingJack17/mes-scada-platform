using FactoryMES.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Core.Interfaces
{
    // IGenericRepository'den miras alarak tüm temel metotları otomatik olarak kazanır.
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Product'a özel metotlar ileride buraya eklenebilir.
        // Örnek: Task<IEnumerable<Product>> GetProductsWithHighStockAsync();
    }
}