using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Requests;
using MediatR;

namespace Energy.Application.Finance.Collection.Commands.CreateCollection;

/// <summary>Yeni Collection oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateCollectionCommand(CreateCollectionRequest Request)
    : IRequest<BaseResponse<Guid>>;
