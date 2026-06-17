/*
 * AppNotify — basit bildirim (toast) cephesi (facade).
 *
 * Sorumluluk:
 *   - Uygulama genelinde kısa durum bildirimleri için sade bir API sunar
 *     (success / info / warning / error).
 *   - Geriye dönük uyumluluk için korunur; çağrıları daha zengin AppAlert motoruna
 *     devreder; AppAlert yüklü değilse DevExtreme'in yerel notify'ına geri düşer.
 *   - fromHttpError(): AppHttp'nin reddettiği hata nesnesini kullanıcıya gösterilen
 *     bir mesaja çevirir; daha önce ele alınmış (handled) yönlendirmeleri yutar.
 *
 * Genel API (window.AppNotify): success, info, warning, error, fromHttpError.
 */
(function (window, $) {
    "use strict";
    // AppNotify eski toast cephesidir; artık AppAlert'e devreder; böylece tüm uygulama
    // tek bir bildirim motorunu paylaşır. AppAlert önce yüklenir.
    function toast(message, type) {
        if (window.AppAlert) {
            window.AppAlert.toast(message, { type: type });
            return;
        }
        DevExpress.ui.notify({ message: message, type: type, displayTime: type === "error" ? 4000 : 2500 });
    }
    window.AppNotify = {
        // Başarı bildirimi gösterir (mesaj verilmezse varsayılan "kaydedildi").
        success: function (m) { toast(m || window.AppL10n.notifications.saved, "success"); },
        // Bilgi bildirimi gösterir.
        info: function (m) { toast(m, "info"); },
        // Uyarı bildirimi gösterir.
        warning: function (m) { toast(m, "warning"); },
        // Hata bildirimi gösterir (mesaj verilmezse varsayılan "başarısız").
        error: function (m) { toast(m || window.AppL10n.notifications.failed, "error"); },
        // AppHttp hata nesnesini hata bildirimine çevirir; ele alınmış yönlendirmeleri atlar.
        fromHttpError: function (err) {
            if (err && err.handled) { return; }
            var msg = (window.AppHttp && typeof window.AppHttp.errorText === "function")
                ? window.AppHttp.errorText(err, window.AppL10n.notifications.genericError)
                : ((err && err.message) ? err.message : window.AppL10n.notifications.genericError);
            toast(msg, "error");
        }
    };
})(window, jQuery);
