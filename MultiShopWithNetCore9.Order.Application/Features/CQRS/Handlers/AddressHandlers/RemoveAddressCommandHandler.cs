using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;

public class RemoveAddressCommandHandler
{
    public readonly IGenericRepository<Address> _repository;

    public RemoveAddressCommandHandler(IGenericRepository<Address> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveAddressCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.DeleteAsync(value);
    }
}
