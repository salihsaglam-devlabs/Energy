using Energy.Application.Documents.Files.Services;
using MediatR;

namespace Energy.Application.Documents.Files.Queries.GetDocumentVersionContent;

/// <summary>Bir versiyonun indirilebilir dosya içeriğini getiren sorgu.</summary>
public sealed record GetDocumentVersionContentQuery(Guid VersionId)
    : IRequest<DocumentDownload?>;
