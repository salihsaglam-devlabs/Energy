using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.ProgressEntry.Queries.GetProgressEntryList;

/// <summary>Sayfalanmış ProgressEntry listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProgressEntryListQuery(GetProgressEntryListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>>;
