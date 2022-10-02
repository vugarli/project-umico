using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Products.Queries;

public static class GetAllProductRatingsQueryV1
{
    public record GetAllProductRatingsQuery(int productId) : IRequest<ICollection<ProductRatingDto>>;
    
    public class GetAllProductRatingsQueryHandler : IRequestHandler<GetAllProductRatingsQuery,ICollection<ProductRatingDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllProductRatingsQueryHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        
        public async Task<ICollection<ProductRatingDto>> Handle(GetAllProductRatingsQuery request, CancellationToken cancellationToken)
        {
            var product = await _dbContext
                .Products.Include(p=>p.ProductRatings)
                .SingleOrDefaultAsync(p => p.Id == request.productId, cancellationToken);
            if (product is null)
            {
                throw new NotFoundException(nameof(Product),nameof(Product.Id),request.productId);
            }
            
            var ratings = _mapper.Map<ICollection<ProductRatingDto>>(product.ProductRatings);
            
            return ratings;
        }
    }
}