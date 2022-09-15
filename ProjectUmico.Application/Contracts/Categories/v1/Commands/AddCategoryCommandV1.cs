using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Commands;

public static class AddCategoryCommandV1
{
    public record AddCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public string CategoryName { get; set; } = default!;
        public int? ParentId { get; set; } = default!;
    }
    
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand,Result<CategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public AddCategoryCommandHandler(IMapper mapper,IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public async Task<Result<CategoryDto>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? parentCategory = null;
            if (request.ParentId != null)
            {
                parentCategory = await _dbContext.Categories.SingleOrDefaultAsync(c=>c.Id == request.ParentId,cancellationToken);
            
                if (parentCategory is null)
                {
                    throw new NotFoundException("Specified Parent category not found");
                }
            }
        
            var category = new Category()
            {
                Name = request.CategoryName,
                ParentId = parentCategory != null ? parentCategory.Id : null
            };
        
            _dbContext.Categories.Add(category);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
        
            if (result > 0)
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Result<CategoryDto>.Success(categoryDto);
            }
            else return Result<CategoryDto>.Failure();
        }
    }
}