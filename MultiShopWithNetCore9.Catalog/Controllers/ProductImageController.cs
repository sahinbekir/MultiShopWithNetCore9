using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.Catalog.Dtos.ProductImageDtos;
using MultiShopWithNetCore9.Catalog.Services.ProductImageServices;

namespace MultiShopWithNetCore9.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductImageController : ControllerBase
{
    private readonly IProductImageService _ProductImageService;

    public ProductImageController(IProductImageService ProductImageService)
    {
        _ProductImageService = ProductImageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductImageList()
    {
        try
        {
            var values = await _ProductImageService.GetAllProductImageAsync();
            return Ok(values);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not result" + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductImageById(string id)
    {
        try
        {
            var value = await _ProductImageService.GetByIdProductImageAsync(id);
            return Ok(value);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not find" + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductImage(CreateProductImageDto createProductImageDto)
    {
        try
        {
            await _ProductImageService.CreateProductImageAsync(createProductImageDto);
            return Ok("Item Created");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not create" + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductImage(string id)
    {
        try
        {
            await _ProductImageService.DeleteProductImageAsync(id);
            return Ok("Item Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not delete" + ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto updateProductImageDto)
    {
        try
        {
            await _ProductImageService.UpdateProductImageAsync(updateProductImageDto);
            return Ok("Item Updated");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not update" + ex.Message);
        }
    }
}
