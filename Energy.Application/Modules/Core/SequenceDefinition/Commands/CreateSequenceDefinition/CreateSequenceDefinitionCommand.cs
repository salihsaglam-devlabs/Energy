using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.SequenceDefinition.Commands.CreateSequenceDefinition;

/// <summary>Yeni SequenceDefinition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateSequenceDefinitionCommand(CreateSequenceDefinitionRequest Request)
    : IRequest<BaseResponse<Guid>>;
