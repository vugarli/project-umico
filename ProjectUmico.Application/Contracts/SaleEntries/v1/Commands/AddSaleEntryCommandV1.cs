using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Enums;
using umico.Models;

namespace ProjectUmico.Application.Contracts.SaleEntries.v1.Commands;

public static class AddSaleEntryCommandV1
{
    public class AddSaleEntryCommand : IRequest<Result<CompanyProductSaleEntryDto>>
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public SaleEntryTypes SaleEntryType { get; set; } = SaleEntryTypes.Normal;
        public int? PromotionId { get; set; }
    }
    
    public class AddSaleEntryCommandHandler : IRequestHandler<AddSaleEntryCommand,Result<CompanyProductSaleEntryDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public AddSaleEntryCommandHandler(IApplicationDbContext dbContext,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<Result<CompanyProductSaleEntryDto>> Handle(AddSaleEntryCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext
                .Products
                .SingleOrDefaultAsync(p => p.Id == request.ProductId,cancellationToken);

            if (product is null)
            {
                return Result<CompanyProductSaleEntryDto>
                        .Failure(new NotFoundException(nameof(Product), nameof(Product.Id), request.ProductId));
            }

            var saleEntry = _mapper.Map<CompanyProductSaleEntry>(request);

            saleEntry.CompanyId = _currentUserService.UserId!;

            _dbContext.CompanyProductSaleEntries.Add(saleEntry);
            
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            
            if (result > 0)
            {
                return Result<CompanyProductSaleEntryDto>.Success();
            }
            else return Result<CompanyProductSaleEntryDto>.Failure();

        }
    }
}