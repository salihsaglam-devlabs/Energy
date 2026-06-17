#!/usr/bin/env python3
"""
Per-screen BUSINESS PURPOSE info text for every data / process / report screen.

Replaces the generic "{Entity} kayıtlarını listeleyebilir..." placeholder in
{ScreenId}.Help.Intro with a concrete, page-specific description of WHICH business
processes that page manages. Also adds {ScreenId}.Help.ScreenKindTitle ("Ekran
Türü" / "Screen Type") so the info panel can label the screen-kind section.

TR -> SharedResource.tr-TR.resx ; EN -> SharedResource.en-US.resx + neutral.
Idempotent: existing values are overwritten with the curated text.
"""
import os, re

BASE = os.path.join(os.path.dirname(__file__), "..", "..", "Energy.Localization", "Resources")

# ScreenId -> (Turkish business description, English business description)
DESC = {
    # ---- Assets ----
    "Assets.EquipmentAsset": ("İş makinesi, ekipman ve demirbaşların ana kayıtlarını (kod, tür, seri no, satın alma tarihi) yönetirsiniz. Sahadaki tüm ekipman envanterinin tek kaynağıdır; atama ve bakım kayıtları bu karta bağlanır.", "Manage the master records of machinery, equipment and fixed assets (code, type, serial no, purchase date). It is the single source for the field equipment inventory; assignments and maintenance link to it."),
    "Assets.EquipmentAssignment": ("Ekipmanların hangi projeye, çalışana veya depoya hangi tarih aralığında tahsis edildiğini yönetirsiniz. Bir ekipmanın anlık olarak nerede ve kimde olduğunu izlemeyi sağlar.", "Manage which project, employee or warehouse each equipment is assigned to and for which date range. Lets you track where and with whom an equipment currently is."),
    "Assets.EquipmentMaintenance": ("Ekipmanların planlı ve yapılan bakım kayıtlarını (bakım türü, planlanan/tamamlanan tarih, maliyet) yönetirsiniz. Periyodik bakım ve arıza takibini sağlar.", "Manage scheduled and performed equipment maintenance records (type, planned/completed date, cost). Supports periodic maintenance and breakdown tracking."),
    # ---- Budget ----
    "Budget.Budget": ("Proje ve masraf merkezi bazında yıllık bütçe başlıklarını tanımlarsınız. Planlanan harcama çerçevesini oluşturur; bütçe kalemleri bu başlığa bağlanır.", "Define annual budget headers per project and cost center. Sets the planned spending framework; budget lines attach to it."),
    "Budget.BudgetLine": ("Bir bütçenin kalem kalem planlanan tutarlarını (açıklama, masraf merkezi, planlanan tutar) yönetirsiniz. Bütçenin detay dağılımını oluşturur.", "Manage a budget's line-by-line planned amounts (description, cost center, planned amount). Builds the detailed breakdown of the budget."),
    # ---- BusinessPartners ----
    "BusinessPartners.BusinessPartner": ("Müşteri, tedarikçi ve taşeron gibi iş ortaklarının cari kartlarını (tür, kod, vergi no, iletişim) yönetirsiniz. Satınalma, satış ve finans işlemlerinin ortak kaynağıdır.", "Manage the account cards of business partners such as customers, suppliers and subcontractors (type, code, tax no, contact). The shared source for procurement, sales and finance transactions."),
    "BusinessPartners.BusinessPartnerAddress": ("Bir iş ortağına ait adresleri (fatura, sevkiyat vb. türde) yönetirsiniz.", "Manage the addresses of a business partner (billing, shipping, etc.)."),
    "BusinessPartners.BusinessPartnerBankAccount": ("İş ortağının banka hesaplarını (IBAN, banka, para birimi) yönetirsiniz; ödeme ve tahsilat işlemlerinde kullanılır.", "Manage a business partner's bank accounts (IBAN, bank, currency); used in payment and collection transactions."),
    "BusinessPartners.BusinessPartnerContact": ("İş ortağının yetkili/irtibat kişilerini (ad, unvan, telefon, e-posta) yönetirsiniz.", "Manage a business partner's authorized contacts (name, title, phone, email)."),
    # ---- Catalog ----
    "Catalog.Brand": ("Malzeme markalarını tanımlarsınız; malzeme kartlarında marka seçiminde kullanılır.", "Define material brands; used when selecting a brand on material cards."),
    "Catalog.Material": ("Stok ve satınalmada kullanılan malzeme/ürün ana kartlarını (kod, kategori, ölçü birimi, parti/seri takibi) yönetirsiniz. Tüm stok ve satınalma işlemlerinin temelidir.", "Manage the master cards of materials/products used in stock and procurement (code, category, unit, batch/serial tracking). The foundation of all stock and purchasing transactions."),
    "Catalog.MaterialAttributeDefinition": ("Malzemelere uygulanabilecek öznitelik (özellik) tanımlarını yönetirsiniz; örneğin renk, ölçü, kalite.", "Manage attribute (property) definitions that can apply to materials, e.g. color, size, grade."),
    "Catalog.MaterialAttributeOption": ("Bir özniteliğin seçilebilir değer listesini (örn. Kırmızı, Mavi) yönetirsiniz.", "Manage the selectable value list of an attribute (e.g. Red, Blue)."),
    "Catalog.MaterialAttributeValue": ("Bir malzemeye atanmış öznitelik değerlerini yönetirsiniz.", "Manage the attribute values assigned to a material."),
    "Catalog.MaterialCategory": ("Malzeme kategorilerini hiyerarşik olarak yönetirsiniz; raporlama ve filtreleme için sınıflandırma sağlar.", "Manage material categories hierarchically; provides classification for reporting and filtering."),
    "Catalog.MaterialCategoryAttribute": ("Bir malzeme kategorisine bağlı zorunlu/opsiyonel öznitelikleri tanımlarsınız.", "Define the required/optional attributes tied to a material category."),
    "Catalog.MaterialUnitConversion": ("Bir malzeme için ölçü birimleri arası dönüşüm katsayılarını (örn. koli↔adet) yönetirsiniz.", "Manage unit-of-measure conversion factors for a material (e.g. box↔piece)."),
    # ---- Contracts ----
    "Contracts.Contract": ("Müşteri ve taşeron sözleşmelerini (tür, proje, tutar, tarih, durum) yönetirsiniz. Hakediş ve sözleşme kalemlerinin dayanağıdır.", "Manage customer and subcontractor contracts (type, project, amount, date, status). The basis for progress payments and contract lines."),
    "Contracts.ContractAmendment": ("Sözleşmedeki ek ve zeyilname değişikliklerini (tutar farkı, tarih, açıklama) yönetirsiniz.", "Manage contract amendments and addenda (amount delta, date, description)."),
    "Contracts.ContractLine": ("Sözleşmenin kalem bazında (açıklama, miktar, birim fiyat) detaylarını yönetirsiniz.", "Manage a contract's line-level details (description, quantity, unit price)."),
    "Contracts.ContractParty": ("Bir sözleşmedeki tarafları (iş ortağı + rol) tanımlarsınız.", "Define the parties of a contract (business partner + role)."),
    # ---- Core ----
    "Core.AuditLog": ("Sistemdeki tüm API isteklerinin denetim kayıtlarını (kullanıcı, IP, durum kodu, süre, hata) salt-okunur listelersiniz. Güvenlik denetimi ve sorun analizi için kullanılır.", "View read-only audit records of all API requests (user, IP, status code, duration, error). Used for security auditing and troubleshooting."),
    "Core.Branch": ("Şirketlere bağlı şube kayıtlarını yönetirsiniz.", "Manage branch records belonging to companies."),
    "Core.Company": ("Sistemdeki şirket (tüzel kişilik) ana kayıtlarını (kod, vergi no, ana para birimi) yönetirsiniz. Çok şirketli yapının temelidir.", "Manage the master records of companies/legal entities (code, tax no, base currency). The foundation of the multi-company structure."),
    "Core.Currency": ("Para birimlerini (kod, sembol) yönetirsiniz; tüm tutar alanlarında kullanılır.", "Manage currencies (code, symbol); used across all amount fields."),
    "Core.Department": ("Şirket departmanlarını hiyerarşik olarak ve yöneticisiyle birlikte yönetirsiniz.", "Manage company departments hierarchically and with their manager."),
    "Core.ExchangeRate": ("Para birimleri için tarih bazlı döviz kurlarını yönetirsiniz; çok para birimli işlemlerde dönüşümde kullanılır.", "Manage date-based exchange rates for currencies; used for conversion in multi-currency transactions."),
    "Core.LocalizationResource": ("Arayüz çevirilerini (anahtar, kültür, değer) yönetirsiniz; metinleri ekleyip güncelleyebilirsiniz.", "Manage UI translations (key, culture, value); add and update interface texts."),
    "Core.SequenceDefinition": ("Belge numaralandırma kurallarını (önek, format, dolgu, sıradaki numara) yönetirsiniz.", "Manage document numbering rules (prefix, format, padding, next number)."),
    "Core.SystemSetting": ("Sistem genel ayarlarını (anahtar/değer, kategori) yönetirsiniz.", "Manage global system settings (key/value, category)."),
    "Core.UnitConversion": ("Genel ölçü birimi dönüşüm katsayılarını yönetirsiniz.", "Manage global unit-of-measure conversion factors."),
    "Core.UnitOfMeasure": ("Ölçü birimlerini (kod, sembol) yönetirsiniz; malzeme ve stok işlemlerinde kullanılır.", "Manage units of measure (code, symbol); used in material and stock transactions."),
    # ---- Documents ----
    "Documents.Document": ("Belge kartlarını (klasör, ad, durum, güncel sürüm) yönetirsiniz. Doküman yönetiminin ana kaydıdır.", "Manage document cards (folder, name, status, current version). The main record of document management."),
    "Documents.DocumentFolder": ("Belge klasörlerini hiyerarşik olarak yönetirsiniz.", "Manage document folders hierarchically."),
    "Documents.DocumentPermission": ("Belgelere kullanıcı/rol bazlı erişim yetkilerini yönetirsiniz.", "Manage user/role based access permissions to documents."),
    "Documents.DocumentRelation": ("Belgelerin ilgili modül ve kayıtlarla ilişkilerini yönetirsiniz.", "Manage relations between documents and related modules/records."),
    "Documents.DocumentVersion": ("Bir belgenin sürümlerini (dosya adı, boyut, yükleme tarihi) yönetirsiniz.", "Manage a document's versions (file name, size, upload date)."),
    # ---- FieldOperations ----
    "FieldOperations.DailySiteReport": ("Şantiye günlük raporlarını (tarih, hava durumu, notlar, durum) yönetirsiniz. Sahadaki günlük ilerleme ile işçi, ekipman ve malzeme kullanımının kaydıdır.", "Manage daily site reports (date, weather, notes, status). The record of daily field progress and labor, equipment and material usage."),
    "FieldOperations.DailySiteReportEquipment": ("Günlük şantiye raporuna bağlı ekipman çalışma saatlerini yönetirsiniz.", "Manage equipment working hours tied to a daily site report."),
    "FieldOperations.DailySiteReportMaterial": ("Günlük şantiye raporuna bağlı malzeme kullanımını yönetirsiniz.", "Manage material usage tied to a daily site report."),
    "FieldOperations.DailySiteReportWorker": ("Günlük şantiye raporuna bağlı işçi/personel çalışma saatlerini yönetirsiniz.", "Manage worker/personnel working hours tied to a daily site report."),
    "FieldOperations.MeasurementSheet": ("Metraj/ölçüm sayfalarını (proje, sözleşme, tarih, durum) yönetirsiniz. Hakediş hesaplarının dayanağıdır.", "Manage measurement sheets (project, contract, date, status). The basis for progress payment calculations."),
    "FieldOperations.MeasurementSheetLine": ("Metraj sayfasının kalem kalem ölçümlerini (açıklama, miktar, birim fiyat) yönetirsiniz.", "Manage a measurement sheet's line-by-line measurements (description, quantity, unit price)."),
    "FieldOperations.ProgressEntry": ("Proje ve faz bazında fiziksel ilerleme girişlerini (yüzde, miktar, tarih) yönetirsiniz.", "Manage physical progress entries per project and phase (percentage, quantity, date)."),
    # ---- Finance ----
    "Finance.Collection": ("Müşterilerden yapılan tahsilatları (tutar, finansal hesap, tarih, durum) yönetirsiniz.", "Manage collections received from customers (amount, financial account, date, status)."),
    "Finance.CollectionAllocation": ("Bir tahsilatın hangi alacaklara mahsup edildiğini yönetirsiniz.", "Manage which receivables a collection is allocated to."),
    "Finance.CostCenter": ("Masraf merkezlerini hiyerarşik olarak yönetirsiniz. Maliyet dağıtımı ve bütçenin temelidir.", "Manage cost centers hierarchically. The basis for cost distribution and budgeting."),
    "Finance.FinancialAccount": ("Kasa ve banka gibi finansal hesapları (tür, para birimi) yönetirsiniz.", "Manage financial accounts such as cash and bank (type, currency)."),
    "Finance.FinancialTransaction": ("Genel finansal hareketleri (gelir/gider türü, hesap, tutar, ilişkili kayıt) yönetirsiniz.", "Manage general financial transactions (income/expense type, account, amount, related record)."),
    "Finance.FinancialTransactionLine": ("Bir finansal hareketin masraf merkezi/proje bazında detay satırlarını yönetirsiniz.", "Manage a financial transaction's detail lines per cost center/project."),
    "Finance.Payable": ("Tedarikçilere olan borçları (tutar, kalan, vade, ilişkili kayıt) yönetir ve izlersiniz.", "Manage and track payables to suppliers (amount, remaining, due date, related record)."),
    "Finance.Payment": ("Tedarikçilere yapılan ödemeleri (tutar, hesap, tarih, durum) yönetirsiniz.", "Manage payments made to suppliers (amount, account, date, status)."),
    "Finance.PaymentAllocation": ("Bir ödemenin hangi borçlara mahsup edildiğini yönetirsiniz.", "Manage which payables a payment is allocated to."),
    "Finance.Receivable": ("Müşterilerden alacakları (tutar, kalan, vade, ilişkili kayıt) yönetir ve izlersiniz.", "Manage and track receivables from customers (amount, remaining, due date, related record)."),
    # ---- HR ----
    "HR.Timesheet": ("Personel puantaj dönemlerini (dönem başı/sonu, durum) yönetirsiniz. İşçilik saatlerinin toplandığı belgedir.", "Manage personnel timesheet periods (period start/end, status). The document where labor hours are collected."),
    "HR.TimesheetLine": ("Puantajın çalışan, proje ve gün bazında normal/mesai saatlerini yönetirsiniz.", "Manage a timesheet's normal/overtime hours per employee, project and day."),
    # ---- Inventory ----
    "Inventory.StockBalance": ("Depo × malzeme bazında anlık stok bakiyelerini (miktar, rezerve, toplam maliyet) salt-okunur izlersiniz.", "View read-only current stock balances per warehouse × material (quantity, reserved, total cost)."),
    "Inventory.StockCount": ("Stok sayım belgelerini (depo, sayım no, tarih, durum) yönetirsiniz.", "Manage stock count documents (warehouse, count no, date, status)."),
    "Inventory.StockCountLine": ("Sayım belgesinde malzeme bazında sistem ve sayılan miktar farklarını yönetirsiniz.", "Manage system vs. counted quantity differences per material on a count document."),
    "Inventory.StockDocument": ("Giriş, çıkış ve transfer stok belgelerini (tür, depo, tarih, durum) yönetirsiniz.", "Manage inbound, outbound and transfer stock documents (type, warehouse, date, status)."),
    "Inventory.StockDocumentLine": ("Stok belgesinin malzeme kalemlerini (miktar, birim fiyat, para birimi) yönetirsiniz.", "Manage a stock document's material lines (quantity, unit price, currency)."),
    "Inventory.StockDocumentType": ("Stok belge türlerini (yön: giriş/çıkış) tanımlarsınız.", "Define stock document types (direction: in/out)."),
    "Inventory.StockIssueAllocation": ("Stok çıkışında hangi partilerden (lot) ne kadar düşüldüğünü FIFO mantığıyla izlersiniz.", "Track which lots a stock issue was drawn from and how much, following FIFO."),
    "Inventory.StockLot": ("Parti/lot bazında stok kayıtlarını (başlangıç/kalan miktar, birim maliyet, giriş tarihi) yönetirsiniz.", "Manage stock records per lot (initial/remaining quantity, unit cost, received date)."),
    "Inventory.StockReservation": ("Stok rezervasyonlarını (depo, malzeme, miktar, ilişkili kayıt) yönetirsiniz.", "Manage stock reservations (warehouse, material, quantity, related record)."),
    "Inventory.StockTransaction": ("Tüm stok hareketlerini (belge, lot, miktar, birim maliyet, tarih) salt-okunur izlersiniz.", "View read-only all stock transactions (document, lot, quantity, unit cost, date)."),
    "Inventory.Warehouse": ("Depoları (şirket, şube, proje, tür) yönetirsiniz.", "Manage warehouses (company, branch, project, type)."),
    "Inventory.WarehouseLocation": ("Depo içi raf/lokasyonları hiyerarşik olarak yönetirsiniz.", "Manage in-warehouse shelves/locations hierarchically."),
    "Inventory.WarehouseTransfer": ("Depolar arası transfer belgelerini (kaynak/hedef depo, tarih, durum) yönetirsiniz.", "Manage warehouse-to-warehouse transfer documents (source/target warehouse, date, status)."),
    "Inventory.WarehouseTransferLine": ("Transfer belgesinin malzeme kalemlerini (miktar) yönetirsiniz.", "Manage a transfer document's material lines (quantity)."),
    # ---- Notifications ----
    "Notifications.Notification": ("Sistem bildirimlerini (başlık, içerik, tür, ilişkili kayıt) yönetir ve görüntülersiniz.", "Manage and view system notifications (title, body, type, related record)."),
    "Notifications.NotificationPreference": ("Kullanıcı bazlı bildirim tercihlerini (uygulama içi / e-posta) yönetirsiniz.", "Manage per-user notification preferences (in-app / email)."),
    "Notifications.NotificationRecipient": ("Bildirim alıcılarının okundu/okunma tarihi durumunu yönetirsiniz.", "Manage notification recipients' read status and read time."),
    # ---- Operations ----
    "Operations.WorkOrder": ("İş emirlerini (tür, proje, faz, lokasyon, durum, plan tarihleri) yönetirsiniz. Saha operasyonlarının merkezidir; atama, malzeme ve kontrol listeleri buna bağlanır.", "Manage work orders (type, project, phase, location, status, planned dates). The center of field operations; assignments, materials and checklists link to it."),
    "Operations.WorkOrderAssignment": ("İş emrine atanan personel ve kullanıcıları (rol) yönetirsiniz.", "Manage personnel and users assigned to a work order (role)."),
    "Operations.WorkOrderChecklist": ("İş emrine bağlı kontrol listelerini yönetirsiniz.", "Manage checklists tied to a work order."),
    "Operations.WorkOrderChecklistItem": ("Kontrol listesi maddelerini (açıklama, zorunlu, tamamlandı) yönetirsiniz.", "Manage checklist items (description, required, completed)."),
    "Operations.WorkOrderMaterialPlan": ("İş emri için planlanan malzeme miktarlarını yönetirsiniz.", "Manage planned material quantities for a work order."),
    "Operations.WorkOrderMaterialUsage": ("İş emrinde fiilen kullanılan malzemeleri yönetirsiniz.", "Manage materials actually used on a work order."),
    "Operations.WorkOrderStatusHistory": ("İş emrinin durum değişiklik geçmişini (önceki/sonraki durum, tarih) salt-okunur izlersiniz.", "View read-only the work order's status change history (from/to status, date)."),
    "Operations.WorkOrderType": ("İş emri türlerini tanımlarsınız.", "Define work order types."),
    # ---- Organization ----
    "Organization.Employee": ("Çalışan ana kayıtlarını (şirket, departman, pozisyon, kimlik, işe giriş/çıkış) yönetirsiniz. İK ve puantaj işlemlerinin temelidir.", "Manage employee master records (company, department, position, identity, hire/termination). The basis for HR and timesheet operations."),
    "Organization.EmployeePosition": ("Pozisyon ve unvanları tanımlarsınız.", "Define positions and titles."),
    "Organization.EmployeeSkill": ("Yetkinlik (beceri) tanımlarını yönetirsiniz.", "Manage skill definitions."),
    "Organization.EmployeeSkillAssignment": ("Çalışanlara yetkinlik atamalarını (seviye) yönetirsiniz.", "Manage skill assignments to employees (level)."),
    "Organization.ExpenseClaim": ("Personel masraf taleplerini (çalışan, proje, tutar, durum) yönetirsiniz.", "Manage employee expense claims (employee, project, amount, status)."),
    "Organization.ExpenseClaimLine": ("Masraf talebinin kalemlerini (açıklama, tarih, tutar, kategori) yönetirsiniz.", "Manage an expense claim's lines (description, date, amount, category)."),
    "Organization.LeaveRequest": ("Personel izin taleplerini (tür, tarih aralığı, gün, durum) yönetirsiniz.", "Manage employee leave requests (type, date range, days, status)."),
    # ---- Procurement ----
    "Procurement.PurchaseOrder": ("Satınalma siparişlerini (tedarikçi, proje, sipariş no, para birimi, durum) yönetirsiniz.", "Manage purchase orders (supplier, project, order no, currency, status)."),
    "Procurement.PurchaseOrderLine": ("Satınalma siparişinin kalemlerini (malzeme, miktar, birim fiyat, teslim alınan miktar) yönetirsiniz.", "Manage a purchase order's lines (material, quantity, unit price, received quantity)."),
    "Procurement.PurchaseReceipt": ("Mal kabul/irsaliye belgelerini (tedarikçi, sipariş, depo, tarih, durum) yönetirsiniz.", "Manage goods receipt documents (supplier, order, warehouse, date, status)."),
    "Procurement.PurchaseReceiptLine": ("Mal kabul belgesinin kalemlerini (malzeme, miktar, birim fiyat) yönetirsiniz.", "Manage a goods receipt's lines (material, quantity, unit price)."),
    "Procurement.SupplierInvoice": ("Tedarikçi faturalarını (sipariş, mal kabul, tarih, toplam tutar, durum) yönetirsiniz.", "Manage supplier invoices (order, receipt, date, total amount, status)."),
    "Procurement.SupplierInvoiceLine": ("Tedarikçi faturasının kalemlerini (malzeme, miktar, birim fiyat, vergi) yönetirsiniz.", "Manage a supplier invoice's lines (material, quantity, unit price, tax)."),
    "Procurement.SupplierQuote": ("Tedarikçi tekliflerini (teklif no, tarih, ödeme vadesi, durum, seçildi) yönetirsiniz. Teklif karşılaştırması için kullanılır.", "Manage supplier quotes (quote no, date, payment term, status, selected). Used for quote comparison."),
    "Procurement.SupplierQuoteLine": ("Tedarikçi teklifinin kalemlerini (malzeme, miktar, fiyat, iskonto, teslim süresi) yönetirsiniz.", "Manage a supplier quote's lines (material, quantity, price, discount, delivery days)."),
    # ---- ProgressPayments ----
    "ProgressPayments.ProgressPayment": ("Hakediş belgelerini (sözleşme, dönem, brüt/net tutar, kesinti, durum) yönetirsiniz.", "Manage progress payment documents (contract, period, gross/net amount, deduction, status)."),
    "ProgressPayments.ProgressPaymentDeduction": ("Hakedişten yapılan kesintileri (tür, tutar, not) yönetirsiniz.", "Manage deductions on a progress payment (type, amount, note)."),
    "ProgressPayments.ProgressPaymentLine": ("Hakediş kalemlerini (sözleşme/metraj kalemi, miktar, birim fiyat, tutar) yönetirsiniz.", "Manage progress payment lines (contract/measurement line, quantity, unit price, amount)."),
    # ---- Projects ----
    "Projects.Project": ("Proje ana kayıtlarını (şirket, tür, durum, müşteri, yönetici, tarih) yönetirsiniz. Tüm operasyon ve maliyetlerin bağlandığı merkezdir.", "Manage project master records (company, type, status, customer, manager, dates). The hub all operations and costs link to."),
    "Projects.ProjectLocation": ("Proje içi lokasyonları hiyerarşik olarak yönetirsiniz.", "Manage in-project locations hierarchically."),
    "Projects.ProjectMember": ("Proje ekibini (kullanıcı/çalışan, rol) yönetirsiniz.", "Manage the project team (user/employee, role)."),
    "Projects.ProjectNote": ("Projeye ait notları (başlık, içerik) yönetirsiniz.", "Manage notes belonging to a project (title, body)."),
    "Projects.ProjectPhas": ("Proje fazlarını (hiyerarşi, ilerleme yüzdesi) yönetirsiniz.", "Manage project phases (hierarchy, progress percentage)."),
    "Projects.ProjectStatus": ("Proje durum tanımlarını (kod, sıra, kapalı durum) yönetirsiniz.", "Manage project status definitions (code, order, closed state)."),
    "Projects.ProjectType": ("Proje türlerini tanımlarsınız.", "Define project types."),
    # ---- Reporting ----
    "Reporting.DashboardWidget": ("Gösterge paneli (dashboard) widget tanımlarını (kod, modül, tür, yetki, sıra) yönetirsiniz.", "Manage dashboard widget definitions (code, module, type, permission, order)."),
    "Reporting.ReportDefinition": ("Rapor tanımlarını (kod, modül, sorgu anahtarı, gerekli yetki) yönetirsiniz.", "Manage report definitions (code, module, query key, required permission)."),
    # ---- Requests ----
    "Requests.Request": ("Talep (satınalma/malzeme) kayıtlarını (tür, proje, talep eden, durum) yönetirsiniz. Satınalma sürecinin başlangıç belgesidir.", "Manage request (purchase/material) records (type, project, requester, status). The starting document of the procurement process."),
    "Requests.RequestLine": ("Talep kalemlerini (malzeme, miktar, ölçü birimi, not) yönetirsiniz.", "Manage request lines (material, quantity, unit, note)."),
    "Requests.RequestType": ("Talep türlerini (kod, ad, kategori) tanımlarsınız.", "Define request types (code, name, category)."),
    # ---- Workflow ----
    "Workflow.ApprovalAction": ("Onay akışında yapılan aksiyonları (kim, ne zaman, tür, not) salt-okunur izlersiniz.", "View read-only actions taken in an approval flow (who, when, type, note)."),
    "Workflow.ApprovalCondition": ("Onay akışının hangi koşullarda devreye gireceğini (alan, operatör, değer) yönetirsiniz.", "Manage the conditions under which an approval flow triggers (field, operator, value)."),
    "Workflow.ApprovalDefinition": ("Onay akışı tanımlarını (kod, ilgili modül/varlık) yönetirsiniz.", "Manage approval flow definitions (code, related module/entity)."),
    "Workflow.ApprovalDefinitionVersion": ("Onay tanımının sürümlerini (sürüm no, yürürlük tarihleri) yönetirsiniz.", "Manage an approval definition's versions (version no, effective dates)."),
    "Workflow.ApprovalDelegation": ("Onay yetkisi devirlerini (vekalet: devreden/devralan, tarih aralığı) yönetirsiniz.", "Manage approval delegations (delegator/delegate, date range)."),
    "Workflow.ApprovalRequest": ("Onay isteklerini (tanım sürümü, ilgili kayıt, durum, mevcut adım) yönetir ve izlersiniz.", "Manage and track approval requests (definition version, related record, status, current step)."),
    "Workflow.ApprovalRequestApprover": ("Bir onay isteğindeki onaycıları ve durumlarını salt-okunur izlersiniz.", "View read-only the approvers of an approval request and their status."),
    "Workflow.ApprovalRequestStep": ("Onay isteğinin adımlarını (adım no, durum, mod) izlersiniz.", "View an approval request's steps (step no, status, mode)."),
    "Workflow.ApprovalStepApprover": ("Onay adımı onaycı tanımlarını (onaycı türü: kullanıcı/rol/departman) yönetirsiniz.", "Manage approval step approver definitions (approver type: user/role/department)."),
    "Workflow.ApprovalStepDefinition": ("Onay adım tanımlarını (adım no, onay modu, gerekli onay sayısı) yönetirsiniz.", "Manage approval step definitions (step no, approval mode, required approval count)."),
    # ---- Process screens ----
    "Finance.Processes.PaymentAllocation": ("Yapılan ödemeleri açık tedarikçi borçlarına toplu olarak mahsup ettiğiniz operasyonel süreç ekranıdır.", "An operational process screen where you allocate payments to open supplier payables in bulk."),
    "Finance.Processes.ProgressPaymentPosting": ("Onaylanmış hakedişleri finansal kayıtlara ve borç/alacaklara işleyen operasyonel süreç ekranıdır.", "An operational process screen that posts approved progress payments into financial records and payables/receivables."),
    "Finance.Processes.TimesheetCost": ("Onaylı puantajları çalışan başına işçilik maliyetine dönüştürüp ilgili proje/masraf merkezine yansıtan süreç ekranıdır.", "A process screen that converts approved timesheets into labor cost per employee and posts it to the related project/cost center."),
    "Inventory.Processes.StockIssue": ("Depodan FIFO (ilk giren ilk çıkar) mantığıyla parti bazlı stok çıkışı yaptığınız operasyonel süreç ekranıdır.", "An operational process screen where you issue stock from a warehouse on a lot basis using FIFO."),
    "Inventory.Processes.StockTransfer": ("İki depo arasında stok transferini parti ve maliyetiyle birlikte yürüttüğünüz süreç ekranıdır.", "A process screen where you run stock transfer between two warehouses, carrying lots and cost."),
    "Procurement.Processes.GoodsReceipt": ("Satınalma siparişine karşı gelen malların kabulünü ve stok girişini yaptığınız operasyonel süreç ekranıdır.", "An operational process screen where you receive goods against a purchase order and post the stock-in."),
    "Workflow.Processes.Approval": ("Size düşen bekleyen onayları görüntüleyip onaylama, reddetme veya iade işlemini yürüttüğünüz süreç ekranıdır.", "A process screen where you view your pending approvals and approve, reject or return them."),
    # ---- Report screens ----
    "Finance.Reports.PayableAging": ("Tedarikçi borçlarının vade/yaşlandırma dökümünü filtreleyip dışa aktardığınız rapor ekranıdır.", "A report screen where you filter and export the aging breakdown of supplier payables."),
    "Finance.Reports.ReceivableAging": ("Müşteri alacaklarının vade/yaşlandırma dökümünü filtreleyip dışa aktardığınız rapor ekranıdır.", "A report screen where you filter and export the aging breakdown of customer receivables."),
    "HR.Reports.TimesheetSummary": ("Puantaj saatlerinin dönem ve proje bazında özetini sunan rapor ekranıdır.", "A report screen presenting the summary of timesheet hours by period and project."),
    "Inventory.Reports.StockBalanceReport": ("Depo ve malzeme bazında güncel stok bakiyelerini sunan rapor ekranıdır.", "A report screen presenting current stock balances by warehouse and material."),
    "Procurement.Reports.PurchaseOrderSummary": ("Satınalma siparişlerinin tedarikçi/durum bazında özetini sunan rapor ekranıdır.", "A report screen presenting purchase orders summarized by supplier/status."),
    "ProgressPayments.Reports.ProgressPaymentSummary": ("Hakedişlerin sözleşme ve dönem bazında özetini sunan rapor ekranıdır.", "A report screen presenting progress payments summarized by contract and period."),
    "Projects.Reports.ProjectStatusReport": ("Projelerin durum ve ilerleme özetini sunan rapor ekranıdır.", "A report screen presenting the status and progress summary of projects."),
}

KIND_TITLE = ("Ekran Türü", "Screen Type")


def upsert(txt, name, value):
    """Replace existing data value or append before </root>."""
    pat = re.compile(r'(<data name="' + re.escape(name) + r'"[^>]*>\s*<value>).*?(</value>)', re.S)
    if pat.search(txt):
        return pat.sub(lambda m: m.group(1) + value + m.group(2), txt, count=1)
    block = f'  <data name="{name}" xml:space="preserve">\n    <value>{value}</value>\n  </data>\n'
    return txt.replace("</root>", block + "</root>")


def run(fn, idx, kind_idx):
    path = os.path.join(BASE, fn)
    txt = open(path, encoding="utf-8").read()
    n = 0
    for sid, vals in DESC.items():
        txt = upsert(txt, f"{sid}.Help.Intro", vals[idx]); n += 1
        txt = upsert(txt, f"{sid}.Help.ScreenKindTitle", KIND_TITLE[kind_idx]); n += 1
    open(path, "w", encoding="utf-8").write(txt)
    print(f"{fn}: {n} upserts ({len(DESC)} screens)")


run("SharedResource.tr-TR.resx", 0, 0)   # Turkish
run("SharedResource.en-US.resx", 1, 1)   # English
run("SharedResource.resx", 1, 1)         # neutral -> English
print("Done.")

