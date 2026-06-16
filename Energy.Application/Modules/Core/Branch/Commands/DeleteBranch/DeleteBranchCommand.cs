using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Commands.DeleteBranch;

/// <summary>Branch kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteBranchCommand(Guid Id) : IRequest<BaseResponse<bool>>;
