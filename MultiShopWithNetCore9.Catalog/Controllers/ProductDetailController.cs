using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.Catalog.Dtos.ProductDetailDtos;
using MultiShopWithNetCore9.Catalog.Services.ProductDetailServices;

namespace MultiShopWithNetCore9.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductDetailController : ControllerBase
{
    private readonly IProductDetailService _productDetailService;

    public ProductDetailController(IProductDetailService productDetailService)
    {
        _productDetailService = productDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductDetailList()
    {
        try
        {
            var values = await _productDetailService.GetAllProductDetailAsync();
            return Ok(values);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not result" + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductDetailById(string id)
    {
        try
        {
            var value = await _productDetailService.GetByIdProductDetailAsync(id);
            return Ok(value);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not find" + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductDetail(CreateProductDetailDto createProductDetailDto)
    {
        try
        {
            await _productDetailService.CreateProductDetailAsync(createProductDetailDto);
            return Ok("Item Created");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not create" + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductDetail(string id)
    {
        try
        {
            await _productDetailService.DeleteProductDetailAsync(id);
            return Ok("Item Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not delete" + ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto updateProductDetailDto)
    {
        try
        {
            await _productDetailService.UpdateProductDetailAsync(updateProductDetailDto);
            return Ok("Item Updated");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not update" + ex.Message);
        }
    }
}
