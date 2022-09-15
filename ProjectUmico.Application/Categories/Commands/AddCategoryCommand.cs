using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Commands;

public class AddCategoryCommand : IRequest<Result<CategoryDto>>
{
    public AddCategoryCommand(CategoryDto categoryDto)
    {
        CategoryDto = categoryDto;
    }

    public CategoryDto CategoryDto { get; }
}

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand,Result<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddCategoryCommandHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? parentCategory = null;
        if (request.CategoryDto.CategoryParentName != null)
        {
            parentCategory = await _context.Categories.SingleOrDefaultAsync(c=>c.Name == request.CategoryDto.CategoryParentName);
            
            if (parentCategory is null)
            {
                throw new NotFoundException("Specified Parent category not found");
            }
        }
        
        var category = new Category()
        {
            Name = request.CategoryDto.CategoryName,
            ParentId = parentCategory != null ? parentCategory.Id : null
        };
        
        _context.Categories.Add(category);

        var result = await _context.SaveChangesAsync(cancellationToken);
        
        if (result > 0)
        {
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Success(categoryDto);
        }
        else return Result<CategoryDto>.Failure();
    }
}