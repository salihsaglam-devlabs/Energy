using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Department.Commands.DeleteDepartment;

/// <summary>Department kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDepartmentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
