/*
 * AppGridL10n — sistem geneli DevExtreme yerelleştirme + alan-tipi + lookup
 * otomasyonu + güvenlik sertleştirmesi.
 *
 * Sözlükler (sunucudan yerel kültüre göre üretilir):
 *   window.AppL10n.captions      : "supplierId" -> "Tedarikçi" / "Supplier"
 *   window.AppL10n.fieldTypes    : "startDate"  -> "datetime"
 *                                  "amount"     -> "number"
 *                                  "isActive"   -> "boolean"
 *                                  "id"         -> "guid"
 *   window.AppL10n.fieldLookups  : "supplierId" -> "/business-partners/.../lookup"
 *
 * Otomatik davranışlar:
 *   #1 Caption        — sözlükteki TR/EN değeri her zaman geçerli kazanır.
 *   #2 Doğru editor   — "datetime" -> dxDateBox, "date" -> dxDateBox,
 *                       "number" -> dxNumberBox, "boolean" -> dxCheckBox,
 *                       grid kolonu için dataType ayarlanır.
 *   #3 Lookup zorlama — "supplierId" gibi FK alanı için kullanıcıya serbest metin
 *                       açılmaz; ilgili tablodan SelectBox/Lookup ile seçtirilir.
 *   #4 Lookup display — ham GUID asla görünmez (displayName/name/code fallback).
 *   #5 Hata mesajı    — [object Object] kaçaklarına son savunma.
 *
 * Çalışma şekli: ekran kodlarına dokunmadan; jQuery plugin'leri + DevExpress
 * defaultOptions üzerinden iki katmanlı yamalanır.
 */
(function (window, $) {
    "use strict";

    var DX = window.DevExpress;
    if (!DX || !$) { return; }

    var L10n = window.AppL10n || {};
    var SERVER_CAPTIONS = L10n.captions || {};
    var FIELD_TYPES = L10n.fieldTypes || {};
    var FIELD_LOOKUPS = L10n.fieldLookups || {};
    var ENUM_VALUES = L10n.enumValues || {};

    // Sayfa-özel override hala mümkün: window.AppCaptions[k] = "..."
    window.AppCaptions = window.AppCaptions || {};
    for (var k in SERVER_CAPTIONS) {
        if (Object.prototype.hasOwnProperty.call(SERVER_CAPTIONS, k) && window.AppCaptions[k] == null) {
            window.AppCaptions[k] = SERVER_CAPTIONS[k];
        }
    }

    function humanize(field) {
        if (!field) { return ""; }
        var s = String(field).replace(/Id$/, "");
        s = s.replace(/([a-z0-9])([A-Z])/g, "$1 $2");
        s = s.replace(/^./, function (c) { return c.toUpperCase(); });
        return s;
    }
    function captionFor(field) {
        if (!field) { return ""; }
        return (window.AppCaptions && window.AppCaptions[field]) || humanize(field);
    }
    function typeFor(field) { return field && FIELD_TYPES[field]; }
    function lookupUrlFor(field) { return field && FIELD_LOOKUPS[field]; }

    // -- Lookup display güvenliği --------------------------------------------
    var DISPLAY_FIELDS = ["displayName", "name", "displayText", "title", "code", "label", "fullName", "userName"];

    function safeDisplay(item) {
        if (item == null) { return ""; }
        if (typeof item !== "object") { return String(item); }
        for (var i = 0; i < DISPLAY_FIELDS.length; i++) {
            var v = item[DISPLAY_FIELDS[i]];
            if (v != null && v !== "") { return String(v); }
        }
        var id = item.id != null ? String(item.id) : "";
        var miss = (L10n.screen && L10n.screen.lookupMissing) || "(unnamed)";
        if (!id) { return miss; }
        var tail = id.length > 8 ? id.substring(id.length - 6) : id;
        return miss + " #" + tail;
    }

    function wrapLookupDisplayExpr(lookup) {
        if (!lookup || typeof lookup !== "object") { return; }
        var orig = lookup.displayExpr;
        if (typeof orig === "function") { return; }
        lookup.displayExpr = function (item) {
            if (item == null) { return ""; }
            if (typeof orig === "string" && orig) {
                var v = item[orig];
                if (v != null && v !== "") { return String(v); }
            }
            return safeDisplay(item);
        };
    }

    // -- Lookup CustomStore üretici (cache'li) -------------------------------
    var lookupCache = {};
    function buildLookupStore(url) {
        if (!url) { return null; }
        if (lookupCache[url]) { return lookupCache[url]; }
        var store = new DX.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () {
                if (window.AppHttp && typeof window.AppHttp.get === "function") {
                    return window.AppHttp.get(url);
                }
                // Geri dönüş: jQuery ile
                return $.getJSON(url);
            }
        });
        lookupCache[url] = store;
        return store;
    }

    function makeLookupConfig(url) {
        var store = buildLookupStore(url);
        if (!store) { return null; }
        var cfg = { dataSource: store, valueExpr: "id", displayExpr: "displayName" };
        wrapLookupDisplayExpr(cfg);
        return cfg;
    }

    // -- DX format yardımcısı: tarih/sayı --------------------------------------
    function applyTypeToColumn(col) {
        if (!col || !col.dataField) { return; }
        var t = typeFor(col.dataField);
        if (!t) { return; }
        if (t === "datetime") {
            if (!col.dataType) { col.dataType = "datetime"; }
            if (!col.format) { col.format = "shortDateShortTime"; }
            col.editorOptions = $.extend({ type: "datetime", displayFormat: "shortDateShortTime", useMaskBehavior: true }, col.editorOptions || {});
        } else if (t === "date") {
            if (!col.dataType) { col.dataType = "date"; }
            if (!col.format) { col.format = "shortDate"; }
            col.editorOptions = $.extend({ type: "date", displayFormat: "shortDate", useMaskBehavior: true }, col.editorOptions || {});
        } else if (t === "number") {
            if (!col.dataType) { col.dataType = "number"; }
            col.editorOptions = $.extend({ showSpinButtons: false }, col.editorOptions || {});
        } else if (t === "boolean") {
            if (!col.dataType) { col.dataType = "boolean"; }
        } else if (t === "guid") {
            // GUID'ler asla serbest metin değil; FK ise lookup, değilse sadece görüntü.
            if (!col.dataType) { col.dataType = "string"; }
            // ID gibi kimlik kolonları varsayılan olarak gizlenir (HIDDEN_FIELDS pattern).
        }
    }

    function applyLookupToColumn(col) {
        if (!col || !col.dataField) { return; }
        if (col.lookup) { wrapLookupDisplayExpr(col.lookup); return; }
        var url = lookupUrlFor(col.dataField);
        if (!url) { return; }
        var cfg = makeLookupConfig(url);
        if (cfg) { col.lookup = cfg; }
    }

    // Enum/durum metni yerelleştirme: hücre değeri enum sözlüğünde birebir varsa
    // (ör. "Approved" -> "Onaylandı") kullanıcıya yerelleştirilmiş metin gösterilir.
    // Yalnızca tam eşleşmeler dönüştürülür; sayı/tarih/lookup kolonları etkilenmez.
    function applyEnumTextToColumn(col) {
        if (!col || !col.dataField) { return; }
        if (col.lookup) { return; }
        var t = typeFor(col.dataField);
        if (t === "date" || t === "datetime" || t === "number" || t === "boolean") { return; }
        if (col.customizeText) { return; }
        col.customizeText = function (cellInfo) {
            var v = cellInfo && cellInfo.value;
            if (v == null) { return ""; }
            var key = String(v);
            if (Object.prototype.hasOwnProperty.call(ENUM_VALUES, key)) {
                return ENUM_VALUES[key];
            }
            return cellInfo.valueText != null ? cellInfo.valueText : key;
        };
    }

    // -- Grid kolonları --------------------------------------------------------
    function enhanceColumns(columns) {
        if (!columns || !columns.length) { return; }
        for (var i = 0; i < columns.length; i++) {
            var col = columns[i];
            if (!col || typeof col !== "object") { continue; }
            // Caption
            if (col.dataField) {
                var dictCap = window.AppCaptions && window.AppCaptions[col.dataField];
                if (dictCap) {
                    col.caption = dictCap;
                } else if (col.caption == null || col.caption === "") {
                    col.caption = humanize(col.dataField);
                }
            }
            // Önce lookup (FK ise) — type belirleme sonrasında bozulmasın.
            applyLookupToColumn(col);
            // Sonra tip + editor (lookup yoksa veya tarih/sayı için)
            if (!col.lookup) { applyTypeToColumn(col); }
            // Son olarak enum/durum metni yerelleştirme (string kolonlar için).
            applyEnumTextToColumn(col);
            // Nested
            if (Array.isArray(col.columns)) { enhanceColumns(col.columns); }
        }
    }

    // -- Form item'ları (recursive) -------------------------------------------
    function applyTypeToFormItem(item) {
        if (!item || !item.dataField) { return; }
        var t = typeFor(item.dataField);
        if (!t) { return; }
        if (t === "datetime") {
            if (!item.editorType) { item.editorType = "dxDateBox"; }
            item.editorOptions = $.extend({ type: "datetime", displayFormat: "shortDateShortTime", useMaskBehavior: true }, item.editorOptions || {});
        } else if (t === "date") {
            if (!item.editorType) { item.editorType = "dxDateBox"; }
            item.editorOptions = $.extend({ type: "date", displayFormat: "shortDate", useMaskBehavior: true }, item.editorOptions || {});
        } else if (t === "number") {
            if (!item.editorType) { item.editorType = "dxNumberBox"; }
        } else if (t === "boolean") {
            if (!item.editorType) { item.editorType = "dxCheckBox"; }
        }
    }

    function applyLookupToFormItem(item) {
        if (!item || !item.dataField) { return; }
        // Zaten SelectBox/Lookup ise dokunma — sadece displayExpr güvenliği uygula.
        if (item.editorType === "dxSelectBox" || item.editorType === "dxLookup" || item.editorType === "dxTagBox") {
            if (item.editorOptions) { wrapLookupDisplayExpr(item.editorOptions); }
            return;
        }
        var url = lookupUrlFor(item.dataField);
        if (!url) { return; }
        item.editorType = "dxSelectBox";
        var cfg = makeLookupConfig(url);
        var base = {
            dataSource: cfg.dataSource,
            valueExpr: cfg.valueExpr,
            displayExpr: cfg.displayExpr,
            searchEnabled: true,
            showClearButton: true
        };
        item.editorOptions = $.extend(base, item.editorOptions || {});
    }

    function enhanceFormItems(items) {
        if (!Array.isArray(items)) { return; }
        for (var i = 0; i < items.length; i++) {
            var it = items[i];
            if (!it || typeof it !== "object") { continue; }
            if (it.itemType === "group" && Array.isArray(it.items)) { enhanceFormItems(it.items); continue; }
            if (it.itemType === "tabbed" && Array.isArray(it.tabs)) {
                for (var t = 0; t < it.tabs.length; t++) {
                    if (it.tabs[t] && Array.isArray(it.tabs[t].items)) { enhanceFormItems(it.tabs[t].items); }
                }
                continue;
            }
            if (it.dataField) {
                // Label
                var dictCap = window.AppCaptions && window.AppCaptions[it.dataField];
                if (dictCap) {
                    it.label = it.label || {};
                    it.label.text = dictCap;
                } else if (!it.label || !it.label.text) {
                    it.label = it.label || {};
                    it.label.text = humanize(it.dataField);
                }
                // Önce lookup (FK ise SelectBox), yoksa tip-tabanlı editor
                applyLookupToFormItem(it);
                if (it.editorType !== "dxSelectBox" && it.editorType !== "dxLookup" && it.editorType !== "dxTagBox") {
                    applyTypeToFormItem(it);
                }
            }
        }
    }

    window.AppGridL10n = {
        enhanceColumns: enhanceColumns,
        enhanceFormItems: enhanceFormItems,
        captionFor: captionFor,
        typeFor: typeFor,
        lookupUrlFor: lookupUrlFor,
        buildLookupStore: buildLookupStore,
        makeLookupConfig: makeLookupConfig,
        safeDisplay: safeDisplay
    };

    // ====================================================================
    // KATMAN A — jQuery plugin sarmalayıcıları
    // ====================================================================
    function wrap(name, transform) {
        if (typeof $.fn[name] !== "function") { return; }
        var orig = $.fn[name];
        var wrapped = function (options) {
            if (options && typeof options === "object" && !Array.isArray(options)) {
                try { transform(options); } catch (e) { /* yok say */ }
            }
            return orig.apply(this, arguments);
        };
        for (var p in orig) {
            if (Object.prototype.hasOwnProperty.call(orig, p)) {
                try { wrapped[p] = orig[p]; } catch (e) { /* yok say */ }
            }
        }
        $.fn[name] = wrapped;
    }

    ["dxDataGrid", "dxTreeList"].forEach(function (widget) {
        wrap(widget, function (options) {
            var origCustomize = options.customizeColumns;
            options.customizeColumns = function (columns) {
                if (typeof origCustomize === "function") {
                    try { origCustomize.call(this, columns); } catch (e) { /* yok say */ }
                }
                enhanceColumns(columns);
            };
            if (Array.isArray(options.columns)) { enhanceColumns(options.columns); }
        });
    });

    wrap("dxForm", function (options) {
        if (Array.isArray(options.items)) { enhanceFormItems(options.items); }
    });

    ["dxSelectBox", "dxLookup", "dxTagBox", "dxDropDownBox", "dxAutocomplete"].forEach(function (widget) {
        wrap(widget, function (options) { wrapLookupDisplayExpr(options); });
    });

    // ====================================================================
    // KATMAN B — DX defaultOptions: contentReady'de columnOption ile zorla
    // ====================================================================
    function reCaptionGrid(component) {
        if (!component || typeof component.option !== "function") { return; }
        var cols = component.option("columns");
        if (!cols || !cols.length) { return; }
        for (var i = 0; i < cols.length; i++) {
            var col = cols[i];
            if (!col || !col.dataField) { continue; }
            // Caption
            var cap = window.AppCaptions && window.AppCaptions[col.dataField];
            if (cap && col.caption !== cap) {
                try { component.columnOption(i, "caption", cap); } catch (e) {}
            }
            // dataType (sadece set edilmemişse)
            var t = typeFor(col.dataField);
            if (t && !col.dataType && (t === "date" || t === "datetime" || t === "number" || t === "boolean")) {
                try { component.columnOption(i, "dataType", t); } catch (e) {}
            }
            // Lookup (FK + henüz lookup yoksa)
            var url = lookupUrlFor(col.dataField);
            if (url && !col.lookup) {
                var cfg = makeLookupConfig(url);
                if (cfg) {
                    try { component.columnOption(i, "lookup", cfg); } catch (e) {}
                }
            }
        }
    }

    function attachReCaption(component) {
        if (!component || typeof component.on !== "function") { return; }
        var done = false;
        component.on("contentReady", function () {
            if (done) { return; }
            done = true;
            try { reCaptionGrid(component); } catch (e) {}
            setTimeout(function () { done = false; }, 50);
        });
    }

    if (DX.ui && DX.ui.dxDataGrid && typeof DX.ui.dxDataGrid.defaultOptions === "function") {
        try {
            DX.ui.dxDataGrid.defaultOptions({
                options: { onInitialized: function (e) { attachReCaption(e.component); } }
            });
        } catch (e) {}
    }
    if (DX.ui && DX.ui.dxTreeList && typeof DX.ui.dxTreeList.defaultOptions === "function") {
        try {
            DX.ui.dxTreeList.defaultOptions({
                options: { onInitialized: function (e) { attachReCaption(e.component); } }
            });
        } catch (e) {}
    }

    // ====================================================================
    // KATMAN C — Hata sertleştirme
    // ====================================================================
    var alert = window.AppAlert;
    function coerceMessage(input) {
        if (input == null) { return ""; }
        if (typeof input === "string") { return input; }
        if (alert && typeof alert.toText === "function") { return alert.toText(input); }
        if (typeof input === "object" && typeof input.message === "string") { return input.message; }
        try { return String(input); } catch (e) { return ""; }
    }
    if (DX.ui && typeof DX.ui.notify === "function") {
        var origNotify = DX.ui.notify;
        DX.ui.notify = function (message, type, displayTime) {
            if (message && typeof message === "object" && !("type" in message && "message" in message)) {
                message = coerceMessage(message);
            } else if (message && typeof message === "object" && typeof message.message !== "string") {
                message.message = coerceMessage(message.message);
            }
            return origNotify.apply(this, [message, type, displayTime]);
        };
    }

    window.addEventListener("unhandledrejection", function (ev) {
        var reason = ev && ev.reason;
        if (reason && reason.handled) { return; }
        var msg = coerceMessage(reason);
        if (msg && window.AppAlert) { window.AppAlert.error(msg); }
    });
    window.addEventListener("error", function (ev) {
        if (ev && ev.error && typeof ev.error === "object") {
            var msg = coerceMessage(ev.error);
            if (msg && window.AppAlert) { window.AppAlert.error(msg); }
        }
    });
})(window, window.jQuery);

