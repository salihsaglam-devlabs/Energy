// ScreenKeys: ekran bazlı yerelleştirme anahtarları (otomatik üretildi).
//
// Her ekranın TÜM çeviri anahtarları kendi nested sınıfında listelenir;
// böylece bir ekranın çeviri kapsamı tek bir yerde açıkça görülür ve yönetilebilir.
// Ortak anahtar yoktur: aynı metin farklı ekranlarda farklı anahtarla tutulur.

namespace Energy.Localization;

public static class ScreenKeys
{
    /// <summary>Assets.EquipmentAsset — Ekipman Varlığı / Equipment Asset</summary>
    public static class Assets_EquipmentAsset
    {
        public const string ScreenId = "Assets.EquipmentAsset";
        public const string Title = "Assets.EquipmentAsset.Title";
        public const string Description = "Assets.EquipmentAsset.Description";
        public static class Columns
        {
            public const string companyId = "Assets.EquipmentAsset.Columns.companyId";
            public const string code = "Assets.EquipmentAsset.Columns.code";
            public const string name = "Assets.EquipmentAsset.Columns.name";
            public const string assetType = "Assets.EquipmentAsset.Columns.assetType";
            public const string serialNo = "Assets.EquipmentAsset.Columns.serialNo";
            public const string purchaseDate = "Assets.EquipmentAsset.Columns.purchaseDate";
            public const string isActive = "Assets.EquipmentAsset.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Assets.EquipmentAsset.Actions.New";
            public const string Edit = "Assets.EquipmentAsset.Actions.Edit";
            public const string Delete = "Assets.EquipmentAsset.Actions.Delete";
            public const string Save = "Assets.EquipmentAsset.Actions.Save";
            public const string Cancel = "Assets.EquipmentAsset.Actions.Cancel";
            public const string Export = "Assets.EquipmentAsset.Actions.Export";
            public const string Refresh = "Assets.EquipmentAsset.Actions.Refresh";
            public const string ColumnChooser = "Assets.EquipmentAsset.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Assets.EquipmentAsset.Grid.Search";
            public const string NoData = "Assets.EquipmentAsset.Grid.NoData";
            public const string Loading = "Assets.EquipmentAsset.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Assets.EquipmentAsset.Notifications.Saved";
            public const string Updated = "Assets.EquipmentAsset.Notifications.Updated";
            public const string Deleted = "Assets.EquipmentAsset.Notifications.Deleted";
            public const string Error = "Assets.EquipmentAsset.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Assets.EquipmentAsset.Popup.CreateTitle";
            public const string EditTitle = "Assets.EquipmentAsset.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Assets.EquipmentAsset.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Assets.EquipmentAsset.Confirm.Delete";
        }
    }

    /// <summary>Assets.EquipmentAssignment — Ekipman Ataması / Equipment Assignment</summary>
    public static class Assets_EquipmentAssignment
    {
        public const string ScreenId = "Assets.EquipmentAssignment";
        public const string Title = "Assets.EquipmentAssignment.Title";
        public const string Description = "Assets.EquipmentAssignment.Description";
        public static class Columns
        {
            public const string equipmentAssetId = "Assets.EquipmentAssignment.Columns.equipmentAssetId";
            public const string projectId = "Assets.EquipmentAssignment.Columns.projectId";
            public const string employeeId = "Assets.EquipmentAssignment.Columns.employeeId";
            public const string warehouseId = "Assets.EquipmentAssignment.Columns.warehouseId";
            public const string startDate = "Assets.EquipmentAssignment.Columns.startDate";
            public const string endDate = "Assets.EquipmentAssignment.Columns.endDate";
            public const string isActive = "Assets.EquipmentAssignment.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Assets.EquipmentAssignment.Actions.New";
            public const string Edit = "Assets.EquipmentAssignment.Actions.Edit";
            public const string Delete = "Assets.EquipmentAssignment.Actions.Delete";
            public const string Save = "Assets.EquipmentAssignment.Actions.Save";
            public const string Cancel = "Assets.EquipmentAssignment.Actions.Cancel";
            public const string Export = "Assets.EquipmentAssignment.Actions.Export";
            public const string Refresh = "Assets.EquipmentAssignment.Actions.Refresh";
            public const string ColumnChooser = "Assets.EquipmentAssignment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Assets.EquipmentAssignment.Grid.Search";
            public const string NoData = "Assets.EquipmentAssignment.Grid.NoData";
            public const string Loading = "Assets.EquipmentAssignment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Assets.EquipmentAssignment.Notifications.Saved";
            public const string Updated = "Assets.EquipmentAssignment.Notifications.Updated";
            public const string Deleted = "Assets.EquipmentAssignment.Notifications.Deleted";
            public const string Error = "Assets.EquipmentAssignment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Assets.EquipmentAssignment.Popup.CreateTitle";
            public const string EditTitle = "Assets.EquipmentAssignment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Assets.EquipmentAssignment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Assets.EquipmentAssignment.Confirm.Delete";
        }
    }

    /// <summary>Assets.EquipmentMaintenance — Ekipman Bakımı / Equipment Maintenance</summary>
    public static class Assets_EquipmentMaintenance
    {
        public const string ScreenId = "Assets.EquipmentMaintenance";
        public const string Title = "Assets.EquipmentMaintenance.Title";
        public const string Description = "Assets.EquipmentMaintenance.Description";
        public static class Columns
        {
            public const string equipmentAssetId = "Assets.EquipmentMaintenance.Columns.equipmentAssetId";
            public const string maintenanceType = "Assets.EquipmentMaintenance.Columns.maintenanceType";
            public const string scheduledDate = "Assets.EquipmentMaintenance.Columns.scheduledDate";
            public const string completedDate = "Assets.EquipmentMaintenance.Columns.completedDate";
            public const string cost = "Assets.EquipmentMaintenance.Columns.cost";
            public const string note = "Assets.EquipmentMaintenance.Columns.note";
        }
        public static class Actions
        {
            public const string New = "Assets.EquipmentMaintenance.Actions.New";
            public const string Edit = "Assets.EquipmentMaintenance.Actions.Edit";
            public const string Delete = "Assets.EquipmentMaintenance.Actions.Delete";
            public const string Save = "Assets.EquipmentMaintenance.Actions.Save";
            public const string Cancel = "Assets.EquipmentMaintenance.Actions.Cancel";
            public const string Export = "Assets.EquipmentMaintenance.Actions.Export";
            public const string Refresh = "Assets.EquipmentMaintenance.Actions.Refresh";
            public const string ColumnChooser = "Assets.EquipmentMaintenance.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Assets.EquipmentMaintenance.Grid.Search";
            public const string NoData = "Assets.EquipmentMaintenance.Grid.NoData";
            public const string Loading = "Assets.EquipmentMaintenance.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Assets.EquipmentMaintenance.Notifications.Saved";
            public const string Updated = "Assets.EquipmentMaintenance.Notifications.Updated";
            public const string Deleted = "Assets.EquipmentMaintenance.Notifications.Deleted";
            public const string Error = "Assets.EquipmentMaintenance.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Assets.EquipmentMaintenance.Popup.CreateTitle";
            public const string EditTitle = "Assets.EquipmentMaintenance.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Assets.EquipmentMaintenance.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Assets.EquipmentMaintenance.Confirm.Delete";
        }
    }

    /// <summary>Budget.Budget — Bü#tçe / Budget</summary>
    public static class Budget_Budget
    {
        public const string ScreenId = "Budget.Budget";
        public const string Title = "Budget.Budget.Title";
        public const string Description = "Budget.Budget.Description";
        public static class Columns
        {
            public const string projectId = "Budget.Budget.Columns.projectId";
            public const string costCenterId = "Budget.Budget.Columns.costCenterId";
            public const string currencyId = "Budget.Budget.Columns.currencyId";
            public const string name = "Budget.Budget.Columns.name";
            public const string year = "Budget.Budget.Columns.year";
            public const string isActive = "Budget.Budget.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Budget.Budget.Actions.New";
            public const string Edit = "Budget.Budget.Actions.Edit";
            public const string Delete = "Budget.Budget.Actions.Delete";
            public const string Save = "Budget.Budget.Actions.Save";
            public const string Cancel = "Budget.Budget.Actions.Cancel";
            public const string Export = "Budget.Budget.Actions.Export";
            public const string Refresh = "Budget.Budget.Actions.Refresh";
            public const string ColumnChooser = "Budget.Budget.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Budget.Budget.Grid.Search";
            public const string NoData = "Budget.Budget.Grid.NoData";
            public const string Loading = "Budget.Budget.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Budget.Budget.Notifications.Saved";
            public const string Updated = "Budget.Budget.Notifications.Updated";
            public const string Deleted = "Budget.Budget.Notifications.Deleted";
            public const string Error = "Budget.Budget.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Budget.Budget.Popup.CreateTitle";
            public const string EditTitle = "Budget.Budget.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Budget.Budget.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Budget.Budget.Confirm.Delete";
        }
    }

    /// <summary>Budget.BudgetLine — Bü#tçe Kalemi / Budget Line</summary>
    public static class Budget_BudgetLine
    {
        public const string ScreenId = "Budget.BudgetLine";
        public const string Title = "Budget.BudgetLine.Title";
        public const string Description = "Budget.BudgetLine.Description";
        public static class Columns
        {
            public const string budgetId = "Budget.BudgetLine.Columns.budgetId";
            public const string projectId = "Budget.BudgetLine.Columns.projectId";
            public const string costCenterId = "Budget.BudgetLine.Columns.costCenterId";
            public const string description = "Budget.BudgetLine.Columns.description";
            public const string plannedAmount = "Budget.BudgetLine.Columns.plannedAmount";
        }
        public static class Actions
        {
            public const string New = "Budget.BudgetLine.Actions.New";
            public const string Edit = "Budget.BudgetLine.Actions.Edit";
            public const string Delete = "Budget.BudgetLine.Actions.Delete";
            public const string Save = "Budget.BudgetLine.Actions.Save";
            public const string Cancel = "Budget.BudgetLine.Actions.Cancel";
            public const string Export = "Budget.BudgetLine.Actions.Export";
            public const string Refresh = "Budget.BudgetLine.Actions.Refresh";
            public const string ColumnChooser = "Budget.BudgetLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Budget.BudgetLine.Grid.Search";
            public const string NoData = "Budget.BudgetLine.Grid.NoData";
            public const string Loading = "Budget.BudgetLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Budget.BudgetLine.Notifications.Saved";
            public const string Updated = "Budget.BudgetLine.Notifications.Updated";
            public const string Deleted = "Budget.BudgetLine.Notifications.Deleted";
            public const string Error = "Budget.BudgetLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Budget.BudgetLine.Popup.CreateTitle";
            public const string EditTitle = "Budget.BudgetLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Budget.BudgetLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Budget.BudgetLine.Confirm.Delete";
        }
    }

    /// <summary>BusinessPartners.BusinessPartner — İş Ortağı / Business Partner</summary>
    public static class BusinessPartners_BusinessPartner
    {
        public const string ScreenId = "BusinessPartners.BusinessPartner";
        public const string Title = "BusinessPartners.BusinessPartner.Title";
        public const string Description = "BusinessPartners.BusinessPartner.Description";
        public static class Columns
        {
            public const string partnerType = "BusinessPartners.BusinessPartner.Columns.partnerType";
            public const string code = "BusinessPartners.BusinessPartner.Columns.code";
            public const string name = "BusinessPartners.BusinessPartner.Columns.name";
            public const string taxNumber = "BusinessPartners.BusinessPartner.Columns.taxNumber";
            public const string taxOffice = "BusinessPartners.BusinessPartner.Columns.taxOffice";
            public const string phone = "BusinessPartners.BusinessPartner.Columns.phone";
            public const string email = "BusinessPartners.BusinessPartner.Columns.email";
            public const string isActive = "BusinessPartners.BusinessPartner.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "BusinessPartners.BusinessPartner.Actions.New";
            public const string Edit = "BusinessPartners.BusinessPartner.Actions.Edit";
            public const string Delete = "BusinessPartners.BusinessPartner.Actions.Delete";
            public const string Save = "BusinessPartners.BusinessPartner.Actions.Save";
            public const string Cancel = "BusinessPartners.BusinessPartner.Actions.Cancel";
            public const string Export = "BusinessPartners.BusinessPartner.Actions.Export";
            public const string Refresh = "BusinessPartners.BusinessPartner.Actions.Refresh";
            public const string ColumnChooser = "BusinessPartners.BusinessPartner.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "BusinessPartners.BusinessPartner.Grid.Search";
            public const string NoData = "BusinessPartners.BusinessPartner.Grid.NoData";
            public const string Loading = "BusinessPartners.BusinessPartner.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "BusinessPartners.BusinessPartner.Notifications.Saved";
            public const string Updated = "BusinessPartners.BusinessPartner.Notifications.Updated";
            public const string Deleted = "BusinessPartners.BusinessPartner.Notifications.Deleted";
            public const string Error = "BusinessPartners.BusinessPartner.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "BusinessPartners.BusinessPartner.Popup.CreateTitle";
            public const string EditTitle = "BusinessPartners.BusinessPartner.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "BusinessPartners.BusinessPartner.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "BusinessPartners.BusinessPartner.Confirm.Delete";
        }
    }

    /// <summary>BusinessPartners.BusinessPartnerAddress — İş Ortağı Adresi / Business Partner Address</summary>
    public static class BusinessPartners_BusinessPartnerAddress
    {
        public const string ScreenId = "BusinessPartners.BusinessPartnerAddress";
        public const string Title = "BusinessPartners.BusinessPartnerAddress.Title";
        public const string Description = "BusinessPartners.BusinessPartnerAddress.Description";
        public static class Columns
        {
            public const string businessPartnerId = "BusinessPartners.BusinessPartnerAddress.Columns.businessPartnerId";
            public const string addressType = "BusinessPartners.BusinessPartnerAddress.Columns.addressType";
            public const string addressLine = "BusinessPartners.BusinessPartnerAddress.Columns.addressLine";
            public const string city = "BusinessPartners.BusinessPartnerAddress.Columns.city";
            public const string country = "BusinessPartners.BusinessPartnerAddress.Columns.country";
            public const string postalCode = "BusinessPartners.BusinessPartnerAddress.Columns.postalCode";
            public const string isPrimary = "BusinessPartners.BusinessPartnerAddress.Columns.isPrimary";
        }
        public static class Actions
        {
            public const string New = "BusinessPartners.BusinessPartnerAddress.Actions.New";
            public const string Edit = "BusinessPartners.BusinessPartnerAddress.Actions.Edit";
            public const string Delete = "BusinessPartners.BusinessPartnerAddress.Actions.Delete";
            public const string Save = "BusinessPartners.BusinessPartnerAddress.Actions.Save";
            public const string Cancel = "BusinessPartners.BusinessPartnerAddress.Actions.Cancel";
            public const string Export = "BusinessPartners.BusinessPartnerAddress.Actions.Export";
            public const string Refresh = "BusinessPartners.BusinessPartnerAddress.Actions.Refresh";
            public const string ColumnChooser = "BusinessPartners.BusinessPartnerAddress.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "BusinessPartners.BusinessPartnerAddress.Grid.Search";
            public const string NoData = "BusinessPartners.BusinessPartnerAddress.Grid.NoData";
            public const string Loading = "BusinessPartners.BusinessPartnerAddress.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "BusinessPartners.BusinessPartnerAddress.Notifications.Saved";
            public const string Updated = "BusinessPartners.BusinessPartnerAddress.Notifications.Updated";
            public const string Deleted = "BusinessPartners.BusinessPartnerAddress.Notifications.Deleted";
            public const string Error = "BusinessPartners.BusinessPartnerAddress.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "BusinessPartners.BusinessPartnerAddress.Popup.CreateTitle";
            public const string EditTitle = "BusinessPartners.BusinessPartnerAddress.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "BusinessPartners.BusinessPartnerAddress.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "BusinessPartners.BusinessPartnerAddress.Confirm.Delete";
        }
    }

    /// <summary>BusinessPartners.BusinessPartnerBankAccount — İş Ortağı Banka Hesabı / Business Partner Bank Account</summary>
    public static class BusinessPartners_BusinessPartnerBankAccount
    {
        public const string ScreenId = "BusinessPartners.BusinessPartnerBankAccount";
        public const string Title = "BusinessPartners.BusinessPartnerBankAccount.Title";
        public const string Description = "BusinessPartners.BusinessPartnerBankAccount.Description";
        public static class Columns
        {
            public const string businessPartnerId = "BusinessPartners.BusinessPartnerBankAccount.Columns.businessPartnerId";
            public const string bankName = "BusinessPartners.BusinessPartnerBankAccount.Columns.bankName";
            public const string branch = "BusinessPartners.BusinessPartnerBankAccount.Columns.branch";
            public const string iban = "BusinessPartners.BusinessPartnerBankAccount.Columns.iban";
            public const string currencyId = "BusinessPartners.BusinessPartnerBankAccount.Columns.currencyId";
            public const string isPrimary = "BusinessPartners.BusinessPartnerBankAccount.Columns.isPrimary";
        }
        public static class Actions
        {
            public const string New = "BusinessPartners.BusinessPartnerBankAccount.Actions.New";
            public const string Edit = "BusinessPartners.BusinessPartnerBankAccount.Actions.Edit";
            public const string Delete = "BusinessPartners.BusinessPartnerBankAccount.Actions.Delete";
            public const string Save = "BusinessPartners.BusinessPartnerBankAccount.Actions.Save";
            public const string Cancel = "BusinessPartners.BusinessPartnerBankAccount.Actions.Cancel";
            public const string Export = "BusinessPartners.BusinessPartnerBankAccount.Actions.Export";
            public const string Refresh = "BusinessPartners.BusinessPartnerBankAccount.Actions.Refresh";
            public const string ColumnChooser = "BusinessPartners.BusinessPartnerBankAccount.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "BusinessPartners.BusinessPartnerBankAccount.Grid.Search";
            public const string NoData = "BusinessPartners.BusinessPartnerBankAccount.Grid.NoData";
            public const string Loading = "BusinessPartners.BusinessPartnerBankAccount.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "BusinessPartners.BusinessPartnerBankAccount.Notifications.Saved";
            public const string Updated = "BusinessPartners.BusinessPartnerBankAccount.Notifications.Updated";
            public const string Deleted = "BusinessPartners.BusinessPartnerBankAccount.Notifications.Deleted";
            public const string Error = "BusinessPartners.BusinessPartnerBankAccount.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "BusinessPartners.BusinessPartnerBankAccount.Popup.CreateTitle";
            public const string EditTitle = "BusinessPartners.BusinessPartnerBankAccount.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "BusinessPartners.BusinessPartnerBankAccount.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "BusinessPartners.BusinessPartnerBankAccount.Confirm.Delete";
        }
    }

    /// <summary>BusinessPartners.BusinessPartnerContact — İş Ortağı İletişim Kişisi / Business Partner Contact</summary>
    public static class BusinessPartners_BusinessPartnerContact
    {
        public const string ScreenId = "BusinessPartners.BusinessPartnerContact";
        public const string Title = "BusinessPartners.BusinessPartnerContact.Title";
        public const string Description = "BusinessPartners.BusinessPartnerContact.Description";
        public static class Columns
        {
            public const string businessPartnerId = "BusinessPartners.BusinessPartnerContact.Columns.businessPartnerId";
            public const string fullName = "BusinessPartners.BusinessPartnerContact.Columns.fullName";
            public const string title = "BusinessPartners.BusinessPartnerContact.Columns.title";
            public const string phone = "BusinessPartners.BusinessPartnerContact.Columns.phone";
            public const string email = "BusinessPartners.BusinessPartnerContact.Columns.email";
            public const string isPrimary = "BusinessPartners.BusinessPartnerContact.Columns.isPrimary";
        }
        public static class Actions
        {
            public const string New = "BusinessPartners.BusinessPartnerContact.Actions.New";
            public const string Edit = "BusinessPartners.BusinessPartnerContact.Actions.Edit";
            public const string Delete = "BusinessPartners.BusinessPartnerContact.Actions.Delete";
            public const string Save = "BusinessPartners.BusinessPartnerContact.Actions.Save";
            public const string Cancel = "BusinessPartners.BusinessPartnerContact.Actions.Cancel";
            public const string Export = "BusinessPartners.BusinessPartnerContact.Actions.Export";
            public const string Refresh = "BusinessPartners.BusinessPartnerContact.Actions.Refresh";
            public const string ColumnChooser = "BusinessPartners.BusinessPartnerContact.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "BusinessPartners.BusinessPartnerContact.Grid.Search";
            public const string NoData = "BusinessPartners.BusinessPartnerContact.Grid.NoData";
            public const string Loading = "BusinessPartners.BusinessPartnerContact.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "BusinessPartners.BusinessPartnerContact.Notifications.Saved";
            public const string Updated = "BusinessPartners.BusinessPartnerContact.Notifications.Updated";
            public const string Deleted = "BusinessPartners.BusinessPartnerContact.Notifications.Deleted";
            public const string Error = "BusinessPartners.BusinessPartnerContact.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "BusinessPartners.BusinessPartnerContact.Popup.CreateTitle";
            public const string EditTitle = "BusinessPartners.BusinessPartnerContact.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "BusinessPartners.BusinessPartnerContact.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "BusinessPartners.BusinessPartnerContact.Confirm.Delete";
        }
    }

    /// <summary>Catalog.Brand — Marka / Brand</summary>
    public static class Catalog_Brand
    {
        public const string ScreenId = "Catalog.Brand";
        public const string Title = "Catalog.Brand.Title";
        public const string Description = "Catalog.Brand.Description";
        public static class Columns
        {
            public const string code = "Catalog.Brand.Columns.code";
            public const string name = "Catalog.Brand.Columns.name";
            public const string isActive = "Catalog.Brand.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Catalog.Brand.Actions.New";
            public const string Edit = "Catalog.Brand.Actions.Edit";
            public const string Delete = "Catalog.Brand.Actions.Delete";
            public const string Save = "Catalog.Brand.Actions.Save";
            public const string Cancel = "Catalog.Brand.Actions.Cancel";
            public const string Export = "Catalog.Brand.Actions.Export";
            public const string Refresh = "Catalog.Brand.Actions.Refresh";
            public const string ColumnChooser = "Catalog.Brand.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.Brand.Grid.Search";
            public const string NoData = "Catalog.Brand.Grid.NoData";
            public const string Loading = "Catalog.Brand.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.Brand.Notifications.Saved";
            public const string Updated = "Catalog.Brand.Notifications.Updated";
            public const string Deleted = "Catalog.Brand.Notifications.Deleted";
            public const string Error = "Catalog.Brand.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.Brand.Popup.CreateTitle";
            public const string EditTitle = "Catalog.Brand.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.Brand.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.Brand.Confirm.Delete";
        }
    }

    /// <summary>Catalog.Material — Malzeme / Material</summary>
    public static class Catalog_Material
    {
        public const string ScreenId = "Catalog.Material";
        public const string Title = "Catalog.Material.Title";
        public const string Description = "Catalog.Material.Description";
        public static class Columns
        {
            public const string materialCategoryId = "Catalog.Material.Columns.materialCategoryId";
            public const string brandId = "Catalog.Material.Columns.brandId";
            public const string baseUnitOfMeasureId = "Catalog.Material.Columns.baseUnitOfMeasureId";
            public const string code = "Catalog.Material.Columns.code";
            public const string name = "Catalog.Material.Columns.name";
            public const string isBatchTracked = "Catalog.Material.Columns.isBatchTracked";
            public const string isSerialTracked = "Catalog.Material.Columns.isSerialTracked";
            public const string isActive = "Catalog.Material.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Catalog.Material.Actions.New";
            public const string Edit = "Catalog.Material.Actions.Edit";
            public const string Delete = "Catalog.Material.Actions.Delete";
            public const string Save = "Catalog.Material.Actions.Save";
            public const string Cancel = "Catalog.Material.Actions.Cancel";
            public const string Export = "Catalog.Material.Actions.Export";
            public const string Refresh = "Catalog.Material.Actions.Refresh";
            public const string ColumnChooser = "Catalog.Material.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.Material.Grid.Search";
            public const string NoData = "Catalog.Material.Grid.NoData";
            public const string Loading = "Catalog.Material.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.Material.Notifications.Saved";
            public const string Updated = "Catalog.Material.Notifications.Updated";
            public const string Deleted = "Catalog.Material.Notifications.Deleted";
            public const string Error = "Catalog.Material.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.Material.Popup.CreateTitle";
            public const string EditTitle = "Catalog.Material.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.Material.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.Material.Confirm.Delete";
        }
    }

    /// <summary>Catalog.MaterialAttributeDefinition — Malzeme Özellik Tanımı / Material Attribute Definition</summary>
    public static class Catalog_MaterialAttributeDefinition
    {
        public const string ScreenId = "Catalog.MaterialAttributeDefinition";
        public const string Title = "Catalog.MaterialAttributeDefinition.Title";
        public const string Description = "Catalog.MaterialAttributeDefinition.Description";
        public static class Columns
        {
            public const string code = "Catalog.MaterialAttributeDefinition.Columns.code";
            public const string name = "Catalog.MaterialAttributeDefinition.Columns.name";
            public const string dataType = "Catalog.MaterialAttributeDefinition.Columns.dataType";
            public const string isActive = "Catalog.MaterialAttributeDefinition.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Catalog.MaterialAttributeDefinition.Actions.New";
            public const string Edit = "Catalog.MaterialAttributeDefinition.Actions.Edit";
            public const string Delete = "Catalog.MaterialAttributeDefinition.Actions.Delete";
            public const string Save = "Catalog.MaterialAttributeDefinition.Actions.Save";
            public const string Cancel = "Catalog.MaterialAttributeDefinition.Actions.Cancel";
            public const string Export = "Catalog.MaterialAttributeDefinition.Actions.Export";
            public const string Refresh = "Catalog.MaterialAttributeDefinition.Actions.Refresh";
            public const string ColumnChooser = "Catalog.MaterialAttributeDefinition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.MaterialAttributeDefinition.Grid.Search";
            public const string NoData = "Catalog.MaterialAttributeDefinition.Grid.NoData";
            public const string Loading = "Catalog.MaterialAttributeDefinition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.MaterialAttributeDefinition.Notifications.Saved";
            public const string Updated = "Catalog.MaterialAttributeDefinition.Notifications.Updated";
            public const string Deleted = "Catalog.MaterialAttributeDefinition.Notifications.Deleted";
            public const string Error = "Catalog.MaterialAttributeDefinition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.MaterialAttributeDefinition.Popup.CreateTitle";
            public const string EditTitle = "Catalog.MaterialAttributeDefinition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.MaterialAttributeDefinition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.MaterialAttributeDefinition.Confirm.Delete";
        }
    }

    /// <summary>Catalog.MaterialAttributeOption — Malzeme Özellik Seçeneği / Material Attribute Option</summary>
    public static class Catalog_MaterialAttributeOption
    {
        public const string ScreenId = "Catalog.MaterialAttributeOption";
        public const string Title = "Catalog.MaterialAttributeOption.Title";
        public const string Description = "Catalog.MaterialAttributeOption.Description";
        public static class Columns
        {
            public const string materialAttributeDefinitionId = "Catalog.MaterialAttributeOption.Columns.materialAttributeDefinitionId";
            public const string value = "Catalog.MaterialAttributeOption.Columns.value";
            public const string displayOrder = "Catalog.MaterialAttributeOption.Columns.displayOrder";
        }
        public static class Actions
        {
            public const string New = "Catalog.MaterialAttributeOption.Actions.New";
            public const string Edit = "Catalog.MaterialAttributeOption.Actions.Edit";
            public const string Delete = "Catalog.MaterialAttributeOption.Actions.Delete";
            public const string Save = "Catalog.MaterialAttributeOption.Actions.Save";
            public const string Cancel = "Catalog.MaterialAttributeOption.Actions.Cancel";
            public const string Export = "Catalog.MaterialAttributeOption.Actions.Export";
            public const string Refresh = "Catalog.MaterialAttributeOption.Actions.Refresh";
            public const string ColumnChooser = "Catalog.MaterialAttributeOption.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.MaterialAttributeOption.Grid.Search";
            public const string NoData = "Catalog.MaterialAttributeOption.Grid.NoData";
            public const string Loading = "Catalog.MaterialAttributeOption.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.MaterialAttributeOption.Notifications.Saved";
            public const string Updated = "Catalog.MaterialAttributeOption.Notifications.Updated";
            public const string Deleted = "Catalog.MaterialAttributeOption.Notifications.Deleted";
            public const string Error = "Catalog.MaterialAttributeOption.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.MaterialAttributeOption.Popup.CreateTitle";
            public const string EditTitle = "Catalog.MaterialAttributeOption.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.MaterialAttributeOption.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.MaterialAttributeOption.Confirm.Delete";
        }
    }

    /// <summary>Catalog.MaterialAttributeValue — Malzeme Özellik Değeri / Material Attribute Value</summary>
    public static class Catalog_MaterialAttributeValue
    {
        public const string ScreenId = "Catalog.MaterialAttributeValue";
        public const string Title = "Catalog.MaterialAttributeValue.Title";
        public const string Description = "Catalog.MaterialAttributeValue.Description";
        public static class Columns
        {
            public const string materialId = "Catalog.MaterialAttributeValue.Columns.materialId";
            public const string materialAttributeDefinitionId = "Catalog.MaterialAttributeValue.Columns.materialAttributeDefinitionId";
            public const string optionId = "Catalog.MaterialAttributeValue.Columns.optionId";
            public const string valueText = "Catalog.MaterialAttributeValue.Columns.valueText";
            public const string valueNumber = "Catalog.MaterialAttributeValue.Columns.valueNumber";
            public const string valueBoolean = "Catalog.MaterialAttributeValue.Columns.valueBoolean";
            public const string valueDate = "Catalog.MaterialAttributeValue.Columns.valueDate";
        }
        public static class Actions
        {
            public const string New = "Catalog.MaterialAttributeValue.Actions.New";
            public const string Edit = "Catalog.MaterialAttributeValue.Actions.Edit";
            public const string Delete = "Catalog.MaterialAttributeValue.Actions.Delete";
            public const string Save = "Catalog.MaterialAttributeValue.Actions.Save";
            public const string Cancel = "Catalog.MaterialAttributeValue.Actions.Cancel";
            public const string Export = "Catalog.MaterialAttributeValue.Actions.Export";
            public const string Refresh = "Catalog.MaterialAttributeValue.Actions.Refresh";
            public const string ColumnChooser = "Catalog.MaterialAttributeValue.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.MaterialAttributeValue.Grid.Search";
            public const string NoData = "Catalog.MaterialAttributeValue.Grid.NoData";
            public const string Loading = "Catalog.MaterialAttributeValue.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.MaterialAttributeValue.Notifications.Saved";
            public const string Updated = "Catalog.MaterialAttributeValue.Notifications.Updated";
            public const string Deleted = "Catalog.MaterialAttributeValue.Notifications.Deleted";
            public const string Error = "Catalog.MaterialAttributeValue.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.MaterialAttributeValue.Popup.CreateTitle";
            public const string EditTitle = "Catalog.MaterialAttributeValue.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.MaterialAttributeValue.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.MaterialAttributeValue.Confirm.Delete";
        }
    }

    /// <summary>Catalog.MaterialCategory — Malzeme Kategorisi / Material Category</summary>
    public static class Catalog_MaterialCategory
    {
        public const string ScreenId = "Catalog.MaterialCategory";
        public const string Title = "Catalog.MaterialCategory.Title";
        public const string Description = "Catalog.MaterialCategory.Description";
        public static class Columns
        {
            public const string parentCategoryId = "Catalog.MaterialCategory.Columns.parentCategoryId";
            public const string code = "Catalog.MaterialCategory.Columns.code";
            public const string name = "Catalog.MaterialCategory.Columns.name";
            public const string isActive = "Catalog.MaterialCategory.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Catalog.MaterialCategory.Actions.New";
            public const string Edit = "Catalog.MaterialCategory.Actions.Edit";
            public const string Delete = "Catalog.MaterialCategory.Actions.Delete";
            public const string Save = "Catalog.MaterialCategory.Actions.Save";
            public const string Cancel = "Catalog.MaterialCategory.Actions.Cancel";
            public const string Export = "Catalog.MaterialCategory.Actions.Export";
            public const string Refresh = "Catalog.MaterialCategory.Actions.Refresh";
            public const string ColumnChooser = "Catalog.MaterialCategory.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.MaterialCategory.Grid.Search";
            public const string NoData = "Catalog.MaterialCategory.Grid.NoData";
            public const string Loading = "Catalog.MaterialCategory.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.MaterialCategory.Notifications.Saved";
            public const string Updated = "Catalog.MaterialCategory.Notifications.Updated";
            public const string Deleted = "Catalog.MaterialCategory.Notifications.Deleted";
            public const string Error = "Catalog.MaterialCategory.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.MaterialCategory.Popup.CreateTitle";
            public const string EditTitle = "Catalog.MaterialCategory.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.MaterialCategory.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.MaterialCategory.Confirm.Delete";
        }
    }

    /// <summary>Catalog.MaterialCategoryAttribute — Malzeme Kategori Özelliği / Material Category Attribute</summary>
    public static class Catalog_MaterialCategoryAttribute
    {
        public const string ScreenId = "Catalog.MaterialCategoryAttribute";
        public const string Title = "Catalog.MaterialCategoryAttribute.Title";
        public const string Description = "Catalog.MaterialCategoryAttribute.Description";
        public static class Columns
        {
            public const string materialCategoryId = "Catalog.MaterialCategoryAttribute.Columns.materialCategoryId";
            public const string materialAttributeDefinitionId = "Catalog.MaterialCategoryAttribute.Columns.materialAttributeDefinitionId";
            public const string isRequired = "Catalog.MaterialCategoryAttribute.Columns.isRequired";
            public const string displayOrder = "Catalog.MaterialCategoryAttribute.Columns.displayOrder";
        }
        public static class Actions
        {
            public const string New = "Catalog.MaterialCategoryAttribute.Actions.New";
            public const string Edit = "Catalog.MaterialCategoryAttribute.Actions.Edit";
            public const string Delete = "Catalog.MaterialCategoryAttribute.Actions.Delete";
            public const string Save = "Catalog.MaterialCategoryAttribute.Actions.Save";
            public const string Cancel = "Catalog.MaterialCategoryAttribute.Actions.Cancel";
            public const string Export = "Catalog.MaterialCategoryAttribute.Actions.Export";
            public const string Refresh = "Catalog.MaterialCategoryAttribute.Actions.Refresh";
            public const string ColumnChooser = "Catalog.MaterialCategoryAttribute.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.MaterialCategoryAttribute.Grid.Search";
            public const string NoData = "Catalog.MaterialCategoryAttribute.Grid.NoData";
            public const string Loading = "Catalog.MaterialCategoryAttribute.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.MaterialCategoryAttribute.Notifications.Saved";
            public const string Updated = "Catalog.MaterialCategoryAttribute.Notifications.Updated";
            public const string Deleted = "Catalog.MaterialCategoryAttribute.Notifications.Deleted";
            public const string Error = "Catalog.MaterialCategoryAttribute.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.MaterialCategoryAttribute.Popup.CreateTitle";
            public const string EditTitle = "Catalog.MaterialCategoryAttribute.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.MaterialCategoryAttribute.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.MaterialCategoryAttribute.Confirm.Delete";
        }
    }

    /// <summary>Catalog.MaterialUnitConversion — Malzeme Birim Dönüşümü / Material Unit Conversion</summary>
    public static class Catalog_MaterialUnitConversion
    {
        public const string ScreenId = "Catalog.MaterialUnitConversion";
        public const string Title = "Catalog.MaterialUnitConversion.Title";
        public const string Description = "Catalog.MaterialUnitConversion.Description";
        public static class Columns
        {
            public const string materialId = "Catalog.MaterialUnitConversion.Columns.materialId";
            public const string fromUnitOfMeasureId = "Catalog.MaterialUnitConversion.Columns.fromUnitOfMeasureId";
            public const string toUnitOfMeasureId = "Catalog.MaterialUnitConversion.Columns.toUnitOfMeasureId";
            public const string factor = "Catalog.MaterialUnitConversion.Columns.factor";
        }
        public static class Actions
        {
            public const string New = "Catalog.MaterialUnitConversion.Actions.New";
            public const string Edit = "Catalog.MaterialUnitConversion.Actions.Edit";
            public const string Delete = "Catalog.MaterialUnitConversion.Actions.Delete";
            public const string Save = "Catalog.MaterialUnitConversion.Actions.Save";
            public const string Cancel = "Catalog.MaterialUnitConversion.Actions.Cancel";
            public const string Export = "Catalog.MaterialUnitConversion.Actions.Export";
            public const string Refresh = "Catalog.MaterialUnitConversion.Actions.Refresh";
            public const string ColumnChooser = "Catalog.MaterialUnitConversion.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Catalog.MaterialUnitConversion.Grid.Search";
            public const string NoData = "Catalog.MaterialUnitConversion.Grid.NoData";
            public const string Loading = "Catalog.MaterialUnitConversion.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Catalog.MaterialUnitConversion.Notifications.Saved";
            public const string Updated = "Catalog.MaterialUnitConversion.Notifications.Updated";
            public const string Deleted = "Catalog.MaterialUnitConversion.Notifications.Deleted";
            public const string Error = "Catalog.MaterialUnitConversion.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Catalog.MaterialUnitConversion.Popup.CreateTitle";
            public const string EditTitle = "Catalog.MaterialUnitConversion.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Catalog.MaterialUnitConversion.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Catalog.MaterialUnitConversion.Confirm.Delete";
        }
    }

    /// <summary>Contracts.Contract — Sözleşme / Contract</summary>
    public static class Contracts_Contract
    {
        public const string ScreenId = "Contracts.Contract";
        public const string Title = "Contracts.Contract.Title";
        public const string Description = "Contracts.Contract.Description";
        public static class Columns
        {
            public const string contractType = "Contracts.Contract.Columns.contractType";
            public const string projectId = "Contracts.Contract.Columns.projectId";
            public const string contractNo = "Contracts.Contract.Columns.contractNo";
            public const string currencyId = "Contracts.Contract.Columns.currencyId";
            public const string contractAmount = "Contracts.Contract.Columns.contractAmount";
            public const string title = "Contracts.Contract.Columns.title";
            public const string startDate = "Contracts.Contract.Columns.startDate";
            public const string endDate = "Contracts.Contract.Columns.endDate";
            public const string status = "Contracts.Contract.Columns.status";
        }
        public static class Actions
        {
            public const string New = "Contracts.Contract.Actions.New";
            public const string Edit = "Contracts.Contract.Actions.Edit";
            public const string Delete = "Contracts.Contract.Actions.Delete";
            public const string Save = "Contracts.Contract.Actions.Save";
            public const string Cancel = "Contracts.Contract.Actions.Cancel";
            public const string Export = "Contracts.Contract.Actions.Export";
            public const string Refresh = "Contracts.Contract.Actions.Refresh";
            public const string ColumnChooser = "Contracts.Contract.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Contracts.Contract.Grid.Search";
            public const string NoData = "Contracts.Contract.Grid.NoData";
            public const string Loading = "Contracts.Contract.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Contracts.Contract.Notifications.Saved";
            public const string Updated = "Contracts.Contract.Notifications.Updated";
            public const string Deleted = "Contracts.Contract.Notifications.Deleted";
            public const string Error = "Contracts.Contract.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Contracts.Contract.Popup.CreateTitle";
            public const string EditTitle = "Contracts.Contract.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Contracts.Contract.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Contracts.Contract.Confirm.Delete";
        }
    }

    /// <summary>Contracts.ContractAmendment — Sözleşme Zeyilnamesi / Contract Amendment</summary>
    public static class Contracts_ContractAmendment
    {
        public const string ScreenId = "Contracts.ContractAmendment";
        public const string Title = "Contracts.ContractAmendment.Title";
        public const string Description = "Contracts.ContractAmendment.Description";
        public static class Columns
        {
            public const string contractId = "Contracts.ContractAmendment.Columns.contractId";
            public const string amendmentNo = "Contracts.ContractAmendment.Columns.amendmentNo";
            public const string amendmentDate = "Contracts.ContractAmendment.Columns.amendmentDate";
            public const string description = "Contracts.ContractAmendment.Columns.description";
            public const string amountDelta = "Contracts.ContractAmendment.Columns.amountDelta";
        }
        public static class Actions
        {
            public const string New = "Contracts.ContractAmendment.Actions.New";
            public const string Edit = "Contracts.ContractAmendment.Actions.Edit";
            public const string Delete = "Contracts.ContractAmendment.Actions.Delete";
            public const string Save = "Contracts.ContractAmendment.Actions.Save";
            public const string Cancel = "Contracts.ContractAmendment.Actions.Cancel";
            public const string Export = "Contracts.ContractAmendment.Actions.Export";
            public const string Refresh = "Contracts.ContractAmendment.Actions.Refresh";
            public const string ColumnChooser = "Contracts.ContractAmendment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Contracts.ContractAmendment.Grid.Search";
            public const string NoData = "Contracts.ContractAmendment.Grid.NoData";
            public const string Loading = "Contracts.ContractAmendment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Contracts.ContractAmendment.Notifications.Saved";
            public const string Updated = "Contracts.ContractAmendment.Notifications.Updated";
            public const string Deleted = "Contracts.ContractAmendment.Notifications.Deleted";
            public const string Error = "Contracts.ContractAmendment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Contracts.ContractAmendment.Popup.CreateTitle";
            public const string EditTitle = "Contracts.ContractAmendment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Contracts.ContractAmendment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Contracts.ContractAmendment.Confirm.Delete";
        }
    }

    /// <summary>Contracts.ContractLine — Sözleşme Kalemi / Contract Line</summary>
    public static class Contracts_ContractLine
    {
        public const string ScreenId = "Contracts.ContractLine";
        public const string Title = "Contracts.ContractLine.Title";
        public const string Description = "Contracts.ContractLine.Description";
        public static class Columns
        {
            public const string contractId = "Contracts.ContractLine.Columns.contractId";
            public const string description = "Contracts.ContractLine.Columns.description";
            public const string quantity = "Contracts.ContractLine.Columns.quantity";
            public const string unitPrice = "Contracts.ContractLine.Columns.unitPrice";
        }
        public static class Actions
        {
            public const string New = "Contracts.ContractLine.Actions.New";
            public const string Edit = "Contracts.ContractLine.Actions.Edit";
            public const string Delete = "Contracts.ContractLine.Actions.Delete";
            public const string Save = "Contracts.ContractLine.Actions.Save";
            public const string Cancel = "Contracts.ContractLine.Actions.Cancel";
            public const string Export = "Contracts.ContractLine.Actions.Export";
            public const string Refresh = "Contracts.ContractLine.Actions.Refresh";
            public const string ColumnChooser = "Contracts.ContractLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Contracts.ContractLine.Grid.Search";
            public const string NoData = "Contracts.ContractLine.Grid.NoData";
            public const string Loading = "Contracts.ContractLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Contracts.ContractLine.Notifications.Saved";
            public const string Updated = "Contracts.ContractLine.Notifications.Updated";
            public const string Deleted = "Contracts.ContractLine.Notifications.Deleted";
            public const string Error = "Contracts.ContractLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Contracts.ContractLine.Popup.CreateTitle";
            public const string EditTitle = "Contracts.ContractLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Contracts.ContractLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Contracts.ContractLine.Confirm.Delete";
        }
    }

    /// <summary>Contracts.ContractParty — Sözleşme Tarafı / Contract Party</summary>
    public static class Contracts_ContractParty
    {
        public const string ScreenId = "Contracts.ContractParty";
        public const string Title = "Contracts.ContractParty.Title";
        public const string Description = "Contracts.ContractParty.Description";
        public static class Columns
        {
            public const string contractId = "Contracts.ContractParty.Columns.contractId";
            public const string businessPartnerId = "Contracts.ContractParty.Columns.businessPartnerId";
            public const string partyRole = "Contracts.ContractParty.Columns.partyRole";
        }
        public static class Actions
        {
            public const string New = "Contracts.ContractParty.Actions.New";
            public const string Edit = "Contracts.ContractParty.Actions.Edit";
            public const string Delete = "Contracts.ContractParty.Actions.Delete";
            public const string Save = "Contracts.ContractParty.Actions.Save";
            public const string Cancel = "Contracts.ContractParty.Actions.Cancel";
            public const string Export = "Contracts.ContractParty.Actions.Export";
            public const string Refresh = "Contracts.ContractParty.Actions.Refresh";
            public const string ColumnChooser = "Contracts.ContractParty.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Contracts.ContractParty.Grid.Search";
            public const string NoData = "Contracts.ContractParty.Grid.NoData";
            public const string Loading = "Contracts.ContractParty.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Contracts.ContractParty.Notifications.Saved";
            public const string Updated = "Contracts.ContractParty.Notifications.Updated";
            public const string Deleted = "Contracts.ContractParty.Notifications.Deleted";
            public const string Error = "Contracts.ContractParty.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Contracts.ContractParty.Popup.CreateTitle";
            public const string EditTitle = "Contracts.ContractParty.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Contracts.ContractParty.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Contracts.ContractParty.Confirm.Delete";
        }
    }

    /// <summary>Core.AuditLog — Denetim Günlüğü / Audit Log</summary>
    public static class Core_AuditLog
    {
        public const string ScreenId = "Core.AuditLog";
        public const string Title = "Core.AuditLog.Title";
        public const string Description = "Core.AuditLog.Description";
        public static class Columns
        {
            public const string occurredAt = "Core.AuditLog.Columns.occurredAt";
            public const string userId = "Core.AuditLog.Columns.userId";
            public const string userName = "Core.AuditLog.Columns.userName";
            public const string ipAddress = "Core.AuditLog.Columns.ipAddress";
            public const string httpMethod = "Core.AuditLog.Columns.httpMethod";
            public const string path = "Core.AuditLog.Columns.path";
            public const string queryString = "Core.AuditLog.Columns.queryString";
            public const string statusCode = "Core.AuditLog.Columns.statusCode";
            public const string isSuccess = "Core.AuditLog.Columns.isSuccess";
            public const string source = "Core.AuditLog.Columns.source";
            public const string requestBody = "Core.AuditLog.Columns.requestBody";
            public const string responseBody = "Core.AuditLog.Columns.responseBody";
            public const string hasException = "Core.AuditLog.Columns.hasException";
            public const string exceptionType = "Core.AuditLog.Columns.exceptionType";
            public const string exceptionMessage = "Core.AuditLog.Columns.exceptionMessage";
            public const string correlationId = "Core.AuditLog.Columns.correlationId";
            public const string durationMs = "Core.AuditLog.Columns.durationMs";
        }
        public static class Actions
        {
            public const string New = "Core.AuditLog.Actions.New";
            public const string Edit = "Core.AuditLog.Actions.Edit";
            public const string Delete = "Core.AuditLog.Actions.Delete";
            public const string Save = "Core.AuditLog.Actions.Save";
            public const string Cancel = "Core.AuditLog.Actions.Cancel";
            public const string Export = "Core.AuditLog.Actions.Export";
            public const string Refresh = "Core.AuditLog.Actions.Refresh";
            public const string ColumnChooser = "Core.AuditLog.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.AuditLog.Grid.Search";
            public const string NoData = "Core.AuditLog.Grid.NoData";
            public const string Loading = "Core.AuditLog.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.AuditLog.Notifications.Saved";
            public const string Updated = "Core.AuditLog.Notifications.Updated";
            public const string Deleted = "Core.AuditLog.Notifications.Deleted";
            public const string Error = "Core.AuditLog.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.AuditLog.Popup.CreateTitle";
            public const string EditTitle = "Core.AuditLog.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.AuditLog.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.AuditLog.Confirm.Delete";
        }
    }

    /// <summary>Core.Branch — Şube / Branch</summary>
    public static class Core_Branch
    {
        public const string ScreenId = "Core.Branch";
        public const string Title = "Core.Branch.Title";
        public const string Description = "Core.Branch.Description";
        public static class Columns
        {
            public const string companyId = "Core.Branch.Columns.companyId";
            public const string code = "Core.Branch.Columns.code";
            public const string name = "Core.Branch.Columns.name";
            public const string address = "Core.Branch.Columns.address";
            public const string isActive = "Core.Branch.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Core.Branch.Actions.New";
            public const string Edit = "Core.Branch.Actions.Edit";
            public const string Delete = "Core.Branch.Actions.Delete";
            public const string Save = "Core.Branch.Actions.Save";
            public const string Cancel = "Core.Branch.Actions.Cancel";
            public const string Export = "Core.Branch.Actions.Export";
            public const string Refresh = "Core.Branch.Actions.Refresh";
            public const string ColumnChooser = "Core.Branch.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.Branch.Grid.Search";
            public const string NoData = "Core.Branch.Grid.NoData";
            public const string Loading = "Core.Branch.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.Branch.Notifications.Saved";
            public const string Updated = "Core.Branch.Notifications.Updated";
            public const string Deleted = "Core.Branch.Notifications.Deleted";
            public const string Error = "Core.Branch.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.Branch.Popup.CreateTitle";
            public const string EditTitle = "Core.Branch.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.Branch.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.Branch.Confirm.Delete";
        }
    }

    /// <summary>Core.Company — Şirket / Company</summary>
    public static class Core_Company
    {
        public const string ScreenId = "Core.Company";
        public const string Title = "Core.Company.Title";
        public const string Description = "Core.Company.Description";
        public static class Columns
        {
            public const string code = "Core.Company.Columns.code";
            public const string name = "Core.Company.Columns.name";
            public const string baseCurrencyId = "Core.Company.Columns.baseCurrencyId";
            public const string taxNumber = "Core.Company.Columns.taxNumber";
            public const string address = "Core.Company.Columns.address";
            public const string isActive = "Core.Company.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Core.Company.Actions.New";
            public const string Edit = "Core.Company.Actions.Edit";
            public const string Delete = "Core.Company.Actions.Delete";
            public const string Save = "Core.Company.Actions.Save";
            public const string Cancel = "Core.Company.Actions.Cancel";
            public const string Export = "Core.Company.Actions.Export";
            public const string Refresh = "Core.Company.Actions.Refresh";
            public const string ColumnChooser = "Core.Company.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.Company.Grid.Search";
            public const string NoData = "Core.Company.Grid.NoData";
            public const string Loading = "Core.Company.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.Company.Notifications.Saved";
            public const string Updated = "Core.Company.Notifications.Updated";
            public const string Deleted = "Core.Company.Notifications.Deleted";
            public const string Error = "Core.Company.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.Company.Popup.CreateTitle";
            public const string EditTitle = "Core.Company.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.Company.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.Company.Confirm.Delete";
        }
    }

    /// <summary>Core.Currency — Para Birimi / Currency</summary>
    public static class Core_Currency
    {
        public const string ScreenId = "Core.Currency";
        public const string Title = "Core.Currency.Title";
        public const string Description = "Core.Currency.Description";
        public static class Columns
        {
            public const string code = "Core.Currency.Columns.code";
            public const string name = "Core.Currency.Columns.name";
            public const string symbol = "Core.Currency.Columns.symbol";
            public const string isActive = "Core.Currency.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Core.Currency.Actions.New";
            public const string Edit = "Core.Currency.Actions.Edit";
            public const string Delete = "Core.Currency.Actions.Delete";
            public const string Save = "Core.Currency.Actions.Save";
            public const string Cancel = "Core.Currency.Actions.Cancel";
            public const string Export = "Core.Currency.Actions.Export";
            public const string Refresh = "Core.Currency.Actions.Refresh";
            public const string ColumnChooser = "Core.Currency.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.Currency.Grid.Search";
            public const string NoData = "Core.Currency.Grid.NoData";
            public const string Loading = "Core.Currency.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.Currency.Notifications.Saved";
            public const string Updated = "Core.Currency.Notifications.Updated";
            public const string Deleted = "Core.Currency.Notifications.Deleted";
            public const string Error = "Core.Currency.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.Currency.Popup.CreateTitle";
            public const string EditTitle = "Core.Currency.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.Currency.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.Currency.Confirm.Delete";
        }
    }

    /// <summary>Core.Department — Departman / Department</summary>
    public static class Core_Department
    {
        public const string ScreenId = "Core.Department";
        public const string Title = "Core.Department.Title";
        public const string Description = "Core.Department.Description";
        public static class Columns
        {
            public const string companyId = "Core.Department.Columns.companyId";
            public const string parentDepartmentId = "Core.Department.Columns.parentDepartmentId";
            public const string code = "Core.Department.Columns.code";
            public const string name = "Core.Department.Columns.name";
            public const string managerUserId = "Core.Department.Columns.managerUserId";
            public const string isActive = "Core.Department.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Core.Department.Actions.New";
            public const string Edit = "Core.Department.Actions.Edit";
            public const string Delete = "Core.Department.Actions.Delete";
            public const string Save = "Core.Department.Actions.Save";
            public const string Cancel = "Core.Department.Actions.Cancel";
            public const string Export = "Core.Department.Actions.Export";
            public const string Refresh = "Core.Department.Actions.Refresh";
            public const string ColumnChooser = "Core.Department.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.Department.Grid.Search";
            public const string NoData = "Core.Department.Grid.NoData";
            public const string Loading = "Core.Department.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.Department.Notifications.Saved";
            public const string Updated = "Core.Department.Notifications.Updated";
            public const string Deleted = "Core.Department.Notifications.Deleted";
            public const string Error = "Core.Department.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.Department.Popup.CreateTitle";
            public const string EditTitle = "Core.Department.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.Department.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.Department.Confirm.Delete";
        }
    }

    /// <summary>Core.ExchangeRate — Döviz Kuru / Exchange Rate</summary>
    public static class Core_ExchangeRate
    {
        public const string ScreenId = "Core.ExchangeRate";
        public const string Title = "Core.ExchangeRate.Title";
        public const string Description = "Core.ExchangeRate.Description";
        public static class Columns
        {
            public const string currencyId = "Core.ExchangeRate.Columns.currencyId";
            public const string rateDate = "Core.ExchangeRate.Columns.rateDate";
            public const string rate = "Core.ExchangeRate.Columns.rate";
        }
        public static class Actions
        {
            public const string New = "Core.ExchangeRate.Actions.New";
            public const string Edit = "Core.ExchangeRate.Actions.Edit";
            public const string Delete = "Core.ExchangeRate.Actions.Delete";
            public const string Save = "Core.ExchangeRate.Actions.Save";
            public const string Cancel = "Core.ExchangeRate.Actions.Cancel";
            public const string Export = "Core.ExchangeRate.Actions.Export";
            public const string Refresh = "Core.ExchangeRate.Actions.Refresh";
            public const string ColumnChooser = "Core.ExchangeRate.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.ExchangeRate.Grid.Search";
            public const string NoData = "Core.ExchangeRate.Grid.NoData";
            public const string Loading = "Core.ExchangeRate.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.ExchangeRate.Notifications.Saved";
            public const string Updated = "Core.ExchangeRate.Notifications.Updated";
            public const string Deleted = "Core.ExchangeRate.Notifications.Deleted";
            public const string Error = "Core.ExchangeRate.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.ExchangeRate.Popup.CreateTitle";
            public const string EditTitle = "Core.ExchangeRate.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.ExchangeRate.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.ExchangeRate.Confirm.Delete";
        }
    }

    /// <summary>Core.LocalizationResource — Yerelleştirme Kaynağı / Localization Resource</summary>
    public static class Core_LocalizationResource
    {
        public const string ScreenId = "Core.LocalizationResource";
        public const string Title = "Core.LocalizationResource.Title";
        public const string Description = "Core.LocalizationResource.Description";
        public static class Columns
        {
            public const string key = "Core.LocalizationResource.Columns.key";
            public const string culture = "Core.LocalizationResource.Columns.culture";
            public const string value = "Core.LocalizationResource.Columns.value";
        }
        public static class Actions
        {
            public const string New = "Core.LocalizationResource.Actions.New";
            public const string Edit = "Core.LocalizationResource.Actions.Edit";
            public const string Delete = "Core.LocalizationResource.Actions.Delete";
            public const string Save = "Core.LocalizationResource.Actions.Save";
            public const string Cancel = "Core.LocalizationResource.Actions.Cancel";
            public const string Export = "Core.LocalizationResource.Actions.Export";
            public const string Refresh = "Core.LocalizationResource.Actions.Refresh";
            public const string ColumnChooser = "Core.LocalizationResource.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.LocalizationResource.Grid.Search";
            public const string NoData = "Core.LocalizationResource.Grid.NoData";
            public const string Loading = "Core.LocalizationResource.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.LocalizationResource.Notifications.Saved";
            public const string Updated = "Core.LocalizationResource.Notifications.Updated";
            public const string Deleted = "Core.LocalizationResource.Notifications.Deleted";
            public const string Error = "Core.LocalizationResource.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.LocalizationResource.Popup.CreateTitle";
            public const string EditTitle = "Core.LocalizationResource.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.LocalizationResource.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.LocalizationResource.Confirm.Delete";
        }
    }

    /// <summary>Core.SequenceDefinition — Sıra Tanımı / Sequence Definition</summary>
    public static class Core_SequenceDefinition
    {
        public const string ScreenId = "Core.SequenceDefinition";
        public const string Title = "Core.SequenceDefinition.Title";
        public const string Description = "Core.SequenceDefinition.Description";
        public static class Columns
        {
            public const string module = "Core.SequenceDefinition.Columns.module";
            public const string entityType = "Core.SequenceDefinition.Columns.entityType";
            public const string prefix = "Core.SequenceDefinition.Columns.prefix";
            public const string padding = "Core.SequenceDefinition.Columns.padding";
            public const string nextNumber = "Core.SequenceDefinition.Columns.nextNumber";
            public const string format = "Core.SequenceDefinition.Columns.format";
        }
        public static class Actions
        {
            public const string New = "Core.SequenceDefinition.Actions.New";
            public const string Edit = "Core.SequenceDefinition.Actions.Edit";
            public const string Delete = "Core.SequenceDefinition.Actions.Delete";
            public const string Save = "Core.SequenceDefinition.Actions.Save";
            public const string Cancel = "Core.SequenceDefinition.Actions.Cancel";
            public const string Export = "Core.SequenceDefinition.Actions.Export";
            public const string Refresh = "Core.SequenceDefinition.Actions.Refresh";
            public const string ColumnChooser = "Core.SequenceDefinition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.SequenceDefinition.Grid.Search";
            public const string NoData = "Core.SequenceDefinition.Grid.NoData";
            public const string Loading = "Core.SequenceDefinition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.SequenceDefinition.Notifications.Saved";
            public const string Updated = "Core.SequenceDefinition.Notifications.Updated";
            public const string Deleted = "Core.SequenceDefinition.Notifications.Deleted";
            public const string Error = "Core.SequenceDefinition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.SequenceDefinition.Popup.CreateTitle";
            public const string EditTitle = "Core.SequenceDefinition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.SequenceDefinition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.SequenceDefinition.Confirm.Delete";
        }
    }

    /// <summary>Core.SystemSetting — Sistem Ayarı / System Setting</summary>
    public static class Core_SystemSetting
    {
        public const string ScreenId = "Core.SystemSetting";
        public const string Title = "Core.SystemSetting.Title";
        public const string Description = "Core.SystemSetting.Description";
        public static class Columns
        {
            public const string key = "Core.SystemSetting.Columns.key";
            public const string value = "Core.SystemSetting.Columns.value";
            public const string category = "Core.SystemSetting.Columns.category";
            public const string descriptionKey = "Core.SystemSetting.Columns.descriptionKey";
        }
        public static class Actions
        {
            public const string New = "Core.SystemSetting.Actions.New";
            public const string Edit = "Core.SystemSetting.Actions.Edit";
            public const string Delete = "Core.SystemSetting.Actions.Delete";
            public const string Save = "Core.SystemSetting.Actions.Save";
            public const string Cancel = "Core.SystemSetting.Actions.Cancel";
            public const string Export = "Core.SystemSetting.Actions.Export";
            public const string Refresh = "Core.SystemSetting.Actions.Refresh";
            public const string ColumnChooser = "Core.SystemSetting.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.SystemSetting.Grid.Search";
            public const string NoData = "Core.SystemSetting.Grid.NoData";
            public const string Loading = "Core.SystemSetting.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.SystemSetting.Notifications.Saved";
            public const string Updated = "Core.SystemSetting.Notifications.Updated";
            public const string Deleted = "Core.SystemSetting.Notifications.Deleted";
            public const string Error = "Core.SystemSetting.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.SystemSetting.Popup.CreateTitle";
            public const string EditTitle = "Core.SystemSetting.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.SystemSetting.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.SystemSetting.Confirm.Delete";
        }
    }

    /// <summary>Core.UnitConversion — Birim Dönüşümü / Unit Conversion</summary>
    public static class Core_UnitConversion
    {
        public const string ScreenId = "Core.UnitConversion";
        public const string Title = "Core.UnitConversion.Title";
        public const string Description = "Core.UnitConversion.Description";
        public static class Columns
        {
            public const string fromUnitOfMeasureId = "Core.UnitConversion.Columns.fromUnitOfMeasureId";
            public const string toUnitOfMeasureId = "Core.UnitConversion.Columns.toUnitOfMeasureId";
            public const string factor = "Core.UnitConversion.Columns.factor";
        }
        public static class Actions
        {
            public const string New = "Core.UnitConversion.Actions.New";
            public const string Edit = "Core.UnitConversion.Actions.Edit";
            public const string Delete = "Core.UnitConversion.Actions.Delete";
            public const string Save = "Core.UnitConversion.Actions.Save";
            public const string Cancel = "Core.UnitConversion.Actions.Cancel";
            public const string Export = "Core.UnitConversion.Actions.Export";
            public const string Refresh = "Core.UnitConversion.Actions.Refresh";
            public const string ColumnChooser = "Core.UnitConversion.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.UnitConversion.Grid.Search";
            public const string NoData = "Core.UnitConversion.Grid.NoData";
            public const string Loading = "Core.UnitConversion.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.UnitConversion.Notifications.Saved";
            public const string Updated = "Core.UnitConversion.Notifications.Updated";
            public const string Deleted = "Core.UnitConversion.Notifications.Deleted";
            public const string Error = "Core.UnitConversion.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.UnitConversion.Popup.CreateTitle";
            public const string EditTitle = "Core.UnitConversion.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.UnitConversion.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.UnitConversion.Confirm.Delete";
        }
    }

    /// <summary>Core.UnitOfMeasure — Ölçü Birimi / Unit Of Measure</summary>
    public static class Core_UnitOfMeasure
    {
        public const string ScreenId = "Core.UnitOfMeasure";
        public const string Title = "Core.UnitOfMeasure.Title";
        public const string Description = "Core.UnitOfMeasure.Description";
        public static class Columns
        {
            public const string code = "Core.UnitOfMeasure.Columns.code";
            public const string name = "Core.UnitOfMeasure.Columns.name";
            public const string symbol = "Core.UnitOfMeasure.Columns.symbol";
            public const string isActive = "Core.UnitOfMeasure.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Core.UnitOfMeasure.Actions.New";
            public const string Edit = "Core.UnitOfMeasure.Actions.Edit";
            public const string Delete = "Core.UnitOfMeasure.Actions.Delete";
            public const string Save = "Core.UnitOfMeasure.Actions.Save";
            public const string Cancel = "Core.UnitOfMeasure.Actions.Cancel";
            public const string Export = "Core.UnitOfMeasure.Actions.Export";
            public const string Refresh = "Core.UnitOfMeasure.Actions.Refresh";
            public const string ColumnChooser = "Core.UnitOfMeasure.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Core.UnitOfMeasure.Grid.Search";
            public const string NoData = "Core.UnitOfMeasure.Grid.NoData";
            public const string Loading = "Core.UnitOfMeasure.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Core.UnitOfMeasure.Notifications.Saved";
            public const string Updated = "Core.UnitOfMeasure.Notifications.Updated";
            public const string Deleted = "Core.UnitOfMeasure.Notifications.Deleted";
            public const string Error = "Core.UnitOfMeasure.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Core.UnitOfMeasure.Popup.CreateTitle";
            public const string EditTitle = "Core.UnitOfMeasure.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Core.UnitOfMeasure.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Core.UnitOfMeasure.Confirm.Delete";
        }
    }

    /// <summary>Documents.Document — Belge / Document</summary>
    public static class Documents_Document
    {
        public const string ScreenId = "Documents.Document";
        public const string Title = "Documents.Document.Title";
        public const string Description = "Documents.Document.Description";
        public static class Columns
        {
            public const string documentFolderId = "Documents.Document.Columns.documentFolderId";
            public const string name = "Documents.Document.Columns.name";
            public const string description = "Documents.Document.Columns.description";
            public const string status = "Documents.Document.Columns.status";
            public const string currentVersionNo = "Documents.Document.Columns.currentVersionNo";
        }
        public static class Actions
        {
            public const string New = "Documents.Document.Actions.New";
            public const string Edit = "Documents.Document.Actions.Edit";
            public const string Delete = "Documents.Document.Actions.Delete";
            public const string Save = "Documents.Document.Actions.Save";
            public const string Cancel = "Documents.Document.Actions.Cancel";
            public const string Export = "Documents.Document.Actions.Export";
            public const string Refresh = "Documents.Document.Actions.Refresh";
            public const string ColumnChooser = "Documents.Document.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Documents.Document.Grid.Search";
            public const string NoData = "Documents.Document.Grid.NoData";
            public const string Loading = "Documents.Document.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Documents.Document.Notifications.Saved";
            public const string Updated = "Documents.Document.Notifications.Updated";
            public const string Deleted = "Documents.Document.Notifications.Deleted";
            public const string Error = "Documents.Document.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Documents.Document.Popup.CreateTitle";
            public const string EditTitle = "Documents.Document.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Documents.Document.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Documents.Document.Confirm.Delete";
        }
    }

    /// <summary>Documents.DocumentFolder — Belge Klasörü / Document Folder</summary>
    public static class Documents_DocumentFolder
    {
        public const string ScreenId = "Documents.DocumentFolder";
        public const string Title = "Documents.DocumentFolder.Title";
        public const string Description = "Documents.DocumentFolder.Description";
        public static class Columns
        {
            public const string parentFolderId = "Documents.DocumentFolder.Columns.parentFolderId";
            public const string name = "Documents.DocumentFolder.Columns.name";
        }
        public static class Actions
        {
            public const string New = "Documents.DocumentFolder.Actions.New";
            public const string Edit = "Documents.DocumentFolder.Actions.Edit";
            public const string Delete = "Documents.DocumentFolder.Actions.Delete";
            public const string Save = "Documents.DocumentFolder.Actions.Save";
            public const string Cancel = "Documents.DocumentFolder.Actions.Cancel";
            public const string Export = "Documents.DocumentFolder.Actions.Export";
            public const string Refresh = "Documents.DocumentFolder.Actions.Refresh";
            public const string ColumnChooser = "Documents.DocumentFolder.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Documents.DocumentFolder.Grid.Search";
            public const string NoData = "Documents.DocumentFolder.Grid.NoData";
            public const string Loading = "Documents.DocumentFolder.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Documents.DocumentFolder.Notifications.Saved";
            public const string Updated = "Documents.DocumentFolder.Notifications.Updated";
            public const string Deleted = "Documents.DocumentFolder.Notifications.Deleted";
            public const string Error = "Documents.DocumentFolder.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Documents.DocumentFolder.Popup.CreateTitle";
            public const string EditTitle = "Documents.DocumentFolder.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Documents.DocumentFolder.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Documents.DocumentFolder.Confirm.Delete";
        }
    }

    /// <summary>Documents.DocumentPermission — Belge İzni / Document Permission</summary>
    public static class Documents_DocumentPermission
    {
        public const string ScreenId = "Documents.DocumentPermission";
        public const string Title = "Documents.DocumentPermission.Title";
        public const string Description = "Documents.DocumentPermission.Description";
        public static class Columns
        {
            public const string documentId = "Documents.DocumentPermission.Columns.documentId";
            public const string userId = "Documents.DocumentPermission.Columns.userId";
            public const string roleId = "Documents.DocumentPermission.Columns.roleId";
            public const string accessType = "Documents.DocumentPermission.Columns.accessType";
        }
        public static class Actions
        {
            public const string New = "Documents.DocumentPermission.Actions.New";
            public const string Edit = "Documents.DocumentPermission.Actions.Edit";
            public const string Delete = "Documents.DocumentPermission.Actions.Delete";
            public const string Save = "Documents.DocumentPermission.Actions.Save";
            public const string Cancel = "Documents.DocumentPermission.Actions.Cancel";
            public const string Export = "Documents.DocumentPermission.Actions.Export";
            public const string Refresh = "Documents.DocumentPermission.Actions.Refresh";
            public const string ColumnChooser = "Documents.DocumentPermission.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Documents.DocumentPermission.Grid.Search";
            public const string NoData = "Documents.DocumentPermission.Grid.NoData";
            public const string Loading = "Documents.DocumentPermission.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Documents.DocumentPermission.Notifications.Saved";
            public const string Updated = "Documents.DocumentPermission.Notifications.Updated";
            public const string Deleted = "Documents.DocumentPermission.Notifications.Deleted";
            public const string Error = "Documents.DocumentPermission.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Documents.DocumentPermission.Popup.CreateTitle";
            public const string EditTitle = "Documents.DocumentPermission.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Documents.DocumentPermission.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Documents.DocumentPermission.Confirm.Delete";
        }
    }

    /// <summary>Documents.DocumentRelation — Belge İlişkisi / Document Relation</summary>
    public static class Documents_DocumentRelation
    {
        public const string ScreenId = "Documents.DocumentRelation";
        public const string Title = "Documents.DocumentRelation.Title";
        public const string Description = "Documents.DocumentRelation.Description";
        public static class Columns
        {
            public const string documentId = "Documents.DocumentRelation.Columns.documentId";
            public const string relatedModule = "Documents.DocumentRelation.Columns.relatedModule";
            public const string relatedEntityType = "Documents.DocumentRelation.Columns.relatedEntityType";
            public const string relatedEntityId = "Documents.DocumentRelation.Columns.relatedEntityId";
        }
        public static class Actions
        {
            public const string New = "Documents.DocumentRelation.Actions.New";
            public const string Edit = "Documents.DocumentRelation.Actions.Edit";
            public const string Delete = "Documents.DocumentRelation.Actions.Delete";
            public const string Save = "Documents.DocumentRelation.Actions.Save";
            public const string Cancel = "Documents.DocumentRelation.Actions.Cancel";
            public const string Export = "Documents.DocumentRelation.Actions.Export";
            public const string Refresh = "Documents.DocumentRelation.Actions.Refresh";
            public const string ColumnChooser = "Documents.DocumentRelation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Documents.DocumentRelation.Grid.Search";
            public const string NoData = "Documents.DocumentRelation.Grid.NoData";
            public const string Loading = "Documents.DocumentRelation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Documents.DocumentRelation.Notifications.Saved";
            public const string Updated = "Documents.DocumentRelation.Notifications.Updated";
            public const string Deleted = "Documents.DocumentRelation.Notifications.Deleted";
            public const string Error = "Documents.DocumentRelation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Documents.DocumentRelation.Popup.CreateTitle";
            public const string EditTitle = "Documents.DocumentRelation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Documents.DocumentRelation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Documents.DocumentRelation.Confirm.Delete";
        }
    }

    /// <summary>Documents.DocumentVersion — Belge Sürümü / Document Version</summary>
    public static class Documents_DocumentVersion
    {
        public const string ScreenId = "Documents.DocumentVersion";
        public const string Title = "Documents.DocumentVersion.Title";
        public const string Description = "Documents.DocumentVersion.Description";
        public static class Columns
        {
            public const string documentId = "Documents.DocumentVersion.Columns.documentId";
            public const string versionNo = "Documents.DocumentVersion.Columns.versionNo";
            public const string fileName = "Documents.DocumentVersion.Columns.fileName";
            public const string filePath = "Documents.DocumentVersion.Columns.filePath";
            public const string fileSize = "Documents.DocumentVersion.Columns.fileSize";
            public const string contentType = "Documents.DocumentVersion.Columns.contentType";
            public const string uploadedAt = "Documents.DocumentVersion.Columns.uploadedAt";
        }
        public static class Actions
        {
            public const string New = "Documents.DocumentVersion.Actions.New";
            public const string Edit = "Documents.DocumentVersion.Actions.Edit";
            public const string Delete = "Documents.DocumentVersion.Actions.Delete";
            public const string Save = "Documents.DocumentVersion.Actions.Save";
            public const string Cancel = "Documents.DocumentVersion.Actions.Cancel";
            public const string Export = "Documents.DocumentVersion.Actions.Export";
            public const string Refresh = "Documents.DocumentVersion.Actions.Refresh";
            public const string ColumnChooser = "Documents.DocumentVersion.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Documents.DocumentVersion.Grid.Search";
            public const string NoData = "Documents.DocumentVersion.Grid.NoData";
            public const string Loading = "Documents.DocumentVersion.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Documents.DocumentVersion.Notifications.Saved";
            public const string Updated = "Documents.DocumentVersion.Notifications.Updated";
            public const string Deleted = "Documents.DocumentVersion.Notifications.Deleted";
            public const string Error = "Documents.DocumentVersion.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Documents.DocumentVersion.Popup.CreateTitle";
            public const string EditTitle = "Documents.DocumentVersion.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Documents.DocumentVersion.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Documents.DocumentVersion.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.DailySiteReport — Günlük Şantiye Raporu / Daily Site Report</summary>
    public static class FieldOperations_DailySiteReport
    {
        public const string ScreenId = "FieldOperations.DailySiteReport";
        public const string Title = "FieldOperations.DailySiteReport.Title";
        public const string Description = "FieldOperations.DailySiteReport.Description";
        public static class Columns
        {
            public const string projectId = "FieldOperations.DailySiteReport.Columns.projectId";
            public const string workOrderId = "FieldOperations.DailySiteReport.Columns.workOrderId";
            public const string reportNo = "FieldOperations.DailySiteReport.Columns.reportNo";
            public const string reportDate = "FieldOperations.DailySiteReport.Columns.reportDate";
            public const string weather = "FieldOperations.DailySiteReport.Columns.weather";
            public const string notes = "FieldOperations.DailySiteReport.Columns.notes";
            public const string status = "FieldOperations.DailySiteReport.Columns.status";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.DailySiteReport.Actions.New";
            public const string Edit = "FieldOperations.DailySiteReport.Actions.Edit";
            public const string Delete = "FieldOperations.DailySiteReport.Actions.Delete";
            public const string Save = "FieldOperations.DailySiteReport.Actions.Save";
            public const string Cancel = "FieldOperations.DailySiteReport.Actions.Cancel";
            public const string Export = "FieldOperations.DailySiteReport.Actions.Export";
            public const string Refresh = "FieldOperations.DailySiteReport.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.DailySiteReport.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.DailySiteReport.Grid.Search";
            public const string NoData = "FieldOperations.DailySiteReport.Grid.NoData";
            public const string Loading = "FieldOperations.DailySiteReport.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.DailySiteReport.Notifications.Saved";
            public const string Updated = "FieldOperations.DailySiteReport.Notifications.Updated";
            public const string Deleted = "FieldOperations.DailySiteReport.Notifications.Deleted";
            public const string Error = "FieldOperations.DailySiteReport.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.DailySiteReport.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.DailySiteReport.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.DailySiteReport.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.DailySiteReport.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.DailySiteReportEquipment — Günlük Şantiye Raporu Ekipmanı / Daily Site Report Equipment</summary>
    public static class FieldOperations_DailySiteReportEquipment
    {
        public const string ScreenId = "FieldOperations.DailySiteReportEquipment";
        public const string Title = "FieldOperations.DailySiteReportEquipment.Title";
        public const string Description = "FieldOperations.DailySiteReportEquipment.Description";
        public static class Columns
        {
            public const string dailySiteReportId = "FieldOperations.DailySiteReportEquipment.Columns.dailySiteReportId";
            public const string equipmentAssetId = "FieldOperations.DailySiteReportEquipment.Columns.equipmentAssetId";
            public const string equipmentText = "FieldOperations.DailySiteReportEquipment.Columns.equipmentText";
            public const string hours = "FieldOperations.DailySiteReportEquipment.Columns.hours";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.DailySiteReportEquipment.Actions.New";
            public const string Edit = "FieldOperations.DailySiteReportEquipment.Actions.Edit";
            public const string Delete = "FieldOperations.DailySiteReportEquipment.Actions.Delete";
            public const string Save = "FieldOperations.DailySiteReportEquipment.Actions.Save";
            public const string Cancel = "FieldOperations.DailySiteReportEquipment.Actions.Cancel";
            public const string Export = "FieldOperations.DailySiteReportEquipment.Actions.Export";
            public const string Refresh = "FieldOperations.DailySiteReportEquipment.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.DailySiteReportEquipment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.DailySiteReportEquipment.Grid.Search";
            public const string NoData = "FieldOperations.DailySiteReportEquipment.Grid.NoData";
            public const string Loading = "FieldOperations.DailySiteReportEquipment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.DailySiteReportEquipment.Notifications.Saved";
            public const string Updated = "FieldOperations.DailySiteReportEquipment.Notifications.Updated";
            public const string Deleted = "FieldOperations.DailySiteReportEquipment.Notifications.Deleted";
            public const string Error = "FieldOperations.DailySiteReportEquipment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.DailySiteReportEquipment.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.DailySiteReportEquipment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.DailySiteReportEquipment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.DailySiteReportEquipment.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.DailySiteReportMaterial — Günlük Şantiye Raporu Malzemesi / Daily Site Report Material</summary>
    public static class FieldOperations_DailySiteReportMaterial
    {
        public const string ScreenId = "FieldOperations.DailySiteReportMaterial";
        public const string Title = "FieldOperations.DailySiteReportMaterial.Title";
        public const string Description = "FieldOperations.DailySiteReportMaterial.Description";
        public static class Columns
        {
            public const string dailySiteReportId = "FieldOperations.DailySiteReportMaterial.Columns.dailySiteReportId";
            public const string materialId = "FieldOperations.DailySiteReportMaterial.Columns.materialId";
            public const string quantity = "FieldOperations.DailySiteReportMaterial.Columns.quantity";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.DailySiteReportMaterial.Actions.New";
            public const string Edit = "FieldOperations.DailySiteReportMaterial.Actions.Edit";
            public const string Delete = "FieldOperations.DailySiteReportMaterial.Actions.Delete";
            public const string Save = "FieldOperations.DailySiteReportMaterial.Actions.Save";
            public const string Cancel = "FieldOperations.DailySiteReportMaterial.Actions.Cancel";
            public const string Export = "FieldOperations.DailySiteReportMaterial.Actions.Export";
            public const string Refresh = "FieldOperations.DailySiteReportMaterial.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.DailySiteReportMaterial.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.DailySiteReportMaterial.Grid.Search";
            public const string NoData = "FieldOperations.DailySiteReportMaterial.Grid.NoData";
            public const string Loading = "FieldOperations.DailySiteReportMaterial.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.DailySiteReportMaterial.Notifications.Saved";
            public const string Updated = "FieldOperations.DailySiteReportMaterial.Notifications.Updated";
            public const string Deleted = "FieldOperations.DailySiteReportMaterial.Notifications.Deleted";
            public const string Error = "FieldOperations.DailySiteReportMaterial.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.DailySiteReportMaterial.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.DailySiteReportMaterial.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.DailySiteReportMaterial.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.DailySiteReportMaterial.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.DailySiteReportWorker — Günlük Şantiye Raporu İşçisi / Daily Site Report Worker</summary>
    public static class FieldOperations_DailySiteReportWorker
    {
        public const string ScreenId = "FieldOperations.DailySiteReportWorker";
        public const string Title = "FieldOperations.DailySiteReportWorker.Title";
        public const string Description = "FieldOperations.DailySiteReportWorker.Description";
        public static class Columns
        {
            public const string dailySiteReportId = "FieldOperations.DailySiteReportWorker.Columns.dailySiteReportId";
            public const string employeeId = "FieldOperations.DailySiteReportWorker.Columns.employeeId";
            public const string hoursWorked = "FieldOperations.DailySiteReportWorker.Columns.hoursWorked";
            public const string note = "FieldOperations.DailySiteReportWorker.Columns.note";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.DailySiteReportWorker.Actions.New";
            public const string Edit = "FieldOperations.DailySiteReportWorker.Actions.Edit";
            public const string Delete = "FieldOperations.DailySiteReportWorker.Actions.Delete";
            public const string Save = "FieldOperations.DailySiteReportWorker.Actions.Save";
            public const string Cancel = "FieldOperations.DailySiteReportWorker.Actions.Cancel";
            public const string Export = "FieldOperations.DailySiteReportWorker.Actions.Export";
            public const string Refresh = "FieldOperations.DailySiteReportWorker.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.DailySiteReportWorker.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.DailySiteReportWorker.Grid.Search";
            public const string NoData = "FieldOperations.DailySiteReportWorker.Grid.NoData";
            public const string Loading = "FieldOperations.DailySiteReportWorker.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.DailySiteReportWorker.Notifications.Saved";
            public const string Updated = "FieldOperations.DailySiteReportWorker.Notifications.Updated";
            public const string Deleted = "FieldOperations.DailySiteReportWorker.Notifications.Deleted";
            public const string Error = "FieldOperations.DailySiteReportWorker.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.DailySiteReportWorker.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.DailySiteReportWorker.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.DailySiteReportWorker.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.DailySiteReportWorker.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.MeasurementSheet — Ölçüm Föyü / Measurement Sheet</summary>
    public static class FieldOperations_MeasurementSheet
    {
        public const string ScreenId = "FieldOperations.MeasurementSheet";
        public const string Title = "FieldOperations.MeasurementSheet.Title";
        public const string Description = "FieldOperations.MeasurementSheet.Description";
        public static class Columns
        {
            public const string projectId = "FieldOperations.MeasurementSheet.Columns.projectId";
            public const string contractId = "FieldOperations.MeasurementSheet.Columns.contractId";
            public const string sheetNo = "FieldOperations.MeasurementSheet.Columns.sheetNo";
            public const string sheetDate = "FieldOperations.MeasurementSheet.Columns.sheetDate";
            public const string status = "FieldOperations.MeasurementSheet.Columns.status";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.MeasurementSheet.Actions.New";
            public const string Edit = "FieldOperations.MeasurementSheet.Actions.Edit";
            public const string Delete = "FieldOperations.MeasurementSheet.Actions.Delete";
            public const string Save = "FieldOperations.MeasurementSheet.Actions.Save";
            public const string Cancel = "FieldOperations.MeasurementSheet.Actions.Cancel";
            public const string Export = "FieldOperations.MeasurementSheet.Actions.Export";
            public const string Refresh = "FieldOperations.MeasurementSheet.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.MeasurementSheet.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.MeasurementSheet.Grid.Search";
            public const string NoData = "FieldOperations.MeasurementSheet.Grid.NoData";
            public const string Loading = "FieldOperations.MeasurementSheet.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.MeasurementSheet.Notifications.Saved";
            public const string Updated = "FieldOperations.MeasurementSheet.Notifications.Updated";
            public const string Deleted = "FieldOperations.MeasurementSheet.Notifications.Deleted";
            public const string Error = "FieldOperations.MeasurementSheet.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.MeasurementSheet.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.MeasurementSheet.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.MeasurementSheet.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.MeasurementSheet.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.MeasurementSheetLine — Ölçüm Föyü Kalemi / Measurement Sheet Line</summary>
    public static class FieldOperations_MeasurementSheetLine
    {
        public const string ScreenId = "FieldOperations.MeasurementSheetLine";
        public const string Title = "FieldOperations.MeasurementSheetLine.Title";
        public const string Description = "FieldOperations.MeasurementSheetLine.Description";
        public static class Columns
        {
            public const string measurementSheetId = "FieldOperations.MeasurementSheetLine.Columns.measurementSheetId";
            public const string description = "FieldOperations.MeasurementSheetLine.Columns.description";
            public const string quantity = "FieldOperations.MeasurementSheetLine.Columns.quantity";
            public const string unitPrice = "FieldOperations.MeasurementSheetLine.Columns.unitPrice";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.MeasurementSheetLine.Actions.New";
            public const string Edit = "FieldOperations.MeasurementSheetLine.Actions.Edit";
            public const string Delete = "FieldOperations.MeasurementSheetLine.Actions.Delete";
            public const string Save = "FieldOperations.MeasurementSheetLine.Actions.Save";
            public const string Cancel = "FieldOperations.MeasurementSheetLine.Actions.Cancel";
            public const string Export = "FieldOperations.MeasurementSheetLine.Actions.Export";
            public const string Refresh = "FieldOperations.MeasurementSheetLine.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.MeasurementSheetLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.MeasurementSheetLine.Grid.Search";
            public const string NoData = "FieldOperations.MeasurementSheetLine.Grid.NoData";
            public const string Loading = "FieldOperations.MeasurementSheetLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.MeasurementSheetLine.Notifications.Saved";
            public const string Updated = "FieldOperations.MeasurementSheetLine.Notifications.Updated";
            public const string Deleted = "FieldOperations.MeasurementSheetLine.Notifications.Deleted";
            public const string Error = "FieldOperations.MeasurementSheetLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.MeasurementSheetLine.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.MeasurementSheetLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.MeasurementSheetLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.MeasurementSheetLine.Confirm.Delete";
        }
    }

    /// <summary>FieldOperations.ProgressEntry — İlerleme Kaydı / Progress Entry</summary>
    public static class FieldOperations_ProgressEntry
    {
        public const string ScreenId = "FieldOperations.ProgressEntry";
        public const string Title = "FieldOperations.ProgressEntry.Title";
        public const string Description = "FieldOperations.ProgressEntry.Description";
        public static class Columns
        {
            public const string projectId = "FieldOperations.ProgressEntry.Columns.projectId";
            public const string projectPhaseId = "FieldOperations.ProgressEntry.Columns.projectPhaseId";
            public const string entryDate = "FieldOperations.ProgressEntry.Columns.entryDate";
            public const string quantity = "FieldOperations.ProgressEntry.Columns.quantity";
            public const string percentage = "FieldOperations.ProgressEntry.Columns.percentage";
            public const string note = "FieldOperations.ProgressEntry.Columns.note";
        }
        public static class Actions
        {
            public const string New = "FieldOperations.ProgressEntry.Actions.New";
            public const string Edit = "FieldOperations.ProgressEntry.Actions.Edit";
            public const string Delete = "FieldOperations.ProgressEntry.Actions.Delete";
            public const string Save = "FieldOperations.ProgressEntry.Actions.Save";
            public const string Cancel = "FieldOperations.ProgressEntry.Actions.Cancel";
            public const string Export = "FieldOperations.ProgressEntry.Actions.Export";
            public const string Refresh = "FieldOperations.ProgressEntry.Actions.Refresh";
            public const string ColumnChooser = "FieldOperations.ProgressEntry.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "FieldOperations.ProgressEntry.Grid.Search";
            public const string NoData = "FieldOperations.ProgressEntry.Grid.NoData";
            public const string Loading = "FieldOperations.ProgressEntry.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "FieldOperations.ProgressEntry.Notifications.Saved";
            public const string Updated = "FieldOperations.ProgressEntry.Notifications.Updated";
            public const string Deleted = "FieldOperations.ProgressEntry.Notifications.Deleted";
            public const string Error = "FieldOperations.ProgressEntry.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "FieldOperations.ProgressEntry.Popup.CreateTitle";
            public const string EditTitle = "FieldOperations.ProgressEntry.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "FieldOperations.ProgressEntry.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "FieldOperations.ProgressEntry.Confirm.Delete";
        }
    }

    /// <summary>Finance.Collection — Tahsilat / Collection</summary>
    public static class Finance_Collection
    {
        public const string ScreenId = "Finance.Collection";
        public const string Title = "Finance.Collection.Title";
        public const string Description = "Finance.Collection.Description";
        public static class Columns
        {
            public const string partnerId = "Finance.Collection.Columns.partnerId";
            public const string currencyId = "Finance.Collection.Columns.currencyId";
            public const string financialAccountId = "Finance.Collection.Columns.financialAccountId";
            public const string amount = "Finance.Collection.Columns.amount";
            public const string collectionDate = "Finance.Collection.Columns.collectionDate";
            public const string collectionNo = "Finance.Collection.Columns.collectionNo";
            public const string status = "Finance.Collection.Columns.status";
            public const string approvalRequestId = "Finance.Collection.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Finance.Collection.Actions.New";
            public const string Edit = "Finance.Collection.Actions.Edit";
            public const string Delete = "Finance.Collection.Actions.Delete";
            public const string Save = "Finance.Collection.Actions.Save";
            public const string Cancel = "Finance.Collection.Actions.Cancel";
            public const string Export = "Finance.Collection.Actions.Export";
            public const string Refresh = "Finance.Collection.Actions.Refresh";
            public const string ColumnChooser = "Finance.Collection.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.Collection.Grid.Search";
            public const string NoData = "Finance.Collection.Grid.NoData";
            public const string Loading = "Finance.Collection.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.Collection.Notifications.Saved";
            public const string Updated = "Finance.Collection.Notifications.Updated";
            public const string Deleted = "Finance.Collection.Notifications.Deleted";
            public const string Error = "Finance.Collection.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.Collection.Popup.CreateTitle";
            public const string EditTitle = "Finance.Collection.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.Collection.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.Collection.Confirm.Delete";
        }
    }

    /// <summary>Finance.CollectionAllocation — Tahsilat Dağıtımı / Collection Allocation</summary>
    public static class Finance_CollectionAllocation
    {
        public const string ScreenId = "Finance.CollectionAllocation";
        public const string Title = "Finance.CollectionAllocation.Title";
        public const string Description = "Finance.CollectionAllocation.Description";
        public static class Columns
        {
            public const string collectionId = "Finance.CollectionAllocation.Columns.collectionId";
            public const string receivableId = "Finance.CollectionAllocation.Columns.receivableId";
            public const string amount = "Finance.CollectionAllocation.Columns.amount";
        }
        public static class Actions
        {
            public const string New = "Finance.CollectionAllocation.Actions.New";
            public const string Edit = "Finance.CollectionAllocation.Actions.Edit";
            public const string Delete = "Finance.CollectionAllocation.Actions.Delete";
            public const string Save = "Finance.CollectionAllocation.Actions.Save";
            public const string Cancel = "Finance.CollectionAllocation.Actions.Cancel";
            public const string Export = "Finance.CollectionAllocation.Actions.Export";
            public const string Refresh = "Finance.CollectionAllocation.Actions.Refresh";
            public const string ColumnChooser = "Finance.CollectionAllocation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.CollectionAllocation.Grid.Search";
            public const string NoData = "Finance.CollectionAllocation.Grid.NoData";
            public const string Loading = "Finance.CollectionAllocation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.CollectionAllocation.Notifications.Saved";
            public const string Updated = "Finance.CollectionAllocation.Notifications.Updated";
            public const string Deleted = "Finance.CollectionAllocation.Notifications.Deleted";
            public const string Error = "Finance.CollectionAllocation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.CollectionAllocation.Popup.CreateTitle";
            public const string EditTitle = "Finance.CollectionAllocation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.CollectionAllocation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.CollectionAllocation.Confirm.Delete";
        }
    }

    /// <summary>Finance.CostCenter — Masraf Merkezi / Cost Center</summary>
    public static class Finance_CostCenter
    {
        public const string ScreenId = "Finance.CostCenter";
        public const string Title = "Finance.CostCenter.Title";
        public const string Description = "Finance.CostCenter.Description";
        public static class Columns
        {
            public const string parentCostCenterId = "Finance.CostCenter.Columns.parentCostCenterId";
            public const string code = "Finance.CostCenter.Columns.code";
            public const string name = "Finance.CostCenter.Columns.name";
            public const string isActive = "Finance.CostCenter.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Finance.CostCenter.Actions.New";
            public const string Edit = "Finance.CostCenter.Actions.Edit";
            public const string Delete = "Finance.CostCenter.Actions.Delete";
            public const string Save = "Finance.CostCenter.Actions.Save";
            public const string Cancel = "Finance.CostCenter.Actions.Cancel";
            public const string Export = "Finance.CostCenter.Actions.Export";
            public const string Refresh = "Finance.CostCenter.Actions.Refresh";
            public const string ColumnChooser = "Finance.CostCenter.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.CostCenter.Grid.Search";
            public const string NoData = "Finance.CostCenter.Grid.NoData";
            public const string Loading = "Finance.CostCenter.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.CostCenter.Notifications.Saved";
            public const string Updated = "Finance.CostCenter.Notifications.Updated";
            public const string Deleted = "Finance.CostCenter.Notifications.Deleted";
            public const string Error = "Finance.CostCenter.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.CostCenter.Popup.CreateTitle";
            public const string EditTitle = "Finance.CostCenter.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.CostCenter.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.CostCenter.Confirm.Delete";
        }
    }

    /// <summary>Finance.FinancialAccount — Finansal Hesap / Financial Account</summary>
    public static class Finance_FinancialAccount
    {
        public const string ScreenId = "Finance.FinancialAccount";
        public const string Title = "Finance.FinancialAccount.Title";
        public const string Description = "Finance.FinancialAccount.Description";
        public static class Columns
        {
            public const string code = "Finance.FinancialAccount.Columns.code";
            public const string name = "Finance.FinancialAccount.Columns.name";
            public const string accountType = "Finance.FinancialAccount.Columns.accountType";
            public const string currencyId = "Finance.FinancialAccount.Columns.currencyId";
            public const string isActive = "Finance.FinancialAccount.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Finance.FinancialAccount.Actions.New";
            public const string Edit = "Finance.FinancialAccount.Actions.Edit";
            public const string Delete = "Finance.FinancialAccount.Actions.Delete";
            public const string Save = "Finance.FinancialAccount.Actions.Save";
            public const string Cancel = "Finance.FinancialAccount.Actions.Cancel";
            public const string Export = "Finance.FinancialAccount.Actions.Export";
            public const string Refresh = "Finance.FinancialAccount.Actions.Refresh";
            public const string ColumnChooser = "Finance.FinancialAccount.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.FinancialAccount.Grid.Search";
            public const string NoData = "Finance.FinancialAccount.Grid.NoData";
            public const string Loading = "Finance.FinancialAccount.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.FinancialAccount.Notifications.Saved";
            public const string Updated = "Finance.FinancialAccount.Notifications.Updated";
            public const string Deleted = "Finance.FinancialAccount.Notifications.Deleted";
            public const string Error = "Finance.FinancialAccount.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.FinancialAccount.Popup.CreateTitle";
            public const string EditTitle = "Finance.FinancialAccount.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.FinancialAccount.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.FinancialAccount.Confirm.Delete";
        }
    }

    /// <summary>Finance.FinancialTransaction — Finansal İşlem / Financial Transaction</summary>
    public static class Finance_FinancialTransaction
    {
        public const string ScreenId = "Finance.FinancialTransaction";
        public const string Title = "Finance.FinancialTransaction.Title";
        public const string Description = "Finance.FinancialTransaction.Description";
        public static class Columns
        {
            public const string transactionType = "Finance.FinancialTransaction.Columns.transactionType";
            public const string projectId = "Finance.FinancialTransaction.Columns.projectId";
            public const string partnerId = "Finance.FinancialTransaction.Columns.partnerId";
            public const string currencyId = "Finance.FinancialTransaction.Columns.currencyId";
            public const string amount = "Finance.FinancialTransaction.Columns.amount";
            public const string relatedModule = "Finance.FinancialTransaction.Columns.relatedModule";
            public const string relatedEntityType = "Finance.FinancialTransaction.Columns.relatedEntityType";
            public const string relatedEntityId = "Finance.FinancialTransaction.Columns.relatedEntityId";
            public const string financialAccountId = "Finance.FinancialTransaction.Columns.financialAccountId";
            public const string costCenterId = "Finance.FinancialTransaction.Columns.costCenterId";
            public const string transactionDate = "Finance.FinancialTransaction.Columns.transactionDate";
            public const string description = "Finance.FinancialTransaction.Columns.description";
            public const string isReversed = "Finance.FinancialTransaction.Columns.isReversed";
        }
        public static class Actions
        {
            public const string New = "Finance.FinancialTransaction.Actions.New";
            public const string Edit = "Finance.FinancialTransaction.Actions.Edit";
            public const string Delete = "Finance.FinancialTransaction.Actions.Delete";
            public const string Save = "Finance.FinancialTransaction.Actions.Save";
            public const string Cancel = "Finance.FinancialTransaction.Actions.Cancel";
            public const string Export = "Finance.FinancialTransaction.Actions.Export";
            public const string Refresh = "Finance.FinancialTransaction.Actions.Refresh";
            public const string ColumnChooser = "Finance.FinancialTransaction.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.FinancialTransaction.Grid.Search";
            public const string NoData = "Finance.FinancialTransaction.Grid.NoData";
            public const string Loading = "Finance.FinancialTransaction.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.FinancialTransaction.Notifications.Saved";
            public const string Updated = "Finance.FinancialTransaction.Notifications.Updated";
            public const string Deleted = "Finance.FinancialTransaction.Notifications.Deleted";
            public const string Error = "Finance.FinancialTransaction.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.FinancialTransaction.Popup.CreateTitle";
            public const string EditTitle = "Finance.FinancialTransaction.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.FinancialTransaction.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.FinancialTransaction.Confirm.Delete";
        }
    }

    /// <summary>Finance.FinancialTransactionLine — Finansal İşlem Kalemi / Financial Transaction Line</summary>
    public static class Finance_FinancialTransactionLine
    {
        public const string ScreenId = "Finance.FinancialTransactionLine";
        public const string Title = "Finance.FinancialTransactionLine.Title";
        public const string Description = "Finance.FinancialTransactionLine.Description";
        public static class Columns
        {
            public const string financialTransactionId = "Finance.FinancialTransactionLine.Columns.financialTransactionId";
            public const string costCenterId = "Finance.FinancialTransactionLine.Columns.costCenterId";
            public const string projectId = "Finance.FinancialTransactionLine.Columns.projectId";
            public const string amount = "Finance.FinancialTransactionLine.Columns.amount";
            public const string description = "Finance.FinancialTransactionLine.Columns.description";
        }
        public static class Actions
        {
            public const string New = "Finance.FinancialTransactionLine.Actions.New";
            public const string Edit = "Finance.FinancialTransactionLine.Actions.Edit";
            public const string Delete = "Finance.FinancialTransactionLine.Actions.Delete";
            public const string Save = "Finance.FinancialTransactionLine.Actions.Save";
            public const string Cancel = "Finance.FinancialTransactionLine.Actions.Cancel";
            public const string Export = "Finance.FinancialTransactionLine.Actions.Export";
            public const string Refresh = "Finance.FinancialTransactionLine.Actions.Refresh";
            public const string ColumnChooser = "Finance.FinancialTransactionLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.FinancialTransactionLine.Grid.Search";
            public const string NoData = "Finance.FinancialTransactionLine.Grid.NoData";
            public const string Loading = "Finance.FinancialTransactionLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.FinancialTransactionLine.Notifications.Saved";
            public const string Updated = "Finance.FinancialTransactionLine.Notifications.Updated";
            public const string Deleted = "Finance.FinancialTransactionLine.Notifications.Deleted";
            public const string Error = "Finance.FinancialTransactionLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.FinancialTransactionLine.Popup.CreateTitle";
            public const string EditTitle = "Finance.FinancialTransactionLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.FinancialTransactionLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.FinancialTransactionLine.Confirm.Delete";
        }
    }

    /// <summary>Finance.Payable — Borç / Payable</summary>
    public static class Finance_Payable
    {
        public const string ScreenId = "Finance.Payable";
        public const string Title = "Finance.Payable.Title";
        public const string Description = "Finance.Payable.Description";
        public static class Columns
        {
            public const string partnerId = "Finance.Payable.Columns.partnerId";
            public const string currencyId = "Finance.Payable.Columns.currencyId";
            public const string amount = "Finance.Payable.Columns.amount";
            public const string remainingAmount = "Finance.Payable.Columns.remainingAmount";
            public const string dueDate = "Finance.Payable.Columns.dueDate";
            public const string relatedModule = "Finance.Payable.Columns.relatedModule";
            public const string relatedEntityType = "Finance.Payable.Columns.relatedEntityType";
            public const string relatedEntityId = "Finance.Payable.Columns.relatedEntityId";
            public const string isClosed = "Finance.Payable.Columns.isClosed";
        }
        public static class Actions
        {
            public const string New = "Finance.Payable.Actions.New";
            public const string Edit = "Finance.Payable.Actions.Edit";
            public const string Delete = "Finance.Payable.Actions.Delete";
            public const string Save = "Finance.Payable.Actions.Save";
            public const string Cancel = "Finance.Payable.Actions.Cancel";
            public const string Export = "Finance.Payable.Actions.Export";
            public const string Refresh = "Finance.Payable.Actions.Refresh";
            public const string ColumnChooser = "Finance.Payable.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.Payable.Grid.Search";
            public const string NoData = "Finance.Payable.Grid.NoData";
            public const string Loading = "Finance.Payable.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.Payable.Notifications.Saved";
            public const string Updated = "Finance.Payable.Notifications.Updated";
            public const string Deleted = "Finance.Payable.Notifications.Deleted";
            public const string Error = "Finance.Payable.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.Payable.Popup.CreateTitle";
            public const string EditTitle = "Finance.Payable.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.Payable.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.Payable.Confirm.Delete";
        }
    }

    /// <summary>Finance.Payment — Ödeme / Payment</summary>
    public static class Finance_Payment
    {
        public const string ScreenId = "Finance.Payment";
        public const string Title = "Finance.Payment.Title";
        public const string Description = "Finance.Payment.Description";
        public static class Columns
        {
            public const string partnerId = "Finance.Payment.Columns.partnerId";
            public const string currencyId = "Finance.Payment.Columns.currencyId";
            public const string financialAccountId = "Finance.Payment.Columns.financialAccountId";
            public const string amount = "Finance.Payment.Columns.amount";
            public const string paymentDate = "Finance.Payment.Columns.paymentDate";
            public const string paymentNo = "Finance.Payment.Columns.paymentNo";
            public const string status = "Finance.Payment.Columns.status";
            public const string approvalRequestId = "Finance.Payment.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Finance.Payment.Actions.New";
            public const string Edit = "Finance.Payment.Actions.Edit";
            public const string Delete = "Finance.Payment.Actions.Delete";
            public const string Save = "Finance.Payment.Actions.Save";
            public const string Cancel = "Finance.Payment.Actions.Cancel";
            public const string Export = "Finance.Payment.Actions.Export";
            public const string Refresh = "Finance.Payment.Actions.Refresh";
            public const string ColumnChooser = "Finance.Payment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.Payment.Grid.Search";
            public const string NoData = "Finance.Payment.Grid.NoData";
            public const string Loading = "Finance.Payment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.Payment.Notifications.Saved";
            public const string Updated = "Finance.Payment.Notifications.Updated";
            public const string Deleted = "Finance.Payment.Notifications.Deleted";
            public const string Error = "Finance.Payment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.Payment.Popup.CreateTitle";
            public const string EditTitle = "Finance.Payment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.Payment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.Payment.Confirm.Delete";
        }
    }

    /// <summary>Finance.PaymentAllocation — Ödeme Dağıtımı / Payment Allocation</summary>
    public static class Finance_PaymentAllocation
    {
        public const string ScreenId = "Finance.PaymentAllocation";
        public const string Title = "Finance.PaymentAllocation.Title";
        public const string Description = "Finance.PaymentAllocation.Description";
        public static class Columns
        {
            public const string paymentId = "Finance.PaymentAllocation.Columns.paymentId";
            public const string payableId = "Finance.PaymentAllocation.Columns.payableId";
            public const string amount = "Finance.PaymentAllocation.Columns.amount";
        }
        public static class Actions
        {
            public const string New = "Finance.PaymentAllocation.Actions.New";
            public const string Edit = "Finance.PaymentAllocation.Actions.Edit";
            public const string Delete = "Finance.PaymentAllocation.Actions.Delete";
            public const string Save = "Finance.PaymentAllocation.Actions.Save";
            public const string Cancel = "Finance.PaymentAllocation.Actions.Cancel";
            public const string Export = "Finance.PaymentAllocation.Actions.Export";
            public const string Refresh = "Finance.PaymentAllocation.Actions.Refresh";
            public const string ColumnChooser = "Finance.PaymentAllocation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.PaymentAllocation.Grid.Search";
            public const string NoData = "Finance.PaymentAllocation.Grid.NoData";
            public const string Loading = "Finance.PaymentAllocation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.PaymentAllocation.Notifications.Saved";
            public const string Updated = "Finance.PaymentAllocation.Notifications.Updated";
            public const string Deleted = "Finance.PaymentAllocation.Notifications.Deleted";
            public const string Error = "Finance.PaymentAllocation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.PaymentAllocation.Popup.CreateTitle";
            public const string EditTitle = "Finance.PaymentAllocation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.PaymentAllocation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.PaymentAllocation.Confirm.Delete";
        }
    }

    /// <summary>Finance.Receivable — Alacak / Receivable</summary>
    public static class Finance_Receivable
    {
        public const string ScreenId = "Finance.Receivable";
        public const string Title = "Finance.Receivable.Title";
        public const string Description = "Finance.Receivable.Description";
        public static class Columns
        {
            public const string partnerId = "Finance.Receivable.Columns.partnerId";
            public const string currencyId = "Finance.Receivable.Columns.currencyId";
            public const string amount = "Finance.Receivable.Columns.amount";
            public const string remainingAmount = "Finance.Receivable.Columns.remainingAmount";
            public const string dueDate = "Finance.Receivable.Columns.dueDate";
            public const string relatedModule = "Finance.Receivable.Columns.relatedModule";
            public const string relatedEntityType = "Finance.Receivable.Columns.relatedEntityType";
            public const string relatedEntityId = "Finance.Receivable.Columns.relatedEntityId";
            public const string isClosed = "Finance.Receivable.Columns.isClosed";
        }
        public static class Actions
        {
            public const string New = "Finance.Receivable.Actions.New";
            public const string Edit = "Finance.Receivable.Actions.Edit";
            public const string Delete = "Finance.Receivable.Actions.Delete";
            public const string Save = "Finance.Receivable.Actions.Save";
            public const string Cancel = "Finance.Receivable.Actions.Cancel";
            public const string Export = "Finance.Receivable.Actions.Export";
            public const string Refresh = "Finance.Receivable.Actions.Refresh";
            public const string ColumnChooser = "Finance.Receivable.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Finance.Receivable.Grid.Search";
            public const string NoData = "Finance.Receivable.Grid.NoData";
            public const string Loading = "Finance.Receivable.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Finance.Receivable.Notifications.Saved";
            public const string Updated = "Finance.Receivable.Notifications.Updated";
            public const string Deleted = "Finance.Receivable.Notifications.Deleted";
            public const string Error = "Finance.Receivable.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Finance.Receivable.Popup.CreateTitle";
            public const string EditTitle = "Finance.Receivable.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Finance.Receivable.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Finance.Receivable.Confirm.Delete";
        }
    }

    /// <summary>HR.Timesheet — Puantaj / Timesheet</summary>
    public static class HR_Timesheet
    {
        public const string ScreenId = "HR.Timesheet";
        public const string Title = "HR.Timesheet.Title";
        public const string Description = "HR.Timesheet.Description";
        public static class Columns
        {
            public const string timesheetNo = "HR.Timesheet.Columns.timesheetNo";
            public const string periodStart = "HR.Timesheet.Columns.periodStart";
            public const string periodEnd = "HR.Timesheet.Columns.periodEnd";
            public const string status = "HR.Timesheet.Columns.status";
            public const string approvalRequestId = "HR.Timesheet.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "HR.Timesheet.Actions.New";
            public const string Edit = "HR.Timesheet.Actions.Edit";
            public const string Delete = "HR.Timesheet.Actions.Delete";
            public const string Save = "HR.Timesheet.Actions.Save";
            public const string Cancel = "HR.Timesheet.Actions.Cancel";
            public const string Export = "HR.Timesheet.Actions.Export";
            public const string Refresh = "HR.Timesheet.Actions.Refresh";
            public const string ColumnChooser = "HR.Timesheet.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "HR.Timesheet.Grid.Search";
            public const string NoData = "HR.Timesheet.Grid.NoData";
            public const string Loading = "HR.Timesheet.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "HR.Timesheet.Notifications.Saved";
            public const string Updated = "HR.Timesheet.Notifications.Updated";
            public const string Deleted = "HR.Timesheet.Notifications.Deleted";
            public const string Error = "HR.Timesheet.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "HR.Timesheet.Popup.CreateTitle";
            public const string EditTitle = "HR.Timesheet.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "HR.Timesheet.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "HR.Timesheet.Confirm.Delete";
        }
    }

    /// <summary>HR.TimesheetLine — Puantaj Kalemi / Timesheet Line</summary>
    public static class HR_TimesheetLine
    {
        public const string ScreenId = "HR.TimesheetLine";
        public const string Title = "HR.TimesheetLine.Title";
        public const string Description = "HR.TimesheetLine.Description";
        public static class Columns
        {
            public const string timesheetId = "HR.TimesheetLine.Columns.timesheetId";
            public const string employeeId = "HR.TimesheetLine.Columns.employeeId";
            public const string projectId = "HR.TimesheetLine.Columns.projectId";
            public const string workOrderId = "HR.TimesheetLine.Columns.workOrderId";
            public const string workDate = "HR.TimesheetLine.Columns.workDate";
            public const string normalHours = "HR.TimesheetLine.Columns.normalHours";
            public const string overtimeHours = "HR.TimesheetLine.Columns.overtimeHours";
            public const string hourlyCost = "HR.TimesheetLine.Columns.hourlyCost";
        }
        public static class Actions
        {
            public const string New = "HR.TimesheetLine.Actions.New";
            public const string Edit = "HR.TimesheetLine.Actions.Edit";
            public const string Delete = "HR.TimesheetLine.Actions.Delete";
            public const string Save = "HR.TimesheetLine.Actions.Save";
            public const string Cancel = "HR.TimesheetLine.Actions.Cancel";
            public const string Export = "HR.TimesheetLine.Actions.Export";
            public const string Refresh = "HR.TimesheetLine.Actions.Refresh";
            public const string ColumnChooser = "HR.TimesheetLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "HR.TimesheetLine.Grid.Search";
            public const string NoData = "HR.TimesheetLine.Grid.NoData";
            public const string Loading = "HR.TimesheetLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "HR.TimesheetLine.Notifications.Saved";
            public const string Updated = "HR.TimesheetLine.Notifications.Updated";
            public const string Deleted = "HR.TimesheetLine.Notifications.Deleted";
            public const string Error = "HR.TimesheetLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "HR.TimesheetLine.Popup.CreateTitle";
            public const string EditTitle = "HR.TimesheetLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "HR.TimesheetLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "HR.TimesheetLine.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockBalance — Stok Bakiyesi / Stock Balance</summary>
    public static class Inventory_StockBalance
    {
        public const string ScreenId = "Inventory.StockBalance";
        public const string Title = "Inventory.StockBalance.Title";
        public const string Description = "Inventory.StockBalance.Description";
        public static class Columns
        {
            public const string warehouseId = "Inventory.StockBalance.Columns.warehouseId";
            public const string materialId = "Inventory.StockBalance.Columns.materialId";
            public const string quantity = "Inventory.StockBalance.Columns.quantity";
            public const string reservedQuantity = "Inventory.StockBalance.Columns.reservedQuantity";
            public const string totalCost = "Inventory.StockBalance.Columns.totalCost";
            public const string lastRecalculatedAt = "Inventory.StockBalance.Columns.lastRecalculatedAt";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockBalance.Actions.New";
            public const string Edit = "Inventory.StockBalance.Actions.Edit";
            public const string Delete = "Inventory.StockBalance.Actions.Delete";
            public const string Save = "Inventory.StockBalance.Actions.Save";
            public const string Cancel = "Inventory.StockBalance.Actions.Cancel";
            public const string Export = "Inventory.StockBalance.Actions.Export";
            public const string Refresh = "Inventory.StockBalance.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockBalance.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockBalance.Grid.Search";
            public const string NoData = "Inventory.StockBalance.Grid.NoData";
            public const string Loading = "Inventory.StockBalance.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockBalance.Notifications.Saved";
            public const string Updated = "Inventory.StockBalance.Notifications.Updated";
            public const string Deleted = "Inventory.StockBalance.Notifications.Deleted";
            public const string Error = "Inventory.StockBalance.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockBalance.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockBalance.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockBalance.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockBalance.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockCount — Stok Sayımı / Stock Count</summary>
    public static class Inventory_StockCount
    {
        public const string ScreenId = "Inventory.StockCount";
        public const string Title = "Inventory.StockCount.Title";
        public const string Description = "Inventory.StockCount.Description";
        public static class Columns
        {
            public const string warehouseId = "Inventory.StockCount.Columns.warehouseId";
            public const string countNo = "Inventory.StockCount.Columns.countNo";
            public const string countDate = "Inventory.StockCount.Columns.countDate";
            public const string status = "Inventory.StockCount.Columns.status";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockCount.Actions.New";
            public const string Edit = "Inventory.StockCount.Actions.Edit";
            public const string Delete = "Inventory.StockCount.Actions.Delete";
            public const string Save = "Inventory.StockCount.Actions.Save";
            public const string Cancel = "Inventory.StockCount.Actions.Cancel";
            public const string Export = "Inventory.StockCount.Actions.Export";
            public const string Refresh = "Inventory.StockCount.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockCount.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockCount.Grid.Search";
            public const string NoData = "Inventory.StockCount.Grid.NoData";
            public const string Loading = "Inventory.StockCount.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockCount.Notifications.Saved";
            public const string Updated = "Inventory.StockCount.Notifications.Updated";
            public const string Deleted = "Inventory.StockCount.Notifications.Deleted";
            public const string Error = "Inventory.StockCount.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockCount.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockCount.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockCount.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockCount.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockCountLine — Stok Sayımı Kalemi / Stock Count Line</summary>
    public static class Inventory_StockCountLine
    {
        public const string ScreenId = "Inventory.StockCountLine";
        public const string Title = "Inventory.StockCountLine.Title";
        public const string Description = "Inventory.StockCountLine.Description";
        public static class Columns
        {
            public const string stockCountId = "Inventory.StockCountLine.Columns.stockCountId";
            public const string materialId = "Inventory.StockCountLine.Columns.materialId";
            public const string systemQuantity = "Inventory.StockCountLine.Columns.systemQuantity";
            public const string countedQuantity = "Inventory.StockCountLine.Columns.countedQuantity";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockCountLine.Actions.New";
            public const string Edit = "Inventory.StockCountLine.Actions.Edit";
            public const string Delete = "Inventory.StockCountLine.Actions.Delete";
            public const string Save = "Inventory.StockCountLine.Actions.Save";
            public const string Cancel = "Inventory.StockCountLine.Actions.Cancel";
            public const string Export = "Inventory.StockCountLine.Actions.Export";
            public const string Refresh = "Inventory.StockCountLine.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockCountLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockCountLine.Grid.Search";
            public const string NoData = "Inventory.StockCountLine.Grid.NoData";
            public const string Loading = "Inventory.StockCountLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockCountLine.Notifications.Saved";
            public const string Updated = "Inventory.StockCountLine.Notifications.Updated";
            public const string Deleted = "Inventory.StockCountLine.Notifications.Deleted";
            public const string Error = "Inventory.StockCountLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockCountLine.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockCountLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockCountLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockCountLine.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockDocument — Stok Belgesi / Stock Document</summary>
    public static class Inventory_StockDocument
    {
        public const string ScreenId = "Inventory.StockDocument";
        public const string Title = "Inventory.StockDocument.Title";
        public const string Description = "Inventory.StockDocument.Description";
        public static class Columns
        {
            public const string documentTypeId = "Inventory.StockDocument.Columns.documentTypeId";
            public const string sourceWarehouseId = "Inventory.StockDocument.Columns.sourceWarehouseId";
            public const string targetWarehouseId = "Inventory.StockDocument.Columns.targetWarehouseId";
            public const string projectId = "Inventory.StockDocument.Columns.projectId";
            public const string status = "Inventory.StockDocument.Columns.status";
            public const string documentNo = "Inventory.StockDocument.Columns.documentNo";
            public const string documentDate = "Inventory.StockDocument.Columns.documentDate";
            public const string note = "Inventory.StockDocument.Columns.note";
            public const string approvalRequestId = "Inventory.StockDocument.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockDocument.Actions.New";
            public const string Edit = "Inventory.StockDocument.Actions.Edit";
            public const string Delete = "Inventory.StockDocument.Actions.Delete";
            public const string Save = "Inventory.StockDocument.Actions.Save";
            public const string Cancel = "Inventory.StockDocument.Actions.Cancel";
            public const string Export = "Inventory.StockDocument.Actions.Export";
            public const string Refresh = "Inventory.StockDocument.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockDocument.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockDocument.Grid.Search";
            public const string NoData = "Inventory.StockDocument.Grid.NoData";
            public const string Loading = "Inventory.StockDocument.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockDocument.Notifications.Saved";
            public const string Updated = "Inventory.StockDocument.Notifications.Updated";
            public const string Deleted = "Inventory.StockDocument.Notifications.Deleted";
            public const string Error = "Inventory.StockDocument.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockDocument.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockDocument.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockDocument.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockDocument.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockDocumentLine — Stok Belgesi Kalemi / Stock Document Line</summary>
    public static class Inventory_StockDocumentLine
    {
        public const string ScreenId = "Inventory.StockDocumentLine";
        public const string Title = "Inventory.StockDocumentLine.Title";
        public const string Description = "Inventory.StockDocumentLine.Description";
        public static class Columns
        {
            public const string stockDocumentId = "Inventory.StockDocumentLine.Columns.stockDocumentId";
            public const string materialId = "Inventory.StockDocumentLine.Columns.materialId";
            public const string unitOfMeasureId = "Inventory.StockDocumentLine.Columns.unitOfMeasureId";
            public const string quantity = "Inventory.StockDocumentLine.Columns.quantity";
            public const string unitPrice = "Inventory.StockDocumentLine.Columns.unitPrice";
            public const string currencyId = "Inventory.StockDocumentLine.Columns.currencyId";
            public const string note = "Inventory.StockDocumentLine.Columns.note";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockDocumentLine.Actions.New";
            public const string Edit = "Inventory.StockDocumentLine.Actions.Edit";
            public const string Delete = "Inventory.StockDocumentLine.Actions.Delete";
            public const string Save = "Inventory.StockDocumentLine.Actions.Save";
            public const string Cancel = "Inventory.StockDocumentLine.Actions.Cancel";
            public const string Export = "Inventory.StockDocumentLine.Actions.Export";
            public const string Refresh = "Inventory.StockDocumentLine.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockDocumentLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockDocumentLine.Grid.Search";
            public const string NoData = "Inventory.StockDocumentLine.Grid.NoData";
            public const string Loading = "Inventory.StockDocumentLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockDocumentLine.Notifications.Saved";
            public const string Updated = "Inventory.StockDocumentLine.Notifications.Updated";
            public const string Deleted = "Inventory.StockDocumentLine.Notifications.Deleted";
            public const string Error = "Inventory.StockDocumentLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockDocumentLine.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockDocumentLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockDocumentLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockDocumentLine.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockDocumentType — Stok Belgesi Türü / Stock Document Type</summary>
    public static class Inventory_StockDocumentType
    {
        public const string ScreenId = "Inventory.StockDocumentType";
        public const string Title = "Inventory.StockDocumentType.Title";
        public const string Description = "Inventory.StockDocumentType.Description";
        public static class Columns
        {
            public const string code = "Inventory.StockDocumentType.Columns.code";
            public const string name = "Inventory.StockDocumentType.Columns.name";
            public const string direction = "Inventory.StockDocumentType.Columns.direction";
            public const string isActive = "Inventory.StockDocumentType.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockDocumentType.Actions.New";
            public const string Edit = "Inventory.StockDocumentType.Actions.Edit";
            public const string Delete = "Inventory.StockDocumentType.Actions.Delete";
            public const string Save = "Inventory.StockDocumentType.Actions.Save";
            public const string Cancel = "Inventory.StockDocumentType.Actions.Cancel";
            public const string Export = "Inventory.StockDocumentType.Actions.Export";
            public const string Refresh = "Inventory.StockDocumentType.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockDocumentType.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockDocumentType.Grid.Search";
            public const string NoData = "Inventory.StockDocumentType.Grid.NoData";
            public const string Loading = "Inventory.StockDocumentType.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockDocumentType.Notifications.Saved";
            public const string Updated = "Inventory.StockDocumentType.Notifications.Updated";
            public const string Deleted = "Inventory.StockDocumentType.Notifications.Deleted";
            public const string Error = "Inventory.StockDocumentType.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockDocumentType.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockDocumentType.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockDocumentType.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockDocumentType.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockIssueAllocation — Stok Çıkış Dağıtımı / Stock Issue Allocation</summary>
    public static class Inventory_StockIssueAllocation
    {
        public const string ScreenId = "Inventory.StockIssueAllocation";
        public const string Title = "Inventory.StockIssueAllocation.Title";
        public const string Description = "Inventory.StockIssueAllocation.Description";
        public static class Columns
        {
            public const string stockDocumentLineId = "Inventory.StockIssueAllocation.Columns.stockDocumentLineId";
            public const string stockLotId = "Inventory.StockIssueAllocation.Columns.stockLotId";
            public const string quantity = "Inventory.StockIssueAllocation.Columns.quantity";
            public const string unitCost = "Inventory.StockIssueAllocation.Columns.unitCost";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockIssueAllocation.Actions.New";
            public const string Edit = "Inventory.StockIssueAllocation.Actions.Edit";
            public const string Delete = "Inventory.StockIssueAllocation.Actions.Delete";
            public const string Save = "Inventory.StockIssueAllocation.Actions.Save";
            public const string Cancel = "Inventory.StockIssueAllocation.Actions.Cancel";
            public const string Export = "Inventory.StockIssueAllocation.Actions.Export";
            public const string Refresh = "Inventory.StockIssueAllocation.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockIssueAllocation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockIssueAllocation.Grid.Search";
            public const string NoData = "Inventory.StockIssueAllocation.Grid.NoData";
            public const string Loading = "Inventory.StockIssueAllocation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockIssueAllocation.Notifications.Saved";
            public const string Updated = "Inventory.StockIssueAllocation.Notifications.Updated";
            public const string Deleted = "Inventory.StockIssueAllocation.Notifications.Deleted";
            public const string Error = "Inventory.StockIssueAllocation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockIssueAllocation.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockIssueAllocation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockIssueAllocation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockIssueAllocation.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockLot — Stok Partisi / Stock Lot</summary>
    public static class Inventory_StockLot
    {
        public const string ScreenId = "Inventory.StockLot";
        public const string Title = "Inventory.StockLot.Title";
        public const string Description = "Inventory.StockLot.Description";
        public static class Columns
        {
            public const string warehouseId = "Inventory.StockLot.Columns.warehouseId";
            public const string materialId = "Inventory.StockLot.Columns.materialId";
            public const string sourceStockDocumentLineId = "Inventory.StockLot.Columns.sourceStockDocumentLineId";
            public const string lotNo = "Inventory.StockLot.Columns.lotNo";
            public const string initialQuantity = "Inventory.StockLot.Columns.initialQuantity";
            public const string remainingQuantity = "Inventory.StockLot.Columns.remainingQuantity";
            public const string unitCost = "Inventory.StockLot.Columns.unitCost";
            public const string receivedAt = "Inventory.StockLot.Columns.receivedAt";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockLot.Actions.New";
            public const string Edit = "Inventory.StockLot.Actions.Edit";
            public const string Delete = "Inventory.StockLot.Actions.Delete";
            public const string Save = "Inventory.StockLot.Actions.Save";
            public const string Cancel = "Inventory.StockLot.Actions.Cancel";
            public const string Export = "Inventory.StockLot.Actions.Export";
            public const string Refresh = "Inventory.StockLot.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockLot.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockLot.Grid.Search";
            public const string NoData = "Inventory.StockLot.Grid.NoData";
            public const string Loading = "Inventory.StockLot.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockLot.Notifications.Saved";
            public const string Updated = "Inventory.StockLot.Notifications.Updated";
            public const string Deleted = "Inventory.StockLot.Notifications.Deleted";
            public const string Error = "Inventory.StockLot.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockLot.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockLot.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockLot.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockLot.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockReservation — Stok Rezervasyonu / Stock Reservation</summary>
    public static class Inventory_StockReservation
    {
        public const string ScreenId = "Inventory.StockReservation";
        public const string Title = "Inventory.StockReservation.Title";
        public const string Description = "Inventory.StockReservation.Description";
        public static class Columns
        {
            public const string warehouseId = "Inventory.StockReservation.Columns.warehouseId";
            public const string materialId = "Inventory.StockReservation.Columns.materialId";
            public const string quantity = "Inventory.StockReservation.Columns.quantity";
            public const string relatedModule = "Inventory.StockReservation.Columns.relatedModule";
            public const string relatedEntityType = "Inventory.StockReservation.Columns.relatedEntityType";
            public const string relatedEntityId = "Inventory.StockReservation.Columns.relatedEntityId";
            public const string isReleased = "Inventory.StockReservation.Columns.isReleased";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockReservation.Actions.New";
            public const string Edit = "Inventory.StockReservation.Actions.Edit";
            public const string Delete = "Inventory.StockReservation.Actions.Delete";
            public const string Save = "Inventory.StockReservation.Actions.Save";
            public const string Cancel = "Inventory.StockReservation.Actions.Cancel";
            public const string Export = "Inventory.StockReservation.Actions.Export";
            public const string Refresh = "Inventory.StockReservation.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockReservation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockReservation.Grid.Search";
            public const string NoData = "Inventory.StockReservation.Grid.NoData";
            public const string Loading = "Inventory.StockReservation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockReservation.Notifications.Saved";
            public const string Updated = "Inventory.StockReservation.Notifications.Updated";
            public const string Deleted = "Inventory.StockReservation.Notifications.Deleted";
            public const string Error = "Inventory.StockReservation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockReservation.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockReservation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockReservation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockReservation.Confirm.Delete";
        }
    }

    /// <summary>Inventory.StockTransaction — Stok Hareketi / Stock Transaction</summary>
    public static class Inventory_StockTransaction
    {
        public const string ScreenId = "Inventory.StockTransaction";
        public const string Title = "Inventory.StockTransaction.Title";
        public const string Description = "Inventory.StockTransaction.Description";
        public static class Columns
        {
            public const string stockDocumentId = "Inventory.StockTransaction.Columns.stockDocumentId";
            public const string stockDocumentLineId = "Inventory.StockTransaction.Columns.stockDocumentLineId";
            public const string stockLotId = "Inventory.StockTransaction.Columns.stockLotId";
            public const string warehouseId = "Inventory.StockTransaction.Columns.warehouseId";
            public const string materialId = "Inventory.StockTransaction.Columns.materialId";
            public const string quantity = "Inventory.StockTransaction.Columns.quantity";
            public const string unitCost = "Inventory.StockTransaction.Columns.unitCost";
            public const string transactionDate = "Inventory.StockTransaction.Columns.transactionDate";
        }
        public static class Actions
        {
            public const string New = "Inventory.StockTransaction.Actions.New";
            public const string Edit = "Inventory.StockTransaction.Actions.Edit";
            public const string Delete = "Inventory.StockTransaction.Actions.Delete";
            public const string Save = "Inventory.StockTransaction.Actions.Save";
            public const string Cancel = "Inventory.StockTransaction.Actions.Cancel";
            public const string Export = "Inventory.StockTransaction.Actions.Export";
            public const string Refresh = "Inventory.StockTransaction.Actions.Refresh";
            public const string ColumnChooser = "Inventory.StockTransaction.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.StockTransaction.Grid.Search";
            public const string NoData = "Inventory.StockTransaction.Grid.NoData";
            public const string Loading = "Inventory.StockTransaction.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.StockTransaction.Notifications.Saved";
            public const string Updated = "Inventory.StockTransaction.Notifications.Updated";
            public const string Deleted = "Inventory.StockTransaction.Notifications.Deleted";
            public const string Error = "Inventory.StockTransaction.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.StockTransaction.Popup.CreateTitle";
            public const string EditTitle = "Inventory.StockTransaction.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.StockTransaction.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.StockTransaction.Confirm.Delete";
        }
    }

    /// <summary>Inventory.Warehouse — Depo / Warehouse</summary>
    public static class Inventory_Warehouse
    {
        public const string ScreenId = "Inventory.Warehouse";
        public const string Title = "Inventory.Warehouse.Title";
        public const string Description = "Inventory.Warehouse.Description";
        public static class Columns
        {
            public const string companyId = "Inventory.Warehouse.Columns.companyId";
            public const string branchId = "Inventory.Warehouse.Columns.branchId";
            public const string projectId = "Inventory.Warehouse.Columns.projectId";
            public const string warehouseType = "Inventory.Warehouse.Columns.warehouseType";
            public const string code = "Inventory.Warehouse.Columns.code";
            public const string name = "Inventory.Warehouse.Columns.name";
            public const string isActive = "Inventory.Warehouse.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Inventory.Warehouse.Actions.New";
            public const string Edit = "Inventory.Warehouse.Actions.Edit";
            public const string Delete = "Inventory.Warehouse.Actions.Delete";
            public const string Save = "Inventory.Warehouse.Actions.Save";
            public const string Cancel = "Inventory.Warehouse.Actions.Cancel";
            public const string Export = "Inventory.Warehouse.Actions.Export";
            public const string Refresh = "Inventory.Warehouse.Actions.Refresh";
            public const string ColumnChooser = "Inventory.Warehouse.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.Warehouse.Grid.Search";
            public const string NoData = "Inventory.Warehouse.Grid.NoData";
            public const string Loading = "Inventory.Warehouse.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.Warehouse.Notifications.Saved";
            public const string Updated = "Inventory.Warehouse.Notifications.Updated";
            public const string Deleted = "Inventory.Warehouse.Notifications.Deleted";
            public const string Error = "Inventory.Warehouse.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.Warehouse.Popup.CreateTitle";
            public const string EditTitle = "Inventory.Warehouse.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.Warehouse.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.Warehouse.Confirm.Delete";
        }
    }

    /// <summary>Inventory.WarehouseLocation — Depo Lokasyonu / Warehouse Location</summary>
    public static class Inventory_WarehouseLocation
    {
        public const string ScreenId = "Inventory.WarehouseLocation";
        public const string Title = "Inventory.WarehouseLocation.Title";
        public const string Description = "Inventory.WarehouseLocation.Description";
        public static class Columns
        {
            public const string warehouseId = "Inventory.WarehouseLocation.Columns.warehouseId";
            public const string parentLocationId = "Inventory.WarehouseLocation.Columns.parentLocationId";
            public const string code = "Inventory.WarehouseLocation.Columns.code";
            public const string name = "Inventory.WarehouseLocation.Columns.name";
        }
        public static class Actions
        {
            public const string New = "Inventory.WarehouseLocation.Actions.New";
            public const string Edit = "Inventory.WarehouseLocation.Actions.Edit";
            public const string Delete = "Inventory.WarehouseLocation.Actions.Delete";
            public const string Save = "Inventory.WarehouseLocation.Actions.Save";
            public const string Cancel = "Inventory.WarehouseLocation.Actions.Cancel";
            public const string Export = "Inventory.WarehouseLocation.Actions.Export";
            public const string Refresh = "Inventory.WarehouseLocation.Actions.Refresh";
            public const string ColumnChooser = "Inventory.WarehouseLocation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.WarehouseLocation.Grid.Search";
            public const string NoData = "Inventory.WarehouseLocation.Grid.NoData";
            public const string Loading = "Inventory.WarehouseLocation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.WarehouseLocation.Notifications.Saved";
            public const string Updated = "Inventory.WarehouseLocation.Notifications.Updated";
            public const string Deleted = "Inventory.WarehouseLocation.Notifications.Deleted";
            public const string Error = "Inventory.WarehouseLocation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.WarehouseLocation.Popup.CreateTitle";
            public const string EditTitle = "Inventory.WarehouseLocation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.WarehouseLocation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.WarehouseLocation.Confirm.Delete";
        }
    }

    /// <summary>Inventory.WarehouseTransfer — Depo Transferi / Warehouse Transfer</summary>
    public static class Inventory_WarehouseTransfer
    {
        public const string ScreenId = "Inventory.WarehouseTransfer";
        public const string Title = "Inventory.WarehouseTransfer.Title";
        public const string Description = "Inventory.WarehouseTransfer.Description";
        public static class Columns
        {
            public const string sourceWarehouseId = "Inventory.WarehouseTransfer.Columns.sourceWarehouseId";
            public const string targetWarehouseId = "Inventory.WarehouseTransfer.Columns.targetWarehouseId";
            public const string transferNo = "Inventory.WarehouseTransfer.Columns.transferNo";
            public const string transferDate = "Inventory.WarehouseTransfer.Columns.transferDate";
            public const string status = "Inventory.WarehouseTransfer.Columns.status";
        }
        public static class Actions
        {
            public const string New = "Inventory.WarehouseTransfer.Actions.New";
            public const string Edit = "Inventory.WarehouseTransfer.Actions.Edit";
            public const string Delete = "Inventory.WarehouseTransfer.Actions.Delete";
            public const string Save = "Inventory.WarehouseTransfer.Actions.Save";
            public const string Cancel = "Inventory.WarehouseTransfer.Actions.Cancel";
            public const string Export = "Inventory.WarehouseTransfer.Actions.Export";
            public const string Refresh = "Inventory.WarehouseTransfer.Actions.Refresh";
            public const string ColumnChooser = "Inventory.WarehouseTransfer.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.WarehouseTransfer.Grid.Search";
            public const string NoData = "Inventory.WarehouseTransfer.Grid.NoData";
            public const string Loading = "Inventory.WarehouseTransfer.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.WarehouseTransfer.Notifications.Saved";
            public const string Updated = "Inventory.WarehouseTransfer.Notifications.Updated";
            public const string Deleted = "Inventory.WarehouseTransfer.Notifications.Deleted";
            public const string Error = "Inventory.WarehouseTransfer.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.WarehouseTransfer.Popup.CreateTitle";
            public const string EditTitle = "Inventory.WarehouseTransfer.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.WarehouseTransfer.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.WarehouseTransfer.Confirm.Delete";
        }
    }

    /// <summary>Inventory.WarehouseTransferLine — Depo Transferi Kalemi / Warehouse Transfer Line</summary>
    public static class Inventory_WarehouseTransferLine
    {
        public const string ScreenId = "Inventory.WarehouseTransferLine";
        public const string Title = "Inventory.WarehouseTransferLine.Title";
        public const string Description = "Inventory.WarehouseTransferLine.Description";
        public static class Columns
        {
            public const string warehouseTransferId = "Inventory.WarehouseTransferLine.Columns.warehouseTransferId";
            public const string materialId = "Inventory.WarehouseTransferLine.Columns.materialId";
            public const string quantity = "Inventory.WarehouseTransferLine.Columns.quantity";
        }
        public static class Actions
        {
            public const string New = "Inventory.WarehouseTransferLine.Actions.New";
            public const string Edit = "Inventory.WarehouseTransferLine.Actions.Edit";
            public const string Delete = "Inventory.WarehouseTransferLine.Actions.Delete";
            public const string Save = "Inventory.WarehouseTransferLine.Actions.Save";
            public const string Cancel = "Inventory.WarehouseTransferLine.Actions.Cancel";
            public const string Export = "Inventory.WarehouseTransferLine.Actions.Export";
            public const string Refresh = "Inventory.WarehouseTransferLine.Actions.Refresh";
            public const string ColumnChooser = "Inventory.WarehouseTransferLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Inventory.WarehouseTransferLine.Grid.Search";
            public const string NoData = "Inventory.WarehouseTransferLine.Grid.NoData";
            public const string Loading = "Inventory.WarehouseTransferLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Inventory.WarehouseTransferLine.Notifications.Saved";
            public const string Updated = "Inventory.WarehouseTransferLine.Notifications.Updated";
            public const string Deleted = "Inventory.WarehouseTransferLine.Notifications.Deleted";
            public const string Error = "Inventory.WarehouseTransferLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Inventory.WarehouseTransferLine.Popup.CreateTitle";
            public const string EditTitle = "Inventory.WarehouseTransferLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Inventory.WarehouseTransferLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Inventory.WarehouseTransferLine.Confirm.Delete";
        }
    }

    /// <summary>Notifications.Notification — Bildirim / Notification</summary>
    public static class Notifications_Notification
    {
        public const string ScreenId = "Notifications.Notification";
        public const string Title = "Notifications.Notification.Title";
        public const string Description = "Notifications.Notification.Description";
        public static class Columns
        {
            public const string title = "Notifications.Notification.Columns.title";
            public const string body = "Notifications.Notification.Columns.body";
            public const string notificationType = "Notifications.Notification.Columns.notificationType";
            public const string relatedModule = "Notifications.Notification.Columns.relatedModule";
            public const string relatedEntityType = "Notifications.Notification.Columns.relatedEntityType";
            public const string relatedEntityId = "Notifications.Notification.Columns.relatedEntityId";
        }
        public static class Actions
        {
            public const string New = "Notifications.Notification.Actions.New";
            public const string Edit = "Notifications.Notification.Actions.Edit";
            public const string Delete = "Notifications.Notification.Actions.Delete";
            public const string Save = "Notifications.Notification.Actions.Save";
            public const string Cancel = "Notifications.Notification.Actions.Cancel";
            public const string Export = "Notifications.Notification.Actions.Export";
            public const string Refresh = "Notifications.Notification.Actions.Refresh";
            public const string ColumnChooser = "Notifications.Notification.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Notifications.Notification.Grid.Search";
            public const string NoData = "Notifications.Notification.Grid.NoData";
            public const string Loading = "Notifications.Notification.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Notifications.Notification.Notifications.Saved";
            public const string Updated = "Notifications.Notification.Notifications.Updated";
            public const string Deleted = "Notifications.Notification.Notifications.Deleted";
            public const string Error = "Notifications.Notification.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Notifications.Notification.Popup.CreateTitle";
            public const string EditTitle = "Notifications.Notification.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Notifications.Notification.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Notifications.Notification.Confirm.Delete";
        }
    }

    /// <summary>Notifications.NotificationPreference — Bildirim Tercihi / Notification Preference</summary>
    public static class Notifications_NotificationPreference
    {
        public const string ScreenId = "Notifications.NotificationPreference";
        public const string Title = "Notifications.NotificationPreference.Title";
        public const string Description = "Notifications.NotificationPreference.Description";
        public static class Columns
        {
            public const string userId = "Notifications.NotificationPreference.Columns.userId";
            public const string notificationType = "Notifications.NotificationPreference.Columns.notificationType";
            public const string inAppEnabled = "Notifications.NotificationPreference.Columns.inAppEnabled";
            public const string emailEnabled = "Notifications.NotificationPreference.Columns.emailEnabled";
        }
        public static class Actions
        {
            public const string New = "Notifications.NotificationPreference.Actions.New";
            public const string Edit = "Notifications.NotificationPreference.Actions.Edit";
            public const string Delete = "Notifications.NotificationPreference.Actions.Delete";
            public const string Save = "Notifications.NotificationPreference.Actions.Save";
            public const string Cancel = "Notifications.NotificationPreference.Actions.Cancel";
            public const string Export = "Notifications.NotificationPreference.Actions.Export";
            public const string Refresh = "Notifications.NotificationPreference.Actions.Refresh";
            public const string ColumnChooser = "Notifications.NotificationPreference.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Notifications.NotificationPreference.Grid.Search";
            public const string NoData = "Notifications.NotificationPreference.Grid.NoData";
            public const string Loading = "Notifications.NotificationPreference.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Notifications.NotificationPreference.Notifications.Saved";
            public const string Updated = "Notifications.NotificationPreference.Notifications.Updated";
            public const string Deleted = "Notifications.NotificationPreference.Notifications.Deleted";
            public const string Error = "Notifications.NotificationPreference.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Notifications.NotificationPreference.Popup.CreateTitle";
            public const string EditTitle = "Notifications.NotificationPreference.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Notifications.NotificationPreference.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Notifications.NotificationPreference.Confirm.Delete";
        }
    }

    /// <summary>Notifications.NotificationRecipient — Bildirim Alıcısı / Notification Recipient</summary>
    public static class Notifications_NotificationRecipient
    {
        public const string ScreenId = "Notifications.NotificationRecipient";
        public const string Title = "Notifications.NotificationRecipient.Title";
        public const string Description = "Notifications.NotificationRecipient.Description";
        public static class Columns
        {
            public const string notificationId = "Notifications.NotificationRecipient.Columns.notificationId";
            public const string userId = "Notifications.NotificationRecipient.Columns.userId";
            public const string isRead = "Notifications.NotificationRecipient.Columns.isRead";
            public const string readAt = "Notifications.NotificationRecipient.Columns.readAt";
        }
        public static class Actions
        {
            public const string New = "Notifications.NotificationRecipient.Actions.New";
            public const string Edit = "Notifications.NotificationRecipient.Actions.Edit";
            public const string Delete = "Notifications.NotificationRecipient.Actions.Delete";
            public const string Save = "Notifications.NotificationRecipient.Actions.Save";
            public const string Cancel = "Notifications.NotificationRecipient.Actions.Cancel";
            public const string Export = "Notifications.NotificationRecipient.Actions.Export";
            public const string Refresh = "Notifications.NotificationRecipient.Actions.Refresh";
            public const string ColumnChooser = "Notifications.NotificationRecipient.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Notifications.NotificationRecipient.Grid.Search";
            public const string NoData = "Notifications.NotificationRecipient.Grid.NoData";
            public const string Loading = "Notifications.NotificationRecipient.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Notifications.NotificationRecipient.Notifications.Saved";
            public const string Updated = "Notifications.NotificationRecipient.Notifications.Updated";
            public const string Deleted = "Notifications.NotificationRecipient.Notifications.Deleted";
            public const string Error = "Notifications.NotificationRecipient.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Notifications.NotificationRecipient.Popup.CreateTitle";
            public const string EditTitle = "Notifications.NotificationRecipient.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Notifications.NotificationRecipient.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Notifications.NotificationRecipient.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrder — İş Emri / Work Order</summary>
    public static class Operations_WorkOrder
    {
        public const string ScreenId = "Operations.WorkOrder";
        public const string Title = "Operations.WorkOrder.Title";
        public const string Description = "Operations.WorkOrder.Description";
        public static class Columns
        {
            public const string workOrderTypeId = "Operations.WorkOrder.Columns.workOrderTypeId";
            public const string projectId = "Operations.WorkOrder.Columns.projectId";
            public const string projectPhaseId = "Operations.WorkOrder.Columns.projectPhaseId";
            public const string projectLocationId = "Operations.WorkOrder.Columns.projectLocationId";
            public const string status = "Operations.WorkOrder.Columns.status";
            public const string workOrderNo = "Operations.WorkOrder.Columns.workOrderNo";
            public const string title = "Operations.WorkOrder.Columns.title";
            public const string description = "Operations.WorkOrder.Columns.description";
            public const string plannedStart = "Operations.WorkOrder.Columns.plannedStart";
            public const string plannedEnd = "Operations.WorkOrder.Columns.plannedEnd";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrder.Actions.New";
            public const string Edit = "Operations.WorkOrder.Actions.Edit";
            public const string Delete = "Operations.WorkOrder.Actions.Delete";
            public const string Save = "Operations.WorkOrder.Actions.Save";
            public const string Cancel = "Operations.WorkOrder.Actions.Cancel";
            public const string Export = "Operations.WorkOrder.Actions.Export";
            public const string Refresh = "Operations.WorkOrder.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrder.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrder.Grid.Search";
            public const string NoData = "Operations.WorkOrder.Grid.NoData";
            public const string Loading = "Operations.WorkOrder.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrder.Notifications.Saved";
            public const string Updated = "Operations.WorkOrder.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrder.Notifications.Deleted";
            public const string Error = "Operations.WorkOrder.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrder.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrder.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrder.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrder.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderAssignment — İş Emri Ataması / Work Order Assignment</summary>
    public static class Operations_WorkOrderAssignment
    {
        public const string ScreenId = "Operations.WorkOrderAssignment";
        public const string Title = "Operations.WorkOrderAssignment.Title";
        public const string Description = "Operations.WorkOrderAssignment.Description";
        public static class Columns
        {
            public const string workOrderId = "Operations.WorkOrderAssignment.Columns.workOrderId";
            public const string employeeId = "Operations.WorkOrderAssignment.Columns.employeeId";
            public const string userId = "Operations.WorkOrderAssignment.Columns.userId";
            public const string assignmentRole = "Operations.WorkOrderAssignment.Columns.assignmentRole";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderAssignment.Actions.New";
            public const string Edit = "Operations.WorkOrderAssignment.Actions.Edit";
            public const string Delete = "Operations.WorkOrderAssignment.Actions.Delete";
            public const string Save = "Operations.WorkOrderAssignment.Actions.Save";
            public const string Cancel = "Operations.WorkOrderAssignment.Actions.Cancel";
            public const string Export = "Operations.WorkOrderAssignment.Actions.Export";
            public const string Refresh = "Operations.WorkOrderAssignment.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderAssignment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderAssignment.Grid.Search";
            public const string NoData = "Operations.WorkOrderAssignment.Grid.NoData";
            public const string Loading = "Operations.WorkOrderAssignment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderAssignment.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderAssignment.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderAssignment.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderAssignment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderAssignment.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderAssignment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderAssignment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderAssignment.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderChecklist — İş Emri Kontrol Listesi / Work Order Checklist</summary>
    public static class Operations_WorkOrderChecklist
    {
        public const string ScreenId = "Operations.WorkOrderChecklist";
        public const string Title = "Operations.WorkOrderChecklist.Title";
        public const string Description = "Operations.WorkOrderChecklist.Description";
        public static class Columns
        {
            public const string workOrderId = "Operations.WorkOrderChecklist.Columns.workOrderId";
            public const string name = "Operations.WorkOrderChecklist.Columns.name";
            public const string isRequired = "Operations.WorkOrderChecklist.Columns.isRequired";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderChecklist.Actions.New";
            public const string Edit = "Operations.WorkOrderChecklist.Actions.Edit";
            public const string Delete = "Operations.WorkOrderChecklist.Actions.Delete";
            public const string Save = "Operations.WorkOrderChecklist.Actions.Save";
            public const string Cancel = "Operations.WorkOrderChecklist.Actions.Cancel";
            public const string Export = "Operations.WorkOrderChecklist.Actions.Export";
            public const string Refresh = "Operations.WorkOrderChecklist.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderChecklist.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderChecklist.Grid.Search";
            public const string NoData = "Operations.WorkOrderChecklist.Grid.NoData";
            public const string Loading = "Operations.WorkOrderChecklist.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderChecklist.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderChecklist.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderChecklist.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderChecklist.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderChecklist.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderChecklist.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderChecklist.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderChecklist.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderChecklistItem — İş Emri Kontrol Listesi Maddesi / Work Order Checklist Item</summary>
    public static class Operations_WorkOrderChecklistItem
    {
        public const string ScreenId = "Operations.WorkOrderChecklistItem";
        public const string Title = "Operations.WorkOrderChecklistItem.Title";
        public const string Description = "Operations.WorkOrderChecklistItem.Description";
        public static class Columns
        {
            public const string workOrderChecklistId = "Operations.WorkOrderChecklistItem.Columns.workOrderChecklistId";
            public const string description = "Operations.WorkOrderChecklistItem.Columns.description";
            public const string isRequired = "Operations.WorkOrderChecklistItem.Columns.isRequired";
            public const string isCompleted = "Operations.WorkOrderChecklistItem.Columns.isCompleted";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderChecklistItem.Actions.New";
            public const string Edit = "Operations.WorkOrderChecklistItem.Actions.Edit";
            public const string Delete = "Operations.WorkOrderChecklistItem.Actions.Delete";
            public const string Save = "Operations.WorkOrderChecklistItem.Actions.Save";
            public const string Cancel = "Operations.WorkOrderChecklistItem.Actions.Cancel";
            public const string Export = "Operations.WorkOrderChecklistItem.Actions.Export";
            public const string Refresh = "Operations.WorkOrderChecklistItem.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderChecklistItem.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderChecklistItem.Grid.Search";
            public const string NoData = "Operations.WorkOrderChecklistItem.Grid.NoData";
            public const string Loading = "Operations.WorkOrderChecklistItem.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderChecklistItem.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderChecklistItem.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderChecklistItem.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderChecklistItem.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderChecklistItem.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderChecklistItem.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderChecklistItem.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderChecklistItem.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderMaterialPlan — İş Emri Malzeme Planı / Work Order Material Plan</summary>
    public static class Operations_WorkOrderMaterialPlan
    {
        public const string ScreenId = "Operations.WorkOrderMaterialPlan";
        public const string Title = "Operations.WorkOrderMaterialPlan.Title";
        public const string Description = "Operations.WorkOrderMaterialPlan.Description";
        public static class Columns
        {
            public const string workOrderId = "Operations.WorkOrderMaterialPlan.Columns.workOrderId";
            public const string materialId = "Operations.WorkOrderMaterialPlan.Columns.materialId";
            public const string plannedQuantity = "Operations.WorkOrderMaterialPlan.Columns.plannedQuantity";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderMaterialPlan.Actions.New";
            public const string Edit = "Operations.WorkOrderMaterialPlan.Actions.Edit";
            public const string Delete = "Operations.WorkOrderMaterialPlan.Actions.Delete";
            public const string Save = "Operations.WorkOrderMaterialPlan.Actions.Save";
            public const string Cancel = "Operations.WorkOrderMaterialPlan.Actions.Cancel";
            public const string Export = "Operations.WorkOrderMaterialPlan.Actions.Export";
            public const string Refresh = "Operations.WorkOrderMaterialPlan.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderMaterialPlan.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderMaterialPlan.Grid.Search";
            public const string NoData = "Operations.WorkOrderMaterialPlan.Grid.NoData";
            public const string Loading = "Operations.WorkOrderMaterialPlan.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderMaterialPlan.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderMaterialPlan.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderMaterialPlan.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderMaterialPlan.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderMaterialPlan.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderMaterialPlan.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderMaterialPlan.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderMaterialPlan.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderMaterialUsage — İş Emri Malzeme Kullanımı / Work Order Material Usage</summary>
    public static class Operations_WorkOrderMaterialUsage
    {
        public const string ScreenId = "Operations.WorkOrderMaterialUsage";
        public const string Title = "Operations.WorkOrderMaterialUsage.Title";
        public const string Description = "Operations.WorkOrderMaterialUsage.Description";
        public static class Columns
        {
            public const string workOrderId = "Operations.WorkOrderMaterialUsage.Columns.workOrderId";
            public const string stockDocumentLineId = "Operations.WorkOrderMaterialUsage.Columns.stockDocumentLineId";
            public const string materialId = "Operations.WorkOrderMaterialUsage.Columns.materialId";
            public const string usedQuantity = "Operations.WorkOrderMaterialUsage.Columns.usedQuantity";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderMaterialUsage.Actions.New";
            public const string Edit = "Operations.WorkOrderMaterialUsage.Actions.Edit";
            public const string Delete = "Operations.WorkOrderMaterialUsage.Actions.Delete";
            public const string Save = "Operations.WorkOrderMaterialUsage.Actions.Save";
            public const string Cancel = "Operations.WorkOrderMaterialUsage.Actions.Cancel";
            public const string Export = "Operations.WorkOrderMaterialUsage.Actions.Export";
            public const string Refresh = "Operations.WorkOrderMaterialUsage.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderMaterialUsage.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderMaterialUsage.Grid.Search";
            public const string NoData = "Operations.WorkOrderMaterialUsage.Grid.NoData";
            public const string Loading = "Operations.WorkOrderMaterialUsage.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderMaterialUsage.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderMaterialUsage.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderMaterialUsage.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderMaterialUsage.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderMaterialUsage.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderMaterialUsage.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderMaterialUsage.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderMaterialUsage.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderStatusHistory — İş Emri Durum Geçmişi / Work Order Status History</summary>
    public static class Operations_WorkOrderStatusHistory
    {
        public const string ScreenId = "Operations.WorkOrderStatusHistory";
        public const string Title = "Operations.WorkOrderStatusHistory.Title";
        public const string Description = "Operations.WorkOrderStatusHistory.Description";
        public static class Columns
        {
            public const string workOrderId = "Operations.WorkOrderStatusHistory.Columns.workOrderId";
            public const string fromStatus = "Operations.WorkOrderStatusHistory.Columns.fromStatus";
            public const string toStatus = "Operations.WorkOrderStatusHistory.Columns.toStatus";
            public const string changedAt = "Operations.WorkOrderStatusHistory.Columns.changedAt";
            public const string note = "Operations.WorkOrderStatusHistory.Columns.note";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderStatusHistory.Actions.New";
            public const string Edit = "Operations.WorkOrderStatusHistory.Actions.Edit";
            public const string Delete = "Operations.WorkOrderStatusHistory.Actions.Delete";
            public const string Save = "Operations.WorkOrderStatusHistory.Actions.Save";
            public const string Cancel = "Operations.WorkOrderStatusHistory.Actions.Cancel";
            public const string Export = "Operations.WorkOrderStatusHistory.Actions.Export";
            public const string Refresh = "Operations.WorkOrderStatusHistory.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderStatusHistory.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderStatusHistory.Grid.Search";
            public const string NoData = "Operations.WorkOrderStatusHistory.Grid.NoData";
            public const string Loading = "Operations.WorkOrderStatusHistory.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderStatusHistory.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderStatusHistory.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderStatusHistory.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderStatusHistory.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderStatusHistory.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderStatusHistory.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderStatusHistory.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderStatusHistory.Confirm.Delete";
        }
    }

    /// <summary>Operations.WorkOrderType — İş Emri Türü / Work Order Type</summary>
    public static class Operations_WorkOrderType
    {
        public const string ScreenId = "Operations.WorkOrderType";
        public const string Title = "Operations.WorkOrderType.Title";
        public const string Description = "Operations.WorkOrderType.Description";
        public static class Columns
        {
            public const string code = "Operations.WorkOrderType.Columns.code";
            public const string name = "Operations.WorkOrderType.Columns.name";
            public const string isActive = "Operations.WorkOrderType.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Operations.WorkOrderType.Actions.New";
            public const string Edit = "Operations.WorkOrderType.Actions.Edit";
            public const string Delete = "Operations.WorkOrderType.Actions.Delete";
            public const string Save = "Operations.WorkOrderType.Actions.Save";
            public const string Cancel = "Operations.WorkOrderType.Actions.Cancel";
            public const string Export = "Operations.WorkOrderType.Actions.Export";
            public const string Refresh = "Operations.WorkOrderType.Actions.Refresh";
            public const string ColumnChooser = "Operations.WorkOrderType.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Operations.WorkOrderType.Grid.Search";
            public const string NoData = "Operations.WorkOrderType.Grid.NoData";
            public const string Loading = "Operations.WorkOrderType.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Operations.WorkOrderType.Notifications.Saved";
            public const string Updated = "Operations.WorkOrderType.Notifications.Updated";
            public const string Deleted = "Operations.WorkOrderType.Notifications.Deleted";
            public const string Error = "Operations.WorkOrderType.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Operations.WorkOrderType.Popup.CreateTitle";
            public const string EditTitle = "Operations.WorkOrderType.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Operations.WorkOrderType.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Operations.WorkOrderType.Confirm.Delete";
        }
    }

    /// <summary>Organization.Employee — Personel / Employee</summary>
    public static class Organization_Employee
    {
        public const string ScreenId = "Organization.Employee";
        public const string Title = "Organization.Employee.Title";
        public const string Description = "Organization.Employee.Description";
        public static class Columns
        {
            public const string companyId = "Organization.Employee.Columns.companyId";
            public const string branchId = "Organization.Employee.Columns.branchId";
            public const string departmentId = "Organization.Employee.Columns.departmentId";
            public const string employeePositionId = "Organization.Employee.Columns.employeePositionId";
            public const string userId = "Organization.Employee.Columns.userId";
            public const string code = "Organization.Employee.Columns.code";
            public const string firstName = "Organization.Employee.Columns.firstName";
            public const string lastName = "Organization.Employee.Columns.lastName";
            public const string nationalId = "Organization.Employee.Columns.nationalId";
            public const string phone = "Organization.Employee.Columns.phone";
            public const string email = "Organization.Employee.Columns.email";
            public const string hireDate = "Organization.Employee.Columns.hireDate";
            public const string terminationDate = "Organization.Employee.Columns.terminationDate";
            public const string isActive = "Organization.Employee.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Organization.Employee.Actions.New";
            public const string Edit = "Organization.Employee.Actions.Edit";
            public const string Delete = "Organization.Employee.Actions.Delete";
            public const string Save = "Organization.Employee.Actions.Save";
            public const string Cancel = "Organization.Employee.Actions.Cancel";
            public const string Export = "Organization.Employee.Actions.Export";
            public const string Refresh = "Organization.Employee.Actions.Refresh";
            public const string ColumnChooser = "Organization.Employee.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.Employee.Grid.Search";
            public const string NoData = "Organization.Employee.Grid.NoData";
            public const string Loading = "Organization.Employee.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.Employee.Notifications.Saved";
            public const string Updated = "Organization.Employee.Notifications.Updated";
            public const string Deleted = "Organization.Employee.Notifications.Deleted";
            public const string Error = "Organization.Employee.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.Employee.Popup.CreateTitle";
            public const string EditTitle = "Organization.Employee.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.Employee.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.Employee.Confirm.Delete";
        }
    }

    /// <summary>Organization.EmployeePosition — Personel Pozisyonu / Employee Position</summary>
    public static class Organization_EmployeePosition
    {
        public const string ScreenId = "Organization.EmployeePosition";
        public const string Title = "Organization.EmployeePosition.Title";
        public const string Description = "Organization.EmployeePosition.Description";
        public static class Columns
        {
            public const string code = "Organization.EmployeePosition.Columns.code";
            public const string name = "Organization.EmployeePosition.Columns.name";
            public const string isActive = "Organization.EmployeePosition.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Organization.EmployeePosition.Actions.New";
            public const string Edit = "Organization.EmployeePosition.Actions.Edit";
            public const string Delete = "Organization.EmployeePosition.Actions.Delete";
            public const string Save = "Organization.EmployeePosition.Actions.Save";
            public const string Cancel = "Organization.EmployeePosition.Actions.Cancel";
            public const string Export = "Organization.EmployeePosition.Actions.Export";
            public const string Refresh = "Organization.EmployeePosition.Actions.Refresh";
            public const string ColumnChooser = "Organization.EmployeePosition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.EmployeePosition.Grid.Search";
            public const string NoData = "Organization.EmployeePosition.Grid.NoData";
            public const string Loading = "Organization.EmployeePosition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.EmployeePosition.Notifications.Saved";
            public const string Updated = "Organization.EmployeePosition.Notifications.Updated";
            public const string Deleted = "Organization.EmployeePosition.Notifications.Deleted";
            public const string Error = "Organization.EmployeePosition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.EmployeePosition.Popup.CreateTitle";
            public const string EditTitle = "Organization.EmployeePosition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.EmployeePosition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.EmployeePosition.Confirm.Delete";
        }
    }

    /// <summary>Organization.EmployeeSkill — Personel Yetkinliği / Employee Skill</summary>
    public static class Organization_EmployeeSkill
    {
        public const string ScreenId = "Organization.EmployeeSkill";
        public const string Title = "Organization.EmployeeSkill.Title";
        public const string Description = "Organization.EmployeeSkill.Description";
        public static class Columns
        {
            public const string code = "Organization.EmployeeSkill.Columns.code";
            public const string name = "Organization.EmployeeSkill.Columns.name";
            public const string isActive = "Organization.EmployeeSkill.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Organization.EmployeeSkill.Actions.New";
            public const string Edit = "Organization.EmployeeSkill.Actions.Edit";
            public const string Delete = "Organization.EmployeeSkill.Actions.Delete";
            public const string Save = "Organization.EmployeeSkill.Actions.Save";
            public const string Cancel = "Organization.EmployeeSkill.Actions.Cancel";
            public const string Export = "Organization.EmployeeSkill.Actions.Export";
            public const string Refresh = "Organization.EmployeeSkill.Actions.Refresh";
            public const string ColumnChooser = "Organization.EmployeeSkill.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.EmployeeSkill.Grid.Search";
            public const string NoData = "Organization.EmployeeSkill.Grid.NoData";
            public const string Loading = "Organization.EmployeeSkill.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.EmployeeSkill.Notifications.Saved";
            public const string Updated = "Organization.EmployeeSkill.Notifications.Updated";
            public const string Deleted = "Organization.EmployeeSkill.Notifications.Deleted";
            public const string Error = "Organization.EmployeeSkill.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.EmployeeSkill.Popup.CreateTitle";
            public const string EditTitle = "Organization.EmployeeSkill.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.EmployeeSkill.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.EmployeeSkill.Confirm.Delete";
        }
    }

    /// <summary>Organization.EmployeeSkillAssignment — Personel Yetkinlik Ataması / Employee Skill Assignment</summary>
    public static class Organization_EmployeeSkillAssignment
    {
        public const string ScreenId = "Organization.EmployeeSkillAssignment";
        public const string Title = "Organization.EmployeeSkillAssignment.Title";
        public const string Description = "Organization.EmployeeSkillAssignment.Description";
        public static class Columns
        {
            public const string employeeId = "Organization.EmployeeSkillAssignment.Columns.employeeId";
            public const string employeeSkillId = "Organization.EmployeeSkillAssignment.Columns.employeeSkillId";
            public const string level = "Organization.EmployeeSkillAssignment.Columns.level";
            public const string note = "Organization.EmployeeSkillAssignment.Columns.note";
        }
        public static class Actions
        {
            public const string New = "Organization.EmployeeSkillAssignment.Actions.New";
            public const string Edit = "Organization.EmployeeSkillAssignment.Actions.Edit";
            public const string Delete = "Organization.EmployeeSkillAssignment.Actions.Delete";
            public const string Save = "Organization.EmployeeSkillAssignment.Actions.Save";
            public const string Cancel = "Organization.EmployeeSkillAssignment.Actions.Cancel";
            public const string Export = "Organization.EmployeeSkillAssignment.Actions.Export";
            public const string Refresh = "Organization.EmployeeSkillAssignment.Actions.Refresh";
            public const string ColumnChooser = "Organization.EmployeeSkillAssignment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.EmployeeSkillAssignment.Grid.Search";
            public const string NoData = "Organization.EmployeeSkillAssignment.Grid.NoData";
            public const string Loading = "Organization.EmployeeSkillAssignment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.EmployeeSkillAssignment.Notifications.Saved";
            public const string Updated = "Organization.EmployeeSkillAssignment.Notifications.Updated";
            public const string Deleted = "Organization.EmployeeSkillAssignment.Notifications.Deleted";
            public const string Error = "Organization.EmployeeSkillAssignment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.EmployeeSkillAssignment.Popup.CreateTitle";
            public const string EditTitle = "Organization.EmployeeSkillAssignment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.EmployeeSkillAssignment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.EmployeeSkillAssignment.Confirm.Delete";
        }
    }

    /// <summary>Organization.ExpenseClaim — Masraf Talebi / Expense Claim</summary>
    public static class Organization_ExpenseClaim
    {
        public const string ScreenId = "Organization.ExpenseClaim";
        public const string Title = "Organization.ExpenseClaim.Title";
        public const string Description = "Organization.ExpenseClaim.Description";
        public static class Columns
        {
            public const string employeeId = "Organization.ExpenseClaim.Columns.employeeId";
            public const string projectId = "Organization.ExpenseClaim.Columns.projectId";
            public const string currencyId = "Organization.ExpenseClaim.Columns.currencyId";
            public const string claimNo = "Organization.ExpenseClaim.Columns.claimNo";
            public const string claimDate = "Organization.ExpenseClaim.Columns.claimDate";
            public const string totalAmount = "Organization.ExpenseClaim.Columns.totalAmount";
            public const string status = "Organization.ExpenseClaim.Columns.status";
            public const string approvalRequestId = "Organization.ExpenseClaim.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Organization.ExpenseClaim.Actions.New";
            public const string Edit = "Organization.ExpenseClaim.Actions.Edit";
            public const string Delete = "Organization.ExpenseClaim.Actions.Delete";
            public const string Save = "Organization.ExpenseClaim.Actions.Save";
            public const string Cancel = "Organization.ExpenseClaim.Actions.Cancel";
            public const string Export = "Organization.ExpenseClaim.Actions.Export";
            public const string Refresh = "Organization.ExpenseClaim.Actions.Refresh";
            public const string ColumnChooser = "Organization.ExpenseClaim.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.ExpenseClaim.Grid.Search";
            public const string NoData = "Organization.ExpenseClaim.Grid.NoData";
            public const string Loading = "Organization.ExpenseClaim.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.ExpenseClaim.Notifications.Saved";
            public const string Updated = "Organization.ExpenseClaim.Notifications.Updated";
            public const string Deleted = "Organization.ExpenseClaim.Notifications.Deleted";
            public const string Error = "Organization.ExpenseClaim.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.ExpenseClaim.Popup.CreateTitle";
            public const string EditTitle = "Organization.ExpenseClaim.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.ExpenseClaim.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.ExpenseClaim.Confirm.Delete";
        }
    }

    /// <summary>Organization.ExpenseClaimLine — Masraf Talebi Kalemi / Expense Claim Line</summary>
    public static class Organization_ExpenseClaimLine
    {
        public const string ScreenId = "Organization.ExpenseClaimLine";
        public const string Title = "Organization.ExpenseClaimLine.Title";
        public const string Description = "Organization.ExpenseClaimLine.Description";
        public static class Columns
        {
            public const string expenseClaimId = "Organization.ExpenseClaimLine.Columns.expenseClaimId";
            public const string description = "Organization.ExpenseClaimLine.Columns.description";
            public const string expenseDate = "Organization.ExpenseClaimLine.Columns.expenseDate";
            public const string amount = "Organization.ExpenseClaimLine.Columns.amount";
            public const string category = "Organization.ExpenseClaimLine.Columns.category";
        }
        public static class Actions
        {
            public const string New = "Organization.ExpenseClaimLine.Actions.New";
            public const string Edit = "Organization.ExpenseClaimLine.Actions.Edit";
            public const string Delete = "Organization.ExpenseClaimLine.Actions.Delete";
            public const string Save = "Organization.ExpenseClaimLine.Actions.Save";
            public const string Cancel = "Organization.ExpenseClaimLine.Actions.Cancel";
            public const string Export = "Organization.ExpenseClaimLine.Actions.Export";
            public const string Refresh = "Organization.ExpenseClaimLine.Actions.Refresh";
            public const string ColumnChooser = "Organization.ExpenseClaimLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.ExpenseClaimLine.Grid.Search";
            public const string NoData = "Organization.ExpenseClaimLine.Grid.NoData";
            public const string Loading = "Organization.ExpenseClaimLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.ExpenseClaimLine.Notifications.Saved";
            public const string Updated = "Organization.ExpenseClaimLine.Notifications.Updated";
            public const string Deleted = "Organization.ExpenseClaimLine.Notifications.Deleted";
            public const string Error = "Organization.ExpenseClaimLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.ExpenseClaimLine.Popup.CreateTitle";
            public const string EditTitle = "Organization.ExpenseClaimLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.ExpenseClaimLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.ExpenseClaimLine.Confirm.Delete";
        }
    }

    /// <summary>Organization.LeaveRequest — İzin Talebi / Leave Request</summary>
    public static class Organization_LeaveRequest
    {
        public const string ScreenId = "Organization.LeaveRequest";
        public const string Title = "Organization.LeaveRequest.Title";
        public const string Description = "Organization.LeaveRequest.Description";
        public static class Columns
        {
            public const string employeeId = "Organization.LeaveRequest.Columns.employeeId";
            public const string leaveType = "Organization.LeaveRequest.Columns.leaveType";
            public const string startDate = "Organization.LeaveRequest.Columns.startDate";
            public const string endDate = "Organization.LeaveRequest.Columns.endDate";
            public const string days = "Organization.LeaveRequest.Columns.days";
            public const string reason = "Organization.LeaveRequest.Columns.reason";
            public const string status = "Organization.LeaveRequest.Columns.status";
            public const string approvalRequestId = "Organization.LeaveRequest.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Organization.LeaveRequest.Actions.New";
            public const string Edit = "Organization.LeaveRequest.Actions.Edit";
            public const string Delete = "Organization.LeaveRequest.Actions.Delete";
            public const string Save = "Organization.LeaveRequest.Actions.Save";
            public const string Cancel = "Organization.LeaveRequest.Actions.Cancel";
            public const string Export = "Organization.LeaveRequest.Actions.Export";
            public const string Refresh = "Organization.LeaveRequest.Actions.Refresh";
            public const string ColumnChooser = "Organization.LeaveRequest.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Organization.LeaveRequest.Grid.Search";
            public const string NoData = "Organization.LeaveRequest.Grid.NoData";
            public const string Loading = "Organization.LeaveRequest.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Organization.LeaveRequest.Notifications.Saved";
            public const string Updated = "Organization.LeaveRequest.Notifications.Updated";
            public const string Deleted = "Organization.LeaveRequest.Notifications.Deleted";
            public const string Error = "Organization.LeaveRequest.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Organization.LeaveRequest.Popup.CreateTitle";
            public const string EditTitle = "Organization.LeaveRequest.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Organization.LeaveRequest.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Organization.LeaveRequest.Confirm.Delete";
        }
    }

    /// <summary>Procurement.PurchaseOrder — Satın Alma Siparişi / Purchase Order</summary>
    public static class Procurement_PurchaseOrder
    {
        public const string ScreenId = "Procurement.PurchaseOrder";
        public const string Title = "Procurement.PurchaseOrder.Title";
        public const string Description = "Procurement.PurchaseOrder.Description";
        public static class Columns
        {
            public const string supplierId = "Procurement.PurchaseOrder.Columns.supplierId";
            public const string projectId = "Procurement.PurchaseOrder.Columns.projectId";
            public const string status = "Procurement.PurchaseOrder.Columns.status";
            public const string orderNo = "Procurement.PurchaseOrder.Columns.orderNo";
            public const string currencyId = "Procurement.PurchaseOrder.Columns.currencyId";
            public const string orderDate = "Procurement.PurchaseOrder.Columns.orderDate";
            public const string approvalRequestId = "Procurement.PurchaseOrder.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Procurement.PurchaseOrder.Actions.New";
            public const string Edit = "Procurement.PurchaseOrder.Actions.Edit";
            public const string Delete = "Procurement.PurchaseOrder.Actions.Delete";
            public const string Save = "Procurement.PurchaseOrder.Actions.Save";
            public const string Cancel = "Procurement.PurchaseOrder.Actions.Cancel";
            public const string Export = "Procurement.PurchaseOrder.Actions.Export";
            public const string Refresh = "Procurement.PurchaseOrder.Actions.Refresh";
            public const string ColumnChooser = "Procurement.PurchaseOrder.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.PurchaseOrder.Grid.Search";
            public const string NoData = "Procurement.PurchaseOrder.Grid.NoData";
            public const string Loading = "Procurement.PurchaseOrder.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.PurchaseOrder.Notifications.Saved";
            public const string Updated = "Procurement.PurchaseOrder.Notifications.Updated";
            public const string Deleted = "Procurement.PurchaseOrder.Notifications.Deleted";
            public const string Error = "Procurement.PurchaseOrder.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.PurchaseOrder.Popup.CreateTitle";
            public const string EditTitle = "Procurement.PurchaseOrder.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.PurchaseOrder.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.PurchaseOrder.Confirm.Delete";
        }
    }

    /// <summary>Procurement.PurchaseOrderLine — Satın Alma Siparişi Kalemi / Purchase Order Line</summary>
    public static class Procurement_PurchaseOrderLine
    {
        public const string ScreenId = "Procurement.PurchaseOrderLine";
        public const string Title = "Procurement.PurchaseOrderLine.Title";
        public const string Description = "Procurement.PurchaseOrderLine.Description";
        public static class Columns
        {
            public const string purchaseOrderId = "Procurement.PurchaseOrderLine.Columns.purchaseOrderId";
            public const string requestLineId = "Procurement.PurchaseOrderLine.Columns.requestLineId";
            public const string materialId = "Procurement.PurchaseOrderLine.Columns.materialId";
            public const string quantity = "Procurement.PurchaseOrderLine.Columns.quantity";
            public const string unitPrice = "Procurement.PurchaseOrderLine.Columns.unitPrice";
            public const string currencyId = "Procurement.PurchaseOrderLine.Columns.currencyId";
            public const string receivedQuantity = "Procurement.PurchaseOrderLine.Columns.receivedQuantity";
        }
        public static class Actions
        {
            public const string New = "Procurement.PurchaseOrderLine.Actions.New";
            public const string Edit = "Procurement.PurchaseOrderLine.Actions.Edit";
            public const string Delete = "Procurement.PurchaseOrderLine.Actions.Delete";
            public const string Save = "Procurement.PurchaseOrderLine.Actions.Save";
            public const string Cancel = "Procurement.PurchaseOrderLine.Actions.Cancel";
            public const string Export = "Procurement.PurchaseOrderLine.Actions.Export";
            public const string Refresh = "Procurement.PurchaseOrderLine.Actions.Refresh";
            public const string ColumnChooser = "Procurement.PurchaseOrderLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.PurchaseOrderLine.Grid.Search";
            public const string NoData = "Procurement.PurchaseOrderLine.Grid.NoData";
            public const string Loading = "Procurement.PurchaseOrderLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.PurchaseOrderLine.Notifications.Saved";
            public const string Updated = "Procurement.PurchaseOrderLine.Notifications.Updated";
            public const string Deleted = "Procurement.PurchaseOrderLine.Notifications.Deleted";
            public const string Error = "Procurement.PurchaseOrderLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.PurchaseOrderLine.Popup.CreateTitle";
            public const string EditTitle = "Procurement.PurchaseOrderLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.PurchaseOrderLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.PurchaseOrderLine.Confirm.Delete";
        }
    }

    /// <summary>Procurement.PurchaseReceipt — Satın Alma İrsaliyesi / Purchase Receipt</summary>
    public static class Procurement_PurchaseReceipt
    {
        public const string ScreenId = "Procurement.PurchaseReceipt";
        public const string Title = "Procurement.PurchaseReceipt.Title";
        public const string Description = "Procurement.PurchaseReceipt.Description";
        public static class Columns
        {
            public const string supplierId = "Procurement.PurchaseReceipt.Columns.supplierId";
            public const string purchaseOrderId = "Procurement.PurchaseReceipt.Columns.purchaseOrderId";
            public const string warehouseId = "Procurement.PurchaseReceipt.Columns.warehouseId";
            public const string stockDocumentId = "Procurement.PurchaseReceipt.Columns.stockDocumentId";
            public const string receiptNo = "Procurement.PurchaseReceipt.Columns.receiptNo";
            public const string receiptDate = "Procurement.PurchaseReceipt.Columns.receiptDate";
            public const string status = "Procurement.PurchaseReceipt.Columns.status";
        }
        public static class Actions
        {
            public const string New = "Procurement.PurchaseReceipt.Actions.New";
            public const string Edit = "Procurement.PurchaseReceipt.Actions.Edit";
            public const string Delete = "Procurement.PurchaseReceipt.Actions.Delete";
            public const string Save = "Procurement.PurchaseReceipt.Actions.Save";
            public const string Cancel = "Procurement.PurchaseReceipt.Actions.Cancel";
            public const string Export = "Procurement.PurchaseReceipt.Actions.Export";
            public const string Refresh = "Procurement.PurchaseReceipt.Actions.Refresh";
            public const string ColumnChooser = "Procurement.PurchaseReceipt.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.PurchaseReceipt.Grid.Search";
            public const string NoData = "Procurement.PurchaseReceipt.Grid.NoData";
            public const string Loading = "Procurement.PurchaseReceipt.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.PurchaseReceipt.Notifications.Saved";
            public const string Updated = "Procurement.PurchaseReceipt.Notifications.Updated";
            public const string Deleted = "Procurement.PurchaseReceipt.Notifications.Deleted";
            public const string Error = "Procurement.PurchaseReceipt.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.PurchaseReceipt.Popup.CreateTitle";
            public const string EditTitle = "Procurement.PurchaseReceipt.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.PurchaseReceipt.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.PurchaseReceipt.Confirm.Delete";
        }
    }

    /// <summary>Procurement.PurchaseReceiptLine — Satın Alma İrsaliyesi Kalemi / Purchase Receipt Line</summary>
    public static class Procurement_PurchaseReceiptLine
    {
        public const string ScreenId = "Procurement.PurchaseReceiptLine";
        public const string Title = "Procurement.PurchaseReceiptLine.Title";
        public const string Description = "Procurement.PurchaseReceiptLine.Description";
        public static class Columns
        {
            public const string purchaseReceiptId = "Procurement.PurchaseReceiptLine.Columns.purchaseReceiptId";
            public const string purchaseOrderLineId = "Procurement.PurchaseReceiptLine.Columns.purchaseOrderLineId";
            public const string materialId = "Procurement.PurchaseReceiptLine.Columns.materialId";
            public const string quantity = "Procurement.PurchaseReceiptLine.Columns.quantity";
            public const string unitPrice = "Procurement.PurchaseReceiptLine.Columns.unitPrice";
        }
        public static class Actions
        {
            public const string New = "Procurement.PurchaseReceiptLine.Actions.New";
            public const string Edit = "Procurement.PurchaseReceiptLine.Actions.Edit";
            public const string Delete = "Procurement.PurchaseReceiptLine.Actions.Delete";
            public const string Save = "Procurement.PurchaseReceiptLine.Actions.Save";
            public const string Cancel = "Procurement.PurchaseReceiptLine.Actions.Cancel";
            public const string Export = "Procurement.PurchaseReceiptLine.Actions.Export";
            public const string Refresh = "Procurement.PurchaseReceiptLine.Actions.Refresh";
            public const string ColumnChooser = "Procurement.PurchaseReceiptLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.PurchaseReceiptLine.Grid.Search";
            public const string NoData = "Procurement.PurchaseReceiptLine.Grid.NoData";
            public const string Loading = "Procurement.PurchaseReceiptLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.PurchaseReceiptLine.Notifications.Saved";
            public const string Updated = "Procurement.PurchaseReceiptLine.Notifications.Updated";
            public const string Deleted = "Procurement.PurchaseReceiptLine.Notifications.Deleted";
            public const string Error = "Procurement.PurchaseReceiptLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.PurchaseReceiptLine.Popup.CreateTitle";
            public const string EditTitle = "Procurement.PurchaseReceiptLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.PurchaseReceiptLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.PurchaseReceiptLine.Confirm.Delete";
        }
    }

    /// <summary>Procurement.SupplierInvoice — Tedarikçi Faturası / Supplier Invoice</summary>
    public static class Procurement_SupplierInvoice
    {
        public const string ScreenId = "Procurement.SupplierInvoice";
        public const string Title = "Procurement.SupplierInvoice.Title";
        public const string Description = "Procurement.SupplierInvoice.Description";
        public static class Columns
        {
            public const string supplierId = "Procurement.SupplierInvoice.Columns.supplierId";
            public const string purchaseOrderId = "Procurement.SupplierInvoice.Columns.purchaseOrderId";
            public const string purchaseReceiptId = "Procurement.SupplierInvoice.Columns.purchaseReceiptId";
            public const string currencyId = "Procurement.SupplierInvoice.Columns.currencyId";
            public const string invoiceNo = "Procurement.SupplierInvoice.Columns.invoiceNo";
            public const string invoiceDate = "Procurement.SupplierInvoice.Columns.invoiceDate";
            public const string totalAmount = "Procurement.SupplierInvoice.Columns.totalAmount";
            public const string status = "Procurement.SupplierInvoice.Columns.status";
        }
        public static class Actions
        {
            public const string New = "Procurement.SupplierInvoice.Actions.New";
            public const string Edit = "Procurement.SupplierInvoice.Actions.Edit";
            public const string Delete = "Procurement.SupplierInvoice.Actions.Delete";
            public const string Save = "Procurement.SupplierInvoice.Actions.Save";
            public const string Cancel = "Procurement.SupplierInvoice.Actions.Cancel";
            public const string Export = "Procurement.SupplierInvoice.Actions.Export";
            public const string Refresh = "Procurement.SupplierInvoice.Actions.Refresh";
            public const string ColumnChooser = "Procurement.SupplierInvoice.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.SupplierInvoice.Grid.Search";
            public const string NoData = "Procurement.SupplierInvoice.Grid.NoData";
            public const string Loading = "Procurement.SupplierInvoice.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.SupplierInvoice.Notifications.Saved";
            public const string Updated = "Procurement.SupplierInvoice.Notifications.Updated";
            public const string Deleted = "Procurement.SupplierInvoice.Notifications.Deleted";
            public const string Error = "Procurement.SupplierInvoice.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.SupplierInvoice.Popup.CreateTitle";
            public const string EditTitle = "Procurement.SupplierInvoice.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.SupplierInvoice.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.SupplierInvoice.Confirm.Delete";
        }
    }

    /// <summary>Procurement.SupplierInvoiceLine — Tedarikçi Faturası Kalemi / Supplier Invoice Line</summary>
    public static class Procurement_SupplierInvoiceLine
    {
        public const string ScreenId = "Procurement.SupplierInvoiceLine";
        public const string Title = "Procurement.SupplierInvoiceLine.Title";
        public const string Description = "Procurement.SupplierInvoiceLine.Description";
        public static class Columns
        {
            public const string supplierInvoiceId = "Procurement.SupplierInvoiceLine.Columns.supplierInvoiceId";
            public const string materialId = "Procurement.SupplierInvoiceLine.Columns.materialId";
            public const string description = "Procurement.SupplierInvoiceLine.Columns.description";
            public const string quantity = "Procurement.SupplierInvoiceLine.Columns.quantity";
            public const string unitPrice = "Procurement.SupplierInvoiceLine.Columns.unitPrice";
            public const string taxRate = "Procurement.SupplierInvoiceLine.Columns.taxRate";
        }
        public static class Actions
        {
            public const string New = "Procurement.SupplierInvoiceLine.Actions.New";
            public const string Edit = "Procurement.SupplierInvoiceLine.Actions.Edit";
            public const string Delete = "Procurement.SupplierInvoiceLine.Actions.Delete";
            public const string Save = "Procurement.SupplierInvoiceLine.Actions.Save";
            public const string Cancel = "Procurement.SupplierInvoiceLine.Actions.Cancel";
            public const string Export = "Procurement.SupplierInvoiceLine.Actions.Export";
            public const string Refresh = "Procurement.SupplierInvoiceLine.Actions.Refresh";
            public const string ColumnChooser = "Procurement.SupplierInvoiceLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.SupplierInvoiceLine.Grid.Search";
            public const string NoData = "Procurement.SupplierInvoiceLine.Grid.NoData";
            public const string Loading = "Procurement.SupplierInvoiceLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.SupplierInvoiceLine.Notifications.Saved";
            public const string Updated = "Procurement.SupplierInvoiceLine.Notifications.Updated";
            public const string Deleted = "Procurement.SupplierInvoiceLine.Notifications.Deleted";
            public const string Error = "Procurement.SupplierInvoiceLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.SupplierInvoiceLine.Popup.CreateTitle";
            public const string EditTitle = "Procurement.SupplierInvoiceLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.SupplierInvoiceLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.SupplierInvoiceLine.Confirm.Delete";
        }
    }

    /// <summary>Procurement.SupplierQuote — Tedarikçi Teklifi / Supplier Quote</summary>
    public static class Procurement_SupplierQuote
    {
        public const string ScreenId = "Procurement.SupplierQuote";
        public const string Title = "Procurement.SupplierQuote.Title";
        public const string Description = "Procurement.SupplierQuote.Description";
        public static class Columns
        {
            public const string supplierId = "Procurement.SupplierQuote.Columns.supplierId";
            public const string projectId = "Procurement.SupplierQuote.Columns.projectId";
            public const string currencyId = "Procurement.SupplierQuote.Columns.currencyId";
            public const string quoteNo = "Procurement.SupplierQuote.Columns.quoteNo";
            public const string quoteDate = "Procurement.SupplierQuote.Columns.quoteDate";
            public const string paymentTerm = "Procurement.SupplierQuote.Columns.paymentTerm";
            public const string status = "Procurement.SupplierQuote.Columns.status";
            public const string isSelected = "Procurement.SupplierQuote.Columns.isSelected";
        }
        public static class Actions
        {
            public const string New = "Procurement.SupplierQuote.Actions.New";
            public const string Edit = "Procurement.SupplierQuote.Actions.Edit";
            public const string Delete = "Procurement.SupplierQuote.Actions.Delete";
            public const string Save = "Procurement.SupplierQuote.Actions.Save";
            public const string Cancel = "Procurement.SupplierQuote.Actions.Cancel";
            public const string Export = "Procurement.SupplierQuote.Actions.Export";
            public const string Refresh = "Procurement.SupplierQuote.Actions.Refresh";
            public const string ColumnChooser = "Procurement.SupplierQuote.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.SupplierQuote.Grid.Search";
            public const string NoData = "Procurement.SupplierQuote.Grid.NoData";
            public const string Loading = "Procurement.SupplierQuote.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.SupplierQuote.Notifications.Saved";
            public const string Updated = "Procurement.SupplierQuote.Notifications.Updated";
            public const string Deleted = "Procurement.SupplierQuote.Notifications.Deleted";
            public const string Error = "Procurement.SupplierQuote.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.SupplierQuote.Popup.CreateTitle";
            public const string EditTitle = "Procurement.SupplierQuote.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.SupplierQuote.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.SupplierQuote.Confirm.Delete";
        }
    }

    /// <summary>Procurement.SupplierQuoteLine — Tedarikçi Teklifi Kalemi / Supplier Quote Line</summary>
    public static class Procurement_SupplierQuoteLine
    {
        public const string ScreenId = "Procurement.SupplierQuoteLine";
        public const string Title = "Procurement.SupplierQuoteLine.Title";
        public const string Description = "Procurement.SupplierQuoteLine.Description";
        public static class Columns
        {
            public const string supplierQuoteId = "Procurement.SupplierQuoteLine.Columns.supplierQuoteId";
            public const string requestLineId = "Procurement.SupplierQuoteLine.Columns.requestLineId";
            public const string materialId = "Procurement.SupplierQuoteLine.Columns.materialId";
            public const string description = "Procurement.SupplierQuoteLine.Columns.description";
            public const string quantity = "Procurement.SupplierQuoteLine.Columns.quantity";
            public const string unitPrice = "Procurement.SupplierQuoteLine.Columns.unitPrice";
            public const string taxRate = "Procurement.SupplierQuoteLine.Columns.taxRate";
            public const string discountRate = "Procurement.SupplierQuoteLine.Columns.discountRate";
            public const string deliveryDays = "Procurement.SupplierQuoteLine.Columns.deliveryDays";
        }
        public static class Actions
        {
            public const string New = "Procurement.SupplierQuoteLine.Actions.New";
            public const string Edit = "Procurement.SupplierQuoteLine.Actions.Edit";
            public const string Delete = "Procurement.SupplierQuoteLine.Actions.Delete";
            public const string Save = "Procurement.SupplierQuoteLine.Actions.Save";
            public const string Cancel = "Procurement.SupplierQuoteLine.Actions.Cancel";
            public const string Export = "Procurement.SupplierQuoteLine.Actions.Export";
            public const string Refresh = "Procurement.SupplierQuoteLine.Actions.Refresh";
            public const string ColumnChooser = "Procurement.SupplierQuoteLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Procurement.SupplierQuoteLine.Grid.Search";
            public const string NoData = "Procurement.SupplierQuoteLine.Grid.NoData";
            public const string Loading = "Procurement.SupplierQuoteLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Procurement.SupplierQuoteLine.Notifications.Saved";
            public const string Updated = "Procurement.SupplierQuoteLine.Notifications.Updated";
            public const string Deleted = "Procurement.SupplierQuoteLine.Notifications.Deleted";
            public const string Error = "Procurement.SupplierQuoteLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Procurement.SupplierQuoteLine.Popup.CreateTitle";
            public const string EditTitle = "Procurement.SupplierQuoteLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Procurement.SupplierQuoteLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Procurement.SupplierQuoteLine.Confirm.Delete";
        }
    }

    /// <summary>ProgressPayments.ProgressPayment — Hakediş / Progress Payment</summary>
    public static class ProgressPayments_ProgressPayment
    {
        public const string ScreenId = "ProgressPayments.ProgressPayment";
        public const string Title = "ProgressPayments.ProgressPayment.Title";
        public const string Description = "ProgressPayments.ProgressPayment.Description";
        public static class Columns
        {
            public const string contractId = "ProgressPayments.ProgressPayment.Columns.contractId";
            public const string partnerId = "ProgressPayments.ProgressPayment.Columns.partnerId";
            public const string progressPaymentNo = "ProgressPayments.ProgressPayment.Columns.progressPaymentNo";
            public const string paymentPeriodStart = "ProgressPayments.ProgressPayment.Columns.paymentPeriodStart";
            public const string paymentPeriodEnd = "ProgressPayments.ProgressPayment.Columns.paymentPeriodEnd";
            public const string grossAmount = "ProgressPayments.ProgressPayment.Columns.grossAmount";
            public const string deductionTotal = "ProgressPayments.ProgressPayment.Columns.deductionTotal";
            public const string netAmount = "ProgressPayments.ProgressPayment.Columns.netAmount";
            public const string status = "ProgressPayments.ProgressPayment.Columns.status";
            public const string approvalRequestId = "ProgressPayments.ProgressPayment.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "ProgressPayments.ProgressPayment.Actions.New";
            public const string Edit = "ProgressPayments.ProgressPayment.Actions.Edit";
            public const string Delete = "ProgressPayments.ProgressPayment.Actions.Delete";
            public const string Save = "ProgressPayments.ProgressPayment.Actions.Save";
            public const string Cancel = "ProgressPayments.ProgressPayment.Actions.Cancel";
            public const string Export = "ProgressPayments.ProgressPayment.Actions.Export";
            public const string Refresh = "ProgressPayments.ProgressPayment.Actions.Refresh";
            public const string ColumnChooser = "ProgressPayments.ProgressPayment.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "ProgressPayments.ProgressPayment.Grid.Search";
            public const string NoData = "ProgressPayments.ProgressPayment.Grid.NoData";
            public const string Loading = "ProgressPayments.ProgressPayment.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "ProgressPayments.ProgressPayment.Notifications.Saved";
            public const string Updated = "ProgressPayments.ProgressPayment.Notifications.Updated";
            public const string Deleted = "ProgressPayments.ProgressPayment.Notifications.Deleted";
            public const string Error = "ProgressPayments.ProgressPayment.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "ProgressPayments.ProgressPayment.Popup.CreateTitle";
            public const string EditTitle = "ProgressPayments.ProgressPayment.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "ProgressPayments.ProgressPayment.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "ProgressPayments.ProgressPayment.Confirm.Delete";
        }
    }

    /// <summary>ProgressPayments.ProgressPaymentDeduction — Hakediş Kesintisi / Progress Payment Deduction</summary>
    public static class ProgressPayments_ProgressPaymentDeduction
    {
        public const string ScreenId = "ProgressPayments.ProgressPaymentDeduction";
        public const string Title = "ProgressPayments.ProgressPaymentDeduction.Title";
        public const string Description = "ProgressPayments.ProgressPaymentDeduction.Description";
        public static class Columns
        {
            public const string progressPaymentId = "ProgressPayments.ProgressPaymentDeduction.Columns.progressPaymentId";
            public const string deductionType = "ProgressPayments.ProgressPaymentDeduction.Columns.deductionType";
            public const string amount = "ProgressPayments.ProgressPaymentDeduction.Columns.amount";
            public const string note = "ProgressPayments.ProgressPaymentDeduction.Columns.note";
        }
        public static class Actions
        {
            public const string New = "ProgressPayments.ProgressPaymentDeduction.Actions.New";
            public const string Edit = "ProgressPayments.ProgressPaymentDeduction.Actions.Edit";
            public const string Delete = "ProgressPayments.ProgressPaymentDeduction.Actions.Delete";
            public const string Save = "ProgressPayments.ProgressPaymentDeduction.Actions.Save";
            public const string Cancel = "ProgressPayments.ProgressPaymentDeduction.Actions.Cancel";
            public const string Export = "ProgressPayments.ProgressPaymentDeduction.Actions.Export";
            public const string Refresh = "ProgressPayments.ProgressPaymentDeduction.Actions.Refresh";
            public const string ColumnChooser = "ProgressPayments.ProgressPaymentDeduction.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "ProgressPayments.ProgressPaymentDeduction.Grid.Search";
            public const string NoData = "ProgressPayments.ProgressPaymentDeduction.Grid.NoData";
            public const string Loading = "ProgressPayments.ProgressPaymentDeduction.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "ProgressPayments.ProgressPaymentDeduction.Notifications.Saved";
            public const string Updated = "ProgressPayments.ProgressPaymentDeduction.Notifications.Updated";
            public const string Deleted = "ProgressPayments.ProgressPaymentDeduction.Notifications.Deleted";
            public const string Error = "ProgressPayments.ProgressPaymentDeduction.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "ProgressPayments.ProgressPaymentDeduction.Popup.CreateTitle";
            public const string EditTitle = "ProgressPayments.ProgressPaymentDeduction.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "ProgressPayments.ProgressPaymentDeduction.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "ProgressPayments.ProgressPaymentDeduction.Confirm.Delete";
        }
    }

    /// <summary>ProgressPayments.ProgressPaymentLine — Hakediş Kalemi / Progress Payment Line</summary>
    public static class ProgressPayments_ProgressPaymentLine
    {
        public const string ScreenId = "ProgressPayments.ProgressPaymentLine";
        public const string Title = "ProgressPayments.ProgressPaymentLine.Title";
        public const string Description = "ProgressPayments.ProgressPaymentLine.Description";
        public static class Columns
        {
            public const string progressPaymentId = "ProgressPayments.ProgressPaymentLine.Columns.progressPaymentId";
            public const string contractLineId = "ProgressPayments.ProgressPaymentLine.Columns.contractLineId";
            public const string measurementSheetLineId = "ProgressPayments.ProgressPaymentLine.Columns.measurementSheetLineId";
            public const string description = "ProgressPayments.ProgressPaymentLine.Columns.description";
            public const string quantity = "ProgressPayments.ProgressPaymentLine.Columns.quantity";
            public const string unitPrice = "ProgressPayments.ProgressPaymentLine.Columns.unitPrice";
            public const string amount = "ProgressPayments.ProgressPaymentLine.Columns.amount";
        }
        public static class Actions
        {
            public const string New = "ProgressPayments.ProgressPaymentLine.Actions.New";
            public const string Edit = "ProgressPayments.ProgressPaymentLine.Actions.Edit";
            public const string Delete = "ProgressPayments.ProgressPaymentLine.Actions.Delete";
            public const string Save = "ProgressPayments.ProgressPaymentLine.Actions.Save";
            public const string Cancel = "ProgressPayments.ProgressPaymentLine.Actions.Cancel";
            public const string Export = "ProgressPayments.ProgressPaymentLine.Actions.Export";
            public const string Refresh = "ProgressPayments.ProgressPaymentLine.Actions.Refresh";
            public const string ColumnChooser = "ProgressPayments.ProgressPaymentLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "ProgressPayments.ProgressPaymentLine.Grid.Search";
            public const string NoData = "ProgressPayments.ProgressPaymentLine.Grid.NoData";
            public const string Loading = "ProgressPayments.ProgressPaymentLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "ProgressPayments.ProgressPaymentLine.Notifications.Saved";
            public const string Updated = "ProgressPayments.ProgressPaymentLine.Notifications.Updated";
            public const string Deleted = "ProgressPayments.ProgressPaymentLine.Notifications.Deleted";
            public const string Error = "ProgressPayments.ProgressPaymentLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "ProgressPayments.ProgressPaymentLine.Popup.CreateTitle";
            public const string EditTitle = "ProgressPayments.ProgressPaymentLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "ProgressPayments.ProgressPaymentLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "ProgressPayments.ProgressPaymentLine.Confirm.Delete";
        }
    }

    /// <summary>Projects.Project — Proje / Project</summary>
    public static class Projects_Project
    {
        public const string ScreenId = "Projects.Project";
        public const string Title = "Projects.Project.Title";
        public const string Description = "Projects.Project.Description";
        public static class Columns
        {
            public const string companyId = "Projects.Project.Columns.companyId";
            public const string branchId = "Projects.Project.Columns.branchId";
            public const string projectTypeId = "Projects.Project.Columns.projectTypeId";
            public const string statusId = "Projects.Project.Columns.statusId";
            public const string customerId = "Projects.Project.Columns.customerId";
            public const string managerUserId = "Projects.Project.Columns.managerUserId";
            public const string code = "Projects.Project.Columns.code";
            public const string name = "Projects.Project.Columns.name";
            public const string startDate = "Projects.Project.Columns.startDate";
            public const string endDate = "Projects.Project.Columns.endDate";
            public const string description = "Projects.Project.Columns.description";
        }
        public static class Actions
        {
            public const string New = "Projects.Project.Actions.New";
            public const string Edit = "Projects.Project.Actions.Edit";
            public const string Delete = "Projects.Project.Actions.Delete";
            public const string Save = "Projects.Project.Actions.Save";
            public const string Cancel = "Projects.Project.Actions.Cancel";
            public const string Export = "Projects.Project.Actions.Export";
            public const string Refresh = "Projects.Project.Actions.Refresh";
            public const string ColumnChooser = "Projects.Project.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.Project.Grid.Search";
            public const string NoData = "Projects.Project.Grid.NoData";
            public const string Loading = "Projects.Project.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.Project.Notifications.Saved";
            public const string Updated = "Projects.Project.Notifications.Updated";
            public const string Deleted = "Projects.Project.Notifications.Deleted";
            public const string Error = "Projects.Project.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.Project.Popup.CreateTitle";
            public const string EditTitle = "Projects.Project.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.Project.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.Project.Confirm.Delete";
        }
    }

    /// <summary>Projects.ProjectLocation — Proje Lokasyonu / Project Location</summary>
    public static class Projects_ProjectLocation
    {
        public const string ScreenId = "Projects.ProjectLocation";
        public const string Title = "Projects.ProjectLocation.Title";
        public const string Description = "Projects.ProjectLocation.Description";
        public static class Columns
        {
            public const string projectId = "Projects.ProjectLocation.Columns.projectId";
            public const string parentLocationId = "Projects.ProjectLocation.Columns.parentLocationId";
            public const string code = "Projects.ProjectLocation.Columns.code";
            public const string name = "Projects.ProjectLocation.Columns.name";
        }
        public static class Actions
        {
            public const string New = "Projects.ProjectLocation.Actions.New";
            public const string Edit = "Projects.ProjectLocation.Actions.Edit";
            public const string Delete = "Projects.ProjectLocation.Actions.Delete";
            public const string Save = "Projects.ProjectLocation.Actions.Save";
            public const string Cancel = "Projects.ProjectLocation.Actions.Cancel";
            public const string Export = "Projects.ProjectLocation.Actions.Export";
            public const string Refresh = "Projects.ProjectLocation.Actions.Refresh";
            public const string ColumnChooser = "Projects.ProjectLocation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.ProjectLocation.Grid.Search";
            public const string NoData = "Projects.ProjectLocation.Grid.NoData";
            public const string Loading = "Projects.ProjectLocation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.ProjectLocation.Notifications.Saved";
            public const string Updated = "Projects.ProjectLocation.Notifications.Updated";
            public const string Deleted = "Projects.ProjectLocation.Notifications.Deleted";
            public const string Error = "Projects.ProjectLocation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.ProjectLocation.Popup.CreateTitle";
            public const string EditTitle = "Projects.ProjectLocation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.ProjectLocation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.ProjectLocation.Confirm.Delete";
        }
    }

    /// <summary>Projects.ProjectMember — Proje Üyesi / Project Member</summary>
    public static class Projects_ProjectMember
    {
        public const string ScreenId = "Projects.ProjectMember";
        public const string Title = "Projects.ProjectMember.Title";
        public const string Description = "Projects.ProjectMember.Description";
        public static class Columns
        {
            public const string projectId = "Projects.ProjectMember.Columns.projectId";
            public const string userId = "Projects.ProjectMember.Columns.userId";
            public const string employeeId = "Projects.ProjectMember.Columns.employeeId";
            public const string projectRole = "Projects.ProjectMember.Columns.projectRole";
        }
        public static class Actions
        {
            public const string New = "Projects.ProjectMember.Actions.New";
            public const string Edit = "Projects.ProjectMember.Actions.Edit";
            public const string Delete = "Projects.ProjectMember.Actions.Delete";
            public const string Save = "Projects.ProjectMember.Actions.Save";
            public const string Cancel = "Projects.ProjectMember.Actions.Cancel";
            public const string Export = "Projects.ProjectMember.Actions.Export";
            public const string Refresh = "Projects.ProjectMember.Actions.Refresh";
            public const string ColumnChooser = "Projects.ProjectMember.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.ProjectMember.Grid.Search";
            public const string NoData = "Projects.ProjectMember.Grid.NoData";
            public const string Loading = "Projects.ProjectMember.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.ProjectMember.Notifications.Saved";
            public const string Updated = "Projects.ProjectMember.Notifications.Updated";
            public const string Deleted = "Projects.ProjectMember.Notifications.Deleted";
            public const string Error = "Projects.ProjectMember.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.ProjectMember.Popup.CreateTitle";
            public const string EditTitle = "Projects.ProjectMember.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.ProjectMember.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.ProjectMember.Confirm.Delete";
        }
    }

    /// <summary>Projects.ProjectNote — Proje Notu / Project Note</summary>
    public static class Projects_ProjectNote
    {
        public const string ScreenId = "Projects.ProjectNote";
        public const string Title = "Projects.ProjectNote.Title";
        public const string Description = "Projects.ProjectNote.Description";
        public static class Columns
        {
            public const string projectId = "Projects.ProjectNote.Columns.projectId";
            public const string title = "Projects.ProjectNote.Columns.title";
            public const string body = "Projects.ProjectNote.Columns.body";
        }
        public static class Actions
        {
            public const string New = "Projects.ProjectNote.Actions.New";
            public const string Edit = "Projects.ProjectNote.Actions.Edit";
            public const string Delete = "Projects.ProjectNote.Actions.Delete";
            public const string Save = "Projects.ProjectNote.Actions.Save";
            public const string Cancel = "Projects.ProjectNote.Actions.Cancel";
            public const string Export = "Projects.ProjectNote.Actions.Export";
            public const string Refresh = "Projects.ProjectNote.Actions.Refresh";
            public const string ColumnChooser = "Projects.ProjectNote.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.ProjectNote.Grid.Search";
            public const string NoData = "Projects.ProjectNote.Grid.NoData";
            public const string Loading = "Projects.ProjectNote.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.ProjectNote.Notifications.Saved";
            public const string Updated = "Projects.ProjectNote.Notifications.Updated";
            public const string Deleted = "Projects.ProjectNote.Notifications.Deleted";
            public const string Error = "Projects.ProjectNote.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.ProjectNote.Popup.CreateTitle";
            public const string EditTitle = "Projects.ProjectNote.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.ProjectNote.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.ProjectNote.Confirm.Delete";
        }
    }

    /// <summary>Projects.ProjectPhas — Proje Aşaması / Project Phase</summary>
    public static class Projects_ProjectPhas
    {
        public const string ScreenId = "Projects.ProjectPhas";
        public const string Title = "Projects.ProjectPhas.Title";
        public const string Description = "Projects.ProjectPhas.Description";
        public static class Columns
        {
            public const string projectId = "Projects.ProjectPhas.Columns.projectId";
            public const string parentPhaseId = "Projects.ProjectPhas.Columns.parentPhaseId";
            public const string code = "Projects.ProjectPhas.Columns.code";
            public const string name = "Projects.ProjectPhas.Columns.name";
            public const string progressPercentage = "Projects.ProjectPhas.Columns.progressPercentage";
        }
        public static class Actions
        {
            public const string New = "Projects.ProjectPhas.Actions.New";
            public const string Edit = "Projects.ProjectPhas.Actions.Edit";
            public const string Delete = "Projects.ProjectPhas.Actions.Delete";
            public const string Save = "Projects.ProjectPhas.Actions.Save";
            public const string Cancel = "Projects.ProjectPhas.Actions.Cancel";
            public const string Export = "Projects.ProjectPhas.Actions.Export";
            public const string Refresh = "Projects.ProjectPhas.Actions.Refresh";
            public const string ColumnChooser = "Projects.ProjectPhas.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.ProjectPhas.Grid.Search";
            public const string NoData = "Projects.ProjectPhas.Grid.NoData";
            public const string Loading = "Projects.ProjectPhas.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.ProjectPhas.Notifications.Saved";
            public const string Updated = "Projects.ProjectPhas.Notifications.Updated";
            public const string Deleted = "Projects.ProjectPhas.Notifications.Deleted";
            public const string Error = "Projects.ProjectPhas.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.ProjectPhas.Popup.CreateTitle";
            public const string EditTitle = "Projects.ProjectPhas.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.ProjectPhas.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.ProjectPhas.Confirm.Delete";
        }
    }

    /// <summary>Projects.ProjectStatus — Proje Durumu / Project Status</summary>
    public static class Projects_ProjectStatus
    {
        public const string ScreenId = "Projects.ProjectStatus";
        public const string Title = "Projects.ProjectStatus.Title";
        public const string Description = "Projects.ProjectStatus.Description";
        public static class Columns
        {
            public const string code = "Projects.ProjectStatus.Columns.code";
            public const string name = "Projects.ProjectStatus.Columns.name";
            public const string displayOrder = "Projects.ProjectStatus.Columns.displayOrder";
            public const string isClosedState = "Projects.ProjectStatus.Columns.isClosedState";
            public const string isActive = "Projects.ProjectStatus.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Projects.ProjectStatus.Actions.New";
            public const string Edit = "Projects.ProjectStatus.Actions.Edit";
            public const string Delete = "Projects.ProjectStatus.Actions.Delete";
            public const string Save = "Projects.ProjectStatus.Actions.Save";
            public const string Cancel = "Projects.ProjectStatus.Actions.Cancel";
            public const string Export = "Projects.ProjectStatus.Actions.Export";
            public const string Refresh = "Projects.ProjectStatus.Actions.Refresh";
            public const string ColumnChooser = "Projects.ProjectStatus.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.ProjectStatus.Grid.Search";
            public const string NoData = "Projects.ProjectStatus.Grid.NoData";
            public const string Loading = "Projects.ProjectStatus.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.ProjectStatus.Notifications.Saved";
            public const string Updated = "Projects.ProjectStatus.Notifications.Updated";
            public const string Deleted = "Projects.ProjectStatus.Notifications.Deleted";
            public const string Error = "Projects.ProjectStatus.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.ProjectStatus.Popup.CreateTitle";
            public const string EditTitle = "Projects.ProjectStatus.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.ProjectStatus.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.ProjectStatus.Confirm.Delete";
        }
    }

    /// <summary>Projects.ProjectType — Proje Türü / Project Type</summary>
    public static class Projects_ProjectType
    {
        public const string ScreenId = "Projects.ProjectType";
        public const string Title = "Projects.ProjectType.Title";
        public const string Description = "Projects.ProjectType.Description";
        public static class Columns
        {
            public const string code = "Projects.ProjectType.Columns.code";
            public const string name = "Projects.ProjectType.Columns.name";
            public const string isActive = "Projects.ProjectType.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Projects.ProjectType.Actions.New";
            public const string Edit = "Projects.ProjectType.Actions.Edit";
            public const string Delete = "Projects.ProjectType.Actions.Delete";
            public const string Save = "Projects.ProjectType.Actions.Save";
            public const string Cancel = "Projects.ProjectType.Actions.Cancel";
            public const string Export = "Projects.ProjectType.Actions.Export";
            public const string Refresh = "Projects.ProjectType.Actions.Refresh";
            public const string ColumnChooser = "Projects.ProjectType.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Projects.ProjectType.Grid.Search";
            public const string NoData = "Projects.ProjectType.Grid.NoData";
            public const string Loading = "Projects.ProjectType.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Projects.ProjectType.Notifications.Saved";
            public const string Updated = "Projects.ProjectType.Notifications.Updated";
            public const string Deleted = "Projects.ProjectType.Notifications.Deleted";
            public const string Error = "Projects.ProjectType.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Projects.ProjectType.Popup.CreateTitle";
            public const string EditTitle = "Projects.ProjectType.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Projects.ProjectType.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Projects.ProjectType.Confirm.Delete";
        }
    }

    /// <summary>Reporting.DashboardWidget — Gösterge Paneli Bileşeni / Dashboard Widget</summary>
    public static class Reporting_DashboardWidget
    {
        public const string ScreenId = "Reporting.DashboardWidget";
        public const string Title = "Reporting.DashboardWidget.Title";
        public const string Description = "Reporting.DashboardWidget.Description";
        public static class Columns
        {
            public const string code = "Reporting.DashboardWidget.Columns.code";
            public const string name = "Reporting.DashboardWidget.Columns.name";
            public const string module = "Reporting.DashboardWidget.Columns.module";
            public const string widgetType = "Reporting.DashboardWidget.Columns.widgetType";
            public const string requiredPermissionCode = "Reporting.DashboardWidget.Columns.requiredPermissionCode";
            public const string displayOrder = "Reporting.DashboardWidget.Columns.displayOrder";
            public const string isActive = "Reporting.DashboardWidget.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Reporting.DashboardWidget.Actions.New";
            public const string Edit = "Reporting.DashboardWidget.Actions.Edit";
            public const string Delete = "Reporting.DashboardWidget.Actions.Delete";
            public const string Save = "Reporting.DashboardWidget.Actions.Save";
            public const string Cancel = "Reporting.DashboardWidget.Actions.Cancel";
            public const string Export = "Reporting.DashboardWidget.Actions.Export";
            public const string Refresh = "Reporting.DashboardWidget.Actions.Refresh";
            public const string ColumnChooser = "Reporting.DashboardWidget.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Reporting.DashboardWidget.Grid.Search";
            public const string NoData = "Reporting.DashboardWidget.Grid.NoData";
            public const string Loading = "Reporting.DashboardWidget.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Reporting.DashboardWidget.Notifications.Saved";
            public const string Updated = "Reporting.DashboardWidget.Notifications.Updated";
            public const string Deleted = "Reporting.DashboardWidget.Notifications.Deleted";
            public const string Error = "Reporting.DashboardWidget.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Reporting.DashboardWidget.Popup.CreateTitle";
            public const string EditTitle = "Reporting.DashboardWidget.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Reporting.DashboardWidget.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Reporting.DashboardWidget.Confirm.Delete";
        }
    }

    /// <summary>Reporting.ReportDefinition — Rapor Tanımı / Report Definition</summary>
    public static class Reporting_ReportDefinition
    {
        public const string ScreenId = "Reporting.ReportDefinition";
        public const string Title = "Reporting.ReportDefinition.Title";
        public const string Description = "Reporting.ReportDefinition.Description";
        public static class Columns
        {
            public const string code = "Reporting.ReportDefinition.Columns.code";
            public const string name = "Reporting.ReportDefinition.Columns.name";
            public const string module = "Reporting.ReportDefinition.Columns.module";
            public const string queryKey = "Reporting.ReportDefinition.Columns.queryKey";
            public const string requiredPermissionCode = "Reporting.ReportDefinition.Columns.requiredPermissionCode";
            public const string isActive = "Reporting.ReportDefinition.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Reporting.ReportDefinition.Actions.New";
            public const string Edit = "Reporting.ReportDefinition.Actions.Edit";
            public const string Delete = "Reporting.ReportDefinition.Actions.Delete";
            public const string Save = "Reporting.ReportDefinition.Actions.Save";
            public const string Cancel = "Reporting.ReportDefinition.Actions.Cancel";
            public const string Export = "Reporting.ReportDefinition.Actions.Export";
            public const string Refresh = "Reporting.ReportDefinition.Actions.Refresh";
            public const string ColumnChooser = "Reporting.ReportDefinition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Reporting.ReportDefinition.Grid.Search";
            public const string NoData = "Reporting.ReportDefinition.Grid.NoData";
            public const string Loading = "Reporting.ReportDefinition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Reporting.ReportDefinition.Notifications.Saved";
            public const string Updated = "Reporting.ReportDefinition.Notifications.Updated";
            public const string Deleted = "Reporting.ReportDefinition.Notifications.Deleted";
            public const string Error = "Reporting.ReportDefinition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Reporting.ReportDefinition.Popup.CreateTitle";
            public const string EditTitle = "Reporting.ReportDefinition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Reporting.ReportDefinition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Reporting.ReportDefinition.Confirm.Delete";
        }
    }

    /// <summary>Requests.Request — Talep / Request</summary>
    public static class Requests_Request
    {
        public const string ScreenId = "Requests.Request";
        public const string Title = "Requests.Request.Title";
        public const string Description = "Requests.Request.Description";
        public static class Columns
        {
            public const string requestTypeId = "Requests.Request.Columns.requestTypeId";
            public const string projectId = "Requests.Request.Columns.projectId";
            public const string requestedByUserId = "Requests.Request.Columns.requestedByUserId";
            public const string status = "Requests.Request.Columns.status";
            public const string requestNo = "Requests.Request.Columns.requestNo";
            public const string requestDate = "Requests.Request.Columns.requestDate";
            public const string description = "Requests.Request.Columns.description";
            public const string approvalRequestId = "Requests.Request.Columns.approvalRequestId";
        }
        public static class Actions
        {
            public const string New = "Requests.Request.Actions.New";
            public const string Edit = "Requests.Request.Actions.Edit";
            public const string Delete = "Requests.Request.Actions.Delete";
            public const string Save = "Requests.Request.Actions.Save";
            public const string Cancel = "Requests.Request.Actions.Cancel";
            public const string Export = "Requests.Request.Actions.Export";
            public const string Refresh = "Requests.Request.Actions.Refresh";
            public const string ColumnChooser = "Requests.Request.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Requests.Request.Grid.Search";
            public const string NoData = "Requests.Request.Grid.NoData";
            public const string Loading = "Requests.Request.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Requests.Request.Notifications.Saved";
            public const string Updated = "Requests.Request.Notifications.Updated";
            public const string Deleted = "Requests.Request.Notifications.Deleted";
            public const string Error = "Requests.Request.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Requests.Request.Popup.CreateTitle";
            public const string EditTitle = "Requests.Request.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Requests.Request.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Requests.Request.Confirm.Delete";
        }
    }

    /// <summary>Requests.RequestLine — Talep Kalemi / Request Line</summary>
    public static class Requests_RequestLine
    {
        public const string ScreenId = "Requests.RequestLine";
        public const string Title = "Requests.RequestLine.Title";
        public const string Description = "Requests.RequestLine.Description";
        public static class Columns
        {
            public const string requestId = "Requests.RequestLine.Columns.requestId";
            public const string materialId = "Requests.RequestLine.Columns.materialId";
            public const string requestedMaterialText = "Requests.RequestLine.Columns.requestedMaterialText";
            public const string quantity = "Requests.RequestLine.Columns.quantity";
            public const string unitOfMeasureId = "Requests.RequestLine.Columns.unitOfMeasureId";
            public const string note = "Requests.RequestLine.Columns.note";
        }
        public static class Actions
        {
            public const string New = "Requests.RequestLine.Actions.New";
            public const string Edit = "Requests.RequestLine.Actions.Edit";
            public const string Delete = "Requests.RequestLine.Actions.Delete";
            public const string Save = "Requests.RequestLine.Actions.Save";
            public const string Cancel = "Requests.RequestLine.Actions.Cancel";
            public const string Export = "Requests.RequestLine.Actions.Export";
            public const string Refresh = "Requests.RequestLine.Actions.Refresh";
            public const string ColumnChooser = "Requests.RequestLine.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Requests.RequestLine.Grid.Search";
            public const string NoData = "Requests.RequestLine.Grid.NoData";
            public const string Loading = "Requests.RequestLine.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Requests.RequestLine.Notifications.Saved";
            public const string Updated = "Requests.RequestLine.Notifications.Updated";
            public const string Deleted = "Requests.RequestLine.Notifications.Deleted";
            public const string Error = "Requests.RequestLine.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Requests.RequestLine.Popup.CreateTitle";
            public const string EditTitle = "Requests.RequestLine.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Requests.RequestLine.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Requests.RequestLine.Confirm.Delete";
        }
    }

    /// <summary>Requests.RequestType — Talep Türü / Request Type</summary>
    public static class Requests_RequestType
    {
        public const string ScreenId = "Requests.RequestType";
        public const string Title = "Requests.RequestType.Title";
        public const string Description = "Requests.RequestType.Description";
        public static class Columns
        {
            public const string code = "Requests.RequestType.Columns.code";
            public const string name = "Requests.RequestType.Columns.name";
            public const string category = "Requests.RequestType.Columns.category";
            public const string isActive = "Requests.RequestType.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Requests.RequestType.Actions.New";
            public const string Edit = "Requests.RequestType.Actions.Edit";
            public const string Delete = "Requests.RequestType.Actions.Delete";
            public const string Save = "Requests.RequestType.Actions.Save";
            public const string Cancel = "Requests.RequestType.Actions.Cancel";
            public const string Export = "Requests.RequestType.Actions.Export";
            public const string Refresh = "Requests.RequestType.Actions.Refresh";
            public const string ColumnChooser = "Requests.RequestType.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Requests.RequestType.Grid.Search";
            public const string NoData = "Requests.RequestType.Grid.NoData";
            public const string Loading = "Requests.RequestType.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Requests.RequestType.Notifications.Saved";
            public const string Updated = "Requests.RequestType.Notifications.Updated";
            public const string Deleted = "Requests.RequestType.Notifications.Deleted";
            public const string Error = "Requests.RequestType.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Requests.RequestType.Popup.CreateTitle";
            public const string EditTitle = "Requests.RequestType.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Requests.RequestType.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Requests.RequestType.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalAction — Onay Aksiyonu / Approval Action</summary>
    public static class Workflow_ApprovalAction
    {
        public const string ScreenId = "Workflow.ApprovalAction";
        public const string Title = "Workflow.ApprovalAction.Title";
        public const string Description = "Workflow.ApprovalAction.Description";
        public static class Columns
        {
            public const string approvalRequestId = "Workflow.ApprovalAction.Columns.approvalRequestId";
            public const string approvalRequestStepId = "Workflow.ApprovalAction.Columns.approvalRequestStepId";
            public const string userId = "Workflow.ApprovalAction.Columns.userId";
            public const string actionType = "Workflow.ApprovalAction.Columns.actionType";
            public const string actionAt = "Workflow.ApprovalAction.Columns.actionAt";
            public const string note = "Workflow.ApprovalAction.Columns.note";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalAction.Actions.New";
            public const string Edit = "Workflow.ApprovalAction.Actions.Edit";
            public const string Delete = "Workflow.ApprovalAction.Actions.Delete";
            public const string Save = "Workflow.ApprovalAction.Actions.Save";
            public const string Cancel = "Workflow.ApprovalAction.Actions.Cancel";
            public const string Export = "Workflow.ApprovalAction.Actions.Export";
            public const string Refresh = "Workflow.ApprovalAction.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalAction.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalAction.Grid.Search";
            public const string NoData = "Workflow.ApprovalAction.Grid.NoData";
            public const string Loading = "Workflow.ApprovalAction.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalAction.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalAction.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalAction.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalAction.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalAction.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalAction.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalAction.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalAction.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalCondition — Onay Koşulu / Approval Condition</summary>
    public static class Workflow_ApprovalCondition
    {
        public const string ScreenId = "Workflow.ApprovalCondition";
        public const string Title = "Workflow.ApprovalCondition.Title";
        public const string Description = "Workflow.ApprovalCondition.Description";
        public static class Columns
        {
            public const string approvalDefinitionVersionId = "Workflow.ApprovalCondition.Columns.approvalDefinitionVersionId";
            public const string fieldName = "Workflow.ApprovalCondition.Columns.fieldName";
            public const string @operator = "Workflow.ApprovalCondition.Columns.operator";
            public const string valueText = "Workflow.ApprovalCondition.Columns.valueText";
            public const string valueNumber = "Workflow.ApprovalCondition.Columns.valueNumber";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalCondition.Actions.New";
            public const string Edit = "Workflow.ApprovalCondition.Actions.Edit";
            public const string Delete = "Workflow.ApprovalCondition.Actions.Delete";
            public const string Save = "Workflow.ApprovalCondition.Actions.Save";
            public const string Cancel = "Workflow.ApprovalCondition.Actions.Cancel";
            public const string Export = "Workflow.ApprovalCondition.Actions.Export";
            public const string Refresh = "Workflow.ApprovalCondition.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalCondition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalCondition.Grid.Search";
            public const string NoData = "Workflow.ApprovalCondition.Grid.NoData";
            public const string Loading = "Workflow.ApprovalCondition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalCondition.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalCondition.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalCondition.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalCondition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalCondition.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalCondition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalCondition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalCondition.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalDefinition — Onay Tanımı / Approval Definition</summary>
    public static class Workflow_ApprovalDefinition
    {
        public const string ScreenId = "Workflow.ApprovalDefinition";
        public const string Title = "Workflow.ApprovalDefinition.Title";
        public const string Description = "Workflow.ApprovalDefinition.Description";
        public static class Columns
        {
            public const string code = "Workflow.ApprovalDefinition.Columns.code";
            public const string name = "Workflow.ApprovalDefinition.Columns.name";
            public const string relatedModule = "Workflow.ApprovalDefinition.Columns.relatedModule";
            public const string relatedEntityType = "Workflow.ApprovalDefinition.Columns.relatedEntityType";
            public const string isActive = "Workflow.ApprovalDefinition.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalDefinition.Actions.New";
            public const string Edit = "Workflow.ApprovalDefinition.Actions.Edit";
            public const string Delete = "Workflow.ApprovalDefinition.Actions.Delete";
            public const string Save = "Workflow.ApprovalDefinition.Actions.Save";
            public const string Cancel = "Workflow.ApprovalDefinition.Actions.Cancel";
            public const string Export = "Workflow.ApprovalDefinition.Actions.Export";
            public const string Refresh = "Workflow.ApprovalDefinition.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalDefinition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalDefinition.Grid.Search";
            public const string NoData = "Workflow.ApprovalDefinition.Grid.NoData";
            public const string Loading = "Workflow.ApprovalDefinition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalDefinition.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalDefinition.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalDefinition.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalDefinition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalDefinition.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalDefinition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalDefinition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalDefinition.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalDefinitionVersion — Onay Tanımı Sürümü / Approval Definition Version</summary>
    public static class Workflow_ApprovalDefinitionVersion
    {
        public const string ScreenId = "Workflow.ApprovalDefinitionVersion";
        public const string Title = "Workflow.ApprovalDefinitionVersion.Title";
        public const string Description = "Workflow.ApprovalDefinitionVersion.Description";
        public static class Columns
        {
            public const string approvalDefinitionId = "Workflow.ApprovalDefinitionVersion.Columns.approvalDefinitionId";
            public const string versionNo = "Workflow.ApprovalDefinitionVersion.Columns.versionNo";
            public const string effectiveFrom = "Workflow.ApprovalDefinitionVersion.Columns.effectiveFrom";
            public const string effectiveTo = "Workflow.ApprovalDefinitionVersion.Columns.effectiveTo";
            public const string isActive = "Workflow.ApprovalDefinitionVersion.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalDefinitionVersion.Actions.New";
            public const string Edit = "Workflow.ApprovalDefinitionVersion.Actions.Edit";
            public const string Delete = "Workflow.ApprovalDefinitionVersion.Actions.Delete";
            public const string Save = "Workflow.ApprovalDefinitionVersion.Actions.Save";
            public const string Cancel = "Workflow.ApprovalDefinitionVersion.Actions.Cancel";
            public const string Export = "Workflow.ApprovalDefinitionVersion.Actions.Export";
            public const string Refresh = "Workflow.ApprovalDefinitionVersion.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalDefinitionVersion.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalDefinitionVersion.Grid.Search";
            public const string NoData = "Workflow.ApprovalDefinitionVersion.Grid.NoData";
            public const string Loading = "Workflow.ApprovalDefinitionVersion.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalDefinitionVersion.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalDefinitionVersion.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalDefinitionVersion.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalDefinitionVersion.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalDefinitionVersion.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalDefinitionVersion.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalDefinitionVersion.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalDefinitionVersion.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalDelegation — Onay Devri / Approval Delegation</summary>
    public static class Workflow_ApprovalDelegation
    {
        public const string ScreenId = "Workflow.ApprovalDelegation";
        public const string Title = "Workflow.ApprovalDelegation.Title";
        public const string Description = "Workflow.ApprovalDelegation.Description";
        public static class Columns
        {
            public const string delegatorUserId = "Workflow.ApprovalDelegation.Columns.delegatorUserId";
            public const string delegateUserId = "Workflow.ApprovalDelegation.Columns.delegateUserId";
            public const string startDate = "Workflow.ApprovalDelegation.Columns.startDate";
            public const string endDate = "Workflow.ApprovalDelegation.Columns.endDate";
            public const string isActive = "Workflow.ApprovalDelegation.Columns.isActive";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalDelegation.Actions.New";
            public const string Edit = "Workflow.ApprovalDelegation.Actions.Edit";
            public const string Delete = "Workflow.ApprovalDelegation.Actions.Delete";
            public const string Save = "Workflow.ApprovalDelegation.Actions.Save";
            public const string Cancel = "Workflow.ApprovalDelegation.Actions.Cancel";
            public const string Export = "Workflow.ApprovalDelegation.Actions.Export";
            public const string Refresh = "Workflow.ApprovalDelegation.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalDelegation.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalDelegation.Grid.Search";
            public const string NoData = "Workflow.ApprovalDelegation.Grid.NoData";
            public const string Loading = "Workflow.ApprovalDelegation.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalDelegation.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalDelegation.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalDelegation.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalDelegation.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalDelegation.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalDelegation.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalDelegation.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalDelegation.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalRequest — Onay Talebi / Approval Request</summary>
    public static class Workflow_ApprovalRequest
    {
        public const string ScreenId = "Workflow.ApprovalRequest";
        public const string Title = "Workflow.ApprovalRequest.Title";
        public const string Description = "Workflow.ApprovalRequest.Description";
        public static class Columns
        {
            public const string approvalDefinitionVersionId = "Workflow.ApprovalRequest.Columns.approvalDefinitionVersionId";
            public const string relatedModule = "Workflow.ApprovalRequest.Columns.relatedModule";
            public const string relatedEntityType = "Workflow.ApprovalRequest.Columns.relatedEntityType";
            public const string relatedEntityId = "Workflow.ApprovalRequest.Columns.relatedEntityId";
            public const string requestedByUserId = "Workflow.ApprovalRequest.Columns.requestedByUserId";
            public const string status = "Workflow.ApprovalRequest.Columns.status";
            public const string currentStepNo = "Workflow.ApprovalRequest.Columns.currentStepNo";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalRequest.Actions.New";
            public const string Edit = "Workflow.ApprovalRequest.Actions.Edit";
            public const string Delete = "Workflow.ApprovalRequest.Actions.Delete";
            public const string Save = "Workflow.ApprovalRequest.Actions.Save";
            public const string Cancel = "Workflow.ApprovalRequest.Actions.Cancel";
            public const string Export = "Workflow.ApprovalRequest.Actions.Export";
            public const string Refresh = "Workflow.ApprovalRequest.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalRequest.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalRequest.Grid.Search";
            public const string NoData = "Workflow.ApprovalRequest.Grid.NoData";
            public const string Loading = "Workflow.ApprovalRequest.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalRequest.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalRequest.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalRequest.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalRequest.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalRequest.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalRequest.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalRequest.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalRequest.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalRequestApprover — Onay Talebi Onaylayıcısı / Approval Request Approver</summary>
    public static class Workflow_ApprovalRequestApprover
    {
        public const string ScreenId = "Workflow.ApprovalRequestApprover";
        public const string Title = "Workflow.ApprovalRequestApprover.Title";
        public const string Description = "Workflow.ApprovalRequestApprover.Description";
        public static class Columns
        {
            public const string approvalRequestStepId = "Workflow.ApprovalRequestApprover.Columns.approvalRequestStepId";
            public const string userId = "Workflow.ApprovalRequestApprover.Columns.userId";
            public const string status = "Workflow.ApprovalRequestApprover.Columns.status";
            public const string actionAt = "Workflow.ApprovalRequestApprover.Columns.actionAt";
            public const string delegatedFromUserId = "Workflow.ApprovalRequestApprover.Columns.delegatedFromUserId";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalRequestApprover.Actions.New";
            public const string Edit = "Workflow.ApprovalRequestApprover.Actions.Edit";
            public const string Delete = "Workflow.ApprovalRequestApprover.Actions.Delete";
            public const string Save = "Workflow.ApprovalRequestApprover.Actions.Save";
            public const string Cancel = "Workflow.ApprovalRequestApprover.Actions.Cancel";
            public const string Export = "Workflow.ApprovalRequestApprover.Actions.Export";
            public const string Refresh = "Workflow.ApprovalRequestApprover.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalRequestApprover.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalRequestApprover.Grid.Search";
            public const string NoData = "Workflow.ApprovalRequestApprover.Grid.NoData";
            public const string Loading = "Workflow.ApprovalRequestApprover.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalRequestApprover.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalRequestApprover.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalRequestApprover.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalRequestApprover.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalRequestApprover.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalRequestApprover.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalRequestApprover.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalRequestApprover.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalRequestStep — Onay Talebi Adımı / Approval Request Step</summary>
    public static class Workflow_ApprovalRequestStep
    {
        public const string ScreenId = "Workflow.ApprovalRequestStep";
        public const string Title = "Workflow.ApprovalRequestStep.Title";
        public const string Description = "Workflow.ApprovalRequestStep.Description";
        public static class Columns
        {
            public const string approvalRequestId = "Workflow.ApprovalRequestStep.Columns.approvalRequestId";
            public const string approvalStepDefinitionId = "Workflow.ApprovalRequestStep.Columns.approvalStepDefinitionId";
            public const string stepNo = "Workflow.ApprovalRequestStep.Columns.stepNo";
            public const string status = "Workflow.ApprovalRequestStep.Columns.status";
            public const string approvalMode = "Workflow.ApprovalRequestStep.Columns.approvalMode";
            public const string requiredApprovalCount = "Workflow.ApprovalRequestStep.Columns.requiredApprovalCount";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalRequestStep.Actions.New";
            public const string Edit = "Workflow.ApprovalRequestStep.Actions.Edit";
            public const string Delete = "Workflow.ApprovalRequestStep.Actions.Delete";
            public const string Save = "Workflow.ApprovalRequestStep.Actions.Save";
            public const string Cancel = "Workflow.ApprovalRequestStep.Actions.Cancel";
            public const string Export = "Workflow.ApprovalRequestStep.Actions.Export";
            public const string Refresh = "Workflow.ApprovalRequestStep.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalRequestStep.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalRequestStep.Grid.Search";
            public const string NoData = "Workflow.ApprovalRequestStep.Grid.NoData";
            public const string Loading = "Workflow.ApprovalRequestStep.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalRequestStep.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalRequestStep.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalRequestStep.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalRequestStep.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalRequestStep.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalRequestStep.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalRequestStep.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalRequestStep.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalStepApprover — Onay Adımı Onaylayıcısı / Approval Step Approver</summary>
    public static class Workflow_ApprovalStepApprover
    {
        public const string ScreenId = "Workflow.ApprovalStepApprover";
        public const string Title = "Workflow.ApprovalStepApprover.Title";
        public const string Description = "Workflow.ApprovalStepApprover.Description";
        public static class Columns
        {
            public const string approvalStepDefinitionId = "Workflow.ApprovalStepApprover.Columns.approvalStepDefinitionId";
            public const string approverType = "Workflow.ApprovalStepApprover.Columns.approverType";
            public const string approverUserId = "Workflow.ApprovalStepApprover.Columns.approverUserId";
            public const string approverRoleId = "Workflow.ApprovalStepApprover.Columns.approverRoleId";
            public const string approverDepartmentId = "Workflow.ApprovalStepApprover.Columns.approverDepartmentId";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalStepApprover.Actions.New";
            public const string Edit = "Workflow.ApprovalStepApprover.Actions.Edit";
            public const string Delete = "Workflow.ApprovalStepApprover.Actions.Delete";
            public const string Save = "Workflow.ApprovalStepApprover.Actions.Save";
            public const string Cancel = "Workflow.ApprovalStepApprover.Actions.Cancel";
            public const string Export = "Workflow.ApprovalStepApprover.Actions.Export";
            public const string Refresh = "Workflow.ApprovalStepApprover.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalStepApprover.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalStepApprover.Grid.Search";
            public const string NoData = "Workflow.ApprovalStepApprover.Grid.NoData";
            public const string Loading = "Workflow.ApprovalStepApprover.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalStepApprover.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalStepApprover.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalStepApprover.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalStepApprover.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalStepApprover.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalStepApprover.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalStepApprover.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalStepApprover.Confirm.Delete";
        }
    }

    /// <summary>Workflow.ApprovalStepDefinition — Onay Adımı Tanımı / Approval Step Definition</summary>
    public static class Workflow_ApprovalStepDefinition
    {
        public const string ScreenId = "Workflow.ApprovalStepDefinition";
        public const string Title = "Workflow.ApprovalStepDefinition.Title";
        public const string Description = "Workflow.ApprovalStepDefinition.Description";
        public static class Columns
        {
            public const string approvalDefinitionVersionId = "Workflow.ApprovalStepDefinition.Columns.approvalDefinitionVersionId";
            public const string stepNo = "Workflow.ApprovalStepDefinition.Columns.stepNo";
            public const string approvalMode = "Workflow.ApprovalStepDefinition.Columns.approvalMode";
            public const string requiredApprovalCount = "Workflow.ApprovalStepDefinition.Columns.requiredApprovalCount";
            public const string isRequired = "Workflow.ApprovalStepDefinition.Columns.isRequired";
            public const string name = "Workflow.ApprovalStepDefinition.Columns.name";
        }
        public static class Actions
        {
            public const string New = "Workflow.ApprovalStepDefinition.Actions.New";
            public const string Edit = "Workflow.ApprovalStepDefinition.Actions.Edit";
            public const string Delete = "Workflow.ApprovalStepDefinition.Actions.Delete";
            public const string Save = "Workflow.ApprovalStepDefinition.Actions.Save";
            public const string Cancel = "Workflow.ApprovalStepDefinition.Actions.Cancel";
            public const string Export = "Workflow.ApprovalStepDefinition.Actions.Export";
            public const string Refresh = "Workflow.ApprovalStepDefinition.Actions.Refresh";
            public const string ColumnChooser = "Workflow.ApprovalStepDefinition.Actions.ColumnChooser";
        }
        public static class Grid
        {
            public const string Search = "Workflow.ApprovalStepDefinition.Grid.Search";
            public const string NoData = "Workflow.ApprovalStepDefinition.Grid.NoData";
            public const string Loading = "Workflow.ApprovalStepDefinition.Grid.Loading";
        }
        public static class Notifications
        {
            public const string Saved = "Workflow.ApprovalStepDefinition.Notifications.Saved";
            public const string Updated = "Workflow.ApprovalStepDefinition.Notifications.Updated";
            public const string Deleted = "Workflow.ApprovalStepDefinition.Notifications.Deleted";
            public const string Error = "Workflow.ApprovalStepDefinition.Notifications.Error";
        }
        public static class Popup
        {
            public const string CreateTitle = "Workflow.ApprovalStepDefinition.Popup.CreateTitle";
            public const string EditTitle = "Workflow.ApprovalStepDefinition.Popup.EditTitle";
        }
        public static class Validation
        {
            public const string Required = "Workflow.ApprovalStepDefinition.Validation.Required";
        }
        public static class Confirm
        {
            public const string Delete = "Workflow.ApprovalStepDefinition.Confirm.Delete";
        }
    }

}