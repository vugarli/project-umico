using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Contracts.Products.v1.Commands;
using ProjectUmico.Domain.Exceptions;
using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Application.Common.Validators;

public class AddAttributeToProductCommandValidator : AbstractValidator<AddAttributeToProductCommandV1.AddAttributeToProductCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public AddAttributeToProductCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        RuleFor(a => a.productId).NotNull()
            .WithMessage("ProductId can't be null!");

        RuleFor(c => c.attribute.ParentAttributeId).Null()
            .When(a => a.attribute.AttributeType is AttributeType.AttributeGroup)
            .WithMessage("AttributeGroup can't have parent!");

        RuleFor(c => c.attribute.ParentAttributeId).NotNull()
            .When(a => a.attribute.AttributeType is AttributeType.Attribute)
            .WithMessage("Attribute must have parent!");

        // attributdusa ve parentin Id si null deyilse => Attributun ancaq AttributGroup parenti ola biler
        RuleFor(a => a)
            .MustAsync(AttributeParentMustBeAttributeGroup)
            .When(a => a.attribute.AttributeType is AttributeType.Attribute && a.attribute.ParentAttributeId != null);
    }

    private async Task<bool> AttributeParentMustBeAttributeGroup(
        AddAttributeToProductCommandV1.AddAttributeToProductCommand command, CancellationToken cancellationToken)
    {
        var parentAttribute =
            await _dbContext.Attributes.SingleOrDefaultAsync(a => a.Id == command.attribute.ParentAttributeId,
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