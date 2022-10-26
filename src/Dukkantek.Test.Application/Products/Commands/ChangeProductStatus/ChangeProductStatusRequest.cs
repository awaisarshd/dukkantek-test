using Dukkantek.Test.Application.Common.Interfaces;
using Dukkantek.Test.Application.Common.Models;
using Dukkantek.Test.Domain.Common.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Dukkantek.Test.Application.Products.Commands.ChangeProductStatus;

public class ChangeProductStatusRequest : IRequest<Result>
{
    public int ProductId { get; set; }
    public short StatusId { get; set; }
}

public class ChangeProductStatusRequestHandler : IRequestHandler<ChangeProductStatusRequest, Result>
{
    private readonly IApplicationDbContext _context;

    public ChangeProductStatusRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ChangeProductStatusRequest request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken: cancellationToken);

        if (product is null) return Result.Failure(new List<string>() { "Unable to find the given product." });

        product.StatusId = request.StatusId;

        product.DomainEvents.Add(EntityUpdatedEvent.WithEntity(product));

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
