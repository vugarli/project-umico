using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public int Id { get;}

    public GetCategoryByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery,CategoryDto>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public GetCategoryByIdQueryHandler(IMapper mapper,IApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
    
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category =await _dbContext.Categories.Include(c=>c.Parent).SingleOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(Category),"Id",request.Id);
        }

        return _mapper.Map<CategoryDto>(category);
    }
}

