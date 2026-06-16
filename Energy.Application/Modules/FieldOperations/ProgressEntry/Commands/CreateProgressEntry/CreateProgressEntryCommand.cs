using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.ProgressEntry.Commands.CreateProgressEntry;

/// <summary>Yeni ProgressEntry oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProgressEntryCommand(CreateProgressEntryRequest Request)
    : IRequest<BaseResponse<Guid>>;
