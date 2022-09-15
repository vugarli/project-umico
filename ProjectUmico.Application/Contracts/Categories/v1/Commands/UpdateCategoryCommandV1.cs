using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.v1.Commands;

public static class UpdateCategoryCommandV1
{
    public record UpdateCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public int Id { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public int? ParentId { get; set; } = default!;
    }
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand,Result<CategoryDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        
        public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var oldCategory = await _dbContext.Categories.SingleOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

            if (oldCategory is null)
            {
                throw new NotFoundException(nameof(Category),nameof(Category.Id),request.Id);
            }
            
            // TODO update this part with mapper
            oldCategory.Name = request.CategoryName;
            if (oldCategory.ParentId != null)
            {
                oldCategory.ParentId = request.ParentId;
            }

            int result;
            try
            {
                result = await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException e) // in case of FK constraint violation for parent category id
            {
                throw new NotFoundException(nameof(Category),nameof(Category.Id),request.ParentId!);
            }

            if (result > 0)
            {
                var categoryDto = _mapper.Map<CategoryDto>(oldCategory);
                return Result<CategoryDto>.Success(categoryDto);
            }
            else return Result<CategoryDto>.Failure();
        }
    }
    
    
}