using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ProjectUmico.Application.Categories.Commands;
using ProjectUmico.Application.Categories.Queries;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Web.Models;
using ProjectUmico.Web.ViewModels.Categories;

namespace ProjectUmico.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(IMediator mediator,IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        CategoryDto model;
        try
        {
            model = await _mediator.Send(new GetCategoryByIdQuery(id));
        }
        catch (NotFoundException e)
        {
            ViewData["ErrorDescription"] = e.Message;
            return View("Error");
        }
        
        return View("EditCategory",model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(CategoryDto model)
    {
        Result result;
        try
        {
            result = await _mediator.Send(new UpdateCategoryCommand(model));
        }
        catch (Exception e)
        {
            ViewData["ErrorDescription"] = e.Message;
            return View("Error");
        }

        if (!result.Succeded)
        {
            ViewData["ErrorDescription"] = "Failed";
            return View("Error");
        }
        
        return View("EditCategory",model);
    }

    public async Task<IActionResult> Index([FromQuery] PaginationQuery query)
    {
        var categories = await _mediator.Send(new GetAllCategories(query));
        
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