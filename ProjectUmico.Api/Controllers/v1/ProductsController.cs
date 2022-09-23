using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Contracts.Products.v1.Commands;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Application.Products.Commands;
using ProjectUmico.Application.Products.Queries;
using ProjectUmico.Application.Products.v1.Commands;

namespace ProjectUmico.Api.Controllers.v1;

public class ProductsController : ApiControllerBasev1
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(query));
        if (products.Items.Count == 0)
        {
            return NoContent();
        }

        return Ok(products);
    }

    [HttpGet("{id}", Name = "GetProductByIdV1")]
    public async Task<IActionResult> GetById(int id)
    {
        ProductDto productDto;

        try
        {
            productDto = await _mediator.Send(new GetProductByIdQuery(id));
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }

        return Ok(productDto);
    }

    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Edit([FromRoute]int productId, [FromBody] UpdateProductCommandV1.UpdateProductCommand command)
    {
        if (productId != command.Id)
        {
            return BadRequest();
        }

        Result<ProductDto> result;

        try
        {
            result = await _mediator.Send(command);
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddProductCommandV1.AddProductCommand command)
    {
        Result<ProductDto> result;

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

        var uri = Url.Link("GetProductByIdV1", new {id = result.Value?.Id}) ?? "N/A";

        return Created(uri, result.Value);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteProductCommandV1.DeleteProductCommand command)
    {
        Result<ProductDto> result;

        try
        {
            result = await _mediator.Send(command);
        }
        catch (NotFoundException e)
        {
            return NotFound();
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }

        return Ok(result.Value);
    }


    [HttpPost("{productId:int}/attributes")]
    public async Task<IActionResult> AddAttributeToProduct([FromRoute] int productId,
        [FromBody] AddAttributeToProductCommandV1.AddAttributev2 command)
    {
        Result<AttributeDto> result;
        try
        {
            result = await _mediator.Send(
                new AddAttributeToProductCommandV1.AddAttributeToProductCommand(productId, command));
        }
        catch (DbUpdateException e)
        {
            return BadRequest();
        }
        catch (FluentValidation.ValidationException e)
        {
            return BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (!result.Succeded)
        {
            return BadRequest();
        }

        var url = Url.Link("GetAttributeByIdV1", new {id = result.Value?.Id}) ?? "N/A";
        return Created(url, result.Value);
    }
    
    [HttpGet("{productId:int}/attributes")]
    public async Task<IActionResult> GetAttributesForProduct([FromRoute] int productId,
        [FromQuery] PaginationQuery query)
    {
        PaginatedList<AttributeDto> result;
        try
        {
            result = await _mediator.Send(
                new GetAllAttributesOfProductQueryV1.GetAllAttributesOfProductQuery(productId,query));
        }
        catch (DbUpdateException e)
        {
            return BadRequest();
        }
        catch (FluentValidation.ValidationException e)
        {
            return BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Ok(result);
    }    
    
    [HttpDelete("{productId:int}/attributes")]
    public async Task<IActionResult> GetAttributesForProduct(
        [FromBody] DeleteAttributeFromProductV1.DeleteAttributeFromProduct command)
    {
        bool result;
        try
        {
            result = await _mediator.Send(command);
        }
        catch (DbUpdateException e)
        {
            return BadRequest();
        }
        catch (FluentValidation.ValidationException e)
        {
            return BadRequest();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return NoContent();
    }
}