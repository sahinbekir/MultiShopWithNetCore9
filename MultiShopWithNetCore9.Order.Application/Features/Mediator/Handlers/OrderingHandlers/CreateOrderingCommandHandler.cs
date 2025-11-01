using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class CreateOrderingCommandHandler : IRequestHandler<CreateOrderingCommand>
{
    private readonly IGenericRepository<Ordering> _genericRepository;

    public CreateOrderingCommandHandler(IGenericRepository<Ordering> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task Handle(CreateOrderingCommand request, CancellationToken cancellationToken)
    {
        await _genericRepository.CreateAsync(new Ordering
        {
            OrderDate = request.OrderDate,
            TotalPrice = request.TotalPrice,
            UserId = request.UserId
        });
    }
}
