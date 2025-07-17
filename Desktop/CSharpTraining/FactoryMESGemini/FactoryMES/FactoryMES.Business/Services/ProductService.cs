using FactoryMES.Core.DTOs;
using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<(Product, string)> CreateProductAsync(CreateProductDto productDto)
        {
            // Mükerrer ürün kontrolü mantığı artık burada, yani iş katmanında.
            var existingProduct = await _unitOfWork.Products.FindAsync(p => p.ProductCode.ToLower() == productDto.ProductCode.ToLower());
            if (existingProduct.Any())
            {
                // Başarısızlık durumunda null ürün ve bir hata mesajı dönüyoruz.
                return (null, $"'{productDto.ProductCode}' kodlu ürün zaten mevcut.");
            }

            var newProduct = new Product
            {
                ProductCode = productDto.ProductCode,
                Name = productDto.Name,
                ProductType = productDto.ProductType,
                MinStockLevel = productDto.MinStockLevel,
                StockingUnitOfMeasureId = productDto.StockingUnitOfMeasureId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.Products.AddAsync(newProduct);
            await _unitOfWork.CompleteAsync();

            // Başarılı durumda oluşturulan ürünü ve boş bir hata mesajı dönüyoruz.
            return (newProduct, null);
        }
        public async Task<(Product, string)> UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var productFromDb = await _unitOfWork.Products.GetByIdAsync(id);

            if (productFromDb == null)
            {
                return (null, $"ID'si {id} olan ürün bulunamadı.");
            }

            // DTO'daki verilerle nesneyi güncelle
            productFromDb.Name = productDto.Name;
            productFromDb.ProductType = productDto.ProductType;
            productFromDb.MinStockLevel = productDto.MinStockLevel;
            productFromDb.StockingUnitOfMeasureId = productDto.StockingUnitOfMeasureId;

            await _unitOfWork.CompleteAsync();

            return (productFromDb, null);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productFromDb = await _unitOfWork.Products.GetByIdAsync(id);

            if (productFromDb == null)
            {
                // Ürün bulunamadı, silme işlemi başarısız.
                return false;
            }

            // Repository'deki override ettiğimiz Remove metodunu çağırıyoruz.
            // Bu metot, IsDeleted = true yapacak.
            _unitOfWork.Products.Remove(productFromDb);
            await _unitOfWork.CompleteAsync();

            // İşlem başarılı.
            return true;
        }
    }
}