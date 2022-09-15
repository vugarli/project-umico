using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Contracts.Attributes.v1.Commands;
using ProjectUmico.Application.Contracts.Attributes.v1.Queries;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Api.Controllers.v1;

public class AttributesController : ApiControllerBasev1
{
    private readonly IMediator _mediator;

    public AttributesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]PaginationQuery query)
    {
        var attributes = await _mediator.Send(new GetAllAttributesQueryV1.GetAllAttributesQuery(query));
        if (!attributes.Items.Any())
        {
            return NoContent();
        }
        
        return Ok(attributes);
    }
    
    [HttpGet("{id}",Name = "GetAttributeByIdV1")]
    public async Task<IActionResult> GetById(int id)
    {
        AttributeDto attributeDto;
        try
        {
            attributeDto = await _mediator.Send(new GetAttributeByIdQueryV1.GetAttributeByIdQuery(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        return Ok(attributeDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAttribute([FromBody]AddAttributeCommandV1.AddAttributeCommand command)
    {
        Result<AttributeDto> result;

        try
        {
            result = await _mediator.Send(command);
        }
        catch (DbUpdateException e)
        {
            return BadRequest();
        }
        
        if (!result.Succeded)
        {
            return BadRequest();
        }

        var url = Url.Link("GetAttributeByIdV1", new {id = result.Value?.Id}) ?? "N/A";
        return Created(url,result.Value);
    }    
    
    [HttpPut]
    public async Task<IActionResult> AddAttribute([FromBody]UpdateAttributeCommandV1.UpdateAttributeCommand command)
    {
        Result<AttributeDto> result;

        try
        {
            result = await _mediator.Send(command);
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
    
    [HttpDelete]
    public async Task<IActionResult> AddAttribute(int id)
    {
        Result<AttributeDto> result;

        try
        {
            result = await _mediator.Send(new DeleteAttributeCommandV1.DeleteAttributeCommand(id));
        }
        catch (DbUpdateException e)
        {
            return BadRequest();
        }
        
        if (!result.Succeded)
        {
            return BadRequest();
        }
        return Ok(result.Value);
    }
    
    
    
}