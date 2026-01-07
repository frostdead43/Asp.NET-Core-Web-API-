using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project04_ShopApp.Services;
using Project04_ShopApp.Models;

namespace Project04_ShopApp.Controllers
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
        public ActionResult <IEnumerable<Product>> GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products); 
        }

         [HttpGet("low-stock")]
        public ActionResult <IEnumerable<Product>> GetLowStockProducts([FromQuery] int threshold = 20)
        {
            var products = _productService.GetLowStockProducts(threshold);

            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}")]

        public ActionResult<Product> GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound(new {message=$"{id} id Cant be found!"});
            }
            return Ok(product);
            
        }

        [HttpGet("by-category/{category}")]
        public ActionResult <IEnumerable<Product>> GetProductsByCategory(string category)
        {
            var products = _productService.GetProductsByCategory(category);
            if (products.Count == 0)
            {
                return NotFound(new { message = $"{category} cant be found any product" });
            }
            return Ok(products);
        }

        [HttpPost] 

        public ActionResult<Product> Create([FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest(new {message = "product information cannot be empty"});
            }
            if(string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest(new {message = "product name is required"});
            }
            if(product.Price <=0)
            {
                return BadRequest(new {message = "The product price must be greater than 0"});
            }
            if(product.Stock <0)
            {
                return BadRequest(new {message = "stock quantity cannot be negative"});
            }

            var newProduct = _productService.Add(product);

            return CreatedAtAction(nameof(GetById), new {id=newProduct?.Id}, newProduct);
            
        }

         [HttpPut("{id}")] 

        public ActionResult<Product> Update(int id, [FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest(new {message = "product information cannot be empty"});
            }
            if(id != product.Id)
            {
                return BadRequest(new {message = "The ID value in the URL does not match the ID value in the body!"});
            }
            if(string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest(new {message = "product name is required"});
            }
            if(product.Price <=0)
            {
                return BadRequest(new {message = "The product price must be greater than 0"});
            }
            if(product.Stock <0)
            {
                return BadRequest(new {message = "stock quantity cannot be negative"});
            }
            var updatedProduct = _productService.UpdateProduct(id, product);

            if (updatedProduct == null)
            {
                return NotFound(new {message = $"{id}The update process could not be completed because the product with the specified ID could not be found."});
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]

        public ActionResult<Product> Delete(int id)
        {
            var isSuccess = _productService.Delete(id);
            if (!isSuccess)
            {
                return NotFound(new {message = "Deleted"});
            }
            return Ok(isSuccess);    
        }

        [HttpPatch("{id}/stock")]

        public ActionResult<Product> UpdateStock(int id, [FromBody] StockUpdateRequest stockUpdateRequest)
        {
            try
            {
                var product = _productService.UpdateStock(id,stockUpdateRequest.QuantityChange);
                if(product == null)
                {
                    return NotFound();
                }
                return Ok();
            }
             catch (InvalidOperationException ex)
            {
             return BadRequest(new {message = $"Error: {ex.Message}"});
            }
        }

         [HttpGet("{id}/stock-check")]
        public ActionResult<object> CheckStock(int id, [FromQuery] int quantity)
        {
            var isAvaible = _productService.CheckStockAvaible(id, quantity);
            var product = _productService.GetById(id);
            if (product is null)
            {
                return NotFound(new { message = $"{id}The control process could not be completed because the product with the specified ID could not be found." });
            }
            return Ok(
                new {
                    productId=id,
                    productName=product.Name,
                    currentStock=product.Stock,
                    requestedQuantity = quantity,
                    isAvaible = isAvaible,
                    message = isAvaible ? "Sufficient stock" : "Insufficient Stock."
                }
            );
        }
    }

}
