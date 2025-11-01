using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class RemoveOrderingCommandHandler : IRequestHandler<RemoveOrderingCommand>
{
    private readonly IGenericRepository<Ordering> _genericRepository;

    public RemoveOrderingCommandHandler(IGenericRepository<Ordering> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task Handle(RemoveOrderingCommand request, CancellationToken cancellationToken)
    {
        var value = await _genericRepository.GetByIdAsync(request.Id);
        await _genericRepository.DeleteAsync(value);
    }
}
