using Dukkantek.Test.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Dukkantek.Test.Application.Products.Queries.GetProductsCountPerStatus;

public class GetProductsCountPerStatusQuery : IRequest<IEnumerable<ProductCountDto>>
{

}

public class GetProductsCountPerStatusQueryHandler : IRequestHandler<GetProductsCountPerStatusQuery, IEnumerable<ProductCountDto>>
{
    private readonly IApplicationDbContext _context;
    public GetProductsCountPerStatusQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductCountDto>> Handle(GetProductsCountPerStatusQuery request, CancellationToken cancellationToken)
    {
        var result = await _context
            .Products
            .Include(x => x.Status)
            .GroupBy(x => x.StatusId)
            .Select(g =>
                new ProductCountDto()
                {
                    Status = g.First().Status.Name,
                    Count = g.Count()
                }
            ).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}
