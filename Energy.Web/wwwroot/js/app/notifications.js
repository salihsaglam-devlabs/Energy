(function (window, $) {
    "use strict";
    function show(message, type) {
        DevExpress.ui.notify({
            message: message,
            type: type,
            displayTime: type === "error" ? 4000 : 2500,
            position: { my: "top right", at: "top right", offset: "-16 16" },
            width: 360
        });
    }
    window.AppNotify = {
        success: function (m) { show(m || window.AppL10n.notifications.saved, "success"); },
        info: function (m) { show(m, "info"); },
        warning: function (m) { show(m, "warning"); },
        error: function (m) { show(m || window.AppL10n.notifications.failed, "error"); },
        fromHttpError: function (err) {
            if (err && err.handled) { return; }
            var msg = (err && err.message) ? err.message : window.AppL10n.notifications.genericError;
            show(msg, "error");
        }
    };
})(window, jQuery);
