/*
 * Settings sayfası — kullanıcı bazlı tercih (ayarlar) ekranı.
 *
 * Sorumluluk:
 *   - Bildirim sesi, arama sesi, masaüstü bildirimleri, okundu bilgileri ve tema gibi
 *     kullanıcı tercihlerini görüntüler ve kaydeder (veritabanında kalıcı).
 *   - Tema önizlemesini paylaşılan EnergyUserSettings modülüne devreder (tek doğruluk kaynağı).
 *
 * Genel API: window.AppPages.Settings.init().
 */
(function (window, $) {
    "use strict";

    window.AppPages = window.AppPages || {};

    // Tema uygulaması, paylaşılan user-settings modülüne devredilir (tek doğruluk
    // kaynağı, ayrıca sesler/tema için uygulama genelinde kullanılır); yoksa işlemsiz
    // bir klona geri düşülür.
    function settingsStore() {
        return window.EnergyUserSettings || { data: {}, set: function () { }, applyTheme: function () { } };
    }
    // Temayı paylaşılan ayarlar deposu üzerinden uygular.
    function applyTheme(theme) { settingsStore().applyTheme(theme); }

    window.AppPages.Settings = {
        applyTheme: applyTheme,

        init: function (opts) {
            opts = opts || {};
            var $screen = $("#settings-screen");
            if ($screen.length === 0) { return; }

            function readForm() {
                return {
                    notificationSound: $screen.find('[data-setting="notificationSound"]').is(":checked"),
                    callSound: $screen.find('[data-setting="callSound"]').is(":checked"),
                    desktopNotifications: $screen.find('[data-setting="desktopNotifications"]').is(":checked"),
                    readReceipts: $screen.find('[data-setting="readReceipts"]').is(":checked"),
                    theme: $screen.find('[data-setting="theme"]').val() || "system"
                };
            }

            function fillForm(s) {
                $screen.find('[data-setting="notificationSound"]').prop("checked", !!s.notificationSound);
                $screen.find('[data-setting="callSound"]').prop("checked", !!s.callSound);
                $screen.find('[data-setting="desktopNotifications"]').prop("checked", !!s.desktopNotifications);
                $screen.find('[data-setting="readReceipts"]').prop("checked", !!s.readReceipts);
                $screen.find('[data-setting="theme"]').val(s.theme || "system");
            }

            // Geçerli ayarları sunucudan yükle.
            $.getJSON("/settings/data").done(function (s) {
                s = s || {};
                fillForm(s);
                window.EnergyUserSettings.set(s);
            });

            // Kullanıcı seçiciyi değiştirdikçe canlı tema önizlemesi.
            $screen.find('[data-setting="theme"]').on("change", function () {
                applyTheme($(this).val());
            });

            $("#settings-save").on("click", function () {
                var payload = readForm();
                var token = $("meta[name='csrf-token']").attr("content");
                $.ajax({
                    url: "/settings",
                    method: "POST",
                    contentType: "application/json",
                    headers: token ? { "RequestVerificationToken": token } : {},
                    data: JSON.stringify(payload)
                }).done(function (envelope) {
                    // BaseResponse.IsSuccess JSON'da [JsonPropertyName("success")] ile
                    // "success" olarak serileştirilir; bu yüzden önce "success" okunmalı
                    // (isSuccess/IsSuccess yalnızca geriye dönük güvenlik için).
                    var ok = envelope && (envelope.success || envelope.isSuccess || envelope.IsSuccess);
                    var data = envelope && (envelope.data || envelope.Data);
                    if (ok) {
                        window.EnergyUserSettings.set(data || payload);
                        if (window.AppNotify) { window.AppNotify.success(opts.savedMessage || window.AppL10n.notifications.saved); }
                    } else if (window.AppNotify) {
                        window.AppNotify.error((envelope && (envelope.message || envelope.Message)) || window.AppL10n.notifications.failed);
                    }
                }).fail(function () {
                    if (window.AppNotify) { window.AppNotify.error(window.AppL10n.notifications.networkError); }
                });
            });
        }
    };
})(window, jQuery);

