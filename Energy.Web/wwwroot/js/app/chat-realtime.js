(function (window, $) {
    "use strict";

    // Global, app-wide chat realtime client. Loaded on every authenticated page
    // so the notification bell stays live regardless of the current screen.
    var cfg = (window.AppContext && window.AppContext.chat) || {};
    var $bell = $("#energy-notify");
    if ($bell.length === 0 || !cfg.hubUrl) {
        // Not authenticated / no bell rendered -> nothing to wire. Expose a
        // complete no-op surface so consumers never have to feature-detect.
        window.EnergyChat = window.EnergyChat || {
            subscribe: function () { },
            refreshUnread: function () { },
            onPresence: function () { },
            onTyping: function () { },
            onStatus: function () { },
            onGroupInvite: function () { },
            onGroupChanged: function () { },
            onMessageDeleted: function () { },
            onMessageReacted: function () { },
            onMessagesRead: function () { },
            onCall: function () { },
            callUser: function () { return Promise.resolve(); },
            answerCall: function () { return Promise.resolve(); },
            sendIce: function () { return Promise.resolve(); },
            endCall: function () { return Promise.resolve(); },
            sendTyping: function () { },
            isOnline: function () { return false; },
            getOnlineUsers: function () { return []; },
            status: function () { return "disabled"; }
        };
        return;
    }

    var me = (cfg.me || "").toLowerCase();
    var $badge = $("#energy-notify-badge");
    var $panel = $("#energy-notify-panel");
    var $list = $("#energy-notify-list");
    var subscribers = [];
    var unread = 0;

    // Presence + typing + connection state.
    var onlineUsers = {};          // map: userId(lowercase) -> true
    var presenceSubscribers = [];  // notified on every presence change/snapshot
    var typingSubscribers = [];    // notified on incoming typing indicators
    var statusSubscribers = [];    // notified on connection status changes
    var groupInviteSubscribers = [];  // notified when invited to a group
    var groupChangedSubscribers = []; // notified when a group's roster changes
    var msgDeletedSubscribers = [];
    var msgReactedSubscribers = [];
    var msgReadSubscribers = [];
    var callSubscribers = [];          // notified on call signaling events
    var connection = null;
    var status = "disconnected";
    var lastStatusAt = Date.now();

    // Toolbar connection indicator.
    var $conn = $("#energy-conn");
    var $connDot = $("#energy-conn-dot");
    var $connPanel = $("#energy-conn-panel");
    var $connTitle = $("#energy-conn-title");
    var $connBody = $("#energy-conn-body");
    var connLabels = {
        status: $conn.data("label-status") || "Connection",
        online: $conn.data("label-online") || "Online users",
        connected: $conn.data("status-connected") || "Connected",
        connecting: $conn.data("status-connecting") || "Connecting…",
        reconnecting: $conn.data("status-reconnecting") || "Reconnecting…",
        disconnected: $conn.data("status-disconnected") || "Disconnected"
    };

    function log(state, detail) {
        try { console.info("[chat] " + state + (detail ? ": " + detail : "")); } catch (e) { /* noop */ }
    }

    function statusInfo(s) {
        switch (s) {
            case "connected": return { cls: "is-connected", text: connLabels.connected };
            case "connecting": return { cls: "is-connecting", text: connLabels.connecting };
            case "reconnecting": return { cls: "is-connecting", text: connLabels.reconnecting };
            default: return { cls: "is-down", text: connLabels.disconnected };
        }
    }

    function renderConn() {
        if ($conn.length === 0) { return; }
        var info = statusInfo(status);
        $conn.removeClass("is-connected is-connecting is-down").addClass(info.cls);
        $conn.attr("title", connLabels.status + ": " + info.text);
        if ($connTitle.length) { $connTitle.text(connLabels.status); }
        if ($connBody.length) {
            var onlineCount = Object.keys(onlineUsers).length;
            $connBody.empty();
            $("<div>").addClass("energy-toolbar__conn-row").text(info.text).appendTo($connBody);
            $("<div>").addClass("energy-toolbar__conn-muted")
                .text(connLabels.online + ": " + onlineCount).appendTo($connBody);
            $("<div>").addClass("energy-toolbar__conn-muted")
                .text(new Date(lastStatusAt).toLocaleTimeString()).appendTo($connBody);
        }
    }

    function setStatus(next) {
        status = next;
        lastStatusAt = Date.now();
        log(next);
        statusSubscribers.forEach(function (cb) {
            try { cb({ status: next, at: lastStatusAt }); } catch (e) { /* ignore */ }
        });
        renderConn();
    }

    function notifyPresence(payload) {
        presenceSubscribers.forEach(function (cb) {
            try { cb(payload); } catch (e) { /* ignore subscriber errors */ }
        });
        renderConn();
    }

    function setOnline(userId, isOnline) {
        var key = String(userId || "").toLowerCase();
        if (!key) { return; }
        if (isOnline) { onlineUsers[key] = true; } else { delete onlineUsers[key]; }
        notifyPresence({ userId: key, isOnline: !!isOnline });
    }

    function applySnapshot(ids) {
        onlineUsers = {};
        (ids || []).forEach(function (id) { onlineUsers[String(id).toLowerCase()] = true; });
        log("presence-snapshot", (ids || []).length + " online");
        notifyPresence({ snapshot: true });
    }

    function clearPresence() {
        onlineUsers = {};
        notifyPresence({ snapshot: true });
    }

    function setBadge(count) {
        unread = Math.max(0, count | 0);
        if (unread > 0) {
            $badge.text(unread > 99 ? "99+" : unread).prop("hidden", false);
            $bell.addClass("has-unread");
        } else {
            $badge.prop("hidden", true);
            $bell.removeClass("has-unread");
        }
    }

    function refreshUnread() {
        $.getJSON(cfg.unreadCountUrl)
            .done(function (res) { setBadge(res && res.count ? res.count : 0); })
            .fail(function () { /* keep current badge */ });
    }

    function addNotification(message) {
        var text = (cfg.newMessageFrom || "{0}").replace("{0}", message.senderName || "");
        var $empty = $list.find(".is-empty");
        if ($empty.length) { $empty.remove(); }
        var $item = $("<li>").addClass("energy-toolbar__notify-item");
        $("<span>").addClass("energy-toolbar__notify-item-title").text(text).appendTo($item);
        $("<span>").addClass("energy-toolbar__notify-item-text").text(message.text || "").appendTo($item);
        $item.on("click", function () { window.location.href = cfg.pageUrl; });
        $list.prepend($item);
        // Cap the list to the 20 most recent.
        $list.children().slice(20).remove();
    }

    function togglePanel(show) {
        var visible = !$panel.prop("hidden");
        var next = typeof show === "boolean" ? show : !visible;
        $panel.prop("hidden", !next);
    }

    // Bottom-center, auto-dismissing toast shown when a new message arrives.
    var $toastHost = null;
    function ensureToastHost() {
        if ($toastHost && $toastHost.length) { return $toastHost; }
        $toastHost = $("<div>").addClass("energy-chat-toasts").attr("aria-live", "polite").appendTo(document.body);
        return $toastHost;
    }
    function showCenterToast(message) {
        var host = ensureToastHost();
        var title = (cfg.newMessageFrom || "{0}").replace("{0}", message.senderName || "");
        var body = message.text
            || (message.hasAttachment ? (cfg.attachmentLabel || "Attachment") : "");
        var $toast = $("<div>").addClass("energy-chat-toast");
        var $avatar = $("<span>").addClass("energy-chat-toast__avatar");
        if (message.senderHasProfileImage && message.senderId) {
            $("<img>").attr({ src: "/chat/avatar/" + message.senderId, alt: "" }).appendTo($avatar);
        } else {
            $avatar.addClass("is-initials").text(String(title || "?").trim().charAt(0).toUpperCase() || "?");
        }
        $avatar.appendTo($toast);
        var $meta = $("<div>").addClass("energy-chat-toast__meta");
        $("<div>").addClass("energy-chat-toast__title").text(title).appendTo($meta);
        if (body) { $("<div>").addClass("energy-chat-toast__text").text(body).appendTo($meta); }
        $meta.appendTo($toast);
        $toast.on("click", function () { window.location.href = cfg.pageUrl; });
        host.append($toast);

        // Animate in, then auto-dismiss after a few seconds.
        requestAnimationFrame(function () { $toast.addClass("is-visible"); });
        var hide = function () {
            $toast.removeClass("is-visible");
            setTimeout(function () { $toast.remove(); }, 300);
        };
        var timer = setTimeout(hide, 5000);
        $toast.on("mouseenter", function () { clearTimeout(timer); });
        $toast.on("mouseleave", function () { timer = setTimeout(hide, 2500); });

        // Keep at most 3 stacked toasts.
        host.children().slice(0, -3).remove();
    }

    $bell.on("click", function (e) {
        e.stopPropagation();
        togglePanel();
    });
    $(document).on("click", function () {
        togglePanel(false);
        if ($connPanel.length) { $connPanel.prop("hidden", true); }
    });
    $panel.on("click", function (e) { e.stopPropagation(); });

    // Connection status indicator: click toggles a small details popover.
    $conn.on("click", function (e) {
        e.stopPropagation();
        if ($connPanel.length === 0) { return; }
        var willShow = $connPanel.prop("hidden");
        renderConn();
        $connPanel.prop("hidden", !willShow);
        $panel.prop("hidden", true);
    });
    $connPanel.on("click", function (e) { e.stopPropagation(); });

    function dispatch(message) {
        subscribers.forEach(function (cb) {
            try { cb(message); } catch (err) { /* ignore subscriber errors */ }
        });
    }

    function invokeSafe() {
        var args = Array.prototype.slice.call(arguments);
        try {
            if (connection && connection.state === "Connected") {
                return connection.invoke.apply(connection, args).catch(function () { /* best effort */ });
            }
        } catch (e) { /* never break the UI */ }
        return Promise.resolve();
    }

    function fire(list, payload) {
        list.forEach(function (cb) { try { cb(payload); } catch (e) { /* ignore */ } });
    }

    function handleIncoming(message) {
        if (!message) { return; }
        var fromMe = (String(message.senderId || "").toLowerCase() === me);

        // Let any open chat page react first (it may append + mark read).
        dispatch(message);

        if (fromMe) { return; }

        // Incoming message: bump the bell, drop a notification entry + toast.
        setBadge(unread + 1);
        addNotification(message);
        showCenterToast(message);
    }

    window.EnergyChat = {
        subscribe: function (cb) { if (typeof cb === "function") { subscribers.push(cb); } },
        refreshUnread: refreshUnread,
        onPresence: function (cb) { if (typeof cb === "function") { presenceSubscribers.push(cb); } },
        onTyping: function (cb) { if (typeof cb === "function") { typingSubscribers.push(cb); } },
        onGroupInvite: function (cb) { if (typeof cb === "function") { groupInviteSubscribers.push(cb); } },
        onGroupChanged: function (cb) { if (typeof cb === "function") { groupChangedSubscribers.push(cb); } },
        onMessageDeleted: function (cb) { if (typeof cb === "function") { msgDeletedSubscribers.push(cb); } },
        onMessageReacted: function (cb) { if (typeof cb === "function") { msgReactedSubscribers.push(cb); } },
        onMessagesRead: function (cb) { if (typeof cb === "function") { msgReadSubscribers.push(cb); } },
        onCall: function (cb) { if (typeof cb === "function") { callSubscribers.push(cb); } },
        callUser: function (targetUserId, callerName, offer) { return invokeSafe("CallUser", targetUserId, callerName, offer); },
        answerCall: function (targetUserId, answer) { return invokeSafe("AnswerCall", targetUserId, answer); },
        sendIce: function (targetUserId, candidate) { return invokeSafe("SendIce", targetUserId, candidate); },
        endCall: function (targetUserId) { return invokeSafe("EndCall", targetUserId); },
        sendTyping: function (recipientId, isTyping) {
            try {
                if (connection && connection.state === "Connected" && recipientId) {
                    connection.invoke("Typing", String(recipientId), !!isTyping).catch(function () { /* best effort */ });
                }
            } catch (e) { /* never break the UI on a transport hiccup */ }
        },
        isOnline: function (userId) { return !!onlineUsers[String(userId || "").toLowerCase()]; },
        getOnlineUsers: function () { return Object.keys(onlineUsers); },
        onStatus: function (cb) {
            if (typeof cb === "function") {
                statusSubscribers.push(cb);
                try { cb({ status: status, at: lastStatusAt }); } catch (e) { /* ignore */ }
            }
        },
        status: function () { return status; },
        // Raw SignalR connection state ("Connected", "Connecting", "Reconnecting",
        // "Disconnected", "Disconnecting") for console diagnostics.
        connectionState: function () { return connection ? connection.state : "None"; },
        // One-shot snapshot of everything useful for debugging from the console.
        debug: function () {
            return {
                status: status,
                connectionState: connection ? connection.state : "None",
                lastStatusAt: new Date(lastStatusAt).toLocaleTimeString(),
                onlineCount: Object.keys(onlineUsers).length,
                onlineUsers: Object.keys(onlineUsers),
                hubUrl: cfg.hubUrl,
                me: me
            };
        }
    };

    function connect() {
        if (!window.signalR) { log("signalr-missing"); return; }
        connection = new signalR.HubConnectionBuilder()
            .withUrl(cfg.hubUrl)
            // Retry quickly at first, then back off. When automatic reconnect
            // ultimately gives up, onclose() restarts the whole cycle, so the
            // client keeps trying to connect for the entire session.
            .withAutomaticReconnect([0, 2000, 5000, 10000, 15000, 30000])
            .configureLogging(signalR.LogLevel.Warning)
            .build();

        connection.on("ReceiveMessage", handleIncoming);
        connection.on("PresenceSnapshot", applySnapshot);
        connection.on("PresenceChanged", function (p) {
            if (!p) { return; }
            // Tolerate either camelCase or PascalCase payloads.
            var uid = (p.userId != null) ? p.userId : p.UserId;
            var on = (p.isOnline != null) ? p.isOnline : p.IsOnline;
            setOnline(uid, on);
        });
        connection.on("TypingChanged", function (t) {
            if (!t) { return; }
            var norm = {
                fromUserId: (t.fromUserId != null) ? t.fromUserId : t.FromUserId,
                isTyping: (t.isTyping != null) ? t.isTyping : t.IsTyping
            };
            typingSubscribers.forEach(function (cb) { try { cb(norm); } catch (e) { /* ignore */ } });
        });
        connection.on("GroupInvite", function (p) {
            if (!p) { return; }
            var norm = {
                groupId: (p.groupId != null) ? p.groupId : p.GroupId,
                groupName: (p.groupName != null) ? p.groupName : p.GroupName
            };
            groupInviteSubscribers.forEach(function (cb) { try { cb(norm); } catch (e) { /* ignore */ } });
            // Surface a toast so the user notices even when not on the chat page.
            showCenterToast({ senderName: norm.groupName || "Grup", text: "Yeni grup daveti" });
        });
        connection.on("GroupChanged", function (p) {
            if (!p) { return; }
            var norm = { groupId: (p.groupId != null) ? p.groupId : p.GroupId };
            groupChangedSubscribers.forEach(function (cb) { try { cb(norm); } catch (e) { /* ignore */ } });
        });
        connection.on("MessageDeleted", function (p) { fire(msgDeletedSubscribers, p || {}); });
        connection.on("MessageReacted", function (p) { fire(msgReactedSubscribers, p || {}); });
        connection.on("MessagesRead", function (p) { fire(msgReadSubscribers, p || {}); });
        connection.on("CallOffer", function (p) { fire(callSubscribers, { type: "offer", data: p || {} }); });
        connection.on("CallAnswered", function (p) { fire(callSubscribers, { type: "answer", data: p || {} }); });
        connection.on("CallIce", function (p) { fire(callSubscribers, { type: "ice", data: p || {} }); });
        connection.on("CallEnded", function (p) { fire(callSubscribers, { type: "ended", data: p || {} }); });

        connection.onreconnecting(function (err) {
            clearPresence();
            setStatus("reconnecting");
            log("reconnecting", err && err.message);
        });
        connection.onreconnected(function () {
            setStatus("connected");
            refreshUnread();
            requestPresence();
        });
        connection.onclose(function (err) {
            clearPresence();
            setStatus("closed");
            log("closed", err && err.message);
            // Automatic reconnect exhausted (or start() failed): keep trying so a
            // dropped backend eventually heals without a page reload.
            setTimeout(start, 5000);
        });

        start();
    }

    function requestPresence() {
        try {
            if (connection && connection.state === "Connected") {
                connection.invoke("RequestPresence").catch(function () { /* best effort */ });
            }
        } catch (e) { /* never break the page */ }
    }

    function start() {
        if (!connection) { return; }
        setStatus("connecting");
        connection.start()
            .then(function () { setStatus("connected"); refreshUnread(); requestPresence(); })
            .catch(function (err) {
                setStatus("error");
                log("connect-failed", err && err.message);
                // A failed connect must never bubble up and disturb the page.
                setTimeout(start, 5000);
            });
    }

    renderConn();
    refreshUnread();
    connect();
})(window, jQuery);

