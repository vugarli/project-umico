using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjectUmico.Api.Common.Helpers;

public static class ValidationExceptionExtension
{

    public static ValidationFailedResult ToValidationFailedResult(this FluentValidation.ValidationException exception,ModelStateDictionary modelState)
    {
        foreach (var error in exception.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
        return new ValidationFailedResult(modelState);
    }
    
}