using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
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
    public async Task<IActionResult> GetAll([FromQuery]PaginationQuery query)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(query));
        if (products.Items.Count == 0)
        {
            return NoContent();
        }
        return Ok(products);
    }
    
    [HttpGet("{id}",Name = "GetProductByIdV1")]
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
    
    [HttpPut]
    public async Task<IActionResult> Edit(EditProductCommandV1.EditProductCommand command)
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

        var uri = Url.Link("GetProductByIdV1",new{ id=result.Value?.Id }) ?? "N/A";
        
        return Created(uri,result.Value);
    }    
    [HttpDelete]
    public async Task<IActionResult> Create(DeleteProductCommandV1.DeleteProductCommand command)
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
}