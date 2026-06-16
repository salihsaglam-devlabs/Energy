using FluentValidation;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Requests;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Validators;

/// <summary>CreateApiEndpointRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApiEndpointRequestValidator : AbstractValidator<CreateApiEndpointRequest>
{
    public CreateApiEndpointRequestValidator()
    {
        RuleFor(x => x.Path).NotEmpty();
        RuleFor(x => x.HttpMethod).NotEmpty();
    }
}
