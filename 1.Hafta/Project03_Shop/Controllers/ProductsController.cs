using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project03_Shop.Services;

namespace Project03_Shop.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
  {
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
      _productService = productService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
       var products = _productService.GetAll();
       return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _productService.GetById(id);
        if(product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("low-stock")]    // api/products/low-stock?threshold=8
        public IActionResult GetLowStockProducts([FromQuery] int threshold=10)
        {
            var products = _productService.GetLowStock(threshold);
            return Ok(products);
        }

  }
}
