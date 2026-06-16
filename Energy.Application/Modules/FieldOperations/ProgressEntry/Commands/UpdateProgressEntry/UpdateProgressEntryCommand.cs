using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.ProgressEntry.Commands.UpdateProgressEntry;

/// <summary>Var olan ProgressEntry kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProgressEntryCommand(Guid Id, UpdateProgressEntryRequest Request)
    : IRequest<BaseResponse<bool>>;
