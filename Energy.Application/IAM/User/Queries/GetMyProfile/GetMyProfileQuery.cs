using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.User.Queries.GetMyProfile;

/// <summary>
/// Geçerli (oturum açmış) kullanıcının kendi profil ayrıntılarını döndüren self-servis
/// sorgu. <see cref="GetUserById.GetUserByIdQuery"/> aksine herhangi bir kullanıcıyı
/// kimlikle okumaz; yalnızca istek sahibinin kendi kaydını döndürür ve bu yüzden
/// <c>Profile.Read</c> yetkisiyle korunur (her kullanıcının varsayılan yetkisi).
/// </summary>
public sealed record GetMyProfileQuery
    : IRequest<BaseResponse<UserDetailResponse>>;

