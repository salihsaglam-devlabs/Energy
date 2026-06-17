/*
 * dx-license — DevExtreme deneme/lisans filigranını otomatik gizler.
 *
 * Sorumluluk:
 *   - <dx-license> banner'ı DOM'a enjekte edilir edilmez kapatma düğmesine tıklar ve
 *     yedek olarak öğeyi kaldırır.
 *   - Banner data-permanent olarak işaretli olup yeniden enjekte edilebildiğinden, bir
 *     MutationObserver ile DOM'u sürekli izleyip yeni eklenen filigranları da gizler.
 *
 * Not: Yalnızca görsel bir temizliktir; lisans durumunu değiştirmez.
 */
(function (window, document) {
    "use strict";

    // DevExtreme deneme/lisans filigranını (<dx-license>) DOM'a enjekte edilir
    // edilmez otomatik olarak kapat. Kapatma düğmesine (X) tıklarız ve yedek olarak
    // öğeyi kaldırırız. Banner data-permanent olarak işaretli olduğundan yeniden
    // enjekte edilebilir; bu yüzden bir MutationObserver izlemeyi sürdürür.
    function dismiss(el) {
        if (!el || el.nodeType !== 1) {
            return;
        }

        // Kapatma düğmesi, banner'daki son <div>'dir (X svg'sini içeren).
        var closer = el.querySelector("div:last-child");
        if (closer) {
            try { closer.click(); } catch (e) { /* yok say */ }
        }

        if (el.parentNode) {
            try { el.parentNode.removeChild(el); } catch (e) { /* yok say */ }
        }
    }

    function dismissWithin(node) {
        if (!node || node.nodeType !== 1) {
            return;
        }

        if (node.tagName && node.tagName.toLowerCase() === "dx-license") {
            dismiss(node);
            return;
        }

        if (node.querySelectorAll) {
            var found = node.querySelectorAll("dx-license");
            Array.prototype.forEach.call(found, dismiss);
        }
    }

    function start() {
        // Zaten mevcut olan her şeyi işle.
        dismissWithin(document.body || document.documentElement);

        var observer = new MutationObserver(function (mutations) {
            for (var i = 0; i < mutations.length; i++) {
                var added = mutations[i].addedNodes;
                for (var j = 0; j < added.length; j++) {
                    dismissWithin(added[j]);
                }
            }
        });

        observer.observe(document.documentElement, { childList: true, subtree: true });
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", start);
    } else {
        start();
    }
})(window, document);

