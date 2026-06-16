#!/usr/bin/env python3
"""
Energy — MediatR CQRS generator (mimarî tutarlılık fazı).

Her üretilen iş entity'si için (IAM/Chat hariç — onların controller'ları elle
yazılır) standart CQRS katmanını üretir ve API controller'ını
Controller -> IMediator -> Command/Query -> Handler -> Application Service
akışına çevirir. Şablon, Procurement/PurchaseOrder referans örneğinden birebir
türetilmiştir. Handler'lar iş mantığı içermez; yalnızca mevcut
I{Entity}Service / I{Entity}LookupService sözleşmelerini orkestre eder.

YIKICI DEĞİLDİR: hiçbir klasörü silmez. Yalnızca:
  * Energy.Application/Modules/{Module}/{Entity}/Commands|Queries altını yazar,
  * Energy.Api/Controllers/Modules/{Module}/{Entity}Controller.cs dosyasını
    yerinde MediatR sürümüyle değiştirir.
Processes / Reports / Lookups / Files controller'larına ve IAM/Chat'e dokunmaz.
"""
from __future__ import annotations

import os
import re

from generate_domain import ROOT, build_model

APP_ROOT = os.path.join(ROOT, "Energy.Application", "Modules")
# API controller'ları kurala göre Controllers/{Module}/ altında tutulur (ara "Modules"
# segmenti yoktur). Domain/Application/Infrastructure'da ise "Modules" standardı korunur.
API_ROOT = os.path.join(ROOT, "Energy.Api", "Controllers")
EXCLUDE_MODULES = {"IAM", "Chat"}


def kebab(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "-", name).lower()


def write(path: str, content: str) -> None:
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content if content.endswith("\n") else content + "\n")


def ns(module: str, entity: str, *parts: str) -> str:
    return ".".join((f"Energy.Application.Modules.{module}.{entity}", *parts))


# --------------------------------------------------------------------------- #
# Command / Query record + handler templates
# --------------------------------------------------------------------------- #
def create_command(m, e):
    return f"""using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Requests;
using MediatR;

namespace {ns(m, e, 'Commands', 'Create' + e)};

/// <summary>Yeni {e} oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record Create{e}Command(Create{e}Request Request)
    : IRequest<BaseResponse<Guid>>;
"""


def create_handler(m, e):
    return f"""using {ns(m, e, 'Services')};
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace {ns(m, e, 'Commands', 'Create' + e)};

/// <summary>
/// <see cref="Create{e}Command"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="I{e}Service"/>'i orkestre eder.
/// </summary>
public sealed class Create{e}CommandHandler
    : IRequestHandler<Create{e}Command, BaseResponse<Guid>>
{{
    private readonly I{e}Service _service;

    public Create{e}CommandHandler(I{e}Service service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        Create{e}Command request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}}
"""


def update_command(m, e):
    return f"""using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Requests;
using MediatR;

namespace {ns(m, e, 'Commands', 'Update' + e)};

/// <summary>Var olan {e} kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record Update{e}Command(Guid Id, Update{e}Request Request)
    : IRequest<BaseResponse<bool>>;
"""


def update_handler(m, e):
    return f"""using {ns(m, e, 'Services')};
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace {ns(m, e, 'Commands', 'Update' + e)};

/// <summary>
/// <see cref="Update{e}Command"/> handler'ı. <see cref="I{e}Service"/>'i orkestre eder.
/// </summary>
public sealed class Update{e}CommandHandler
    : IRequestHandler<Update{e}Command, BaseResponse<bool>>
{{
    private readonly I{e}Service _service;

    public Update{e}CommandHandler(I{e}Service service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        Update{e}Command request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}}
"""


def delete_command(m, e):
    return f"""using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace {ns(m, e, 'Commands', 'Delete' + e)};

/// <summary>{e} kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record Delete{e}Command(Guid Id) : IRequest<BaseResponse<bool>>;
"""


def delete_handler(m, e):
    return f"""using {ns(m, e, 'Services')};
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace {ns(m, e, 'Commands', 'Delete' + e)};

/// <summary>
/// <see cref="Delete{e}Command"/> handler'ı. <see cref="I{e}Service"/>'i orkestre eder.
/// </summary>
public sealed class Delete{e}CommandHandler
    : IRequestHandler<Delete{e}Command, BaseResponse<bool>>
{{
    private readonly I{e}Service _service;

    public Delete{e}CommandHandler(I{e}Service service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        Delete{e}Command request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}}
"""


def list_query(m, e):
    return f"""using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Requests;
using Energy.Shared.Models.V1.{m}.{e}.Responses;
using MediatR;

namespace {ns(m, e, 'Queries', 'Get' + e + 'List')};

/// <summary>Sayfalanmış {e} listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record Get{e}ListQuery(Get{e}ListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<{e}ListResponse>>>;
"""


def list_handler(m, e):
    return f"""using {ns(m, e, 'Services')};
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Responses;
using MediatR;

namespace {ns(m, e, 'Queries', 'Get' + e + 'List')};

/// <summary>
/// <see cref="Get{e}ListQuery"/> handler'ı. <see cref="I{e}Service"/>'i orkestre eder.
/// </summary>
public sealed class Get{e}ListQueryHandler
    : IRequestHandler<Get{e}ListQuery, BaseResponse<PaginatedResponse<{e}ListResponse>>>
{{
    private readonly I{e}Service _service;

    public Get{e}ListQueryHandler(I{e}Service service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<{e}ListResponse>>> Handle(
        Get{e}ListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}}
"""


def byid_query(m, e):
    return f"""using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Responses;
using MediatR;

namespace {ns(m, e, 'Queries', 'Get' + e + 'ById')};

/// <summary>Kimliğe göre {e} detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record Get{e}ByIdQuery(Guid Id)
    : IRequest<BaseResponse<{e}DetailResponse>>;
"""


def byid_handler(m, e):
    return f"""using {ns(m, e, 'Services')};
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Responses;
using MediatR;

namespace {ns(m, e, 'Queries', 'Get' + e + 'ById')};

/// <summary>
/// <see cref="Get{e}ByIdQuery"/> handler'ı. <see cref="I{e}Service"/>'i orkestre eder.
/// </summary>
public sealed class Get{e}ByIdQueryHandler
    : IRequestHandler<Get{e}ByIdQuery, BaseResponse<{e}DetailResponse>>
{{
    private readonly I{e}Service _service;

    public Get{e}ByIdQueryHandler(I{e}Service service)
        => _service = service;

    public Task<BaseResponse<{e}DetailResponse>> Handle(
        Get{e}ByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}}
"""


def lookup_query(m, e):
    return f"""using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Responses;
using MediatR;

namespace {ns(m, e, 'Queries', 'Get' + e + 'Lookup')};

/// <summary>{e} lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record Get{e}LookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<{e}LookupResponse>>>;
"""


def lookup_handler(m, e):
    return f"""using {ns(m, e, 'Lookups')};
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Responses;
using MediatR;

namespace {ns(m, e, 'Queries', 'Get' + e + 'Lookup')};

/// <summary>
/// <see cref="Get{e}LookupQuery"/> handler'ı. <see cref="I{e}LookupService"/>'i orkestre eder.
/// </summary>
public sealed class Get{e}LookupQueryHandler
    : IRequestHandler<Get{e}LookupQuery, BaseResponse<IReadOnlyList<{e}LookupResponse>>>
{{
    private readonly I{e}LookupService _lookup;

    public Get{e}LookupQueryHandler(I{e}LookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<{e}LookupResponse>>> Handle(
        Get{e}LookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}}
"""


def controller(m, e, table):
    route = f"api/v{{version:apiVersion}}/{kebab(m)}/{kebab(table)}"
    return f"""using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using {ns(m, e, 'Commands', 'Create' + e)};
using {ns(m, e, 'Commands', 'Delete' + e)};
using {ns(m, e, 'Commands', 'Update' + e)};
using {ns(m, e, 'Queries', 'Get' + e + 'ById')};
using {ns(m, e, 'Queries', 'Get' + e + 'List')};
using {ns(m, e, 'Queries', 'Get' + e + 'Lookup')};
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.{m}.{e}.Requests;
using Energy.Shared.Models.V1.{m}.{e}.Responses;

namespace Energy.Api.Controllers.{m};

/// <summary>
/// {e} uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("{route}")]
public sealed class {e}Controller : ControllerBase
{{
    private readonly IMediator _mediator;

    public {e}Controller(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<{e}ListResponse>>>> GetList([FromQuery] Get{e}ListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new Get{e}ListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{{id:guid}}")]
    public async Task<ActionResult<BaseResponse<{e}DetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new Get{e}ByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<{e}LookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new Get{e}LookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(Create{e}Request request, CancellationToken ct)
        => Ok(await _mediator.Send(new Create{e}Command(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{{id:guid}}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, Update{e}Request request, CancellationToken ct)
        => Ok(await _mediator.Send(new Update{e}Command(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{{id:guid}}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new Delete{e}Command(id), ct));
}}
"""


def main():
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    cqrs = 0
    ctrls = 0
    for t in order:
        m, e = table_module[t], table_entity[t]
        if m in EXCLUDE_MODULES:
            continue
        base = os.path.join(APP_ROOT, m, e)
        files = {
            f"Commands/Create{e}/Create{e}Command.cs": create_command(m, e),
            f"Commands/Create{e}/Create{e}CommandHandler.cs": create_handler(m, e),
            f"Commands/Update{e}/Update{e}Command.cs": update_command(m, e),
            f"Commands/Update{e}/Update{e}CommandHandler.cs": update_handler(m, e),
            f"Commands/Delete{e}/Delete{e}Command.cs": delete_command(m, e),
            f"Commands/Delete{e}/Delete{e}CommandHandler.cs": delete_handler(m, e),
            f"Queries/Get{e}List/Get{e}ListQuery.cs": list_query(m, e),
            f"Queries/Get{e}List/Get{e}ListQueryHandler.cs": list_handler(m, e),
            f"Queries/Get{e}ById/Get{e}ByIdQuery.cs": byid_query(m, e),
            f"Queries/Get{e}ById/Get{e}ByIdQueryHandler.cs": byid_handler(m, e),
            f"Queries/Get{e}Lookup/Get{e}LookupQuery.cs": lookup_query(m, e),
            f"Queries/Get{e}Lookup/Get{e}LookupQueryHandler.cs": lookup_handler(m, e),
        }
        for rel, content in files.items():
            write(os.path.join(base, rel), content)
            cqrs += 1

        ctrl_path = os.path.join(API_ROOT, m, f"{e}Controller.cs")
        write(ctrl_path, controller(m, e, t))
        ctrls += 1

    print(f"Generated {cqrs} CQRS files and rewired {ctrls} API controllers "
          f"to the MediatR flow (IAM/Chat excluded).")


if __name__ == "__main__":
    main()

