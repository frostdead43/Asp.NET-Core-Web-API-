using System;
using Microsoft.EntityFrameworkCore;
using Project04_ShopApp.Data;
using Project04_ShopApp.Models;

namespace Project04_ShopApp.Services;

public class ProductService : IProductService

{

  private readonly AppDbContext _context;

 public ProductService(AppDbContext context)
  {
    _context = context;
  }

  public Task<Product?> AddAsync(Product product)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> CheckStockAvailableAsync(int id, int requestedQuantity)
  {
   var product = await _context.Products.FindAsync(id);
   if(product == null)
    {
      return false;
    }
    return product.Stock >= requestedQuantity;
  }

  public Task<bool> DeleteAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<List<Product>> GetAllAsync()
  {
    var products = await _context.Products.ToListAsync();
    return products;
  }

  public async Task<Product?> GetByIdAsync(int id)
  {
    // var product = await _context.Products.Where(p=>p.Id == id).FirstOrDefaultAsync();
    var product = await _context.Products.FindAsync(id);
    return product;
  }

  public async Task<List<Product>> GetLowStockProductsAsync(int threshold)
  {
    var products = await _context.Products.Where(x=> x.Stock < threshold).ToListAsync();
    return products;
  }

  public async Task<List<Product>> GetProductsByCategoryAsync(string category)
  {
    var products = await _context.Products.Where(x=> x.Category == category).ToListAsync();
    return products;
  }

  public Task<Product?> UpdateProductAsync(int id, Product product)
  {
    throw new NotImplementedException();
  }

  public Task<Product?> UpdateStockAsync(int id, int quantityChange)
  {
    throw new NotImplementedException();
  }
}
