using AutoMapper;
using MediatR;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Products.Commands;

public class AddProductCommand : IRequest<Result>
{
    public ProductDto Product { get; set; }

    public AddProductCommand(ProductDto product)
    {
        Product = product;
    }
}
public class AddProductCommandHandler : IRequestHandler<AddProductCommand,Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public AddProductCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new Product();
        
        _mapper.Map(request.Product,newProduct);

        _dbContext.Products.Add(newProduct);
        var result= await _dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0 ? Result.Success() : Result.Failure();

    }
}