using Energy.Application.Modules.Documents.Files.Services;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersionContent;

/// <summary>Bir versiyonun indirilebilir dosya içeriğini getiren sorgu.</summary>
public sealed record GetDocumentVersionContentQuery(Guid VersionId)
    : IRequest<DocumentDownload?>;
