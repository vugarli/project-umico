using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Products.Commands;

public class EditProductCommand : IRequest<Result<ProductDto>>
{
    public ProductDto Product { get;}
    
    public EditProductCommand(ProductDto product)
    {
        this.Product = product;
    }
}
public class EditProductCommandHandler : IRequestHandler<EditProductCommand,Result<ProductDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EditProductCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<Result<ProductDto>> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var oldProduct = await _dbContext.Products.SingleOrDefaultAsync(p=>p.Id == request.Product.Id);

        if (oldProduct is null)
        {
            throw new NotFoundException(nameof(Product),nameof(Product.Id),request.Product.Id);
        }
        // TODO update this part with mapper
        oldProduct.Name = request.Product.Name;
        oldProduct.SKU = request.Product.SKU;

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        
        if (result > 0)
        {
            var productDto = _mapper.Map<ProductDto>(oldProduct);
            return Result<ProductDto>.Success(productDto);
        }
        else return Result<ProductDto>.Failure();
    }
}