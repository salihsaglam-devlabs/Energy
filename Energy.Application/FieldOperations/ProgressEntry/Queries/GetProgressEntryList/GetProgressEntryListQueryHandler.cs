using Energy.Application.FieldOperations.ProgressEntry.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.ProgressEntry.Queries.GetProgressEntryList;

/// <summary>
/// <see cref="GetProgressEntryListQuery"/> handler'ı. <see cref="IProgressEntryService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressEntryListQueryHandler
    : IRequestHandler<GetProgressEntryListQuery, BaseResponse<PaginatedResponse<ProgressEntryListResponse>>>
{
    private readonly IProgressEntryService _service;

    public GetProgressEntryListQueryHandler(IProgressEntryService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>> Handle(
        GetProgressEntryListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
