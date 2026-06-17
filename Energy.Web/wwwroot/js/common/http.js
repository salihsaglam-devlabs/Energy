/*
 * AppHttp — fetch tabanlı, uygulama geneli ince HTTP istemcisi.
 *
 * Sorumluluk:
 *   - Tüm AJAX çağrıları için tek ve tutarlı bir yüzey sunar (GET/POST/PUT/DELETE + form gönderimi).
 *   - Güvenli varsayılanlar ekler: aynı köken (same-origin) çerezleri, JSON "Accept",
 *     "X-Requested-With" başlığı ve değiştiren (mutating) istekler için CSRF anti-forgery jetonu.
 *   - Yanıtı normalize eder: JSON gövdesini ayrıştırır ve hataları tek tip bir
 *     { status, message, payload } nesnesiyle reddeder (reject).
 *   - Kimlik doğrulama yönlendirmelerini ele alır: 401/403 + { redirect } gelirse
 *     kullanıcıyı ilgili sayfaya yönlendirir ve { handled: true } ile sessizce reddeder.
 *
 * Genel API (window.AppHttp): get, post, put, del, postForm.
 */
(function (window) {
    "use strict";

    // Sayfadaki <meta name="csrf-token"> etiketinden anti-forgery jetonunu okur.
    function getCsrfToken() {
        var meta = document.querySelector('meta[name="csrf-token"]');
        return meta ? meta.getAttribute("content") : "";
    }

    // Yükün, sunucudan gelen bir kimlik doğrulama yönlendirmesi (redirect) olup olmadığını belirler.
    function isAuthRedirect(payload) {
        return payload && typeof payload.redirect === "string" && payload.redirect.length > 0;
    }

    // Sunucu doğrulama (validation) hatalarını okunabilir tek bir metne dönüştürür.
    // "errors" alanı; alan adı -> mesaj listesi sözlüğü, düz dizi veya metin olabilir.
    // Amaç: kullanıcıya asla "[object Object]" göstermemek.
    function flattenErrors(errors) {
        if (!errors) { return ""; }
        if (typeof errors === "string") { return errors; }
        var parts = [];
        if (Array.isArray(errors)) {
            errors.forEach(function (e) {
                var t = (e && typeof e === "object") ? (e.message || e.Message || "") : String(e);
                if (t) { parts.push(t); }
            });
        } else if (typeof errors === "object") {
            Object.keys(errors).forEach(function (key) {
                var v = errors[key];
                if (Array.isArray(v)) { parts.push(v.join(" ")); }
                else if (v && typeof v === "object") { parts.push(v.message || v.Message || ""); }
                else if (v != null) { parts.push(String(v)); }
            });
        }
        return parts.filter(Boolean).join(" \u2022 ");
    }

    // Bir hata yükünden kullanıcı dostu bir mesaj üretir; hiçbir zaman boş/nesne döndürmez.
    function buildErrorMessage(payload, fallback) {
        if (payload) {
            if (typeof payload.message === "string" && payload.message.trim()) { return payload.message; }
            if (typeof payload.Message === "string" && payload.Message.trim()) { return payload.Message; }
            var flat = flattenErrors(payload.errors || payload.Errors);
            if (flat) { return flat; }
        }
        return fallback;
    }

    // İstek başlıklarını oluşturur; değiştiren metotlar için CSRF jetonunu ekler.
    function buildHeaders(method, custom) {
        var headers = Object.assign({
            "Accept": "application/json",
            "X-Requested-With": "XMLHttpRequest"
        }, custom || {});

        if (method !== "GET" && method !== "HEAD") {
            headers["RequestVerificationToken"] = getCsrfToken();
        }

        return headers;
    }

    // Çekirdek istek gönderici: gövdeyi türüne göre kodlar, yanıtı ayrıştırır ve hataları normalize eder.
    function send(method, url, body, options) {
        options = options || {};
        var init = {
            method: method,
            credentials: "same-origin",
            headers: buildHeaders(method, options.headers)
        };

        if (body !== undefined && body !== null) {
            if (body instanceof FormData) {
                // FormData, sınır (boundary) içeren kendi multipart Content-Type'ını ekler.
                init.body = body;
            } else if (typeof body === "string") {
                init.body = body;
                init.headers["Content-Type"] = init.headers["Content-Type"] || "application/x-www-form-urlencoded";
            } else {
                init.body = JSON.stringify(body);
                init.headers["Content-Type"] = "application/json";
            }
        }

        return fetch(url, init).then(function (response) {
            var contentType = response.headers.get("Content-Type") || "";
            var jsonPromise = contentType.indexOf("application/json") >= 0
                ? response.json().catch(function () { return null; })
                : Promise.resolve(null);

            return jsonPromise.then(function (payload) {
                if (response.status === 401 && isAuthRedirect(payload)) {
                    window.AppNotify && window.AppNotify.warning(window.AppL10n.notifications.sessionExpired);
                    window.location.href = payload.redirect;
                    return Promise.reject({ handled: true });
                }
                if (response.status === 403 && isAuthRedirect(payload)) {
                    window.location.href = payload.redirect;
                    return Promise.reject({ handled: true });
                }

                if (!response.ok) {
                    var message = buildErrorMessage(payload, window.AppL10n.notifications.failed);
                    return Promise.reject({ status: response.status, message: message, payload: payload });
                }

                return payload;
            });
        }).catch(function (err) {
            if (err && err.handled) {
                return Promise.reject(err);
            }
            if (err && err.status) {
                return Promise.reject(err);
            }
            // Ağ veya CORS hatası.
            return Promise.reject({ status: 0, message: window.AppL10n.notifications.networkError });
        });
    }

    window.AppHttp = {
        // GET isteği gönderir.
        get: function (url, options) { return send("GET", url, null, options); },
        // POST isteği gönderir (JSON / metin / FormData gövdesi).
        post: function (url, body, options) { return send("POST", url, body, options); },
        // PUT isteği gönderir.
        put: function (url, body, options) { return send("PUT", url, body, options); },
        // DELETE isteği gönderir.
        del: function (url, options) { return send("DELETE", url, null, options); },
        // Bir <form> öğesini FormData olarak POST eder (dosya yüklemeleri dahil).
        postForm: function (url, formElement) {
            var form = new FormData(formElement);
            return send("POST", url, form);
        },
        // Bir hata nesnesini/yükünü kullanıcıya gösterilecek okunabilir bir metne çevirir.
        // Çağrı yerlerinin "[object Object]" üretmesini engellemek için ortak yardımcı.
        errorText: function (err, fallback) {
            if (err && err.handled) { return ""; }
            var fb = fallback || (window.AppL10n && window.AppL10n.notifications && window.AppL10n.notifications.genericError) || "";
            if (err && typeof err.message === "string" && err.message.trim()) { return err.message; }
            return buildErrorMessage(err && err.payload ? err.payload : err, fb);
        }
    };
})(window);
