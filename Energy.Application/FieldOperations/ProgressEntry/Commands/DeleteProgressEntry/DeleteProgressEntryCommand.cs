using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.ProgressEntry.Commands.DeleteProgressEntry;

/// <summary>ProgressEntry kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProgressEntryCommand(Guid Id) : IRequest<BaseResponse<bool>>;
