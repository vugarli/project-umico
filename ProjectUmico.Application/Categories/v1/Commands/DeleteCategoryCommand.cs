using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Result<CategoryDto>>
{
    public DeleteCategoryCommand(int id)
    {
        Id = id;
    }
    public int Id { get; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand,Result<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteCategoryCommandHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _context.Categories.SingleOrDefault(c => c.Id == request.Id);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category),"Id",request.Id);
        }

        _context.Categories.Remove(category);
        int result;
        try
        {
            result = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e) // should delete childs before parent
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

