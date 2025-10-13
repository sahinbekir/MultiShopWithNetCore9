using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.Discount.Dtos;
using MultiShopWithNetCore9.Discount.Services;

namespace MultiShopWithNetCore9.Discount.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountsController : ControllerBase
{
    private readonly IDiscountService _discountService;
    public DiscountsController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDiscountCouponList()
    {
        var values = await _discountService.GetAllDiscountCouponAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountCouponById(int id)
    {
        var value = await _discountService.GetByIdDiscountCouponAsync(id);
        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscountCoupon(CreateDiscountCouponDto createDiscountCouponDto)
    {
        await _discountService.CreateDiscountCouponAsync(createDiscountCouponDto);
        return Ok("Item created");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDiscountCoupon(UpdateDiscountCouponDto updateDiscountCouponDto)
    {
        await _discountService.UpdateDiscountCouponAsync(updateDiscountCouponDto);
        return Ok("Item updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDiscountCoupon(int id)
    {
        await _discountService.DeleteDiscountCouponAsync(id);
        return Ok("Item deleted");
    }
}
