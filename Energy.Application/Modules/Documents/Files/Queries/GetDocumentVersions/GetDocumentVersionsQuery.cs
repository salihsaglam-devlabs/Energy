using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersions;

/// <summary>Bir belgenin dosya versiyon geçmişini getiren sorgu.</summary>
public sealed record GetDocumentVersionsQuery(Guid DocumentId)
    : IRequest<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>;
