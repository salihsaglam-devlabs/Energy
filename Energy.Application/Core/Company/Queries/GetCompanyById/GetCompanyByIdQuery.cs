using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Responses;
using MediatR;

namespace Energy.Application.Core.Company.Queries.GetCompanyById;

/// <summary>Kimliğe göre Company detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetCompanyByIdQuery(Guid Id)
    : IRequest<BaseResponse<CompanyDetailResponse>>;
