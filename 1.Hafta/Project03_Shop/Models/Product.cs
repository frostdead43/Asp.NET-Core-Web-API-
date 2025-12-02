using System;

namespace Project03_Shop.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }=string.Empty;  // Default empty

    public string? Description { get; set; } // skippable
    public decimal Price { get; set; }
    public int Stock { get; set; }
}