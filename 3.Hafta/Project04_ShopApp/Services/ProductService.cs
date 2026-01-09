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

  public async Task<Product?> AddAsync(Product product)
  {

    if(product == null)
    {
      throw new ArgumentException("product information cannot be empty", nameof(product));
    }

    if(string.IsNullOrWhiteSpace(product.Name))
    {
      throw new ArgumentException("product name is required", nameof(product));
    }

    if(product.Stock < 0)
    {
      throw new ArgumentException("Stock cannot be negative!",nameof(product));
    }

    if(product.Price < 0)
    {
      throw new ArgumentException("Price cannot be negative!",nameof(product));
    }

    var exists = await _context.Products.AnyAsync(x=>x.Name == product.Name);

    if(exists)
    {
      throw new InvalidOperationException($"'{product.Name}' already exist!");
    }

    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();
    return product;
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

  public async Task<bool> DeleteAsync(int id)
  {
    var product = await _context.Products.FindAsync(id);
    if(product == null)
    {
      return false;
    }

    _context.Products.Remove(product);
    await _context.SaveChangesAsync();
    return true;
  
  }

  public async Task<List<Product>> GetAllAsync()
  {
    var products = await _context.Products.OrderBy(x=>x.Name).ToListAsync();
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

  public async Task<(List<Product> Products, int TotalCount)> GetProductsPagedAsync(int pageNumber=2, int pageSize=10)
  {
    var totalCount = await _context.Products.CountAsync();
    var products = await _context.Products.OrderBy(X=>X.Name).Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
    return (products, totalCount);
  }

  public async Task<Product?> UpdateProductAsync(int id, Product product)
  {
    if(product == null)
    {
      throw new ArgumentException("product information cannot be empty", nameof(product));
    }

    if(id!=product.Id)
    {
      return null;
    }
    var existingProduct = await _context.Products.FindAsync(id);
    if(existingProduct == null)
    {
      return null;
    }
     if(product.Stock < 0)
    {
      throw new ArgumentException("Stock cannot be negative!",nameof(product));
    }

    if(product.Price < 0)
    {
      throw new ArgumentException("Price cannot be negative!",nameof(product));
    }

    existingProduct.Name = product.Name;
    existingProduct.Price = product.Price;
    existingProduct.Stock = product.Stock;
    existingProduct.Category = product.Category;
    existingProduct.Description = product.Description;
    
    await _context.SaveChangesAsync();
    return existingProduct;
  }

  public async Task<Product?> UpdateStockAsync(int id, int quantityChange)
  {
    var product = await _context.Products.FindAsync(id);
    if(product == null)
    {
      throw new ArgumentNullException(nameof(product), "Product cannot find ");
    }
    var newStock = product.Stock + quantityChange;
    if(newStock<0)
    {
      throw new InvalidOperationException($"Insufficient Stock! Current stock is:{product.Stock}, Requested change is:{quantityChange}");
    }
    product.Stock = newStock;
    await _context.SaveChangesAsync();

    return product;

  }
}
