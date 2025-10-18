using MultiShopWithNetCore9.Order.Application.Features.CQRS.Queries.AddressQueries;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;

public class GetAddressByIdQueryHandler
{
    private readonly IGenericRepository<Address> _repository;
    public GetAddressByIdQueryHandler(IGenericRepository<Address> repository)
    {
        _repository = repository;
    }

    public async Task<GetAddressByIdQueryResult> Handle(GetAddressByIdQuery query)
    {
        var value = await _repository.GetByIdAsync(query.Id);
        return new GetAddressByIdQueryResult
        {
            AddressId = value.AddressId,
            City = value.City,
            Detail = value.Detail,
            District = value.District,
            UserId = value.UserId
        };
    }
}
