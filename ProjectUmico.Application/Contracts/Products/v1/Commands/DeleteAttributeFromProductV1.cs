using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;

namespace ProjectUmico.Application.Contracts.Products.v1.Commands;

public static class DeleteAttributeFromProductV1
{
    public record DeleteAttributeFromProduct([FromRoute]int productId,ICollection<int> attributeIds) : IRequest<bool>;
    
    
    public class DeleteAttributeFromProductHandler : IRequestHandler<DeleteAttributeFromProduct,bool>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DeleteAttributeFromProductHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteAttributeFromProduct request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                    // TODO bulk delete to increase performamce
                .Include(p=>p.Atributes) 
                .SingleOrDefaultAsync(p=>p.Id == request.productId,cancellationToken);

            if (product is null)
            {
                throw new NotFoundException(nameof(Product),nameof(Product.Id),request.productId);
            }

            foreach (var id in request.attributeIds)
            {
                var attribute = product.Atributes.SingleOrDefault(pa=>pa.Id == id);
                if (attribute != null)
                {
                    product.Atributes.Remove(attribute);
                }
                else
                    throw new NotFoundException(nameof(Product.Atributes), nameof(ProductAttribute.Id), id);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}