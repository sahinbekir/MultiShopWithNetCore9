namespace MultiShopWithNetCore9.Order.Application.Features.CQRS.Queries.AddressQueries;

public class GetAddresByIdQuery
{
    public int Id { get; set; }

    public GetAddresByIdQuery(int id)
    {
        Id = id;
    }
}
