using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Commands.DeleteBusinessPartner;

/// <summary>BusinessPartner kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteBusinessPartnerCommand(Guid Id) : IRequest<BaseResponse<bool>>;
