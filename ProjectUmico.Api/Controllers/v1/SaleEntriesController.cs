using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Contracts.SaleEntries.v1.Commands;
using ProjectUmico.Application.Contracts.SaleEntries.v1.Queries;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Api.Controllers.v1;

[Authorize("RequiresCompanyTypeUser")]
public class SaleEntriesController : ApiControllerBasev1
{
    private readonly IMediator _mediator;

    public SaleEntriesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSaleEntry([FromBody] AddSaleEntryCommandV1.AddSaleEntryCommand command)
    {
        Result<CompanyProductSaleEntryDto> result;
        try
        {
            result = await _mediator.Send(command);
        }
        catch (Exception e)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetSaleEntries([FromQuery] PaginationQuery query)
    {
        PaginatedList<CompanyProductSaleEntryDto> saleEntries;

        saleEntries = await _mediator.Send(new GetAllSaleEntriesQueryV1.GetAllSaleEntriesQuery(query));
        
        return Ok(saleEntries);
    }
    
}