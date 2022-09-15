using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Products.v1.Commands;

public static class DeleteProductCommandV1
{
    public record DeleteProductCommand : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
    }
    
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand,Result<ProductDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public DeleteProductCommandHandler(IMapper mapper,IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<Result<ProductDto>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == request.Id,cancellationToken);
            
            if (product is null)
            {
                throw new NotFoundException();
            }

            _dbContext.Products.Remove(product);
            
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            
            if (result > 0)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                return Result<ProductDto>.Success(productDto);
            }
            else return Result<ProductDto>.Failure();

        }
    }
}