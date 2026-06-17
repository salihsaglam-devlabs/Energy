using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Requests;
using MediatR;

namespace Energy.Application.Core.Branch.Commands.CreateBranch;

/// <summary>Yeni Branch oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBranchCommand(CreateBranchRequest Request)
    : IRequest<BaseResponse<Guid>>;
