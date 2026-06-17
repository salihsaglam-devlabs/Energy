using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Requests;
using MediatR;

namespace Energy.Application.Finance.Payable.Commands.CreatePayable;

/// <summary>Yeni Payable oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePayableCommand(CreatePayableRequest Request)
    : IRequest<BaseResponse<Guid>>;
