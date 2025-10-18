using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Queries.AddressQueries;

namespace MultiShopWithNetCore9.Order.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly GetAddressQueryHandler _getAddressQueryHandler;
    private readonly GetAddressByIdQueryHandler _getAddressByIdQueryHandler;
    private readonly CreateAddressCommandHandler _createAddressCommandHandler;
    private readonly UpdateAddressCommandHandler _updateAddressCommandHandler;
    private readonly RemoveAddressCommandHandler _removeAddressCommandHandler;

    public AddressesController(GetAddressQueryHandler getAddressQueryHandler, GetAddressByIdQueryHandler getAddressByIdQueryHandler, CreateAddressCommandHandler createAddressCommandHandler, UpdateAddressCommandHandler updateAddressCommandHandler, RemoveAddressCommandHandler removeAddressCommandHandler)
    {
        _getAddressQueryHandler = getAddressQueryHandler;
        _getAddressByIdQueryHandler = getAddressByIdQueryHandler;
        _createAddressCommandHandler = createAddressCommandHandler;
        _updateAddressCommandHandler = updateAddressCommandHandler;
        _removeAddressCommandHandler = removeAddressCommandHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddressList()
    {
        var values = await _getAddressQueryHandler.Handle();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddressById(int id)
    {
        var value = await _getAddressByIdQueryHandler.Handle(new GetAddressByIdQuery(id));
        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(CreateAddressCommand createAddressCommand)
    {
        await _createAddressCommandHandler.Handle(createAddressCommand);
        return Ok("Item Created.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAddress(UpdateAddressCommand updateAddressCommand)
    {
        await _updateAddressCommandHandler.Handle(updateAddressCommand);
        return Ok("Item Updated.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        await _removeAddressCommandHandler.Handle(new RemoveAddressCommand(id));
        return Ok("Item Removed.");
    }
}
