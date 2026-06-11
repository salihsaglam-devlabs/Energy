using Asp.Versioning;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/api-endpoints")]
public sealed class ApiEndpointsController : ControllerBase
{
    private readonly IApiEndpointService _endpoints;
    public ApiEndpointsController(IApiEndpointService endpoints) { _endpoints = endpoints; }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApiEndpointResponse>>>> GetAll(
        [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(BaseResponse<PaginatedResponse<ApiEndpointResponse>>.Success(await _endpoints.GetAllAsync(request, ct)));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApiEndpointResponse>>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _endpoints.GetByIdAsync(id, ct);
        return item is null ? NotFound(BaseResponse<ApiEndpointResponse>.Failure("Endpoint not found."))
                            : Ok(BaseResponse<ApiEndpointResponse>.Success(item));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<ApiEndpointResponse>>> Create(CreateApiEndpointRequest request, CancellationToken ct)
        => Ok(BaseResponse<ApiEndpointResponse>.Success(await _endpoints.CreateAsync(request, ct)));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApiEndpointResponse>>> Update(Guid id, UpdateApiEndpointRequest request, CancellationToken ct)
        => Ok(BaseResponse<ApiEndpointResponse>.Success(await _endpoints.UpdateAsync(id, request, ct)));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _endpoints.DeleteAsync(id, ct)));
}
