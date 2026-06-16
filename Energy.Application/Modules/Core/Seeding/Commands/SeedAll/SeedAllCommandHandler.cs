using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.Core.Seeding.Commands.SeedAll;

/// <summary><see cref="SeedAllCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SeedAllCommandHandler
    : IRequestHandler<SeedAllCommand, BaseResponse<bool>>
{
    private readonly ISystemSeeder _seeder;

    public SeedAllCommandHandler(ISystemSeeder seeder)
    {
        _seeder = seeder;
    }

    public async Task<BaseResponse<bool>> Handle(SeedAllCommand request, CancellationToken ct)
    {
        await _seeder.SeedAllAsync(ct);
        return BaseResponse<bool>.Success(true);
    }
}
