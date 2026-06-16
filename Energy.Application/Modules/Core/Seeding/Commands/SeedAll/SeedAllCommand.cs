using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Seeding.Commands.SeedAll;

/// <summary>SeedAll</summary>
public sealed record SeedAllCommand()
    : IRequest<BaseResponse<bool>>;
