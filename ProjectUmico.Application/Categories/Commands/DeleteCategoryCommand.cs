using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Result>
{
    public DeleteCategoryCommand(int id)
    {
        Id = id;
    }
    public int Id { get; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand,Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
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

        return result > 0 ? Result.Success() : Result.Failure();
    }
}

