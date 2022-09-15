using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectUmico.Application.Categories.Commands;
using ProjectUmico.Application.Categories.Queries;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Api.Controllers.v1;

public class CategoriesController : ApiControllerBasev1
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(IMediator mediator,IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        return Ok(model);
    }
    
    [HttpPut]
    public async Task<IActionResult> Edit(CategoryDto model)
    {
        Result<CategoryDto> result;
        try
        {
            result = await _mediator.Send(new UpdateCategoryCommand(model));
        }
        catch (Exception e)
        {
            return BadRequest();
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }
        
        return Ok(result.Value);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Result<CategoryDto> result;
        try
        {
            result = await _mediator.Send(new DeleteCategoryCommand(id));
        }
        catch (NotAllowedException e)
        {

            return BadRequest();
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CategoryDto dto)
    {
        Result<CategoryDto> result;
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
            return BadRequest();
        }

        var uri = Url.Link("GetCategoryByIdV1",new{ id=result.Value?.CategoryId }) ?? "N/A";
        
        return Created(uri,result.Value);
    }
    
}