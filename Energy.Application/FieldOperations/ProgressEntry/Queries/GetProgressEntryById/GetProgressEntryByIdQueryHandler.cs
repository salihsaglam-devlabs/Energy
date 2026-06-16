using Energy.Application.FieldOperations.ProgressEntry.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.ProgressEntry.Queries.GetProgressEntryById;

/// <summary>
/// <see cref="GetProgressEntryByIdQuery"/> handler'ı. <see cref="IProgressEntryService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressEntryByIdQueryHandler
    : IRequestHandler<GetProgressEntryByIdQuery, BaseResponse<ProgressEntryDetailResponse>>
{
    private readonly IProgressEntryService _service;

    public GetProgressEntryByIdQueryHandler(IProgressEntryService service)
        => _service = service;

    public Task<BaseResponse<ProgressEntryDetailResponse>> Handle(
        GetProgressEntryByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
