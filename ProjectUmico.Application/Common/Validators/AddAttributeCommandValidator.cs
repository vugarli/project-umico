using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Contracts.Attributes.v1.Commands;
using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Application.Common.Validators;

public class AddAttributeCommandValidator : AbstractValidator<AddAttributeCommandV1.AddAttributeCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public AddAttributeCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        
        RuleFor(c => c.ParentAttributeId).Null()
            .When(a => a.AttributeType is AttributeType.AttributeGroup)
            .WithMessage("AttributeGroup can't have parent!");

        RuleFor(c => c.ParentAttributeId).NotNull()
            .When(a => a.AttributeType is AttributeType.Attribute)
            .WithMessage("Attribute must have parent!");

        // attributdusa ve parentin Id si null deyilse => Attributun ancaq AttributGroup parenti ola biler
        RuleFor(a => a)
            .MustAsync(AttributeParentMustBeAttributeGroup)
            .When(a => a.AttributeType is AttributeType.Attribute && a.ParentAttributeId != null);
    }
    private async Task<bool> AttributeParentMustBeAttributeGroup(
        AddAttributeCommandV1.AddAttributeCommand command, CancellationToken cancellationToken)
    {
        var parentAttribute =
            await _dbContext.Attributes.SingleOrDefaultAsync(a => a.Id == command.ParentAttributeId,
                cancellationToken);

        if (parentAttribute is null)
        {
            return false;
        }

        if (parentAttribute.AttributeType is AttributeType.Attribute)
        {
            return false;
        }

        return true;
    }
}