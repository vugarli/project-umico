using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.v1.Commands;

public static class DeleteCategoryCommandV1
{
    public record DeleteCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public int Id { get; set; }
    }
    
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand,Result<CategoryDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Result<CategoryDto>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(c => c.Id == request.Id,cancellationToken);
            if (category is null)
            {
                throw new NotFoundException(nameof(Category),nameof(Category.Id),request.Id);
            }

            _dbContext.Categories.Remove(category);

            int result;
            try
            {
                result = await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException e) // should delete children before parent
            {
                throw new NotAllowedException("Unable to delete category. Delete child categories first.");
            }

            if (result > 0)
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Result<CategoryDto>.Success(categoryDto);
            }
            else return Result<CategoryDto>.Failure();
        }
    }
    
    
    
}