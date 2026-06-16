using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.ProgressEntry.Queries.GetProgressEntryById;

/// <summary>Kimliğe göre ProgressEntry detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProgressEntryByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProgressEntryDetailResponse>>;
