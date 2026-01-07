using System;
using Project04_ShopApp.Models;

namespace Project04_ShopApp.Services;

public interface IProductService
{
 Task<List<Product>> GetAllAsync();

Task<Product?> GetByIdAsync(int id);

Task<Product?> AddAsync(Product product);

Task<Product?> UpdateProductAsync(int id, Product product);

Task<bool> DeleteAsync(int id);

Task<List<Product>> GetLowStockProductsAsync(int threshold);

Task<List<Product>> GetProductsByCategoryAsync(string category);

Task<Product?> UpdateStockAsync(int id, int quantityChange);

Task<bool> CheckStockAvailableAsync(int id, int requestedQuantity);

}
