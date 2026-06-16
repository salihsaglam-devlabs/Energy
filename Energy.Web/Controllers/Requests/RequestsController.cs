using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using Energy.Web.Clients.Requests.Request;

namespace Energy.Web.Controllers.Requests;

/// <summary>Request ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("requests/requests")]
public sealed class RequestsController : Controller
{
    private readonly IRequestApiClient _api;

    public RequestsController(IRequestApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Requests/Request/Index.cshtml");

    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _api.GetListAsync(pageNumber, take <= 0 ? 20 : take, searchValue, ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.Requests.Request.Responses.RequestListResponse>(), totalCount = page?.TotalCount ?? 0 });
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(string? search = null, bool activeOnly = true, CancellationToken ct = default)
        => Json((await _api.GetLookupAsync(search, activeOnly, ct)).Data ?? []);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail(Guid id, CancellationToken ct)
        => Json(await _api.GetByIdAsync(id, ct));

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateRequestRequest request, CancellationToken ct)
        => Json(await _api.CreateAsync(request, ct));

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequestRequest request, CancellationToken ct)
        => Json(await _api.UpdateAsync(id, request, ct));

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Json(await _api.DeleteAsync(id, ct));
}
