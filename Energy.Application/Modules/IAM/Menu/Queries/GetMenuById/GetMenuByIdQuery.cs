using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.Menu.Queries.GetMenuById;

/// <summary>GetMenuById</summary>
public sealed record GetMenuByIdQuery(Guid Id)
    : IRequest<BaseResponse<MenuResponse>>;
