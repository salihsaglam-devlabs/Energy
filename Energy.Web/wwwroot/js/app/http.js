(function (window) {
    "use strict";

    function getCsrfToken() {
        var meta = document.querySelector('meta[name="csrf-token"]');
        return meta ? meta.getAttribute("content") : "";
    }

    function isAuthRedirect(payload) {
        return payload && typeof payload.redirect === "string" && payload.redirect.length > 0;
    }

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

    function send(method, url, body, options) {
        options = options || {};
        var init = {
            method: method,
            credentials: "same-origin",
            headers: buildHeaders(method, options.headers)
        };

        if (body !== undefined && body !== null) {
            if (body instanceof FormData) {
                // FormData adds its own multipart Content-Type with boundary.
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
                    var message = (payload && payload.message) ? payload.message : window.AppL10n.notifications.failed;
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
            // Network or CORS failure.
            return Promise.reject({ status: 0, message: window.AppL10n.notifications.networkError });
        });
    }

    window.AppHttp = {
        get: function (url, options) { return send("GET", url, null, options); },
        post: function (url, body, options) { return send("POST", url, body, options); },
        put: function (url, body, options) { return send("PUT", url, body, options); },
        del: function (url, options) { return send("DELETE", url, null, options); },
        postForm: function (url, formElement) {
            var form = new FormData(formElement);
            return send("POST", url, form);
        }
    };
})(window);
