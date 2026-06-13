using Asp.Versioning;
using Energy.Application.Common.Crud;
using Energy.Domain.Common;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Enterprise;

/// <summary>
/// Kurumsal modül varlıkları için ortak CRUD denetleyici tabanı. Somut denetleyiciler
/// yalnızca rota ve varlık türünü sağlar; yetkilendirme uç nokta-permission eşlemesiyle
/// (DefaultEndpointPermissionMap) uygulanır.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
public abstract class EnterpriseCrudControllerBase<TEntity> : ControllerBase
    where TEntity : AuditableEntity
{
    /// <summary>Tip güvenli CRUD servisi.</summary>
    protected IGenericCrudService<TEntity> Service { get; }

    /// <summary>CRUD servisini enjekte eder.</summary>
    protected EnterpriseCrudControllerBase(IGenericCrudService<TEntity> service) => Service = service;

    /// <summary>Sayfalı liste (ReadAll yetkisiyle korunur).</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<TEntity>>>> GetAll(
        [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(BaseResponse<PaginatedResponse<TEntity>>.Success(await Service.GetAllAsync(request, ct)));

    /// <summary>Tekil kayıt (Read yetkisiyle korunur).</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<TEntity>>> GetById(Guid id, CancellationToken ct)
    {
        var entity = await Service.GetByIdAsync(id, ct);
        return entity is null
            ? NotFound(BaseResponse<TEntity>.Failure("Record not found."))
            : Ok(BaseResponse<TEntity>.Success(entity));
    }

    /// <summary>Yeni kayıt oluşturur (Create yetkisiyle korunur).</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<TEntity>>> Create([FromBody] TEntity entity, CancellationToken ct)
        => Ok(BaseResponse<TEntity>.Success(await Service.CreateAsync(entity, ct)));

    /// <summary>Var olan kaydı günceller (Update yetkisiyle korunur).</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<TEntity>>> Update(Guid id, [FromBody] TEntity entity, CancellationToken ct)
    {
        var updated = await Service.UpdateAsync(id, entity, ct);
        return updated is null
            ? NotFound(BaseResponse<TEntity>.Failure("Record not found."))
            : Ok(BaseResponse<TEntity>.Success(updated));
    }

    /// <summary>Kaydı yumuşak siler (Delete yetkisiyle korunur).</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await Service.DeleteAsync(id, ct)));
}

