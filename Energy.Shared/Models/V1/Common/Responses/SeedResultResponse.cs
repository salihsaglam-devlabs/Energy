namespace Energy.Shared.Models.V1.Common.Responses;

/// <summary>Bir seed (tohumlama) işleminin sonucu: eklenen, güncellenen ve toplam kayıt sayıları.</summary>
public sealed class SeedResultResponse
{
    /// <summary>Yeni eklenen kayıt sayısı.</summary>
    public int Added { get; init; }

    /// <summary>Güncellenen kayıt sayısı.</summary>
    public int Updated { get; init; }

    /// <summary>İşlem sonundaki toplam kayıt sayısı.</summary>
    public int Total { get; init; }
}
