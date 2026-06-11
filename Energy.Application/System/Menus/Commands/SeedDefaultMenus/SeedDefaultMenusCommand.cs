using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.SeedDefaultMenus;

public sealed record SeedDefaultMenusCommand : IRequest<BaseResponse<SeedResultResponse>>;
