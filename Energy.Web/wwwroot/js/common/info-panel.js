/*
 * AppInfo — sayfa rehberi (Bilgi / Info) bileşeni.
 *
 * Sorumluluk:
 *   - Her ekranda, başlığın yanına bir "Bilgi" düğmesi yerleştirir
 *     (standart ekran başlığı yoksa sayfaya sabitlenmiş yüzen bir düğme).
 *   - Tıklanınca; sayfanın amacını, temel kullanım adımlarını, grid/tablo
 *     özelliklerini, filtreleri, kolonları ve önemli aksiyon düğmelerini
 *     açıklayan, tamamen yerelleştirilmiş bir yardım penceresi (dxPopup) açar.
 *   - Genel (tüm CRUD ekranları için ortak) içerik otomatik üretilir; sayfaya
 *     özel içerik için görünüm, yüklemeden önce window.AppPageHelp ayarlayabilir.
 *
 * Sayfaya özel içerik biçimi (tamamı isteğe bağlı):
 *   window.AppPageHelp = {
 *     title:   "Ekran başlığı (verilmezse otomatik bulunur)",
 *     intro:   "Bu ekranın amacını anlatan paragraf",
 *     steps:   ["Adım 1", "Adım 2", ...],
 *     columns: [{ name: "Kolon", desc: "Açıklama" }, ...],
 *     filters: [{ name: "Filtre", desc: "Açıklama" }, ...],
 *     actions: [{ name: "Buton", desc: "Açıklama" }, ...],
 *     hideGridHelp: true   // genel tablo yardımını gizlemek için
 *   };
 *
 * Genel API: window.AppInfo.init(), window.AppInfo.open().
 */
(function (window, $) {
    "use strict";

    function H() { return (window.AppL10n && window.AppL10n.help) || {}; }

    // "{0}" yer tutucusunu doldurur.
    function fmt(template, arg) {
        return String(template || "").replace("{0}", arg == null ? "" : arg);
    }

    // Sayfada bir DevExtreme veri tablosu olup olmadığını anlık olarak denetler.
    function hasGrid() {
        return !!document.querySelector(".dx-datagrid, .dx-treelist");
    }

    // Sayfa başlığını sırayla en güvenilir kaynaktan bulur.
    function detectTitle() {
        var p = window.AppPageHelp || {};
        if (p.title) { return p.title; }
        var h = document.querySelector(".energy-screen__header h2, .energy-screen__header h1");
        if (h && h.textContent.trim()) { return h.textContent.trim(); }
        var any = document.querySelector(".energy-screen h2, .energy-screen h1, main h2, main h1");
        if (any && any.textContent.trim()) { return any.textContent.trim(); }
        var t = (document.title || "").split(" - ")[0];
        return (t || "").trim();
    }

    // --- içerik oluşturucular ---------------------------------------------------
    function paragraphSection($root, title, text) {
        if (!text) { return; }
        var $s = $("<div class='energy-help__section'></div>").appendTo($root);
        if (title) { $("<h4 class='energy-help__heading'></h4>").text(title).appendTo($s); }
        $("<p class='energy-help__text'></p>").text(text).appendTo($s);
    }

    function listSection($root, title, items) {
        var clean = (items || []).filter(Boolean);
        if (!clean.length) { return; }
        var $s = $("<div class='energy-help__section'></div>").appendTo($root);
        if (title) { $("<h4 class='energy-help__heading'></h4>").text(title).appendTo($s); }
        var $ul = $("<ul class='energy-help__list'></ul>").appendTo($s);
        clean.forEach(function (it) { $("<li></li>").text(it).appendTo($ul); });
    }

    // {name, desc} ya da düz metin dizilerini tanımlı liste olarak gösterir.
    function defSection($root, title, items) {
        var clean = (items || []).filter(Boolean);
        if (!clean.length) { return; }
        var $s = $("<div class='energy-help__section'></div>").appendTo($root);
        if (title) { $("<h4 class='energy-help__heading'></h4>").text(title).appendTo($s); }
        var $ul = $("<ul class='energy-help__list energy-help__list--def'></ul>").appendTo($s);
        clean.forEach(function (it) {
            var $li = $("<li></li>").appendTo($ul);
            if (it && typeof it === "object") {
                if (it.name) { $("<strong></strong>").text(it.name + ": ").appendTo($li); }
                $li.append(document.createTextNode(it.desc || ""));
            } else {
                $li.text(String(it));
            }
        });
    }

    function buildContent() {
        var h = H();
        var page = window.AppPageHelp || {};
        var title = detectTitle();
        var $root = $("<div class='energy-help'></div>");

        // 1) Amaç
        var intro = page.intro || (title ? fmt(h.introEntity, title) : h.introGeneric);
        paragraphSection($root, h.purposeTitle, intro);

        // 2) Temel kullanım adımları — sayfaya özel adımlar her zaman; varsayılan
        //    (Ekle/Düzenle/Sil) adımları yalnızca sayfada bir tablo varsa gösterilir.
        var steps = (page.steps && page.steps.length)
            ? page.steps
            : (hasGrid() ? [h.step1, h.step2, h.step3] : null);
        listSection($root, h.stepsTitle, steps);

        // 3) Sayfaya özel filtreler / kolonlar
        defSection($root, h.filtersTitle, page.filters);
        defSection($root, h.columnsTitle, page.columns);

        // 3b) İlişkili kayıt türleri (FK / lookup alanlarından türetilir)
        if (page.related && page.related.length) {
            var $s = $("<div class='energy-help__section'></div>").appendTo($root);
            $("<h4 class='energy-help__heading'></h4>").text(h.relatedTitle || "").appendTo($s);
            if (h.relatedNote) { $("<p class='energy-help__text'></p>").text(h.relatedNote).appendTo($s); }
            var $ul = $("<ul class='energy-help__list'></ul>").appendTo($s);
            page.related.filter(Boolean).forEach(function (it) {
                $("<li></li>").text(typeof it === "object" ? (it.name || "") : String(it)).appendTo($ul);
            });
        }

        // 4) Genel tablo özellikleri (sayfada bir tablo varsa ve gizlenmediyse)
        if (hasGrid() && !page.hideGridHelp) {
            listSection($root, h.gridTitle, [
                h.gridSearch, h.gridFilterRow, h.gridHeaderFilter,
                h.gridColumnChooser, h.gridSort, h.gridExport, h.gridPaging
            ]);
            listSection($root, h.actionsTitle, [h.actionAdd, h.actionEdit, h.actionDelete]);
            paragraphSection($root, null, h.lookupNote);
        }

        // 5) Sayfaya özel ek aksiyonlar
        defSection($root, h.actionsExtraTitle || h.actionsTitle, page.actions);

        return $root;
    }

    // --- pencere ----------------------------------------------------------------
    var openInstance = null;
    function open() {
        if (openInstance) { return; }
        var $host = $("<div>").appendTo("body");
        openInstance = $host.dxPopup({
            title: H().title || "",
            visible: true,
            width: "min(94vw, 580px)",
            height: "auto",
            maxHeight: "86vh",
            showTitle: true,
            showCloseButton: true,
            dragEnabled: true,
            hideOnOutsideClick: true,
            wrapperAttr: { class: "energy-popup energy-help-popup" },
            contentTemplate: function (content) { $(content).append(buildContent()); },
            onHidden: function () {
                if (openInstance) { openInstance.dispose(); }
                $host.remove();
                openInstance = null;
            }
        }).dxPopup("instance");
    }

    // --- düğme yerleştirme -------------------------------------------------------
    function makeButton(extraClass) {
        var label = H().button || "";
        var $btn = $(
            "<button type='button' class='energy-info-btn " + (extraClass || "") + "' aria-haspopup='dialog'>" +
            "<i class='dx-icon dx-icon-info'></i></button>"
        );
        $btn.attr("title", label).attr("aria-label", label).on("click", open);
        return $btn;
    }

    function init() {
        if (document.querySelector(".energy-info-btn")) { return; } // tekrar eklenmesini önle
        var header = document.querySelector(".energy-screen__header");
        if (header) {
            // Başlığın yanına satır içi düğme.
            $(header).addClass("energy-screen__header--with-info").append(makeButton("energy-info-btn--inline"));
            return;
        }
        // Standart başlık yoksa: sayfaya sabit, yüzen düğme.
        var host = document.querySelector(".energy-screen, .energy-dashboard, main .energy-content, main");
        if (host) {
            $(makeButton("energy-info-btn--floating")).appendTo(document.body);
        }
    }

    window.AppInfo = { init: init, open: open, build: buildContent };
})(window, jQuery);

