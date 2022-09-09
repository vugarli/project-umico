using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ProjectUmico.Application.Categories.Commands;
using ProjectUmico.Application.Categories.Queries;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Web.Models;

namespace ProjectUmico.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _mediator.Send(new GetAllCategories());
        
        var model = new CategoriesViewModel()
        {
            Categories = categories
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Result result;
        try
        {
            result = await _mediator.Send(new DeleteCategoryCommand(id));
        }
        catch (NotAllowedException e)
        {
            ViewData["ErrorDescription"] = e.Message;
            return View("Error");
        }

        if (!result.Succeded)
        {
            ViewData["ErrorDescription"] = "Failed";
            return View("Error");
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CategoryDto dto)
    {
        Result result;
        try
        {
            result = await _mediator.Send(new AddCategoryCommand(dto));
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }

        if (!result.Succeded)
        {
            ViewData["ErrorDescription"] = "Failed";
            return View("Error");
        }

        return RedirectToAction("Index");
    }
    
}