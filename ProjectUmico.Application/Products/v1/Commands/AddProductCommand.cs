// using AutoMapper;
// using MediatR;
// using ProjectUmico.Application.Common.Interfaces;
// using ProjectUmico.Application.Common.Models;
// using ProjectUmico.Application.Dtos;
// using umico.Models;
//
// namespace ProjectUmico.Application.Products.Commands;
//
// public class AddProductCommand : IRequest<Result<ProductDto>>
// {
//     public ProductDto Product { get; set; }
//
//     public AddProductCommand(ProductDto product)
//     {
//         Product = product;
//     }
// }
// public class AddProductCommandHandler : IRequestHandler<AddProductCommand,Result<ProductDto>>
// {
//     private readonly IApplicationDbContext _dbContext;
//     private readonly IMapper _mapper;
//
//     public AddProductCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
//     {
//         _dbContext = dbContext;
//         _mapper = mapper;
//     }
//     public async Task<Result<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
//     {
//         var newProduct = new Product();
//         
//         _mapper.Map(request.Product,newProduct);
//
//         _dbContext.Products.Add(newProduct);
//         var result= await _dbContext.SaveChangesAsync(cancellationToken);
//         
//         if (result > 0)
//         {
//             var productDto = _mapper.Map<ProductDto>(newProduct);
//             return Result<ProductDto>.Success(productDto);
//         }
//         else return Result<ProductDto>.Failure();
//
//     }
// }