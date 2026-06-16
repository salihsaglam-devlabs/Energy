/*
 * Modules sayfası — kurumsal modüllerin generic DevExtreme CRUD ekranı.
 *
 * Sorumluluk:
 *   - /m/{module}/list, POST/PUT/DELETE /m/{module} uç noktalarına bağlı dxDataGrid.
 *   - Sütunları yüklenen veriden otomatik üretir; denetim/Id alanlarını gizler.
 *   - Oluşturma/güncelleme/silme yetkilerini AppAuth.can("<Module>.<Action>") ile sınırlar.
 *   - .xlsx dışa aktarım.
 *
 * Genel API: window.AppPages.Modules.init(routeModule, permModule).
 */
(function (window, $) {
    "use strict";

    var LG = function () { return (window.AppL10n && window.AppL10n.grid) || {}; };
    var LN = function () { return (window.AppL10n && window.AppL10n.notifications) || {}; };
    var LA = function () { return (window.AppL10n && window.AppL10n.moduleActions) || {}; };
    var LD = function () { return (window.AppL10n && window.AppL10n.moduleDetails) || {}; };

    // Düzenleme ve listede gizlenecek teknik alanlar.
    var HIDDEN_FIELDS = [
        "createdAt", "createdBy", "updatedAt", "updatedBy",
        "isDeleted", "deletedAt", "deletedBy"
    ];

    // Modül başına iş kuralı satır eylemleri. Her eylem yalnızca kullanıcıda ilgili
    // yetki (AppAuth.can) varsa görünür; sunucu tarafında da uç nokta-permission
    // eşlemesiyle ayrıca korunur. "note": onay/ret gibi açıklama isteyen eylemler.
    var ROW_ACTIONS = {
        workflow: [
            { key: "approve", label: "approve", perm: "Workflow.Approve", icon: "check", note: true },
            { key: "reject", label: "reject", perm: "Workflow.Reject", icon: "close", note: true },
            { key: "return", label: "return", perm: "Workflow.Return", icon: "undo", note: true },
            { key: "cancel", label: "cancel", perm: "Workflow.Update", icon: "clearformat", note: true }
        ],
        inventory: [
            { key: "reverse", label: "reverse", perm: "Inventory.Reverse", icon: "revert" }
        ],
        procurement: [
            { key: "receive", label: "receive", perm: "Procurement.Approve", icon: "box" }
        ],
        operations: [
            { key: "close", label: "close", perm: "Operations.Update", icon: "todo" },
            { key: "reopen", label: "reopen", perm: "Operations.Update", icon: "refresh" }
        ],
        catalog: [
            { key: "activate", label: "activate", perm: "Catalog.Update", icon: "isnotblank" },
            { key: "validate", label: "validate", perm: "Catalog.Read", icon: "search", method: "get" }
        ]
    };

    // Modül başına ana-detay (master-detail) alt-koleksiyonları. Her giriş bir başlık
    // satırı genişletildiğinde gösterilecek satır grid'ini tanımlar: key = API/proxy detay
    // segmenti, label = AppL10n.moduleDetails çevirisi. Tek koleksiyon doğrudan, birden
    // fazla koleksiyon sekme paneli (dxTabPanel) içinde gösterilir.
    var DETAILS = {
        requests: [{ key: "request-lines", label: "requestLines" }],
        procurement: [{ key: "purchase-order-lines", label: "purchaseOrderLines" }],
        operations: [
            { key: "work-order-assignments", label: "workOrderAssignments" },
            { key: "work-order-material-plans", label: "workOrderMaterialPlans" },
            { key: "work-order-checklists", label: "workOrderChecklists" },
            { key: "work-order-status-histories", label: "workOrderStatusHistories", readonly: true }
        ],
        "field-operations": [
            { key: "daily-site-report-workers", label: "dailySiteReportWorkers" },
            { key: "daily-site-report-equipments", label: "dailySiteReportEquipments" },
            { key: "daily-site-report-materials", label: "dailySiteReportMaterials" }
        ],
        hr: [{ key: "timesheet-lines", label: "timesheetLines" }],
        assets: [
            { key: "equipment-assignments", label: "equipmentAssignments" },
            { key: "equipment-maintenances", label: "equipmentMaintenances" }
        ],
        finance: [{ key: "financial-transaction-lines", label: "financialTransactionLines" }],
        budget: [{ key: "budget-lines", label: "budgetLines" }],
        contracts: [
            { key: "contract-lines", label: "contractLines" },
            { key: "contract-parties", label: "contractParties" },
            { key: "contract-amendments", label: "contractAmendments" }
        ],
        "progress-payments": [
            { key: "progress-payment-lines", label: "progressPaymentLines" },
            { key: "progress-payment-deductions", label: "progressPaymentDeductions" }
        ],
        catalog: [
            { key: "material-attribute-values", label: "materialAttributeValues" },
            { key: "material-unit-conversions", label: "materialUnitConversions" }
        ],
        inventory: [{ key: "warehouse-locations", label: "warehouseLocations" }]
    };

    function notifyError(message) {
        if (window.AppNotify && window.AppNotify.error) { window.AppNotify.error(message || (LN().failed || "İşlem başarısız")); }
    }
    function notifySuccess(message) {
        if (window.AppNotify && window.AppNotify.success) { window.AppNotify.success(message || (LA().succeeded || "Tamamlandı")); }
    }

    // Onay diyaloğu (DevExtreme) — söz (promise) döndürür.
    function confirmAction() {
        var a = LA();
        if (DevExpress && DevExpress.ui && DevExpress.ui.dialog) {
            return DevExpress.ui.dialog.confirm(a.confirmMessage || "Bu işlemi onaylıyor musunuz?", a.confirmTitle || "Onay");
        }
        return Promise.resolve(window.confirm(a.confirmMessage || "Bu işlemi onaylıyor musunuz?"));
    }

    // Bir satır eylemini yürütür: onay → (opsiyonel açıklama) → API → grid yenile.
    function runAction(base, action, row, grid) {
        var id = row.id;
        if (!id) { return; }

        confirmAction().then(function (ok) {
            if (!ok) { return; }

            if (action.method === "get") {
                window.AppHttp.get(base + "/action/" + action.key + "/" + id)
                    .then(function (res) { handleValidation(res); })
                    .catch(function (err) { notifyError(err && err.message); });
                return;
            }

            var body = null;
            if (action.note) {
                var note = window.prompt(LA().notePrompt || "Açıklama (opsiyonel)", "");
                if (note === null) { return; } // kullanıcı vazgeçti
                body = { note: note };
            }

            window.AppHttp.post(base + "/action/" + action.key + "/" + id, body)
                .then(function (res) {
                    if (res && res.success === false) { notifyError(res.message); return; }
                    notifySuccess(res && res.message);
                    grid.refresh();
                })
                .catch(function (err) { notifyError(err && err.message); });
        });
    }

    // Catalog doğrulama (GET) sonucunu gösterir: data, eksik öznitelik mesajlarının dizisidir.
    function handleValidation(res) {
        var a = LA();
        if (res && res.success === false) { notifyError(res.message); return; }
        var issues = (res && res.data) || [];
        if (!issues.length) { notifySuccess(a.validationOk || "Doğrulama başarılı."); return; }
        if (window.AppNotify && window.AppNotify.warning) {
            window.AppNotify.warning((a.validationIssues || "Eksik öznitelikler:") + " " + issues.join(", "));
        }
    }

    // Modül için yetkilendirilmiş satır eylemlerinden bir "buttons" sütunu üretir.
    function buildActionColumn(routeModule, base, auth) {
        var defs = ROW_ACTIONS[routeModule];
        if (!defs || !defs.length) { return null; }

        var allowed = defs.filter(function (d) { return auth.can(d.perm); });
        if (!allowed.length) { return null; }

        var buttons = allowed.map(function (d) {
            return {
                hint: (LA()[d.label] || d.key),
                icon: d.icon,
                onClick: function (e) {
                    runAction(base, d, e.row.data, e.component);
                }
            };
        });

        return {
            type: "buttons",
            caption: (LA().column || (LG().actions) || "İşlemler"),
            width: Math.max(80, allowed.length * 42),
            fixed: true,
            fixedPosition: "right",
            buttons: buttons
        };
    }

    // Bir başlık satırının alt-koleksiyonu için dxDataGrid çizer. Salt-okunur koleksiyonlar
    // (def.readonly) veya yetki yoksa düzenleme kapalıdır; aksi halde ana modülün
    // Create/Update/Delete yetkisine göre satır ekleme/düzenleme/silme açılır.
    function renderDetailGrid(host, base, def, parentId, permModule, auth) {
        var detailPath = base + "/details/" + def.key;
        var writable = !def.readonly;
        var canCreate = writable && auth.can(permModule + ".Create");
        var canUpdate = writable && auth.can(permModule + ".Update");
        var canDelete = writable && auth.can(permModule + ".Delete");

        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    parentId: parentId,
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20
                });
                return window.AppHttp.get(detailPath + "?" + params);
            },
            insert: function (values) {
                return window.AppHttp.post(detailPath + "?parentId=" + encodeURIComponent(parentId), values);
            },
            update: function (key, values) {
                return window.AppHttp.put(detailPath + "/" + key, values);
            },
            remove: function (key) {
                return window.AppHttp.del(detailPath + "/" + key);
            }
        });

        host.dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true },
            showBorders: true,
            columnAutoWidth: true,
            columnHidingEnabled: true,
            hoverStateEnabled: true,
            rowAlternationEnabled: true,
            paging: { pageSize: 10 },
            pager: { visible: true, allowedPageSizes: [10, 20], showPageSizeSelector: true, showInfo: true },
            noDataText: (LG().noData || "Kayıt yok"),
            loadPanel: { enabled: true, text: (LG().loading || "Yükleniyor...") },
            editing: {
                mode: "popup",
                allowAdding: canCreate,
                allowUpdating: canUpdate,
                allowDeleting: canDelete,
                useIcons: true,
                popup: { showTitle: true, width: "min(92vw, 680px)", height: "auto" }
            },
            // DevExtreme yalnızca değişen alanları gönderir; veri kaybını önlemek için
            // güncellemede eski satırla birleştirip tam nesneyi gönderiyoruz.
            onRowUpdating: function (e) {
                e.newData = $.extend({}, e.oldData, e.newData);
            },
            customizeColumns: function (columns) {
                columns.forEach(function (col) {
                    if (col.dataField === "id") {
                        col.visible = false;
                        col.formItem = { visible: false };
                        col.allowEditing = false;
                    } else if (HIDDEN_FIELDS.indexOf(col.dataField) !== -1) {
                        col.visible = false;
                        col.formItem = { visible: false };
                        col.allowEditing = false;
                    }
                });
            },
            onRowInserted: function () { window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); },
            onRowUpdated: function () { window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); },
            onRowRemoved: function () { window.AppNotify && window.AppNotify.success(LN().deleted || "Silindi"); },
            onDataErrorOccurred: function (e) {
                if (window.AppNotify && window.AppNotify.fromHttpError) { window.AppNotify.fromHttpError(e.error); }
            }
        });
    }

    // Modül için ana-detay yapılandırması üretir; alt-koleksiyon yoksa null döner.
    // Tek koleksiyon doğrudan, birden fazla koleksiyon sekme paneliyle gösterilir.
    function buildMasterDetail(routeModule, base, permModule, auth) {
        var defs = DETAILS[routeModule];
        if (!defs || !defs.length) { return null; }

        return {
            enabled: true,
            template: function (container, options) {
                var parentId = options.data && options.data.id;
                if (!parentId) { return; }

                if (defs.length === 1) {
                    renderDetailGrid($("<div>").appendTo(container), base, defs[0], parentId, permModule, auth);
                    return;
                }

                $("<div>").appendTo(container).dxTabPanel({
                    dataSource: defs,
                    deferRendering: true,
                    itemTitleTemplate: function (d) { return (LD()[d.label] || d.key); },
                    itemTemplate: function (d, _index, el) {
                        renderDetailGrid($("<div>").appendTo(el), base, d, parentId, permModule, auth);
                    }
                });
            }
        };
    }

    function exportGrid(e, fileName) {
        if (typeof ExcelJS === "undefined" || typeof saveAs === "undefined" ||
            !DevExpress.excelExporter || !DevExpress.excelExporter.exportDataGrid) { return; }
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet("Data");
        DevExpress.excelExporter.exportDataGrid({ component: e.component, worksheet: worksheet, autoFilterEnabled: true })
            .then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: "application/octet-stream" }), (fileName || "export") + ".xlsx");
                });
            });
        e.cancel = true;
    }

    function init(routeModule, permModule) {
        var base = "/m/" + routeModule;
        var auth = window.AppAuth || { can: function () { return true; } };
        var canCreate = auth.can(permModule + ".Create");
        var canUpdate = auth.can(permModule + ".Update");
        var canDelete = auth.can(permModule + ".Delete");

        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get(base + "/list?" + params);
            },
            insert: function (values) {
                return window.AppHttp.post(base, values);
            },
            update: function (key, values) {
                return window.AppHttp.put(base + "/" + key, values);
            },
            remove: function (key) {
                return window.AppHttp.del(base + "/" + key);
            }
        });

        var actionColumn = buildActionColumn(routeModule, base, auth);
        var masterDetail = buildMasterDetail(routeModule, base, permModule, auth);

        $("#module-grid").dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true, sorting: true },
            masterDetail: masterDetail || { enabled: false },
            showBorders: true,
            headerFilter: { visible: true },
            filterRow: { visible: true },
            showColumnLines: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            allowColumnResizing: true,
            columnResizingMode: "widget",
            columnAutoWidth: true,
            columnHidingEnabled: true,
            width: "100%",
            height: "75vh",
            repaintChangesOnly: true,
            paging: { pageSize: 20 },
            pager: {
                visible: true, allowedPageSizes: [10, 20, 50], showPageSizeSelector: true,
                showInfo: true, showNavigationButtons: true, displayMode: "full"
            },
            searchPanel: { visible: true, placeholder: (LG().search || "Ara..."), width: 240 },
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select", height: 320, search: { enabled: true } },
            loadPanel: { enabled: true, text: (LG().loading || "Yükleniyor...") },
            noDataText: (LG().noData || "Kayıt yok"),
            export: { enabled: true, formats: ["xlsx"] },
            onExporting: function (e) { exportGrid(e, routeModule); },
            editing: {
                mode: "popup",
                allowAdding: canCreate,
                allowUpdating: canUpdate,
                allowDeleting: canDelete,
                useIcons: true,
                popup: { showTitle: true, width: "min(92vw, 720px)", height: "auto" }
            },
            customizeColumns: function (columns) {
                columns.forEach(function (col) {
                    if (col.dataField === "id") {
                        col.visible = false;
                        col.formItem = { visible: false };
                        col.allowEditing = false;
                    } else if (HIDDEN_FIELDS.indexOf(col.dataField) !== -1) {
                        col.visible = false;
                        col.formItem = { visible: false };
                        col.allowEditing = false;
                    }
                });

                // İş kuralı satır eylemleri sütununu (varsa) en sona, sabitlenmiş olarak ekle.
                if (actionColumn) {
                    columns.push(actionColumn);
                }
            },
            onRowInserted: function () { window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); },
            onRowUpdated: function () { window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); },
            onRowRemoved: function () { window.AppNotify && window.AppNotify.success(LN().deleted || "Silindi"); },
            onDataErrorOccurred: function (e) {
                if (window.AppNotify && window.AppNotify.fromHttpError) { window.AppNotify.fromHttpError(e.error); }
            }
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Modules = { init: init };

})(window, window.jQuery);

