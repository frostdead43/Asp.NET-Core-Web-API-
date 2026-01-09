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
  public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _productService.GetAllAsync(); // List<Product>
        return Ok(products);
    }

    [HttpGet("paged")]

    public async Task<ActionResult<IEnumerable<Product>>> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)

        {
            var (product,totalCount) = await _productService.GetProductsPagedAsync(pageNumber, pageSize);
            return Ok(product);
        }



        [HttpGet("low-stock")]
    public async Task<ActionResult <IEnumerable<Product>>> GetLowStockProducts([FromQuery] int threshold = 20)
    {
        var products = await _productService.GetLowStockProductsAsync(threshold);

        return Ok(products);
    }

    [HttpGet("{id}")]

    public  async Task<ActionResult<Product>> GetById([FromRoute] int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(new {message=$"{id} id Cant be found!"});
        }
        return Ok(product);
        
    }

    [HttpGet("by-category/{category}")]
    public async Task<ActionResult <IEnumerable<Product>>> GetProductsByCategory(string category)
    {
        var products = await _productService.GetProductsByCategoryAsync(category);
        if (products.Count == 9)
        {
            return NotFound(new { message = $"{category} cant be found any product" });
        }
        return Ok(products);
    }

    [HttpPost] 

    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {

        try
        {
            var newProduct = await _productService.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new {id=newProduct?.Id}, newProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new {error = ex.Message});
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new {error = ex.Message});
        }
        
        
    }

         [HttpPut("{id}")] 

        public async Task<ActionResult<Product>> Update(int id, [FromBody] Product product)
        {


        if (string.IsNullOrWhiteSpace(product.Name))

        {
            throw new ArgumentException("product name is required", nameof(product));
        }

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, product!);
            if (updatedProduct == null)
            {
                return NotFound(new {message = $"{id}The update process could not be completed because the product with the specified ID could not be found."});
            }
            return Ok(updatedProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new {error = ex.Message});
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new {error = ex.Message});
        }
        
          
          
        }

    [HttpDelete("{id}")]

    public async Task<ActionResult<Product>> Delete(int id)
        {
            var isSuccess = await _productService.DeleteAsync (id);
            if (!isSuccess)
            {
                return NotFound(new {message = "Deleted"});
            }
            return Ok(isSuccess);    
        }

        [HttpPatch("{id}/stock")]

        public async Task<ActionResult<Product>> UpdateStock(int id, [FromBody] StockUpdateRequest stockUpdateRequest)
        {
            try
            {
                var product = await _productService.UpdateStockAsync(id,stockUpdateRequest.QuantityChange);
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
        public async Task<ActionResult<object>> CheckStock(int id, [FromQuery] int quantity)
        {
            var isAvaible = await _productService.CheckStockAvailableAsync(id, quantity);
            var product = await _productService.GetByIdAsync(id);
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
