using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Queries.OrderDetailQueries;

namespace MultiShopWithNetCore9.Order.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly GetOrderDetailQueryHandler _getOrderDetailQueryHandler;
    private readonly GetOrderDetailByIdQueryHandler _getOrderDetailByIdQueryHandler;
    private readonly CreateOrderDetailCommandHandler _createOrderDetailCommandHandler;
    private readonly UpdateOrderDetailCommandHandler _updateOrderDetailCommandHandler;
    private readonly RemoveOrderDetailCommandHandler _removeOrderDetailCommandHandler;

    public OrderDetailController(GetOrderDetailQueryHandler getOrderDetailQueryHandler, GetOrderDetailByIdQueryHandler getOrderDetailByIdQueryHandler, CreateOrderDetailCommandHandler createOrderDetailCommandHandler, UpdateOrderDetailCommandHandler updateOrderDetailCommandHandler, RemoveOrderDetailCommandHandler removeOrderDetailCommandHandler)
    {
        _getOrderDetailQueryHandler = getOrderDetailQueryHandler;
        _getOrderDetailByIdQueryHandler = getOrderDetailByIdQueryHandler;
        _createOrderDetailCommandHandler = createOrderDetailCommandHandler;
        _updateOrderDetailCommandHandler = updateOrderDetailCommandHandler;
        _removeOrderDetailCommandHandler = removeOrderDetailCommandHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderDetailList()
    {
        var values = await _getOrderDetailQueryHandler.Handle();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetailById(int id)
    {
        var value = await _getOrderDetailByIdQueryHandler.Handle(new GetOrderDetailByIdQuery(id));
        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderDetail(CreateOrderDetailCommand createOrderDetailCommand)
    {
        await _createOrderDetailCommandHandler.Handle(createOrderDetailCommand);
        return Ok("Item Created.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrderDetail(UpdateOrderDetailCommand updateOrderDetailCommand)
    {
        await _updateOrderDetailCommandHandler.Handle(updateOrderDetailCommand);
        return Ok("Item Updated.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrderDetail(int id)
    {
        await _removeOrderDetailCommandHandler.Handle(new RemoveOrderDetailCommand(id));
        return Ok("Item Removed.");
    }
}
