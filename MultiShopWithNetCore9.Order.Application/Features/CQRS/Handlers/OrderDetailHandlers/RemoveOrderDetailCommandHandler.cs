using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;

public class RemoveOrderDetailCommandHandler
{
    public readonly IGenericRepository<OrderDetail> _repository;

    public RemoveOrderDetailCommandHandler(IGenericRepository<OrderDetail> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveOrderDetailCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.DeleteAsync(value);
    }
}
