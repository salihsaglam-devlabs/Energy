using Energy.Shared.Models.V1.Common.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Api.Common.Filters;

/// <summary>
/// Her action argümanı için DI'dan ilgili <c>IValidator&lt;T&gt;</c>'yi çözüp çalıştıran
/// merkezi doğrulama filtresi. Doğrulama başarısızsa istek kısa devre yapılır ve
/// standart <see cref="BaseResponse{T}"/> zarfında (HTTP 400) lokalize hata listesi döner.
/// Controller'lar doğrulama mantığı içermez (spec §21).
/// </summary>
public sealed class FluentValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
                context.Result = new BadRequestObjectResult(
                    BaseResponse<object>.Failure("Validation failed.", errors));
                return;
            }
        }

        await next();
    }
}

