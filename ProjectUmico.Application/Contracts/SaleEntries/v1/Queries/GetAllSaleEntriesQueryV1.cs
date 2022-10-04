using AutoMapper;
using MediatR;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Contracts.SaleEntries.v1.Queries;

public class GetAllSaleEntriesQueryV1
{
    public record GetAllSaleEntriesQuery(PaginationQuery query) : 
        IRequest<PaginatedList<CompanyProductSaleEntryDto>>;
    
    
    
    public class GetAllSaleEntriesQueryHandler : IRequestHandler<GetAllSaleEntriesQuery,PaginatedList<CompanyProductSaleEntryDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllSaleEntriesQueryHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<CompanyProductSaleEntryDto>> Handle(GetAllSaleEntriesQuery request, CancellationToken cancellationToken)
        {
            // todo add global filter for this product sale entries
            var saleEntries = await _dbContext.CompanyProductSaleEntries
                .ToPaginatedListWithProjectionAsync<CompanyProductSaleEntry,CompanyProductSaleEntryDto>
                    (request.query.PageSize,request.query.PageNumber,_mapper);

            return saleEntries;
        }
    }
}