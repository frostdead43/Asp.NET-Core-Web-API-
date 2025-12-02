using System;
using Project03_Shop.Models;

namespace Project03_Shop.Services;

public class ProductService : IProductService
{

  private readonly List<Product> _products = [
  
        new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 999.99m, Stock = 15 },
        new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone", Price = 699.99m, Stock = 8 },
        new Product { Id = 3, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 199.99m, Stock = 5 },
        new Product { Id = 4, Name = "Monitor", Description = "4K UHD Monitor", Price = 399.99m, Stock = 12 },
        new Product { Id = 5, Name = "Keyboard", Description = "Mechanical keyboard", Price = 89.99m, Stock = 2 }
  ];
  
  public List<Product> GetAll()
  {
    return _products;
  }

  public Product? GetById(int id)
  {
    return _products.FirstOrDefault(p=> p.Id == id);
  }

  public List<Product> GetLowStock(int threshold = 10)
  {
    return _products.Where(p => p.Stock < threshold).ToList();
  }
}
