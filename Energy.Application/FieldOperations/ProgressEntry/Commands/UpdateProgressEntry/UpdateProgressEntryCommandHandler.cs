using Energy.Application.FieldOperations.ProgressEntry.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.ProgressEntry.Commands.UpdateProgressEntry;

/// <summary>
/// <see cref="UpdateProgressEntryCommand"/> handler'ı. <see cref="IProgressEntryService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProgressEntryCommandHandler
    : IRequestHandler<UpdateProgressEntryCommand, BaseResponse<bool>>
{
    private readonly IProgressEntryService _service;

    public UpdateProgressEntryCommandHandler(IProgressEntryService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProgressEntryCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
