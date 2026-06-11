using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.System.Services;

public interface IApiEndpointService
{
    Task<PaginatedResponse<ApiEndpointResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);
    Task<ApiEndpointResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiEndpointResponse> CreateAsync(CreateApiEndpointRequest request, CancellationToken cancellationToken = default);
    Task<ApiEndpointResponse> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Finds the endpoint matching the supplied request (route template aware).</summary>
    Task<ApiEndpointResponse?> ResolveAsync(string httpMethod, string path, CancellationToken cancellationToken = default);

    /// <summary>Invalidates the in-memory endpoint lookup cache.</summary>
    void InvalidateCache();
}
