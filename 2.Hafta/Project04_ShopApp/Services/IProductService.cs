using System;
using Project04_ShopApp.Models;

namespace Project04_ShopApp.Services;

public interface IProductService
{
  List<Product> GetAll();
  Product? GetById(int id);
  Product? Add(Product product);

  Product? UpdateProduct(int id, Product product);

  bool Delete(int id);

  List<Product> GetLowStockProducts(int thresold);

   List<Product> GetProductsByCategory(string category);
}
