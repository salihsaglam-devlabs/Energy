using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Requests;
using MediatR;

namespace Energy.Application.Core.Branch.Commands.UpdateBranch;

/// <summary>Var olan Branch kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBranchCommand(Guid Id, UpdateBranchRequest Request)
    : IRequest<BaseResponse<bool>>;
