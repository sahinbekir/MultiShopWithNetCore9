using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;

public class UpdateAddressCommandHandler
{
    private readonly IGenericRepository<Address> _repository;

    public UpdateAddressCommandHandler(IGenericRepository<Address> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateAddressCommand command)
    {
        var values = await _repository.GetByIdAsync(command.AddressId);
        values.Detail = command.Detail;
        values.District = command.District;
        values.City = command.City;
        values.UserId = command.UserId;
        await _repository.UpdateAsync(values);
    }
}
