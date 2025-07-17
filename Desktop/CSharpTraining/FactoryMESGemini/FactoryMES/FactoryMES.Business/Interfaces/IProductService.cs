using FactoryMES.Core.DTOs;
using FactoryMES.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<(Product, string)> CreateProductAsync(CreateProductDto productDto);
        Task<(Product, string)> UpdateProductAsync(int id, UpdateProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}