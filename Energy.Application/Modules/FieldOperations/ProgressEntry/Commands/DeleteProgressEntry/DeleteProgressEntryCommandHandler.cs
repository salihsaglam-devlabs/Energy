using Energy.Application.Modules.FieldOperations.ProgressEntry.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.ProgressEntry.Commands.DeleteProgressEntry;

/// <summary>
/// <see cref="DeleteProgressEntryCommand"/> handler'ı. <see cref="IProgressEntryService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProgressEntryCommandHandler
    : IRequestHandler<DeleteProgressEntryCommand, BaseResponse<bool>>
{
    private readonly IProgressEntryService _service;

    public DeleteProgressEntryCommandHandler(IProgressEntryService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProgressEntryCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
