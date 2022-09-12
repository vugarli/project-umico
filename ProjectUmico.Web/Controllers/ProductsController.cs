using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Products.Queries;
using ProjectUmico.Web.Models.Products;

namespace ProjectUmico.Web.Controllers;
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // GET
    
    public async Task<IActionResult> Index(PaginationQuery query)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(query));
        var model = new ProductsViewModel(){Products = products};
        return View(model);
    }
}