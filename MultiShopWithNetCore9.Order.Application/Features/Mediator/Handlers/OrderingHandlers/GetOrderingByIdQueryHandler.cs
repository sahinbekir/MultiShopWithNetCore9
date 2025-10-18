using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class GetOrderingByIdQueryHandler : IRequestHandler<GetOrderingByIdQuery, GetOrderingByIdQueryResult>
{
    private readonly IGenericRepository<Ordering> _genericRepository;

    public GetOrderingByIdQueryHandler(IGenericRepository<Ordering> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<GetOrderingByIdQueryResult> Handle(GetOrderingByIdQuery request, CancellationToken cancellationToken)
    {
        var value = await _genericRepository.GetByIdAsync(request.Id);
        return new GetOrderingByIdQueryResult
        {
            OrderDate = value.OrderDate,
            TotalPrice = value.TotalPrice,
            UserId = value.UserId,
            OrderingId = value.OrderingId
        };
    }
}
