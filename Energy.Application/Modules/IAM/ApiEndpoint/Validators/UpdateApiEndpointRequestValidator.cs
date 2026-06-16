using FluentValidation;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Requests;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Validators;

/// <summary>UpdateApiEndpointRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApiEndpointRequestValidator : AbstractValidator<UpdateApiEndpointRequest>
{
    public UpdateApiEndpointRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
        RuleFor(x => x.HttpMethod).NotEmpty();
    }
}
