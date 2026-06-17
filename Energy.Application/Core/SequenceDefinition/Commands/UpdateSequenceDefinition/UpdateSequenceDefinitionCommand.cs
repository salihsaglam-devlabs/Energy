using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Commands.UpdateSequenceDefinition;

/// <summary>Var olan SequenceDefinition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateSequenceDefinitionCommand(Guid Id, UpdateSequenceDefinitionRequest Request)
    : IRequest<BaseResponse<bool>>;
