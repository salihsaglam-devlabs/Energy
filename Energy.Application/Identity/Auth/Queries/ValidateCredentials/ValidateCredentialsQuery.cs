using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Auth.Queries.ValidateCredentials;

public sealed record ValidateCredentialsQuery(ValidateCredentialsRequest Request)
    : IRequest<BaseResponse<CredentialValidationResponse>>;
