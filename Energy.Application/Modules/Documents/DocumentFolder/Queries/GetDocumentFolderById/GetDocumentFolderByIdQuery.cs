using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderById;

/// <summary>Kimliğe göre DocumentFolder detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDocumentFolderByIdQuery(Guid Id)
    : IRequest<BaseResponse<DocumentFolderDetailResponse>>;
