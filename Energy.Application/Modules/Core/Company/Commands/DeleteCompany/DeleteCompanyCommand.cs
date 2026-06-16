using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Company.Commands.DeleteCompany;

/// <summary>Company kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteCompanyCommand(Guid Id) : IRequest<BaseResponse<bool>>;
