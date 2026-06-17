using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Queries.GetSequenceDefinitionList;

/// <summary>Sayfalanmış SequenceDefinition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetSequenceDefinitionListQuery(GetSequenceDefinitionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>>;
