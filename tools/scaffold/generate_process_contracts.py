#!/usr/bin/env python3
"""
Energy — process & document-file Shared/Application contract generator.

These contracts back the hand-built process screens (Approval, StockIssue,
StockTransfer, GoodsReceipt, TimesheetCost, ProgressPaymentPosting,
PaymentAllocation) and the document file/version feature. They are NOT produced
by the per-entity contract generator, so this dedicated generator keeps them
reproducible and safe across regenerations of the entity contracts.
"""
from __future__ import annotations

import os

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
SHARED = os.path.join(ROOT, "Energy.Shared", "Models", "V1")
APP = os.path.join(ROOT, "Energy.Application", "Modules")
INFRA = os.path.join(ROOT, "Energy.Infrastructure", "Modules")


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content.lstrip("\n"))


FILES = {
    # --- Workflow / Approval ---
    os.path.join(SHARED, "Workflow", "Processes", "Approval", "Responses", "ApprovalRequestListItemResponse.cs"): """
namespace Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;

/// <summary>Onay gelen kutusu satırı (salt-okunur, domain sızdırmaz).</summary>
public sealed class ApprovalRequestListItemResponse
{
    /// <summary>Onay talebi kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Kaynak modül adı.</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>Kaynak nesne türü.</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Kaynak nesne kimliği.</summary>
    public Guid RelatedEntityId { get; set; }

    /// <summary>Talebin güncel durumu (metin).</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Aktif adım numarası.</summary>
    public int CurrentStepNo { get; set; }

    /// <summary>Talep oluşturulma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
""",
    os.path.join(SHARED, "Workflow", "Processes", "Approval", "Requests", "ApprovalActionRequest.cs"): """
namespace Energy.Shared.Models.V1.Workflow.Processes.Approval.Requests;

/// <summary>Onay eylemi (onayla/ret/iptal) isteği. Açıklama/not taşır.</summary>
public sealed class ApprovalActionRequest
{
    /// <summary>Eyleme eşlik eden açıklama/not (opsiyonel).</summary>
    public string? Note { get; set; }
}
""",
    # --- Inventory / StockIssue ---
    os.path.join(SHARED, "Inventory", "Processes", "StockIssue", "Requests", "StockIssueProcessRequest.cs"): """
using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;

/// <summary>Stok çıkış (issue) süreç isteği (FIFO, transaction-güvenli).</summary>
public sealed class StockIssueProcessRequest
{
    /// <summary>Çıkışın yapılacağı depo.</summary>
    [Required]
    public Guid WarehouseId { get; set; }

    /// <summary>Çıkışı yapılacak malzeme.</summary>
    [Required]
    public Guid MaterialId { get; set; }

    /// <summary>Ölçü birimi.</summary>
    [Required]
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Çıkış miktarı (pozitif).</summary>
    [Range(0.000001, double.MaxValue)]
    public decimal Quantity { get; set; }

    /// <summary>İlişkili proje (opsiyonel).</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Açıklama (opsiyonel).</summary>
    public string? Note { get; set; }
}
""",
    os.path.join(SHARED, "Inventory", "Processes", "StockIssue", "Responses", "StockIssueProcessResponse.cs"): """
namespace Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;

/// <summary>Stok çıkış sürecinin sonucu: FIFO toplam maliyet ve tahsis satır sayısı.</summary>
public sealed class StockIssueProcessResponse
{
    /// <summary>FIFO dağıtımıyla hesaplanan toplam maliyet.</summary>
    public decimal TotalCost { get; set; }

    /// <summary>Çıkışın dağıtıldığı lot/tahsis satırı sayısı.</summary>
    public int AllocationCount { get; set; }
}
""",
    # --- Inventory / StockTransfer ---
    os.path.join(SHARED, "Inventory", "Processes", "StockTransfer", "Requests", "StockTransferProcessRequest.cs"): """
using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;

/// <summary>Depolar arası stok transfer süreç isteği (transaction-güvenli).</summary>
public sealed class StockTransferProcessRequest
{
    /// <summary>Kaynak depo.</summary>
    [Required]
    public Guid SourceWarehouseId { get; set; }

    /// <summary>Hedef depo.</summary>
    [Required]
    public Guid TargetWarehouseId { get; set; }

    /// <summary>Transfer edilecek malzeme.</summary>
    [Required]
    public Guid MaterialId { get; set; }

    /// <summary>Ölçü birimi.</summary>
    [Required]
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Transfer miktarı (pozitif).</summary>
    [Range(0.000001, double.MaxValue)]
    public decimal Quantity { get; set; }

    /// <summary>Açıklama (opsiyonel).</summary>
    public string? Note { get; set; }
}
""",
    os.path.join(SHARED, "Inventory", "Processes", "StockTransfer", "Responses", "StockTransferProcessResponse.cs"): """
namespace Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;

/// <summary>Stok transfer sürecinin sonucu: taşınan FIFO maliyet toplamı.</summary>
public sealed class StockTransferProcessResponse
{
    /// <summary>Kaynak çıkışın FIFO toplam maliyeti (hedefe taşınan değer).</summary>
    public decimal TotalCost { get; set; }

    /// <summary>Çıkışın dağıtıldığı lot/tahsis satırı sayısı.</summary>
    public int AllocationCount { get; set; }
}
""",
    # --- Procurement / GoodsReceipt ---
    os.path.join(SHARED, "Procurement", "Processes", "GoodsReceipt", "Requests", "GoodsReceiptProcessRequest.cs"): """
using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;

/// <summary>Mal kabul (goods receipt) süreç isteği (irsaliye -> stok girişi).</summary>
public sealed class GoodsReceiptProcessRequest
{
    /// <summary>Stok girişine dönüştürülecek satınalma irsaliyesi kimliği.</summary>
    [Required]
    public Guid PurchaseReceiptId { get; set; }
}
""",
    # --- Finance / TimesheetCost ---
    os.path.join(SHARED, "Finance", "Processes", "TimesheetCost", "Requests", "TimesheetCostProcessRequest.cs"): """
using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;

/// <summary>Puantaj maliyet süreç isteği (HR Cost akışı).</summary>
public sealed class TimesheetCostProcessRequest
{
    /// <summary>Maliyetlendirilecek puantaj (Timesheet) kimliği.</summary>
    [Required]
    public Guid TimesheetId { get; set; }

    /// <summary>Maliyet hareketinin para birimi.</summary>
    [Required]
    public Guid CurrencyId { get; set; }
}
""",
    os.path.join(SHARED, "Finance", "Processes", "TimesheetCost", "Responses", "TimesheetCostProcessResponse.cs"): """
namespace Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;

/// <summary>Puantaj maliyet sürecinin sonucu: üretilen finansal hareket kimliği.</summary>
public sealed class TimesheetCostProcessResponse
{
    /// <summary>Üretilen finansal maliyet hareketinin kimliği.</summary>
    public Guid FinancialTransactionId { get; set; }
}
""",
    # --- Finance / ProgressPaymentPosting ---
    os.path.join(SHARED, "Finance", "Processes", "ProgressPaymentPosting", "Requests", "ProgressPaymentPostingProcessRequest.cs"): """
using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;

/// <summary>Hakediş muhasebeleştirme süreç isteği (Contracts akışı).</summary>
public sealed class ProgressPaymentPostingProcessRequest
{
    /// <summary>Muhasebeleştirilecek hakediş (ProgressPayment) kimliği.</summary>
    [Required]
    public Guid ProgressPaymentId { get; set; }
}
""",
    os.path.join(SHARED, "Finance", "Processes", "ProgressPaymentPosting", "Responses", "ProgressPaymentPostingProcessResponse.cs"): """
namespace Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;

/// <summary>Hakediş muhasebeleştirme sürecinin sonucu: üretilen finansal hareket kimliği.</summary>
public sealed class ProgressPaymentPostingProcessResponse
{
    /// <summary>Üretilen finansal hareketin kimliği.</summary>
    public Guid FinancialTransactionId { get; set; }
}
""",
    # --- Finance / PaymentAllocation ---
    os.path.join(SHARED, "Finance", "Processes", "PaymentAllocation", "Requests", "PaymentAllocationProcessRequest.cs"): """
using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;

/// <summary>Bir ödemenin tek bir borca (payable) tahsis satırı.</summary>
public sealed class PaymentAllocationLineRequest
{
    /// <summary>Tahsisin uygulanacağı hedef borç (Payable) kimliği.</summary>
    [Required]
    public Guid TargetId { get; set; }

    /// <summary>Bu hedefe tahsis edilen tutar (pozitif).</summary>
    [Range(0.000001, double.MaxValue)]
    public decimal Amount { get; set; }
}

/// <summary>Ödeme tahsis (allocation) süreç isteği (Finance akışı).</summary>
public sealed class PaymentAllocationProcessRequest
{
    /// <summary>Tahsis edilecek ödeme (Payment) kimliği.</summary>
    [Required]
    public Guid PaymentId { get; set; }

    /// <summary>Tahsis satırları (hedef borç + tutar).</summary>
    [Required]
    [MinLength(1)]
    public List<PaymentAllocationLineRequest> Lines { get; set; } = [];
}
""",
    os.path.join(SHARED, "Finance", "Processes", "PaymentAllocation", "Responses", "PaymentAllocationProcessResponse.cs"): """
namespace Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;

/// <summary>Ödeme tahsis sürecinin sonucu: tahsis edilen satır sayısı ve toplam tutar.</summary>
public sealed class PaymentAllocationProcessResponse
{
    /// <summary>Tahsis edilen satır sayısı.</summary>
    public int AllocatedLineCount { get; set; }

    /// <summary>Tahsis edilen toplam tutar.</summary>
    public decimal TotalAllocated { get; set; }
}
""",
    # --- Documents / Files ---
    os.path.join(SHARED, "Documents", "Files", "Responses", "DocumentVersionFileResponse.cs"): """
namespace Energy.Shared.Models.V1.Documents.Files.Responses;

/// <summary>Bir belge versiyonunun dosya meta verisi (salt-okunur).</summary>
public sealed class DocumentVersionFileResponse
{
    /// <summary>Versiyon kaydı kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Bağlı belge kimliği.</summary>
    public Guid DocumentId { get; set; }

    /// <summary>Sıra numarası (1, 2, 3, ...).</summary>
    public int VersionNo { get; set; }

    /// <summary>Yüklenen dosya adı.</summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>Dosya boyutu (bayt).</summary>
    public long FileSize { get; set; }

    /// <summary>MIME içerik türü.</summary>
    public string? ContentType { get; set; }

    /// <summary>Yüklenme zamanı.</summary>
    public DateTime UploadedAt { get; set; }
}
""",
    # --- Documents / Files Application interface ---
    os.path.join(APP, "Documents", "Files", "Services", "IDocumentFileService.cs"): """
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;

namespace Energy.Application.Modules.Documents.Files.Services;

/// <summary>İndirilebilir belge içeriği (stream + meta).</summary>
public sealed record DocumentDownload(Stream Content, string FileName, string ContentType);

/// <summary>Belge dosya/versiyon yönetimi sözleşmesi (transaction-güvenli, storage soyutlaması üzerinden).</summary>
public interface IDocumentFileService
{
    /// <summary>Belgeye yeni bir versiyon yükler; CurrentVersionNo artırılır (tek işlem).</summary>
    Task<BaseResponse<DocumentVersionFileResponse>> UploadNewVersionAsync(
        Guid documentId, Stream content, string fileName, string? contentType, long size, CancellationToken ct = default);

    /// <summary>Belgenin versiyon geçmişini (yeniden eskiye) döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> GetVersionsAsync(Guid documentId, CancellationToken ct = default);

    /// <summary>Bir versiyonun dosya içeriğini indirir; yoksa null.</summary>
    Task<DocumentDownload?> GetVersionContentAsync(Guid versionId, CancellationToken ct = default);
}
""",
    # --- Documents / Files Infrastructure service ---
    os.path.join(INFRA, "Documents", "Files", "DocumentFileService.cs"): """
using Energy.Application.Common.Storage;
using Energy.Application.Modules.Documents.Files.Services;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Modules.Documents.Files;

/// <summary>
/// Belge dosya/versiyon yönetimi servisi. Dosyayı saklama soyutlamasına yazar,
/// ardından DocumentVersion kaydını oluşturur ve Document.CurrentVersionNo değerini
/// artırır (tek SaveChanges = atomik). DB hata alırsa yazılan dosya geri alınır.
/// </summary>
public sealed class DocumentFileService : IDocumentFileService
{
    private readonly EnergyDbContext _db;
    private readonly IFileStorage _storage;

    public DocumentFileService(EnergyDbContext db, IFileStorage storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<BaseResponse<DocumentVersionFileResponse>> UploadNewVersionAsync(
        Guid documentId, Stream content, string fileName, string? contentType, long size, CancellationToken ct = default)
    {
        var document = await _db.Documents.FirstOrDefaultAsync(d => d.Id == documentId, ct);
        if (document is null)
        {
            return BaseResponse<DocumentVersionFileResponse>.Failure("NotFound");
        }

        var nextVersionNo = document.CurrentVersionNo + 1;
        var relativePath = await _storage.SaveAsync(content, fileName, ct);

        try
        {
            var version = new global::Energy.Domain.Modules.Documents.DocumentVersion
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                VersionNo = nextVersionNo,
                FileName = fileName,
                FilePath = relativePath,
                FileSize = size,
                ContentType = contentType,
                UploadedAt = DateTime.UtcNow,
            };
            _db.DocumentVersions.Add(version);
            document.CurrentVersionNo = nextVersionNo;
            await _db.SaveChangesAsync(ct);

            return BaseResponse<DocumentVersionFileResponse>.Success(Map(version), "Uploaded");
        }
        catch
        {
            await _storage.DeleteAsync(relativePath, CancellationToken.None);
            throw;
        }
    }

    public async Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> GetVersionsAsync(Guid documentId, CancellationToken ct = default)
    {
        var items = await _db.DocumentVersions.AsNoTracking()
            .Where(v => v.DocumentId == documentId)
            .OrderByDescending(v => v.VersionNo)
            .Select(v => new DocumentVersionFileResponse
            {
                Id = v.Id,
                DocumentId = v.DocumentId,
                VersionNo = v.VersionNo,
                FileName = v.FileName,
                FileSize = v.FileSize,
                ContentType = v.ContentType,
                UploadedAt = v.UploadedAt,
            })
            .ToListAsync(ct);

        return BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>.Success(items);
    }

    public async Task<DocumentDownload?> GetVersionContentAsync(Guid versionId, CancellationToken ct = default)
    {
        var version = await _db.DocumentVersions.AsNoTracking().FirstOrDefaultAsync(v => v.Id == versionId, ct);
        if (version is null)
        {
            return null;
        }

        var stream = await _storage.OpenAsync(version.FilePath, ct);
        if (stream is null)
        {
            return null;
        }

        return new DocumentDownload(stream, version.FileName, version.ContentType ?? "application/octet-stream");
    }

    private static DocumentVersionFileResponse Map(global::Energy.Domain.Modules.Documents.DocumentVersion v) => new()
    {
        Id = v.Id,
        DocumentId = v.DocumentId,
        VersionNo = v.VersionNo,
        FileName = v.FileName,
        FileSize = v.FileSize,
        ContentType = v.ContentType,
        UploadedAt = v.UploadedAt,
    };
}
""",
}


def main():
    for path, content in FILES.items():
        write(path, content)
    print(f"Restored/generated {len(FILES)} process & document contract files")


if __name__ == "__main__":
    main()

