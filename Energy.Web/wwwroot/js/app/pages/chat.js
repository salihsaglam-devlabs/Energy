(function (window, $) {
    "use strict";

    window.AppPages = window.AppPages || {};

    // Cap attachment size to keep the Base64 round-trip reasonable (10 MB).
    var MAX_ATTACHMENT_BYTES = 10 * 1024 * 1024;

    function readJson($el, attr, fallback) {
        try {
            var raw = $el.attr(attr);
            return raw ? JSON.parse(raw) : fallback;
        } catch (e) {
            return fallback;
        }
    }

    function initials(name) {
        var parts = String(name || "").trim().split(/\s+/).filter(Boolean);
        if (parts.length === 0) { return "?"; }
        if (parts.length === 1) { return parts[0].charAt(0).toUpperCase(); }
        return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
    }

    function avatarUrlFor(userId) {
        return "/chat/avatar/" + userId;
    }

    function attachmentUrlFor(messageId) {
        return "/chat/messages/" + messageId + "/attachment";
    }

    function toChatItem(m, me) {
        var hasImage = m.senderId === me.id ? me.hasImage : !!m.senderHasProfileImage;
        var item = {
            id: m.id,
            timestamp: m.sentAt ? new Date(m.sentAt) : new Date(),
            author: {
                id: m.senderId,
                name: m.senderName || (m.senderId === me.id ? me.name : ""),
                avatarUrl: hasImage ? avatarUrlFor(m.senderId) : undefined
            },
            text: m.text || ""
        };
        if (m.hasAttachment) {
            item.attachment = {
                url: attachmentUrlFor(m.id),
                name: m.attachmentFileName || "file",
                contentType: m.attachmentContentType || "application/octet-stream"
            };
        }
        return item;
    }

    // Renders the body of a message bubble: text plus an optional attachment
    // (inline preview for images, a download link otherwise).
    function buildMessageContent(msg) {
        var $wrap = $("<div>").addClass("energy-chat__msg");
        if (msg.text) {
            $("<div>").addClass("energy-chat__msg-text").text(msg.text).appendTo($wrap);
        }
        var att = msg.attachment;
        if (att) {
            var isImage = String(att.contentType || "").indexOf("image/") === 0;
            if (isImage) {
                var $imgLink = $("<a>")
                    .addClass("energy-chat__att energy-chat__att--image")
                    .attr({ href: att.url, target: "_blank", rel: "noopener" });
                $("<img>").attr({ src: att.url, alt: att.name }).appendTo($imgLink);
                $imgLink.appendTo($wrap);
            } else {
                var $fileLink = $("<a>")
                    .addClass("energy-chat__att energy-chat__att--file")
                    .attr({ href: att.url, download: att.name });
                $("<i>").addClass("fa-solid fa-paperclip").appendTo($fileLink);
                $("<span>").text(att.name).appendTo($fileLink);
                $fileLink.appendTo($wrap);
            }
        }
        return $wrap;
    }

    window.AppPages.Chat = {
        init: function () {
            var $root = $(".energy-chat");
            if ($root.length === 0) { return; }

            var labels = readJson($root, "data-labels", {});
            var me = {
                id: $root.attr("data-user-id"),
                name: $root.attr("data-user-name") || "",
                hasImage: $root.attr("data-user-has-image") === "true"
            };

            var state = { peerId: null, peer: null, items: [], contacts: [] };
            var contactList, chat;
            var typingDebounce = null;     // throttles outgoing "typing" pings
            var peerTypingTimer = null;    // auto-clears the incoming typing flag

            var $windowWrap = $(".energy-chat__window-wrap");
            var $attachBtn = $("#chat-attach-btn");
            var $attachInput = $("#chat-attach-input");
            var $peer = $("#chat-peer");
            var $peerAvatar = $peer.find(".energy-chat__peer-avatar");
            var $peerName = $peer.find(".energy-chat__peer-name");
            var $peerStatus = $peer.find(".energy-chat__peer-status");

            var ENERGY = window.EnergyChat || {};
            function isOnline(userId) { return typeof ENERGY.isOnline === "function" && ENERGY.isOnline(userId); }

            function renderContacts() {
                contactList.option("dataSource", state.contacts);
            }

            function loadContacts() {
                return $.getJSON("/chat/contacts").done(function (data) {
                    state.contacts = data || [];
                    renderContacts();
                });
            }

            function setItems(items) {
                state.items = items;
                chat.option("items", items);
            }

            function appendMessage(m) {
                if (!state.items.some(function (i) { return i.id === m.id; })) {
                    setItems(state.items.concat([toChatItem(m, me)]));
                }
            }

            // ----- Peer header (name + online/offline + typing) -----------------
            function renderPeerStatus() {
                if (!state.peer) { return; }
                if (state.peer.isTyping) {
                    $peerStatus.text(labels.typing || "typing...").addClass("is-typing").removeClass("is-online is-offline");
                    return;
                }
                var online = isOnline(state.peer.id);
                $peerStatus
                    .text(online ? (labels.online || "Online") : (labels.offline || "Offline"))
                    .removeClass("is-typing")
                    .toggleClass("is-online", online)
                    .toggleClass("is-offline", !online);
            }

            function renderPeer() {
                if (!state.peer) { return; }
                $peerName.text(state.peer.fullName || state.peer.userName || "");
                $peerAvatar.empty();
                if (state.peer.hasProfileImage) {
                    $("<img>").attr({ src: avatarUrlFor(state.peer.id), alt: state.peer.fullName || state.peer.userName }).appendTo($peerAvatar);
                    $peerAvatar.removeClass("is-initials");
                } else {
                    $peerAvatar.addClass("is-initials").text(initials(state.peer.fullName || state.peer.userName));
                }
                renderPeerStatus();
            }

            function setPeerTyping(isTyping) {
                if (!state.peer) { return; }
                state.peer.isTyping = !!isTyping;
                renderPeerStatus();
                // dxChat's native "typing" bubble.
                chat.option("typingUsers", isTyping ? [{ id: state.peer.id, name: state.peer.fullName || state.peer.userName }] : []);
                clearTimeout(peerTypingTimer);
                if (isTyping) {
                    peerTypingTimer = setTimeout(function () { setPeerTyping(false); }, 6000);
                }
            }

            function markRead(peerId) {
                var token = $("meta[name='csrf-token']").attr("content");
                return $.ajax({
                    url: "/chat/conversation/" + peerId + "/read",
                    method: "POST",
                    headers: token ? { "RequestVerificationToken": token } : {},
                    credentials: "same-origin"
                }).always(function () {
                    if (window.EnergyChat) { window.EnergyChat.refreshUnread(); }
                    // Clear the unread badge on the contact locally.
                    state.contacts.forEach(function (c) { if (c.id === peerId) { c.unreadCount = 0; } });
                    renderContacts();
                });
            }

            function openConversation(peerId) {
                state.peerId = peerId;
                state.peer = state.contacts.filter(function (c) { return c.id === peerId; })[0] || { id: peerId };
                state.peer.isTyping = false;
                $windowWrap.prop("hidden", false);
                renderPeer();
                chat.option("typingUsers", []);
                $.getJSON("/chat/conversation/" + peerId).done(function (data) {
                    setItems((data || []).map(function (m) { return toChatItem(m, me); }));
                    markRead(peerId);
                });
            }

            // Posts a message (text and/or attachment). The saved row is appended
            // immediately so the sender sees it without waiting for SignalR/refresh;
            // the realtime echo is de-duplicated by id.
            function postMessage(payload) {
                if (!state.peerId) { return; }
                var token = $("meta[name='csrf-token']").attr("content");
                $.ajax({
                    url: "/chat/messages",
                    method: "POST",
                    contentType: "application/json",
                    headers: token ? { "RequestVerificationToken": token } : {},
                    data: JSON.stringify(payload)
                }).done(function (envelope) {
                    var msg = envelope && (envelope.data || envelope.Data);
                    var ok = envelope && (envelope.isSuccess || envelope.IsSuccess);
                    if (ok && msg && state.peerId &&
                        (msg.recipientId === state.peerId || msg.senderId === state.peerId)) {
                        appendMessage(msg);
                    }
                }).fail(function () {
                    if (window.AppNotify) { window.AppNotify.error("Error"); }
                });
            }

            function sendMessage(text) {
                if (!text) { return; }
                postMessage({ recipientId: state.peerId, text: text });
            }

            function sendAttachment(file) {
                if (!state.peerId || !file) { return; }
                if (file.size > MAX_ATTACHMENT_BYTES) {
                    if (window.AppNotify) { window.AppNotify.error(labels.attachFile || "File too large"); }
                    return;
                }
                var reader = new FileReader();
                reader.onload = function () {
                    var result = String(reader.result || "");
                    var base64 = result.indexOf(",") >= 0 ? result.split(",")[1] : result;
                    postMessage({
                        recipientId: state.peerId,
                        text: "",
                        attachmentFileName: file.name,
                        attachmentContentType: file.type || "application/octet-stream",
                        attachmentContentBase64: base64 || ""
                    });
                };
                reader.readAsDataURL(file);
            }

            contactList = $("#chat-contacts").dxList({
                dataSource: [],
                keyExpr: "id",
                selectionMode: "single",
                searchEnabled: true,
                searchExpr: ["fullName", "userName"],
                noDataText: labels.searchContacts,
                onItemClick: function (e) { openConversation(e.itemData.id); },
                itemTemplate: function (data) {
                    var $row = $("<div>").addClass("energy-chat__contact");
                    var online = isOnline(data.id);
                    var $avatar = $("<span>").addClass("energy-chat__contact-avatar");
                    if (data.hasProfileImage) {
                        $("<img>").attr({ src: avatarUrlFor(data.id), alt: data.fullName || data.userName }).appendTo($avatar);
                    } else {
                        $avatar.addClass("is-initials").text(initials(data.fullName || data.userName));
                    }
                    // Presence dot overlaid on the avatar.
                    $("<span>")
                        .addClass("energy-chat__presence-dot")
                        .toggleClass("is-online", online)
                        .toggleClass("is-offline", !online)
                        .appendTo($avatar);
                    $avatar.appendTo($row);

                    var $meta = $("<span>").addClass("energy-chat__contact-meta");
                    $("<span>").addClass("energy-chat__contact-name").text(data.fullName || data.userName).appendTo($meta);
                    $("<span>")
                        .addClass("energy-chat__contact-status")
                        .toggleClass("is-online", online)
                        .toggleClass("is-offline", !online)
                        .text(online ? (labels.online || "Online") : (labels.offline || "Offline"))
                        .appendTo($meta);
                    $meta.appendTo($row);

                    if (data.unreadCount > 0) {
                        $("<span>").addClass("energy-chat__contact-badge").text(data.unreadCount).appendTo($row);
                    }
                    return $row;
                }
            }).dxList("instance");

            chat = $("#chat-window").dxChat({
                user: { id: me.id, name: me.name, avatarUrl: me.hasImage ? avatarUrlFor(me.id) : undefined },
                items: [],
                reloadOnChange: false,
                showAvatar: true,
                showUserName: true,
                showMessageTimestamp: true,
                showDayHeaders: true,
                typingUsers: [],
                messageTemplate: function () {
                    // DevExtreme passes the template data first and the container
                    // element last; stay tolerant of both calling conventions.
                    var args = Array.prototype.slice.call(arguments);
                    var data = args[0];
                    var container = args[args.length - 1];
                    var msg = (data && data.message) ? data.message : data;
                    $(container).append(buildMessageContent(msg || {}));
                },
                onMessageEntered: function (e) {
                    notifyTyping(false);
                    sendMessage(e.message && e.message.text);
                },
                onTypingStart: function () { notifyTyping(true); },
                onTypingEnd: function () { notifyTyping(false); }
            }).dxChat("instance");

            // Throttle outgoing typing pings; always send a trailing "stopped".
            function notifyTyping(isTyping) {
                if (!state.peerId || typeof ENERGY.sendTyping !== "function") { return; }
                if (isTyping) {
                    ENERGY.sendTyping(state.peerId, true);
                    clearTimeout(typingDebounce);
                    typingDebounce = setTimeout(function () { ENERGY.sendTyping(state.peerId, false); }, 3000);
                } else {
                    clearTimeout(typingDebounce);
                    ENERGY.sendTyping(state.peerId, false);
                }
            }

            // File sharing: a button next to the chat opens the file picker and
            // sends the selected file as an attachment message.
            $attachBtn.on("click", function () { $attachInput.trigger("click"); });
            $attachInput.on("change", function () {
                var file = this.files && this.files[0];
                if (file) { sendAttachment(file); }
                $(this).val("");
            });

            loadContacts();

            // Real-time: append messages that belong to the open conversation and
            // refresh the contact ordering/badges as traffic arrives.
            if (window.EnergyChat && window.EnergyChat.subscribe) {
                window.EnergyChat.subscribe(function (m) {
                    var inOpenConversation = state.peerId &&
                        (m.senderId === state.peerId || m.recipientId === state.peerId);

                    if (inOpenConversation) {
                        appendMessage(m);
                        if (m.senderId === state.peerId) {
                            // A real message implicitly means they stopped typing.
                            setPeerTyping(false);
                            markRead(state.peerId);
                        }
                    }
                    // Refresh the sidebar to reflect new last-message ordering/unread.
                    loadContacts();
                });
            }

            // Online/offline presence: repaint contact tags and the conversation
            // header whenever a peer connects or disconnects.
            if (typeof ENERGY.onPresence === "function") {
                ENERGY.onPresence(function (p) {
                    try { contactList.repaint(); } catch (e) { /* list not ready yet */ }
                    if (state.peer && (p.snapshot || p.userId === String(state.peer.id).toLowerCase())) {
                        renderPeerStatus();
                    }
                });
            }

            // Typing indicators from the current conversation peer.
            if (typeof ENERGY.onTyping === "function") {
                ENERGY.onTyping(function (t) {
                    if (!t || !state.peer) { return; }
                    if (String(t.fromUserId).toLowerCase() === String(state.peer.id).toLowerCase()) {
                        setPeerTyping(!!t.isTyping);
                    }
                });
            }
        }
    };
})(window, jQuery);
