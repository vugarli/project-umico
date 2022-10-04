using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models;
using umico.Models.Rating;

namespace ProjectUmico.Application.Contracts.Products.v1.Commands;

public static class AddRatingToProductCommandV1
{
    public record AddRatingToProductCommand(string Comment, int Rate, int ProductId)
        :IRequest<Result<ProductRatingDto>>;

    public class AddRatingToProductCommandHandler 
        : IRequestHandler<AddRatingToProductCommand,Result<ProductRatingDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public AddRatingToProductCommandHandler
            (IApplicationDbContext dbContext,IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<Result<ProductRatingDto>> Handle(AddRatingToProductCommand request, CancellationToken cancellationToken)
        {
            var product = await 
                _dbContext
                    .Products
                    .SingleOrDefaultAsync(p => p.Id == request.ProductId,cancellationToken);
            if (product is null) 
                throw new NotFoundException(nameof(Product), nameof(Product.Id), request.ProductId);

            var rating = _mapper.Map<AddRatingToProductCommand, ProductRating>(request);
            
            rating.RatedUserId = _currentUserService.UserId!; // move to mapper?
            
            product.ProductRatings = new List<ProductRating>()
            {
                rating
            };
            
            _dbContext.Ratings.Add(rating);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<ProductRatingDto>.Success();
        }
    }
}