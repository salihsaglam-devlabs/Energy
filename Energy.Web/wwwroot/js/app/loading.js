(function (window, $) {
    "use strict";

    var loadPanel = null;
    var counter = 0;

    function ensure() {
        if (loadPanel) return loadPanel;
        var $host = $("#energy-loading");
        loadPanel = $host.dxLoadPanel({
            shadingColor: "rgba(15, 23, 42, 0.25)",
            position: { of: window },
            visible: false,
            showIndicator: true,
            showPane: true,
            shading: true,
            closeOnOutsideClick: false,
            message: window.AppL10n.layout.loading
        }).dxLoadPanel("instance");
        return loadPanel;
    }

    window.AppLoading = {
        begin: function () {
            counter++;
            ensure().show();
        },
        end: function () {
            counter = Math.max(0, counter - 1);
            if (counter === 0 && loadPanel) {
                loadPanel.hide();
            }
        },
        wrap: function (promise) {
            window.AppLoading.begin();
            return promise.finally(function () { window.AppLoading.end(); });
        }
    };
})(window, jQuery);
