using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectUmico.Api.Common.Helpers;
using ProjectUmico.Application.Common;
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
    public async Task<IActionResult> Index([FromQuery] PaginationQuery query)
    {
        Result<PaginatedList<AttributeDto>> result;
        
        try
        {
            result = await _mediator.Send(new GetAllAttributesQueryV1.GetAllAttributesQuery(query));
        }
        catch (FluentValidation.ValidationException e)
        {
            return e.ToValidationFailedResult(ModelState);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        return result.Match(() =>
        {
            if (!result.Value!.Items.Any())
            {
                return NoContent();
            }
            return Ok(result.Value);
        }, exception => StatusCode(StatusCodes.Status500InternalServerError));
    }

    [HttpGet("{id}", Name = "GetAttributeByIdV1")]
    public async Task<IActionResult> GetById(int id)
    {
        Result<AttributeDto> result;
        try
        {
            result = await _mediator.Send(new GetAttributeByIdQueryV1.GetAttributeByIdQuery(id));
        }
        catch (FluentValidation.ValidationException exception)
        {
            return exception.ToValidationFailedResult(ModelState);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return result.Match(() =>
                Ok(result.Value),
            exception =>
            {
                if (exception is NotFoundException)
                {
                    return NotFound(exception.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPost]
    public async Task<IActionResult> AddAttribute([FromBody] AddAttributeCommandV1.AddAttributeCommand command)
    {
        Result<AttributeDto> result;

        try
        {
            result = await _mediator.Send(command);
        }
        catch (FluentValidation.ValidationException e)
        {
            return e.ToValidationFailedResult(ModelState);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return result.Match(() =>
        {
            var url = Url.Link("GetAttributeByIdV1", new {id = result.Value?.Id}) ?? "N/A";
            return Created(url, result.Value); 
        }, exception =>
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAttribute(int id,
        [FromBody] UpdateAttributeCommandV1.UpdateAttributeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        Result<AttributeDto> result;

        try
        {
            result = await _mediator.Send(command);
        }
        catch (FluentValidation.ValidationException exception)
        {
            return exception.ToValidationFailedResult(ModelState);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return result.Match(() =>
        {
            return Ok(result.Value);
        }, exception =>
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAttribute(int id)
    {
        Result<AttributeDto> result;

        try
        {
            result = await _mediator.Send(new DeleteAttributeCommandV1.DeleteAttributeCommand(id));
        }
        catch (FluentValidation.ValidationException exception)
        {
            return exception.ToValidationFailedResult(ModelState);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return result.Match(() =>
        {
            return
                Ok(result.Value); // TODO consider to return NoContent https://stackoverflow.com/questions/25970523/restful-what-should-a-delete-response-body-contain

        }, exception =>
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }); 
    }
}