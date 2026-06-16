using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentLookup;

/// <summary>
/// <see cref="GetDailySiteReportEquipmentLookupQuery"/> handler'ı. <see cref="IDailySiteReportEquipmentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportEquipmentLookupQueryHandler
    : IRequestHandler<GetDailySiteReportEquipmentLookupQuery, BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>>
{
    private readonly IDailySiteReportEquipmentLookupService _lookup;

    public GetDailySiteReportEquipmentLookupQueryHandler(IDailySiteReportEquipmentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>> Handle(
        GetDailySiteReportEquipmentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
