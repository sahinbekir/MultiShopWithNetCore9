using MultiShopWithNetCore9.Order.Application.Features.CQRS.Results.OrderDetailResults;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;

public class GetOrderDetailQueryHandler
{
    private readonly IGenericRepository<OrderDetail> _repository;

    public GetOrderDetailQueryHandler(IGenericRepository<OrderDetail> repository)
    {
        _repository = repository;
    }

    public async Task<List<GetOrderDetailQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetOrderDetailQueryResult
        {
            OrderDetailId = x.OrderDetailId,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            ProductAmount = x.ProductAmount,
            ProductTotalPrice = x.ProductTotalPrice,
            ProductPrice = x.ProductPrice,
            OrderingId = x.OrderingId
        }).ToList();
    }
}
