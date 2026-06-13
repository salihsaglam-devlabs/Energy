namespace Energy.Shared.Identity;

/// <summary>
/// Sistemler arası çağrılar için kullanılan, yerleşik ve etkileşimsiz sistem/servis
/// hesabı — örn. oturum açmış kullanıcısı olmayan anonim/dahili istekler için Web
/// katmanı denetim alımı. <see cref="SystemRoles.SuperAdmin"/> rolü atanır; böylece
/// her yetki kontrolünü atlar ve herhangi bir insan (kimliği doğrulanmış) kullanıcıdan
/// tamamen bağımsızdır.
///
/// Parola yapılandırmadan okunur (API tarafında "ServiceAccount:Password", Web
/// tarafında "Api:ServiceAccount:Password") ve her iki katmanın kutudan çıktığı gibi
/// uyuşması için <see cref="DefaultPassword"/> değerine geri düşer.
/// </summary>
public static class ServiceAccount
{
    /// <summary>Servis hesabının kullanıcı adı.</summary>
    public const string UserName = "system";
    /// <summary>Servis hesabının e-posta adresi.</summary>
    public const string Email = "system@energy.local";
    /// <summary>Servis hesabının adı.</summary>
    public const string FirstName = "System";
    /// <summary>Servis hesabının soyadı.</summary>
    public const string LastName = "Service";

    /// <summary>Hiçbir geçersiz kılma yapılandırılmadığında kullanılan yedek gizli anahtar.
    /// Üretimde HEM API HEM de Web katmanında yapılandırma ile geçersiz kılınmalıdır.</summary>
    public const string DefaultPassword = "Sys!Service#2024$Energy";

    /// <summary>API'nin tohumlanan parolayı geçersiz kılmak için okuduğu yapılandırma anahtarı.</summary>
    public const string ApiPasswordConfigKey = "ServiceAccount:Password";

    /// <summary>Web katmanının kimlik bilgilerini geçersiz kılmak için okuduğu kullanıcı adı anahtarı.</summary>
    public const string WebUserNameConfigKey = "Api:ServiceAccount:UserNameOrEmail";
    /// <summary>Web katmanının kimlik bilgilerini geçersiz kılmak için okuduğu parola anahtarı.</summary>
    public const string WebPasswordConfigKey = "Api:ServiceAccount:Password";
}
