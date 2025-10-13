using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MultiShopWithNetCore9.Catalog.Dtos.CategoryDtos;
using MultiShopWithNetCore9.Catalog.Services.CategoryServices;

namespace MultiShopWithNetCore9.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<ProductImageController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<ProductImageController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategoryList()
    {
        try
        {
            _logger.LogInformation("Category list requested at {time}", DateTime.UtcNow);
            var values = await _categoryService.GetAllCategoryAsync();
            _logger.LogInformation("Category list returned {count} items", values.Count());
            return Ok(values);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching category list");
            return BadRequest("Did not result" + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id)
    {
        try
        {
            var value = await _categoryService.GetByIdCategoryAsync(id);
            return Ok(value);
        }
        catch (Exception ex)
        {
            return BadRequest("Did not find" + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        try
        {
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            _logger.LogInformation("Input:" + createCategoryDto.ToJson());
            return Ok("Item Created");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not create" + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok("Item Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not delete" + ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
    {
        try
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return Ok("Item Updated");
        }
        catch (Exception ex)
        {
            return BadRequest("Did not update" + ex.Message);
        }
    }
}
