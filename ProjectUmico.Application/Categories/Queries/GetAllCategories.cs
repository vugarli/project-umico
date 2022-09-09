using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Categories.Queries;

public class GetAllCategories : IRequest<ICollection<CategoryDto>>
{
    
}

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories,ICollection<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<CategoryDto>> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Include(c=>c.Parent).ToListAsync();
        return _mapper.Map<ICollection<CategoryDto>>(categories);
    }
}

