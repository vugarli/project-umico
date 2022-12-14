using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ProjectUmico.Application.Categories.Commands;
using ProjectUmico.Application.Categories.Queries;
using ProjectUmico.Application.Categories.v1.Commands;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Api.Controllers.v1;

public class CategoriesController : ApiControllerBasev1
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery query)
    {
        var categories = await _mediator.Send(new GetAllCategories(query));
        if (categories.Items.Count == 0)
        {
            return NoContent();
        }
        return Ok(categories);
    }
    
    [EntityTagFilter]
    [HttpGet("{id}",Name = "GetCategoryByIdV1")]
    public async Task<IActionResult> GetById(int id)
    {
        CategoryDto model;
        try
        {
            model = await _mediator.Send(new GetCategoryByIdQuery(id));
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }
        
        // TODO refactor this
        
        var cacheControlHeader = new CacheControlHeaderValue();
        cacheControlHeader.Private = true;
        cacheControlHeader.MaxAge = new TimeSpan(0, 10, 0);
        
        
        return Ok(model);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Edit(int id,UpdateCategoryCommandV1.UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        
        Result<CategoryDto> result;
        try
        {
            result = await _mediator.Send(command);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }
        
        return Ok(result.Value);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(DeleteCategoryCommandV1.DeleteCategoryCommand command)
    {
        Result<CategoryDto> result;
        try
        {
            result = await _mediator.Send(command);
        }
        catch (NotAllowedException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {

            return NotFound(e.Message);
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddCategoryCommandV1.AddCategoryCommand command)
    {
        Result<CategoryDto> result;
        try
        {
            result = await _mediator.Send(command);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }

        var uri = Url.Link("GetCategoryByIdV1",new{ id=result.Value?.CategoryId }) ?? "N/A";
        
        return Created(uri,result.Value);
    }
    
}