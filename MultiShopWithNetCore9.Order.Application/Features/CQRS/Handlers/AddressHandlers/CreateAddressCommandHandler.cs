using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;

public class CreateAddressCommandHandler
{
    private readonly IGenericRepository<Address> _repository;

    public CreateAddressCommandHandler(IGenericRepository<Address> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateAddressCommand createAddressCommand)
    {
        await _repository.CreateAsync(new Address
        {
            City = createAddressCommand.City,
            Detail = createAddressCommand.Detail,
            District = createAddressCommand.District,
            UserId = createAddressCommand.UserId
        });
    }
}
