(function (window, $) {
    "use strict";
    // AppNotify is the legacy toast facade; it now delegates to AppAlert so the
    // whole app shares one notification engine. AppAlert is loaded first.
    function toast(message, type) {
        if (window.AppAlert) {
            window.AppAlert.toast(message, { type: type });
            return;
        }
        DevExpress.ui.notify({ message: message, type: type, displayTime: type === "error" ? 4000 : 2500 });
    }
    window.AppNotify = {
        success: function (m) { toast(m || window.AppL10n.notifications.saved, "success"); },
        info: function (m) { toast(m, "info"); },
        warning: function (m) { toast(m, "warning"); },
        error: function (m) { toast(m || window.AppL10n.notifications.failed, "error"); },
        fromHttpError: function (err) {
            if (err && err.handled) { return; }
            var msg = (err && err.message) ? err.message : window.AppL10n.notifications.genericError;
            toast(msg, "error");
        }
    };
})(window, jQuery);
