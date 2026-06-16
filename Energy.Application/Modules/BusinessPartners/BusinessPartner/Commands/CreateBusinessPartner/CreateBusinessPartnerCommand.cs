using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Commands.CreateBusinessPartner;

/// <summary>Yeni BusinessPartner oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBusinessPartnerCommand(CreateBusinessPartnerRequest Request)
    : IRequest<BaseResponse<Guid>>;
