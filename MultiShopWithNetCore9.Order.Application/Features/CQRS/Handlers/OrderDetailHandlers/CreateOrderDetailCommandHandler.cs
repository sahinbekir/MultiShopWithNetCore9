using MultiShopWithNetCore9.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;

public class CreateOrderDetailCommandHandler
{
    private readonly IGenericRepository<OrderDetail> _repository;

    public CreateOrderDetailCommandHandler(IGenericRepository<OrderDetail> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateOrderDetailCommand createOrderDetailCommand)
    {
        await _repository.CreateAsync(new OrderDetail
        {
            ProductId = createOrderDetailCommand.ProductId,
            ProductName = createOrderDetailCommand.ProductName,
            ProductPrice = createOrderDetailCommand.ProductPrice,
            ProductAmount = createOrderDetailCommand.ProductAmount,
            ProductTotalPrice = createOrderDetailCommand.ProductTotalPrice,
            OrderingId = createOrderDetailCommand.OrderingId
        });
    }
}