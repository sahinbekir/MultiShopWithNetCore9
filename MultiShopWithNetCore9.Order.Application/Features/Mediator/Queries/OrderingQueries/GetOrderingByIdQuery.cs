using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Results.OrderingResults;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Queries.OrderingQueries;

public class GetOrderingByIdQuery:IRequest<GetOrderingByIdQueryResult>
{
    public int Id { get; set; }

    public GetOrderingByIdQuery(int id)
    {
        Id = id;
    }
}
