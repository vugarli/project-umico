﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Products.Commands;

public static class AddProductCommandV1
{
    public record AddProductCommand(): IRequest<Result<ProductDto>>
    {
        public string Sku { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }

        public List<int>? Attributes { get; set; } = default;

        public int CategoryId { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand,Result<ProductDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product();
        
            _mapper.Map(request,newProduct);

            if (request.Attributes != null && request.Attributes.Count() > 0 )
            {
                var attributes = new List<ProductAtribute>();
                foreach (var id in request.Attributes)
                {
                    var attribute = new ProductAtribute() {Id = id};
                    attributes.Add(attribute);
                }
                newProduct.Atributes = attributes;
            }

            _dbContext.Products.Attach(newProduct);

            var result= await _dbContext.SaveChangesAsync(cancellationToken);
        
            if (result > 0)
            {
                var productDto = _mapper.Map<ProductDto>(newProduct);
                return Result<ProductDto>.Success(productDto);
            }
            else return Result<ProductDto>.Failure();

        }
    }
}