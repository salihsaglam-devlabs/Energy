using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SequenceDefinition.Commands.DeleteSequenceDefinition;

/// <summary>SequenceDefinition kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteSequenceDefinitionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
