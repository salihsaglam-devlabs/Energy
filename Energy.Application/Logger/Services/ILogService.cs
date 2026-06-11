using Energy.Domain.Logger;

namespace Energy.Application.Logger.Services;

public interface ILogService
{
    Task CreateAsync(
        Log log,
        CancellationToken cancellationToken = default);
}