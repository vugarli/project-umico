using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;


namespace ProjectUmico.Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result<CategoryDto>>
{
    public readonly CategoryDto Category;

    public UpdateCategoryCommand(CategoryDto category)
    {
        Category = category;
    }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand,Result<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var oldCategory = await _context.Categories.Include(c=>c.Parent).SingleOrDefaultAsync(c => c.Id == request.Category.CategoryId,cancellationToken);

        if (oldCategory is null)
        {
            throw new NotFoundException(nameof(CategoryDto),nameof(CategoryDto.CategoryId),request.Category.CategoryId);
        }
        // TODO update this part with mapper
        oldCategory.Name = request.Category.CategoryName;
        if (oldCategory.Parent != null)
        {
            oldCategory.Parent.Name = request.Category.CategoryParentName;
        }
        
        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            var categoryDto = _mapper.Map<CategoryDto>(oldCategory);
            return Result<CategoryDto>.Success(categoryDto);
        }
        else return Result<CategoryDto>.Failure();
    }
}