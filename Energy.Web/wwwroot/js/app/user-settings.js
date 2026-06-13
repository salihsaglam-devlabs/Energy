/*
 * EnergyUserSettings — oturum açmış kullanıcının tercihleri için tek, uygulama geneli depo.
 *
 * Sorumluluk:
 *   - Kimliği doğrulanmış her sayfada (chat-realtime'dan önce) yüklenir; böylece bildirim
 *     sesleri ve tema her yerde tutarlıdır ve oturumlar/cihazlar arasında korunur.
 *   - Tercihleri sunucudan yükler ve uygular: bildirim sesi, arama sesi, masaüstü
 *     bildirimleri, okundu bilgileri ve tema.
 *   - Temayı anında (localStorage'dan) uygulayarak yanıp sönmeyi önler, sonra sunucudan iyileştirir.
 *   - İkili (binary) bir varlık gerektirmeden WebAudio ile bildirim/arama sesi üretir.
 *
 * Genel API (window.EnergyUserSettings): data, applyTheme, beep, set, load.
 */
(function (window) {
    "use strict";

    // Oturum açmış kullanıcının tercihleri için tek, uygulama genelinde depo. Kimliği
    // doğrulanmış her sayfada (chat-realtime'dan önce) yüklenir; böylece bildirim sesleri
    // ve tema her yerde tutarlıdır ve oturumlar/cihazlar arasında korunur.
    var DEFAULTS = {
        notificationSound: true,
        callSound: true,
        desktopNotifications: true,
        readReceipts: true,
        theme: "system"
    };

    var audioCtx = null;

    function applyTheme(theme) {
        var t = (theme || "system").toLowerCase();
        var root = document.documentElement;
        if (t === "light" || t === "dark") {
            root.setAttribute("data-theme", t);
        } else {
            root.removeAttribute("data-theme");
        }
        try { window.localStorage.setItem("energy-theme", t); } catch (e) { /* yok say */ }
    }

    // İkili (binary) bir varlık gerektirmeyen hafif WebAudio sesi. type:
    // "message" (kısa bip) | "call" (daha uzun zil). İlgili tercihle koşullanır.
    function beep(type) {
        var data = api.data || DEFAULTS;
        if (type === "call" && !data.callSound) { return; }
        if (type !== "call" && !data.notificationSound) { return; }
        try {
            var Ctx = window.AudioContext || window.webkitAudioContext;
            if (!Ctx) { return; }
            audioCtx = audioCtx || new Ctx();
            if (audioCtx.state === "suspended") { audioCtx.resume(); }

            var now = audioCtx.currentTime;
            var freq = type === "call" ? 660 : 880;
            var dur = type === "call" ? 0.9 : 0.18;

            var osc = audioCtx.createOscillator();
            var gain = audioCtx.createGain();
            osc.type = "sine";
            osc.frequency.value = freq;
            gain.gain.setValueAtTime(0.0001, now);
            gain.gain.exponentialRampToValueAtTime(0.18, now + 0.02);
            gain.gain.exponentialRampToValueAtTime(0.0001, now + dur);
            osc.connect(gain).connect(audioCtx.destination);
            osc.start(now);
            osc.stop(now + dur + 0.02);
        } catch (e) { /* bir ses için arayüzü asla bozma */ }
    }

    var api = {
        data: (function () {
            var d = {};
            for (var k in DEFAULTS) { if (DEFAULTS.hasOwnProperty(k)) { d[k] = DEFAULTS[k]; } }
            return d;
        })(),
        applyTheme: applyTheme,
        beep: beep,
        set: function (s) {
            if (!s) { return; }
            api.data = {
                notificationSound: s.notificationSound !== false,
                callSound: s.callSound !== false,
                desktopNotifications: s.desktopNotifications !== false,
                readReceipts: s.readReceipts !== false,
                theme: s.theme || "system"
            };
            applyTheme(api.data.theme);
        },
        // Yetkili değerleri sunucudan çek (aşağıdaki anlık localStorage tema uygulamasından
        // sonra); böylece farklı bir cihaz/oturum eşleşir.
        load: function () {
            if (!window.jQuery) { return; }
            window.jQuery.getJSON("/settings/data")
                .done(function (s) { if (s) { api.set(s); } })
                .fail(function () { /* varsayılanları koru */ });
        }
    };

    // Yanıp sönmeyi önlemek için son bilinen değerden anlık tema uygula, sonra
    // sunucudan iyileştir.
    try {
        var stored = window.localStorage.getItem("energy-theme");
        if (stored) { applyTheme(stored); }
    } catch (e) { /* yok say */ }

    window.EnergyUserSettings = api;

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", function () { api.load(); });
    } else {
        api.load();
    }
})(window);

