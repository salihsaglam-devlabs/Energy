namespace Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;

/// <summary>WorkOrderMaterialPlan güncelleme isteği.</summary>
public class UpdateWorkOrderMaterialPlanRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>PlannedQuantity</summary>
    public decimal PlannedQuantity { get; set; }
}
