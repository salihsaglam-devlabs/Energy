#!/usr/bin/env python3
"""
Energy — özel (entity-dışı) API controller'larını MediatR akışına taşıyan generator.

Kapsam: kök controller'lar (IAM/Chat/Core/Home), süreç (Processes) ve rapor (Reports)
controller'ları. Her endpoint için Application katmanında bir Command/Query record +
Handler üretir; controller'lar yalnızca IMediator kullanır. Approval süreci, auth
guard mantığı nedeniyle elle yazılır (bu generator kapsamı dışında).

YIKICI DEĞİLDİR: yalnızca hedef dosyaları yazar ve taşınan eski kök controller
dosyalarını siler (OLD_FILES). Entity CQRS generate_cqrs.py tarafından üretilir.
"""
from __future__ import annotations
import os

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
APP = os.path.join(ROOT, "Energy.Application", "Modules")
API = os.path.join(ROOT, "Energy.Api", "Controllers")


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content if content.endswith("\n") else content + "\n")


def handler_body(ep):
    inner = ep["inner"]
    call = ep["call"]
    mode = ep["mode"]
    pre = ""
    if ep.get("current_user") and not ep.get("no_user_var"):
        pre = "        var currentUserId = _currentUser.UserId ?? Guid.Empty;\n"
    if ep.get("pre_call"):
        pre += "        " + ep["pre_call"] + "\n"
    if mode == "wrap":
        body = f"        var result = await {call};\n        return BaseResponse<{inner}>.Success(result);"
    elif mode == "wrap_msg":
        body = f"        var result = await {call};\n        return BaseResponse<{inner}>.Success(result, \"Completed\");"
    elif mode == "passthrough":
        body = f"        return await {call};"
    elif mode == "notfound_throw":
        body = (f"        var result = await {call}\n"
                f"            ?? throw new NotFoundException({ep['nf_key']}, {ep['nf_arg']});\n"
                f"        return BaseResponse<{inner}>.Success(result);")
    elif mode == "notfound_fail":
        body = (f"        var result = await {call};\n"
                f"        return result is null\n"
                f"            ? BaseResponse<{inner}>.Failure({ep['fail_msg']})\n"
                f"            : BaseResponse<{inner}>.Success(result);")
    elif mode == "void_true":
        body = f"        await {call};\n        return BaseResponse<bool>.Success(true);"
    elif mode == "bool_notfound":
        body = (f"        var ok = await {call};\n"
                f"        if (!ok) throw new NotFoundException({ep['nf_key']}, {ep['nf_arg']});\n"
                f"        return BaseResponse<bool>.Success(true);")
    elif mode == "bool_fail":
        body = (f"        var ok = await {call};\n"
                f"        return ok\n"
                f"            ? BaseResponse<bool>.Success(true)\n"
                f"            : BaseResponse<bool>.Failure({ep['fail_msg']});")
    elif mode == "auth":
        body = (f"        var token = await {call};\n"
                f"        return token is null\n"
                f"            ? BaseResponse<{inner}>.Failure(_localizer[LocalizationKeys.Messages.InvalidCredentials].Value)\n"
                f"            : BaseResponse<{inner}>.Success(token);")
    elif mode == "ingest":
        body = ("        var isSystemService = string.Equals(\n"
                "            _currentUser.UserName, ServiceAccount.UserName, StringComparison.OrdinalIgnoreCase);\n"
                "        var userId = isSystemService ? request.Request.UserId : _currentUser.UserId;\n"
                "        var userName = isSystemService ? request.Request.UserName : _currentUser.UserName;\n"
                "        await _logs.IngestAsync(request.Request, userId, userName, request.IpAddress, ct);\n"
                "        return BaseResponse<bool>.Success(true);")
    elif mode == "file":
        body = f"        return await {call};"
    elif mode == "process":
        body = ("        try\n        {\n"
                + ep["proc_body"]
                + "\n        }\n        catch (InvalidOperationException ex)\n        {\n"
                + f"            return BaseResponse<{inner}>.Failure(ex.Message);\n        }}")
    else:
        raise ValueError(mode)
    return pre + body


def ret_type(ep):
    return f"{ep['inner']}?" if ep["mode"] == "file" else f"BaseResponse<{ep['inner']}>"


def emit_request_and_handler(spec, ep):
    fm = spec["feature_module"]
    feature = ep["feature"]
    name = ep["name"]
    suffix = "Query" if ep["kind"] == "query" else "Command"
    req = name + suffix
    folder_kind = "Queries" if ep["kind"] == "query" else "Commands"
    feature_ns = feature.replace("/", ".")
    ns = f"Energy.Application.Modules.{fm}.{feature_ns}.{folder_kind}.{name}"
    base_dir = os.path.join(APP, fm, *feature.split("/"), folder_kind, name)

    model_usings = spec.get("model_usings", [])
    rt = ret_type(ep)

    # ---- record ----
    rl = ["using Energy.Shared.Models.V1.Common.Responses;"]
    rl += [f"using {u};" for u in model_usings]
    rl += ["using MediatR;", "",
           f"namespace {ns};", "",
           f"/// <summary>{ep.get('doc', name)}</summary>",
           f"public sealed record {req}({ep['ctor']})",
           f"    : IRequest<{rt}>;"]
    write(os.path.join(base_dir, f"{req}.cs"), "\n".join(rl))

    # ---- handler ----
    injects = list(ep["inject"])
    inject_usings = sorted({i[2] for i in injects})
    if ep.get("current_user"):
        injects.append(("ICurrentUser", "_currentUser", "Energy.Application.Identity.Services"))
        inject_usings = sorted(set(inject_usings) | {"Energy.Application.Identity.Services"})

    hl = ["using Energy.Application.Common.Exceptions;",
          "using Energy.Localization;",
          "using Energy.Shared.Models.V1.Common.Responses;"]
    hl += [f"using {u};" for u in model_usings]
    hl += [f"using {u};" for u in inject_usings]
    hl += ["using MediatR;", "",
           f"namespace {ns};", "",
           f"/// <summary><see cref=\"{req}\"/> handler'ı (orkestrasyon).</summary>",
           f"public sealed class {req}Handler",
           f"    : IRequestHandler<{req}, {rt}>",
           "{"]
    for (iface, field, _u) in injects:
        hl.append(f"    private readonly {iface} {field};")
    hl.append("")
    ctor_params = ", ".join(f"{iface} {field.lstrip('_')}" for (iface, field, _u) in injects)
    hl.append(f"    public {req}Handler({ctor_params})")
    hl.append("    {")
    for (iface, field, _u) in injects:
        hl.append(f"        {field} = {field.lstrip('_')};")
    hl.append("    }")
    hl.append("")
    hl.append(f"    public async Task<{rt}> Handle({req} request, CancellationToken ct)")
    hl.append("    {")
    hl.append(handler_body(ep))
    hl.append("    }")
    hl.append("}")
    write(os.path.join(base_dir, f"{req}Handler.cs"), "\n".join(hl))
    return ns, req


def emit_controller(spec, record_namespaces):
    cl = ["using Asp.Versioning;",
          "using MediatR;",
          "using Microsoft.AspNetCore.Mvc;"]
    if spec.get("allow_anonymous_any"):
        cl.append("using Microsoft.AspNetCore.Authorization;")
    cl += ["using Energy.Shared.Models.V1.Common.Responses;"]
    cl += [f"using {u};" for u in spec.get("ctrl_usings", [])]
    cl += [f"using {u};" for u in sorted(set(record_namespaces))]
    cl += [f"using {u};" for u in spec.get("ctrl_extra_usings", [])]
    cl += ["",
           f"namespace Energy.Api.Controllers.{spec['module']};", "",
           f"/// <summary>{spec['summary']}</summary>",
           "[ApiController]",
           '[ApiVersion("1.0")]',
           f'[Route("{spec["route"]}")]',
           f"public sealed class {spec['controller']} : ControllerBase",
           "{",
           "    private readonly IMediator _mediator;", "",
           f"    public {spec['controller']}(IMediator mediator)",
           "        => _mediator = mediator;"]
    for ep in spec["endpoints"]:
        req = ep["name"] + ("Query" if ep["kind"] == "query" else "Command")
        sig = ep.get("sig", "")
        params = (sig + ", " if sig else "") + "CancellationToken ct"
        new_args = ep.get("new_args", "")
        cl.append("")
        cl.append(f"    {ep['attr']}")
        if ep.get("allow_anonymous"):
            cl.append("    [AllowAnonymous]")
        if ep["mode"] == "file":
            cl.append(f"    public async Task<IActionResult> {ep['action']}({params})")
            cl.append("    {")
            cl.append(f"        var result = await _mediator.Send(new {req}({new_args}), ct);")
            cl.append(f"        return result is null ? NotFound() : File({ep['file_args']});")
            cl.append("    }")
        elif ep["mode"] == "auth":
            cl.append(f"    public async Task<ActionResult<BaseResponse<{ep['inner']}>>> {ep['action']}({params})")
            cl.append("    {")
            cl.append(f"        var result = await _mediator.Send(new {req}({new_args}), ct);")
            cl.append("        return result.IsSuccess ? Ok(result) : Unauthorized(result);")
            cl.append("    }")
        elif ep["mode"] == "export":
            cl.append(f"    public async Task<IActionResult> {ep['action']}({params})")
            cl.append("    {")
            cl.append(ep["export_body"])
            cl.append("    }")
        else:
            cl.append(f"    public async Task<ActionResult<BaseResponse<{ep['inner']}>>> {ep['action']}({params})")
            cl.append(f"        => Ok(await _mediator.Send(new {req}({new_args}), ct));")
    cl.append("}")
    write(os.path.join(API, *spec["module"].split("."), f"{spec['controller']}.cs"), "\n".join(cl))


# Eski kök controller dosyaları (taşınanlar) — silinecek.
OLD_FILES = [
    "HomeController.cs", "AuthController.cs", "UsersController.cs", "RolesController.cs",
    "PermissionsController.cs", "MenusController.cs", "ApiEndpointsController.cs",
    "AuditLogsController.cs", "LocalizationController.cs", "SettingsController.cs",
    "SeedController.cs", "ChatController.cs",
]


def main():
    from special_specs import SPECS  # specs ayrı dosyada (okunabilirlik)
    total_files = 0
    for spec in SPECS:
        record_ns = []
        for ep in spec["endpoints"]:
            if ep["mode"] == "export":
                continue  # export endpoint'i record kullanmaz (mevcut data query'sini gönderir)
            ns, _req = emit_request_and_handler(spec, ep)
            record_ns.append(ns)
            total_files += 2
        # export endpoint'lerinin gönderdiği data query namespace'ini ekle
        for ep in spec["endpoints"]:
            if ep["mode"] == "export" and ep.get("uses_ns"):
                record_ns.append(ep["uses_ns"])
        emit_controller(spec, record_ns)
        total_files += 1

    for f in OLD_FILES:
        p = os.path.join(API, f)
        if os.path.exists(p):
            os.remove(p)

    print(f"Generated {total_files} files for {len(SPECS)} special/process/report controllers.")


if __name__ == "__main__":
    main()

