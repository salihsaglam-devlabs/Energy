using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Queries.GetSequenceDefinitionById;

/// <summary>Kimliğe göre SequenceDefinition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetSequenceDefinitionByIdQuery(Guid Id)
    : IRequest<BaseResponse<SequenceDefinitionDetailResponse>>;
