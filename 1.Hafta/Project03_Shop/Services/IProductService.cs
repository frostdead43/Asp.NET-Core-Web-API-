using System;
using Project03_Shop.Models;

namespace Project03_Shop.Services;

public interface IProductService
{
  List<Product> GetAll();
  Product? GetById(int id);
 List<Product> GetLowStock(int threshold= 10);
}
