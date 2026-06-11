(function (window) {
    "use strict";

    var PHONE_BREAKPOINT = 768;
    var TABLET_BREAKPOINT = 1200;

    function viewportWidth() {
        return window.innerWidth || document.documentElement.clientWidth || 1280;
    }

    function isPhone() {
        return viewportWidth() <= PHONE_BREAKPOINT;
    }

    function isTablet() {
        return viewportWidth() <= TABLET_BREAKPOINT;
    }

    function getSearchPanelOptions(placeholder) {
        return {
            visible: true,
            placeholder: placeholder,
            width: isPhone() ? 160 : 240
        };
    }

    function getPagerOptions() {
        return {
            visible: true,
            allowedPageSizes: [10, 20, 50],
            showPageSizeSelector: !isPhone(),
            showInfo: !isPhone(),
            showNavigationButtons: true,
            displayMode: isPhone() ? "compact" : "full"
        };
    }

    function getGridOptions(extra) {
        extra = extra || {};

        return Object.assign({
            // Keep every column visible and sized to its content. When the
            // combined width exceeds the grid, a horizontal scrollbar appears
            // instead of hiding columns.
            columnAutoWidth: true,
            columnHidingEnabled: false,
            wordWrapEnabled: false,
            width: "100%",
            // Fixed viewport height (75% of the screen). Rows scroll inside the
            // grid; navigation between pages is done with the pager.
            height: "75vh",
            repaintChangesOnly: true,
            // Standard (paged) scrolling so the pager, page-size selector and
            // horizontal scrolling all behave correctly together. Virtual mode
            // conflicts with the classic pager and is intentionally not used.
            scrolling: {
                mode: "standard",
                useNative: true,
                showScrollbar: "onScroll",
                columnRenderingMode: "standard"
            }
        }, extra);
    }

    function getPopupOptions(options) {
        options = options || {};
        var mobile = isPhone();
        var tablet = isTablet();

        return Object.assign({}, options, {
            width: mobile ? "100vw" : (tablet ? Math.min(options.width || 640, 720) : (options.width || 640)),
            height: mobile ? "100%" : (options.height || "auto"),
            maxWidth: mobile ? "100vw" : "min(92vw, 960px)",
            maxHeight: mobile ? "100%" : "min(88vh, 820px)",
            fullScreen: mobile ? true : (!!options.fullScreen || false),
            dragEnabled: !mobile,
            resizeEnabled: !mobile,
            hideOnOutsideClick: true,
            showCloseButton: options.showCloseButton !== false,
            wrapperAttr: { class: "energy-popup" },
            onShown: function (e) {
                if (typeof options.onShown === "function") {
                    options.onShown(e);
                }

                e.component.repaint();
            }
        });
    }

    function getFormOptions(options) {
        options = options || {};
        return Object.assign({}, options, {
            colCount: isPhone() ? 1 : (options.colCount || 2),
            labelLocation: options.labelLocation || "top"
        });
    }

    window.AppResponsive = {
        isPhone: isPhone,
        isTablet: isTablet,
        getSearchPanelOptions: getSearchPanelOptions,
        getPagerOptions: getPagerOptions,
        getGridOptions: getGridOptions,
        getPopupOptions: getPopupOptions,
        getFormOptions: getFormOptions
    };
})(window);

