using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.IAM.Menu.Queries.GetMyMenu;

/// <summary>GetMyMenu</summary>
public sealed record GetMyMenuQuery()
    : IRequest<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>>;
