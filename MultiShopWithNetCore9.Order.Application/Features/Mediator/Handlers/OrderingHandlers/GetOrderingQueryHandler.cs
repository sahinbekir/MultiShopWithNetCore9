using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class GetOrderingQueryHandler : IRequestHandler<GetOrderingQuery, List<GetOrderingQueryResult>>
{
    private readonly IGenericRepository<Ordering> _genericRepository;

    public GetOrderingQueryHandler(IGenericRepository<Ordering> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<List<GetOrderingQueryResult>> Handle(GetOrderingQuery request, CancellationToken cancellationToken)
    {
        var values = await _genericRepository.GetAllAsync();
        return values.Select(x => new GetOrderingQueryResult
        {
            OrderingId = x.OrderingId,
            OrderDate = x.OrderDate,
            TotalPrice = x.TotalPrice,
            UserId = x.UserId
        }).ToList();
    }
}
