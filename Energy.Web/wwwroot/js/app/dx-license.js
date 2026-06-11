(function (window, document) {
    "use strict";

    // Auto-dismiss the DevExtreme trial/license watermark (<dx-license>) as soon
    // as it is injected into the DOM. We click its close affordance (the X) and,
    // as a fallback, remove the element. Because the banner is marked
    // data-permanent it can be re-injected, so a MutationObserver keeps watching.
    function dismiss(el) {
        if (!el || el.nodeType !== 1) {
            return;
        }

        // The close button is the last <div> in the banner (the one with the X svg).
        var closer = el.querySelector("div:last-child");
        if (closer) {
            try { closer.click(); } catch (e) { /* ignore */ }
        }

        if (el.parentNode) {
            try { el.parentNode.removeChild(el); } catch (e) { /* ignore */ }
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
        // Handle anything already present.
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

