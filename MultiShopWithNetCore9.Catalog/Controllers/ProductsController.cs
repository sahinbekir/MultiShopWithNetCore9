using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.Catalog.Dtos.ProductDtos;
using MultiShopWithNetCore9.Catalog.Services.ProductServices;

namespace MultiShopWithNetCore9.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductList()
    {
        try
        {
            var values = await _productService.GetAllProductAsync();
            return Ok(values);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not result" + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var value = await _productService.GetByIdProductAsync(id);
            return Ok(value);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not find" + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
    {
        try
        {
            await _productService.CreateProductAsync(createProductDto);
            return Ok("Item Created");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not create" + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Item Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not delete" + ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
    {
        try
        {
            await _productService.UpdateProductAsync(updateProductDto);
            return Ok("Item Updated");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not update" + ex.Message);
        }
    }
}
