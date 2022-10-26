using Dukkantek.Test.Application.Common.Models;
using Dukkantek.Test.Application.Products.Commands.ChangeProductStatus;
using Dukkantek.Test.Application.Products.Queries.GetProductsCountPerStatus;
using Dukkantek.Test.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Dukkantek.Test.UI.API.Controllers;

public class ProductsController : ApiControllerBase
{
    [HttpGet("get-count")]
    public async Task<IEnumerable<ProductCountDto>> GetAsync()
    {
        return await Mediator.Send(new GetProductsCountPerStatusQuery());
    }

    [HttpPost("update-status")]
    public async Task<Result> UpdateAsync(ChangeProductStatusRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("sold")]
    public async Task<Result> SoldProductAsync(int productId)
    {
        return await Mediator.Send(new ChangeProductStatusRequest() { ProductId = productId, StatusId = (short)ProductStatus.Sold });
    }
}
