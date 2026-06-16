namespace Energy.Domain.Common;

/// <summary>
/// Lookup Catalogue sayfasındaki teknik değerler. Veritabanında nvarchar olarak
/// (string dönüşümüyle) saklanır; magic string kullanılmaz.
/// </summary>
public enum ApprovalMode
{
    Sequential = 1,
    ParallelAny = 2,
    ParallelAll = 3,
    Quorum = 4,
}

/// <summary>Onaycı türü.</summary>
public enum ApproverType
{
    User = 1,
    Role = 2,
    ProjectRole = 3,
    DepartmentManager = 4,
}

/// <summary>Onay koşulu operatörü.</summary>
public enum ConditionOperator
{
    Equals = 1,
    NotEquals = 2,
    GreaterThan = 3,
    GreaterThanOrEqual = 4,
    LessThan = 5,
    LessThanOrEqual = 6,
    In = 7,
}

/// <summary>Onay aksiyon türü.</summary>
public enum ApprovalActionType
{
    Approve = 1,
    Reject = 2,
    Return = 3,
    Cancel = 4,
}

/// <summary>Onay talebi durumu.</summary>
public enum ApprovalRequestStatus
{
    Draft = 1,
    Pending = 2,
    Approved = 3,
    Rejected = 4,
    Returned = 5,
    Cancelled = 6,
}

/// <summary>Onay adımı durumu.</summary>
public enum ApprovalStepStatus
{
    Waiting = 1,
    Active = 2,
    Approved = 3,
    Rejected = 4,
    Returned = 5,
    Skipped = 6,
}

/// <summary>Onaycı durumu.</summary>
public enum ApprovalApproverStatus
{
    Waiting = 1,
    Approved = 2,
    Rejected = 3,
    Delegated = 4,
}

/// <summary>Belge durumu (stok belgeleri gibi).</summary>
public enum DocumentStatus
{
    Draft = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5,
    Closed = 6,
}

/// <summary>Talep durumu.</summary>
public enum RequestStatus
{
    Draft = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    Ordered = 5,
    Closed = 6,
}

/// <summary>Satın alma siparişi durumu.</summary>
public enum PurchaseOrderStatus
{
    Draft = 1,
    Approved = 2,
    PartiallyReceived = 3,
    Received = 4,
    Cancelled = 5,
}

/// <summary>İş emri durumu.</summary>
public enum WorkOrderStatus
{
    Draft = 1,
    Assigned = 2,
    InProgress = 3,
    OnHold = 4,
    Completed = 5,
    Closed = 6,
}

/// <summary>Finansal hareket türü.</summary>
public enum FinancialTransactionType
{
    Expense = 1,
    Income = 2,
    Payable = 3,
    Receivable = 4,
    Payment = 5,
    Collection = 6,
}

/// <summary>Depo türü.</summary>
public enum WarehouseType
{
    Central = 1,
    ProjectSite = 2,
    Temporary = 3,
    Vehicle = 4,
    Consignment = 5,
}

/// <summary>Cari taraf türü.</summary>
public enum PartnerType
{
    Customer = 1,
    Supplier = 2,
    Subcontractor = 3,
    Other = 4,
}

/// <summary>Sözleşme türü.</summary>
public enum ContractType
{
    Customer = 1,
    Supplier = 2,
    Subcontractor = 3,
    Rental = 4,
    Service = 5,
}

