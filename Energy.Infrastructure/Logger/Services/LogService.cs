using Energy.Application.Logger.Services;
using Energy.Domain.Logger;
using Energy.Infrastructure.Persistence;

namespace Energy.Infrastructure.Logger.Services;

public sealed class LogService(AppDbContext dbContext) : ILogService
{
    public async Task CreateAsync(
        Log log,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Logs.AddAsync(log, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}