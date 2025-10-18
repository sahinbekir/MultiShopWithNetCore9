using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;

public class UpdateOrderDetailCommandHandler
{
    private readonly IGenericRepository<OrderDetail> _repository;

    public UpdateOrderDetailCommandHandler(IGenericRepository<OrderDetail> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateOrderDetailCommand command)
    {
        var values = await _repository.GetByIdAsync(command.OrderDetailId);
        values.OrderDetailId = command.OrderDetailId;
        values.ProductName = command.ProductName;
        values.ProductPrice = command.ProductPrice;
        values.ProductId = command.ProductId;
        values.OrderingId = command.OrderingId;
        values.ProductAmount = command.ProductAmount;
        values.ProductTotalPrice = command.ProductTotalPrice;
        await _repository.UpdateAsync(values);
    }
}
