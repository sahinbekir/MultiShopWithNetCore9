using MultiShopWithNetCore9.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Results.OrderDetailResults;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;

public class GetOrderDetailByIdQueryHandler
{
    private readonly IGenericRepository<OrderDetail> _repository;
    public GetOrderDetailByIdQueryHandler(IGenericRepository<OrderDetail> repository)
    {
        _repository = repository;
    }

    public async Task<GetOrderDetailByIdQueryResult> Handle(GetOrderDetailByIdQuery query)
    {
        var value = await _repository.GetByIdAsync(query.Id);
        return new GetOrderDetailByIdQueryResult
        {
            OrderDetailId = value.OrderDetailId,
            ProductId = value.ProductId,
            ProductName = value.ProductName,
            ProductAmount = value.ProductAmount,
            ProductTotalPrice = value.ProductTotalPrice,
            ProductPrice = value.ProductPrice,
            OrderingId = value.OrderingId
        };
    }
}
