/*
 * AppAlert — birleşik DevExtreme uyarı / bildirim yüzeyi.
 *
 * Üç sunum modu, tek API:
 *   - "toast"  : alttan kayarak gelen ve kendiliğinden kaybolan, otomatik kapanan
 *                bildirim (birkaçı tetiklendiğinde yukarı doğru yığılır).
 *   - "popup"  : ikon, başlık, mesaj ve eylem düğmeleri içeren modal iletişim kutusu
 *                (dxPopup); Promise<bool> döndüren confirm() destekler.
 *   - "embed"  : bir sayfa kapsayıcısına gömülen satır içi banner; isteğe bağlı olarak
 *                kapatılabilir ve/veya otomatik kapanır.
 *
 * Önem derecesi yardımcıları (success/info/warning/error) varsayılan olarak "toast"
 * kullanır ancak çağrı yerlerini değiştirmeden yüzeyi değiştirmek için
 * { mode: "popup" | "embed" } kabul eder.
 */
(function (window, $) {
    "use strict";

    var dx = window.DevExpress;

    // --- önem derecesi → görsel eşlemesi -----------------------------------------
    var TYPES = {
        success: { dx: "success", icon: "check",       css: "is-success" },
        info:    { dx: "info",    icon: "info",        css: "is-info" },
        warning: { dx: "warning", icon: "warning",     css: "is-warning" },
        error:   { dx: "error",   icon: "errorcircle", css: "is-error" }
    };

    function L() {
        return (window.AppL10n && window.AppL10n.alerts) || {};
    }

    function resolveType(type) {
        return TYPES[type] || TYPES.info;
    }

    function defaultTitle(type) {
        var l = L();
        switch (type) {
            case "success": return l.success || "Success";
            case "warning": return l.warning || "Warning";
            case "error":   return l.error || "Error";
            default:        return l.info || "Information";
        }
    }

    // --- toast (alt, kendiliğinden kapanan) ------------------------------------
    var bottomStack = {
        position: { my: "bottom center", at: "bottom center", of: window, offset: "0 -24" },
        direction: "up-push"
    };

    function toast(message, options) {
        options = options || {};
        var type = options.type || "info";
        var meta = resolveType(type);
        var stack = options.stack || bottomStack;

        dx.ui.notify({
            message: message || "",
            type: meta.dx,
            displayTime: options.displayTime || (type === "error" ? 5000 : 3000),
            width: options.width || "auto",
            minWidth: 280,
            maxWidth: 520,
            animation: {
                show: { type: "slide", duration: 260, from: { position: { my: "top", at: "bottom", of: window } } },
                hide: { type: "fade", duration: 200, to: 0 }
            }
        }, stack);
    }

    // --- popup (modal iletişim kutusu) -----------------------------------------------
    function popup(options) {
        options = options || {};
        var type = options.type || "info";
        var meta = resolveType(type);
        var title = options.title || defaultTitle(type);
        var l = L();
        var resolveFn;
        var result = false;
        var promise = new Promise(function (resolve) { resolveFn = resolve; });

        var buttons = options.buttons;
        if (!buttons) {
            if (options.confirm) {
                buttons = [
                    { text: options.okText || l.ok || "OK", type: "default", stylingMode: "contained",
                      onClick: function () { result = true; instance.hide(); } },
                    { text: options.cancelText || l.cancel || "Cancel", stylingMode: "outlined",
                      onClick: function () { result = false; instance.hide(); } }
                ];
            } else {
                buttons = [
                    { text: options.okText || l.ok || "OK", type: "default", stylingMode: "contained",
                      onClick: function () { result = true; instance.hide(); } }
                ];
            }
        }

        var $host = $("<div>").appendTo("body");
        var instance = $host.dxPopup({
            title: "",
            visible: true,
            width: options.width || 460,
            height: options.height || "auto",
            maxWidth: "92vw",
            showTitle: false,
            dragEnabled: false,
            hideOnOutsideClick: !!options.confirm === false && options.hideOnOutsideClick !== false,
            shading: true,
            wrapperAttr: { class: "energy-alert-popup " + meta.css },
            contentTemplate: function (content) {
                var $wrap = $("<div class='energy-alert-popup__body'></div>");
                $("<div class='energy-alert-popup__icon'></div>")
                    .append($("<i></i>").addClass("dx-icon dx-icon-" + meta.icon))
                    .appendTo($wrap);
                var $text = $("<div class='energy-alert-popup__text'></div>").appendTo($wrap);
                $("<div class='energy-alert-popup__title'></div>").text(title).appendTo($text);
                var $msg = $("<div class='energy-alert-popup__message'></div>").appendTo($text);
                if (options.html) { $msg.html(options.html); } else { $msg.text(options.message || ""); }

                var $footer = $("<div class='energy-alert-popup__footer'></div>");
                buttons.forEach(function (b) {
                    $("<div></div>").appendTo($footer).dxButton({
                        text: b.text, type: b.type || "normal", stylingMode: b.stylingMode || "contained",
                        onClick: b.onClick
                    });
                });
                $(content).append($wrap).append($footer);
            },
            onHidden: function () {
                instance.dispose();
                $host.remove();
                resolveFn(result);
            }
        }).dxPopup("instance");

        return promise;
    }

    function confirm(message, options) {
        options = options || {};
        options.confirm = true;
        options.message = message;
        if (!options.type) { options.type = "warning"; }
        return popup(options);
    }

    // --- embed (satır içi banner) ----------------------------------------------
    function embed(target, message, options) {
        options = options || {};
        var type = options.type || "info";
        var meta = resolveType(type);
        var $target = (target && target.jquery) ? target : $(target);
        if (!$target.length) { return { close: function () {} }; }

        var $alert = $("<div class='energy-alert " + meta.css + "'></div>");
        $("<i class='energy-alert__icon dx-icon dx-icon-" + meta.icon + "'></i>").appendTo($alert);

        var $content = $("<div class='energy-alert__content'></div>").appendTo($alert);
        if (options.title) {
            $("<div class='energy-alert__title'></div>").text(options.title).appendTo($content);
        }
        var $msg = $("<div class='energy-alert__message'></div>").appendTo($content);
        if (options.html) { $msg.html(options.html); } else { $msg.text(message || ""); }

        function close() {
            $alert.addClass("is-leaving");
            window.setTimeout(function () { $alert.remove(); }, 200);
        }

        if (options.dismissible !== false) {
            $("<button type='button' class='energy-alert__close' aria-label='close'>&times;</button>")
                .on("click", close).appendTo($alert);
        }

        if (options.replace !== false) { $target.find("> .energy-alert").remove(); }
        if (options.prepend) { $target.prepend($alert); } else { $target.append($alert); }

        if (options.displayTime && options.displayTime > 0) {
            window.setTimeout(close, options.displayTime);
        }

        return { close: close, element: $alert };
    }

    // --- birleşik giriş + önem derecesi yardımcıları -----------------------------------
    function show(options) {
        options = options || {};
        var mode = options.mode || "toast";
        if (mode === "popup") { return popup(options); }
        if (mode === "embed") { return embed(options.target, options.message, options); }
        toast(options.message, options);
        return Promise.resolve(true);
    }

    function severity(type) {
        return function (message, options) {
            options = options || {};
            options.type = type;
            if (options.mode === "popup") { options.message = message; return popup(options); }
            if (options.mode === "embed") { return embed(options.target, message, options); }
            toast(message, options);
            return Promise.resolve(true);
        };
    }

    window.AppAlert = {
        show: show,
        toast: toast,
        popup: popup,
        confirm: confirm,
        embed: embed,
        success: severity("success"),
        info: severity("info"),
        warning: severity("warning"),
        error: severity("error")
    };
})(window, jQuery);

