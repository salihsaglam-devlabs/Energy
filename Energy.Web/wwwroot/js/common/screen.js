/*
 * AppScreen — entity / process / report ekranlarını görsel olarak işaretler.
 *
 * Amaç (kullanıcı talebi #3):
 *   Entity CRUD ekranları (tekil veri yönetimi) ile süreç/operasyon ekranları
 *   kullanıcı tarafından kolayca ayırt edilebilmelidir. Sayfa içeriğini
 *   bozmadan, header'ın sağına sade, kurumsal bir rozet ekleriz.
 *
 * Sınıflandırma:
 *   - .energy-process            -> Süreç rozeti
 *   - .energy-report             -> Rapor rozeti
 *   - diğer .energy-screen       -> Veri Yönetimi rozeti (varsayılan)
 *
 * Kullanım: Hiçbir view değişikliği gerektirmez; DOMContentLoaded'da çalışır.
 * İstenirse <section data-screen="process|entity|report"> ile zorlanabilir.
 */
(function (window, document) {
    "use strict";

    function L() {
        return (window.AppL10n && window.AppL10n.screen) || {
            entityBadge: "Data Management",
            entityBadgeTitle: "",
            processBadge: "Process",
            processBadgeTitle: "",
            reportBadge: "Report",
            reportBadgeTitle: ""
        };
    }

    function detect(section) {
        var explicit = section.getAttribute("data-screen");
        if (explicit === "process" || explicit === "entity" || explicit === "report") {
            return explicit;
        }
        if (section.classList.contains("energy-process")) { return "process"; }
        if (section.classList.contains("energy-report")) { return "report"; }
        return "entity";
    }

    function ensureBadge(section, kind) {
        if (section.querySelector(":scope > .energy-screen__header .energy-screen__badge")) { return; }
        var header = section.querySelector(":scope > .energy-screen__header");
        if (!header) { return; }
        var l = L();
        var badge = document.createElement("span");
        badge.className = "energy-screen__badge is-" + kind;
        var text, title;
        if (kind === "process") { text = l.processBadge; title = l.processBadgeTitle; }
        else if (kind === "report") { text = l.reportBadge; title = l.reportBadgeTitle; }
        else { text = l.entityBadge; title = l.entityBadgeTitle; }
        badge.textContent = text || "";
        if (title) { badge.setAttribute("title", title); }
        // Header'ın en sağına yerleştir; mevcut iç düzeni bozma.
        header.appendChild(badge);
    }

    function markScreens(root) {
        var scope = root || document;
        var sections = scope.querySelectorAll("section.energy-screen");
        for (var i = 0; i < sections.length; i++) {
            var s = sections[i];
            var kind = detect(s);
            var modifier = "energy-screen--" + kind;
            if (!s.classList.contains(modifier)) { s.classList.add(modifier); }
            ensureBadge(s, kind);
        }
    }

    function init() { markScreens(document); }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }

    window.AppScreen = { mark: markScreens };
})(window, document);

