using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Commands.DeleteBusinessPartnerContact;

/// <summary>BusinessPartnerContact kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteBusinessPartnerContactCommand(Guid Id) : IRequest<BaseResponse<bool>>;
