using Energy.Application.FieldOperations.DailySiteReportMaterial.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialLookup;

/// <summary>
/// <see cref="GetDailySiteReportMaterialLookupQuery"/> handler'ı. <see cref="IDailySiteReportMaterialLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportMaterialLookupQueryHandler
    : IRequestHandler<GetDailySiteReportMaterialLookupQuery, BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>>
{
    private readonly IDailySiteReportMaterialLookupService _lookup;

    public GetDailySiteReportMaterialLookupQueryHandler(IDailySiteReportMaterialLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>> Handle(
        GetDailySiteReportMaterialLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
