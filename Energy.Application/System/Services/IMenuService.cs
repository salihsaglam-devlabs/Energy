using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.System.Services;

public interface IMenuService
{
    Task<PaginatedResponse<MenuResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);
    Task<MenuResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MenuResponse> CreateAsync(CreateMenuRequest request, CancellationToken cancellationToken = default);
    Task<MenuResponse> UpdateAsync(Guid id, UpdateMenuRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Returns the visible menu tree for the supplied user (or anonymous if null).</summary>
    Task<IReadOnlyList<MenuTreeNodeResponse>> GetTreeForUserAsync(Guid? userId, CancellationToken cancellationToken = default);
}
