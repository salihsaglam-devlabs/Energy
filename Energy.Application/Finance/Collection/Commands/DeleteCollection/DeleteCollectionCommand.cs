using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Commands.DeleteCollection;

/// <summary>Collection kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteCollectionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
