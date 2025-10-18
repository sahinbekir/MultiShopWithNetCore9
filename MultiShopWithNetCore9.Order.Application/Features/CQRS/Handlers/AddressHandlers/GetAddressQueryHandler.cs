using MultiShopWithNetCore9.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;

public class GetAddressQueryHandler
{
    private readonly IGenericRepository<Address> _repository;

    public GetAddressQueryHandler(IGenericRepository<Address> repository)
    {
        _repository = repository;
    }

    public async Task<List<GetAddressQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetAddressQueryResult
        {
            AddressId = x.AddressId,
            City = x.City,
            Detail = x.Detail,
            District = x.District,
            UserId = x.UserId
        }).ToList();
    }
}
