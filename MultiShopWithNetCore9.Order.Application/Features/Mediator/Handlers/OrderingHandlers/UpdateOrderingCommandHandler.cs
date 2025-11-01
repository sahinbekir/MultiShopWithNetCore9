using MediatR;
using MultiShopWithNetCore9.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class UpdateOrderingCommandHandler : IRequestHandler<UpdateOrderingCommand>
{
    private readonly IGenericRepository<Ordering> _genericRepository;

    public UpdateOrderingCommandHandler(IGenericRepository<Ordering> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task Handle(UpdateOrderingCommand request, CancellationToken cancellationToken)
    {
        var value = await _genericRepository.GetByIdAsync(request.OrderingId);
        value.OrderDate = request.OrderDate;
        value.UserId = request.UserId;
        value.TotalPrice = request.TotalPrice;
        value.OrderingId = request.OrderingId;
        await _genericRepository.UpdateAsync(value);
    }
}
