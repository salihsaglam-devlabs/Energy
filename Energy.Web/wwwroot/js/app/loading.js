/*
 * AppLoading — uygulama genelinde tek bir yükleme göstergesi (DevExtreme dxLoadPanel).
 *
 * Sorumluluk:
 *   - Tüm sayfalarda ortak, tam ekran bir "yükleniyor" paneli sunar.
 *   - İç içe/eşzamanlı çağrıları bir sayaç (counter) ile yönetir: panel yalnızca
 *     bekleyen tüm işlemler bittiğinde gizlenir; böylece erken kapanma yaşanmaz.
 *
 * Genel API (window.AppLoading):
 *   - begin()        : sayacı artırır ve paneli gösterir.
 *   - end()          : sayacı azaltır; sıfıra ulaşınca paneli gizler.
 *   - wrap(promise)  : verilen promise'i begin/end ile sarar ve geri döndürür.
 *
 * Not: Panel ilk ihtiyaç anında tembel (lazy) olarak oluşturulur.
 */
(function (window, $) {
    "use strict";

    // dxLoadPanel örneği (ilk gösterimde oluşturulur) ve aktif istek sayacı.
    var loadPanel = null;
    var counter = 0;

    // dxLoadPanel örneğini tembel olarak oluşturur ve önbelleğe alınmış örneği döndürür.
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
        // Yeni bir bekleyen işlem başlat: sayacı artır ve paneli göster.
        begin: function () {
            counter++;
            ensure().show();
        },
        // Bir bekleyen işlemi bitir: sayacı azalt; tüm işlemler bitince paneli gizle.
        end: function () {
            counter = Math.max(0, counter - 1);
            if (counter === 0 && loadPanel) {
                loadPanel.hide();
            }
        },
        // Bir promise'i yükleme göstergesiyle sar: başlangıçta begin, tamamlanınca end.
        wrap: function (promise) {
            window.AppLoading.begin();
            return promise.finally(function () { window.AppLoading.end(); });
        }
    };
})(window, jQuery);
