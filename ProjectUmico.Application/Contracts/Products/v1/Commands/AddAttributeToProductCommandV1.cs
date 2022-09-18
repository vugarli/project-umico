﻿using System.ComponentModel;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts.Attributes.v1.Commands;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;

namespace ProjectUmico.Application.Contracts.Products.v1.Commands;

public static class AddAttributeToProductCommandV1
{
    public record AddAttribute
    {
        public int? Id { get; set; }
        public string Value { get; set; }
        public AttributeType AttributeType { get; set; }
        public int? ParentAttributeId { get; set; } = default;
    }

    public record AddAttributeToProductCommand(int productId,AddAttribute attribute) : IRequest<Result<AttributeDto>>;

    public class
        AddAttributeToProductCommandHandler : IRequestHandler<AddAttributeToProductCommand, Result<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddAttributeToProductCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<AttributeDto>> Handle(AddAttributeToProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == request.productId);
            if (product is null) throw new NotFoundException(nameof(Product), nameof(Product.Id), request.productId);

            var attribute = _mapper.Map<ProductAttribute>(request);
            
            
            product.Atributes = new(){attribute};
            _dbContext.Products.Attach(product);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                var attributeDto = _mapper.Map<AttributeDto>(attribute);
                return Result<AttributeDto>.Success(attributeDto);
            }
            else return Result<AttributeDto>.Failure();
        }
    }
}