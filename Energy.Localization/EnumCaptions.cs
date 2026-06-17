// EnumCaptions: enum değerlerinin (Status, Type vb.) kullanıcıya gösterilecek
// karşılığı. Grid kolonlarında ham "Approved/Pending" yerine "Onaylandı/Bekliyor"
// gösterilir. Sunucu UI kültürüne göre tek sözlük yayınlar; grid-l10n.js her
// string kolona customizeText uygular (yalnızca birebir eşleşmeler dönüştürülür).
using System.Collections.Generic;
using System.Globalization;

namespace Energy.Localization;

public static class EnumCaptions
{
    private static readonly Dictionary<string, string> Tr = new(StringComparer.Ordinal)
    {
        // ApprovalRequestStatus / ApprovalStepStatus / DocumentStatus / RequestStatus
        ["Draft"] = "Taslak",
        ["Pending"] = "Bekliyor",
        ["PendingApproval"] = "Onay Bekliyor",
        ["Approved"] = "Onaylandı",
        ["Rejected"] = "Reddedildi",
        ["Returned"] = "İade Edildi",
        ["Cancelled"] = "İptal Edildi",
        ["Closed"] = "Kapatıldı",
        ["Ordered"] = "Sipariş Verildi",
        ["Waiting"] = "Beklemede",
        ["Active"] = "Aktif",
        ["Skipped"] = "Atlandı",
        ["Delegated"] = "Devredildi",
        // PurchaseOrderStatus
        ["PartiallyReceived"] = "Kısmen Teslim Alındı",
        ["Received"] = "Teslim Alındı",
        // WorkOrderStatus
        ["Assigned"] = "Atandı",
        ["InProgress"] = "Devam Ediyor",
        ["OnHold"] = "Beklemede",
        ["Completed"] = "Tamamlandı",
        // ApprovalActionType
        ["Approve"] = "Onayla",
        ["Reject"] = "Reddet",
        ["Return"] = "İade Et",
        ["Cancel"] = "İptal Et",
        // ApprovalMode
        ["Sequential"] = "Sıralı",
        ["ParallelAny"] = "Paralel (Herhangi)",
        ["ParallelAll"] = "Paralel (Tümü)",
        ["Quorum"] = "Çoğunluk",
        // ApproverType
        ["User"] = "Kullanıcı",
        ["Role"] = "Rol",
        ["ProjectRole"] = "Proje Rolü",
        ["DepartmentManager"] = "Departman Yöneticisi",
        // ConditionOperator
        ["Equals"] = "Eşittir",
        ["NotEquals"] = "Eşit Değildir",
        ["GreaterThan"] = "Büyüktür",
        ["GreaterThanOrEqual"] = "Büyük veya Eşittir",
        ["LessThan"] = "Küçüktür",
        ["LessThanOrEqual"] = "Küçük veya Eşittir",
        ["In"] = "İçinde",
        // FinancialTransactionType
        ["Expense"] = "Gider",
        ["Income"] = "Gelir",
        ["Payable"] = "Borç",
        ["Receivable"] = "Alacak",
        ["Payment"] = "Ödeme",
        ["Collection"] = "Tahsilat",
        // WarehouseType
        ["Central"] = "Merkez",
        ["ProjectSite"] = "Proje Sahası",
        ["Temporary"] = "Geçici",
        ["Vehicle"] = "Araç",
        ["Consignment"] = "Konsinye",
        // PartnerType / ContractType
        ["Customer"] = "Müşteri",
        ["Supplier"] = "Tedarikçi",
        ["Subcontractor"] = "Taşeron",
        ["Other"] = "Diğer",
        ["Rental"] = "Kiralama",
        ["Service"] = "Hizmet",
        // ChatGroupMemberStatus
        ["Accepted"] = "Kabul Edildi",
        ["Declined"] = "Reddedildi",
    };

    private static readonly Dictionary<string, string> En = new(StringComparer.Ordinal)
    {
        ["Draft"] = "Draft",
        ["Pending"] = "Pending",
        ["PendingApproval"] = "Pending Approval",
        ["Approved"] = "Approved",
        ["Rejected"] = "Rejected",
        ["Returned"] = "Returned",
        ["Cancelled"] = "Cancelled",
        ["Closed"] = "Closed",
        ["Ordered"] = "Ordered",
        ["Waiting"] = "Waiting",
        ["Active"] = "Active",
        ["Skipped"] = "Skipped",
        ["Delegated"] = "Delegated",
        ["PartiallyReceived"] = "Partially Received",
        ["Received"] = "Received",
        ["Assigned"] = "Assigned",
        ["InProgress"] = "In Progress",
        ["OnHold"] = "On Hold",
        ["Completed"] = "Completed",
        ["Approve"] = "Approve",
        ["Reject"] = "Reject",
        ["Return"] = "Return",
        ["Cancel"] = "Cancel",
        ["Sequential"] = "Sequential",
        ["ParallelAny"] = "Parallel (Any)",
        ["ParallelAll"] = "Parallel (All)",
        ["Quorum"] = "Quorum",
        ["User"] = "User",
        ["Role"] = "Role",
        ["ProjectRole"] = "Project Role",
        ["DepartmentManager"] = "Department Manager",
        ["Equals"] = "Equals",
        ["NotEquals"] = "Not Equals",
        ["GreaterThan"] = "Greater Than",
        ["GreaterThanOrEqual"] = "Greater Than or Equal",
        ["LessThan"] = "Less Than",
        ["LessThanOrEqual"] = "Less Than or Equal",
        ["In"] = "In",
        ["Expense"] = "Expense",
        ["Income"] = "Income",
        ["Payable"] = "Payable",
        ["Receivable"] = "Receivable",
        ["Payment"] = "Payment",
        ["Collection"] = "Collection",
        ["Central"] = "Central",
        ["ProjectSite"] = "Project Site",
        ["Temporary"] = "Temporary",
        ["Vehicle"] = "Vehicle",
        ["Consignment"] = "Consignment",
        ["Customer"] = "Customer",
        ["Supplier"] = "Supplier",
        ["Subcontractor"] = "Subcontractor",
        ["Other"] = "Other",
        ["Rental"] = "Rental",
        ["Service"] = "Service",
        ["Accepted"] = "Accepted",
        ["Declined"] = "Declined",
    };

    public static IReadOnlyDictionary<string, string> ForCulture(CultureInfo? culture)
    {
        var name = culture?.Name ?? string.Empty;
        return name.StartsWith("tr", StringComparison.OrdinalIgnoreCase) ? Tr : En;
    }
}

