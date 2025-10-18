using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Results.OrderingResults;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Queries.OrderingQueries;

public class GetOrderingQuery:IRequest<List<GetOrderingQueryResult>>
{
}
