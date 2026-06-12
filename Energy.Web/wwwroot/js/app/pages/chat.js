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
        // Carry the full metadata so the bubble can render quotes, reactions,
        // read-receipts and the per-message action menu.
        item.meta = {
            id: m.id,
            senderId: m.senderId,
            isMine: m.senderId === me.id,
            isRead: !!m.isRead,
            groupId: m.groupId || null,
            replyToText: m.replyToText || "",
            replyToSender: m.replyToSenderName || "",
            reactions: m.reactions || []
        };
        return item;
    }

    var REACTION_EMOJIS = ["👍", "❤️", "😂", "😮", "😢", "🙏"];

    // Renders the body of a message bubble: optional reply quote, text, optional
    // attachment, reaction chips, read-receipt ticks and the action menu.
    function buildMessageContent(msg) {
        var meta = (msg && msg.meta) || {};
        var $wrap = $("<div>").addClass("energy-chat__msg").attr("data-msg-id", meta.id || "");

        if (meta.replyToText || meta.replyToSender) {
            var $quote = $("<div>").addClass("energy-chat__quote");
            $("<span>").addClass("energy-chat__quote-sender").text(meta.replyToSender || "").appendTo($quote);
            $("<span>").addClass("energy-chat__quote-text").text(meta.replyToText || "").appendTo($quote);
            $quote.appendTo($wrap);
        }

        if (msg.text) {
            $("<div>").addClass("energy-chat__msg-text").text(msg.text).appendTo($wrap);
        }

        var att = msg.attachment;
        if (att) {
            var ct = String(att.contentType || "");
            var isImage = ct.indexOf("image/") === 0;
            var isAudio = ct.indexOf("audio/") === 0;
            if (isImage) {
                var $imgLink = $("<a>")
                    .addClass("energy-chat__att energy-chat__att--image")
                    .attr({ href: att.url, target: "_blank", rel: "noopener" });
                $("<img>").attr({ src: att.url, alt: att.name }).appendTo($imgLink);
                $imgLink.appendTo($wrap);
            } else if (isAudio) {
                var $audioWrap = $("<div>").addClass("energy-chat__att energy-chat__att--audio");
                $("<audio>").attr({ controls: "controls", preload: "metadata", src: att.url }).appendTo($audioWrap);
                $audioWrap.appendTo($wrap);
            } else {
                var $fileLink = $("<a>")
                    .addClass("energy-chat__att energy-chat__att--file")
                    .attr({ href: att.url, download: att.name });
                $("<i>").addClass("fa-solid fa-paperclip").appendTo($fileLink);
                $("<span>").text(att.name).appendTo($fileLink);
                $fileLink.appendTo($wrap);
            }
        }

        // Reaction chips.
        if (meta.reactions && meta.reactions.length) {
            var $reactions = $("<div>").addClass("energy-chat__reactions");
            meta.reactions.forEach(function (r) {
                $("<button>")
                    .addClass("energy-chat__reaction" + (r.reacted ? " is-mine" : ""))
                    .attr({ "data-chat-action": "react", "data-emoji": r.emoji, "data-msg-id": meta.id })
                    .text(r.emoji + " " + r.count)
                    .appendTo($reactions);
            });
            $reactions.appendTo($wrap);
        }

        // Read-receipt ticks for own direct messages.
        if (meta.isMine && !meta.groupId) {
            $("<span>")
                .addClass("energy-chat__ticks" + (meta.isRead ? " is-read" : ""))
                .html(meta.isRead ? '<i class="fa-solid fa-check-double"></i>' : '<i class="fa-solid fa-check"></i>')
                .appendTo($wrap);
        }

        // Per-message action toolbar.
        var $tools = $("<div>").addClass("energy-chat__msg-tools");
        var $react = $("<button>").addClass("energy-chat__msg-action")
            .attr({ "data-chat-action": "palette", "data-msg-id": meta.id, title: "Tepki" })
            .append($("<i>").addClass("fa-regular fa-face-smile"));
        var $palette = $("<span>").addClass("energy-chat__palette");
        REACTION_EMOJIS.forEach(function (e) {
            $("<button>").addClass("energy-chat__palette-emoji")
                .attr({ "data-chat-action": "react", "data-emoji": e, "data-msg-id": meta.id })
                .text(e).appendTo($palette);
        });
        $react.append($palette).appendTo($tools);
        $("<button>").addClass("energy-chat__msg-action")
            .attr({ "data-chat-action": "reply", "data-msg-id": meta.id, title: "Yanıtla" })
            .append($("<i>").addClass("fa-solid fa-reply")).appendTo($tools);
        $("<button>").addClass("energy-chat__msg-action")
            .attr({ "data-chat-action": "forward", "data-msg-id": meta.id, title: "İlet" })
            .append($("<i>").addClass("fa-solid fa-share")).appendTo($tools);
        if (meta.isMine) {
            $("<button>").addClass("energy-chat__msg-action is-danger")
                .attr({ "data-chat-action": "delete", "data-msg-id": meta.id, title: "Sil" })
                .append($("<i>").addClass("fa-solid fa-trash")).appendTo($tools);
        }
        $tools.appendTo($wrap);

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

            var state = {
                mode: "direct",       // "direct" | "group"
                peerId: null, peer: null,
                groupId: null, group: null,
                items: [], contacts: [], groups: [], invites: [],
                replyTo: null         // { id, text, sender }
            };
            var contactList, chat, groupList;
            var typingDebounce = null;     // throttles outgoing "typing" pings
            var peerTypingTimer = null;    // auto-clears the incoming typing flag

            var $windowWrap = $(".energy-chat__window-wrap");
            var $attachBtn = $("#chat-attach-btn");
            var $attachInput = $("#chat-attach-input");
            var $peer = $("#chat-peer");
            var $peerAvatar = $peer.find(".energy-chat__peer-avatar");
            var $peerName = $peer.find(".energy-chat__peer-name");
            var $peerStatus = $peer.find(".energy-chat__peer-status");

            // Mobile drawer + voice recorder elements.
            var $sidebar = $("#chat-sidebar");
            var $backdrop = $("#chat-backdrop");
            var $drawerToggle = $("#chat-drawer-toggle");
            var $drawerClose = $("#chat-drawer-close");
            var $voiceBtn = $("#chat-voice-btn");
            var $recorder = $("#chat-recorder");
            var $recTime = $("#chat-recorder-time");
            var $recCancel = $("#chat-rec-cancel");
            var $recSend = $("#chat-rec-send");

            // Groups UI elements.
            var $tabContacts = $("#chat-tab-contacts");
            var $tabGroups = $("#chat-tab-groups");
            var $paneContacts = $("#chat-pane-contacts");
            var $paneGroups = $("#chat-pane-groups");
            var $invites = $("#chat-invites");
            var $inviteBadge = $("#chat-invite-badge");
            var $newGroupBtn = $("#chat-new-group");
            var $groupManage = $("#chat-group-manage");
            var $groupPopup = $("#chat-group-popup");
            var $membersPopup = $("#chat-members-popup");
            var $forwardPopup = $("#chat-forward-popup");

            // Reply bar + call elements.
            var $replyBar = $("#chat-reply-bar");
            var $replySender = $("#chat-reply-sender");
            var $replyText = $("#chat-reply-text");
            var $replyCancel = $("#chat-reply-cancel");
            var $callBtn = $("#chat-call-btn");

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
                state.mode = "direct";
                state.groupId = null;
                state.group = null;
                state.peerId = peerId;
                state.peer = state.contacts.filter(function (c) { return c.id === peerId; })[0] || { id: peerId };
                state.peer.isTyping = false;
                $windowWrap.prop("hidden", false);
                $groupManage.prop("hidden", true);
                $callBtn.prop("hidden", false);
                cancelReply();
                closeDrawer();
                renderPeer();
                chat.option("typingUsers", []);
                $.getJSON("/chat/conversation/" + peerId).done(function (data) {
                    setItems((data || []).map(function (m) { return toChatItem(m, me); }));
                    markRead(peerId);
                });
            }

            // ----- Target helpers (direct peer vs group) ------------------------
            function hasTarget() { return state.mode === "group" ? !!state.groupId : !!state.peerId; }
            function targetPayload(extra) {
                var base = state.mode === "group" ? { groupId: state.groupId } : { recipientId: state.peerId };
                return $.extend(base, extra || {});
            }
            function messageBelongsHere(msg) {
                return state.mode === "group"
                    ? (msg.groupId && msg.groupId === state.groupId)
                    : (!msg.groupId && (msg.recipientId === state.peerId || msg.senderId === state.peerId));
            }

            // Posts a message (text and/or attachment). The saved row is appended
            // immediately so the sender sees it without waiting for SignalR/refresh;
            // the realtime echo is de-duplicated by id.
            function postMessage(payload) {
                if (!hasTarget()) { return; }
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
                    if (ok && msg && messageBelongsHere(msg)) {
                        appendMessage(msg);
                    }
                }).fail(function () {
                    if (window.AppNotify) { window.AppNotify.error("Error"); }
                });
            }

            function sendMessage(text) {
                if (!text || !hasTarget()) { return; }
                var extra = { text: text };
                if (state.replyTo) { extra.replyToMessageId = state.replyTo.id; }
                postMessage(targetPayload(extra));
                cancelReply();
            }

            function sendAttachment(file) {
                if (!hasTarget() || !file) { return; }
                if (file.size > MAX_ATTACHMENT_BYTES) {
                    if (window.AppNotify) { window.AppNotify.error(labels.attachFile || "File too large"); }
                    return;
                }
                var reader = new FileReader();
                reader.onload = function () {
                    var result = String(reader.result || "");
                    var base64 = result.indexOf(",") >= 0 ? result.split(",")[1] : result;
                    postMessage(targetPayload({
                        text: "",
                        attachmentFileName: file.name,
                        attachmentContentType: file.type || "application/octet-stream",
                        attachmentContentBase64: base64 || ""
                    }));
                };
                reader.readAsDataURL(file);
            }

            // Shared sender for recorded blobs (voice messages). Same pipeline as
            // file attachments: Base64 over the existing /chat/messages endpoint.
            function sendBlob(blob, fileName, contentType) {
                if (!hasTarget() || !blob) { return; }
                if (blob.size > MAX_ATTACHMENT_BYTES) {
                    if (window.AppNotify) { window.AppNotify.error(labels.attachFile || "File too large"); }
                    return;
                }
                var reader = new FileReader();
                reader.onload = function () {
                    var result = String(reader.result || "");
                    var base64 = result.indexOf(",") >= 0 ? result.split(",")[1] : result;
                    postMessage(targetPayload({
                        text: "",
                        attachmentFileName: fileName,
                        attachmentContentType: contentType || blob.type || "application/octet-stream",
                        attachmentContentBase64: base64 || ""
                    }));
                };
                reader.readAsDataURL(blob);
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

            // ----- Mobile drawer (contacts slide-in) ----------------------------
            function openDrawer() {
                $sidebar.addClass("is-open");
                $backdrop.prop("hidden", false);
            }
            function closeDrawer() {
                $sidebar.removeClass("is-open");
                $backdrop.prop("hidden", true);
            }
            $drawerToggle.on("click", openDrawer);
            $drawerClose.on("click", closeDrawer);
            $backdrop.on("click", closeDrawer);

            // ----- Voice messages (MediaRecorder) -------------------------------
            var recorder = { media: null, chunks: [], stream: null, timer: null, startedAt: 0, mime: "" };

            function fmtTime(ms) {
                var s = Math.floor(ms / 1000);
                var m = Math.floor(s / 60);
                s = s % 60;
                return m + ":" + (s < 10 ? "0" + s : s);
            }

            function tickRecorder() {
                $recTime.text(fmtTime(Date.now() - recorder.startedAt));
            }

            function stopTracks() {
                if (recorder.stream) {
                    recorder.stream.getTracks().forEach(function (t) { try { t.stop(); } catch (e) { /* noop */ } });
                }
                recorder.stream = null;
            }

            function resetRecorderUi() {
                clearInterval(recorder.timer);
                recorder.timer = null;
                $recorder.prop("hidden", true);
                $voiceBtn.prop("hidden", false).removeClass("is-recording");
                $recTime.text("0:00");
            }

            function pickAudioMime() {
                if (!window.MediaRecorder || !MediaRecorder.isTypeSupported) { return ""; }
                var candidates = ["audio/webm;codecs=opus", "audio/webm", "audio/ogg;codecs=opus", "audio/mp4"];
                for (var i = 0; i < candidates.length; i++) {
                    if (MediaRecorder.isTypeSupported(candidates[i])) { return candidates[i]; }
                }
                return "";
            }

            function startRecording() {
                if (!hasTarget()) { return; }
                if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia || !window.MediaRecorder) {
                    if (window.AppNotify) { window.AppNotify.error(labels.voiceUnsupported || "Voice recording is not supported."); }
                    return;
                }
                navigator.mediaDevices.getUserMedia({ audio: true }).then(function (stream) {
                    recorder.stream = stream;
                    recorder.mime = pickAudioMime();
                    recorder.media = recorder.mime ? new MediaRecorder(stream, { mimeType: recorder.mime }) : new MediaRecorder(stream);
                    recorder.chunks = [];
                    recorder.cancelled = false;
                    recorder.media.ondataavailable = function (e) { if (e.data && e.data.size > 0) { recorder.chunks.push(e.data); } };
                    recorder.media.onstop = function () {
                        stopTracks();
                        var wasCancelled = recorder.cancelled;
                        var type = recorder.mime || (recorder.chunks[0] && recorder.chunks[0].type) || "audio/webm";
                        var blob = new Blob(recorder.chunks, { type: type });
                        recorder.chunks = [];
                        resetRecorderUi();
                        if (!wasCancelled && blob.size > 0) {
                            var ext = type.indexOf("ogg") >= 0 ? "ogg" : (type.indexOf("mp4") >= 0 ? "m4a" : "webm");
                            sendBlob(blob, "voice-" + Date.now() + "." + ext, type);
                        }
                    };
                    recorder.startedAt = Date.now();
                    recorder.media.start();
                    $voiceBtn.prop("hidden", true).addClass("is-recording");
                    $recorder.prop("hidden", false);
                    tickRecorder();
                    recorder.timer = setInterval(tickRecorder, 250);
                }).catch(function () {
                    if (window.AppNotify) { window.AppNotify.error(labels.voiceDenied || "Microphone access was denied."); }
                });
            }

            function finishRecording(cancel) {
                if (!recorder.media || recorder.media.state === "inactive") { resetRecorderUi(); return; }
                recorder.cancelled = !!cancel;
                try { recorder.media.stop(); } catch (e) { resetRecorderUi(); }
            }

            $voiceBtn.on("click", startRecording);
            $recSend.on("click", function () { finishRecording(false); });
            $recCancel.on("click", function () { finishRecording(true); });

            // ================= Groups =================
            function renderGroupHeader() {
                if (!state.group) { return; }
                $peerName.text(state.group.name || "");
                $peerStatus.text((state.group.memberCount || 0) + " üye").removeClass("is-online is-offline is-typing");
                $peerAvatar.empty().addClass("is-initials").text(initials(state.group.name));
            }

            function openGroup(group) {
                state.mode = "group";
                state.peerId = null;
                state.peer = null;
                state.groupId = group.id;
                state.group = group;
                $windowWrap.prop("hidden", false);
                $groupManage.prop("hidden", false);
                $callBtn.prop("hidden", true);
                cancelReply();
                closeDrawer();
                renderGroupHeader();
                chat.option("typingUsers", []);
                $.getJSON("/chat/groups/" + group.id + "/conversation").done(function (data) {
                    setItems((data || []).map(function (m) { return toChatItem(m, me); }));
                });
            }

            function renderGroups() { groupList.option("dataSource", state.groups); }

            function loadGroups() {
                return $.getJSON("/chat/groups").done(function (data) {
                    state.groups = data || [];
                    renderGroups();
                });
            }

            function renderInvites() {
                $invites.empty();
                var count = state.invites.length;
                if (count > 0) {
                    $inviteBadge.text(count).prop("hidden", false);
                } else {
                    $inviteBadge.prop("hidden", true);
                }
                state.invites.forEach(function (inv) {
                    var $card = $("<div>").addClass("energy-chat__invite");
                    var $info = $("<div>").addClass("energy-chat__invite-info");
                    $("<span>").addClass("energy-chat__invite-name").text(inv.groupName).appendTo($info);
                    $("<span>").addClass("energy-chat__invite-by").text((inv.invitedByName || "") + " davet etti").appendTo($info);
                    $info.appendTo($card);
                    var $actions = $("<div>").addClass("energy-chat__invite-actions");
                    $("<button>").addClass("energy-chat__icon-btn is-primary").attr("title", "Kabul et")
                        .append($("<i>").addClass("fa-solid fa-check"))
                        .on("click", function () { respondInvite(inv.groupId, true); })
                        .appendTo($actions);
                    $("<button>").addClass("energy-chat__icon-btn is-danger").attr("title", "Reddet")
                        .append($("<i>").addClass("fa-solid fa-xmark"))
                        .on("click", function () { respondInvite(inv.groupId, false); })
                        .appendTo($actions);
                    $actions.appendTo($card);
                    $card.appendTo($invites);
                });
            }

            function loadInvites() {
                return $.getJSON("/chat/groups/invites").done(function (data) {
                    state.invites = data || [];
                    renderInvites();
                });
            }

            function postJson(url, body) {
                var token = $("meta[name='csrf-token']").attr("content");
                return $.ajax({
                    url: url, method: "POST", contentType: "application/json",
                    headers: token ? { "RequestVerificationToken": token } : {},
                    data: JSON.stringify(body || {})
                });
            }

            function respondInvite(groupId, accept) {
                postJson("/chat/groups/" + groupId + "/respond", { accept: accept }).always(function () {
                    loadInvites();
                    loadGroups();
                });
            }

            groupList = $("#chat-groups").dxList({
                dataSource: [],
                keyExpr: "id",
                selectionMode: "single",
                noDataText: "Grup yok",
                onItemClick: function (e) { openGroup(e.itemData); },
                itemTemplate: function (data) {
                    var $row = $("<div>").addClass("energy-chat__contact");
                    var $avatar = $("<span>").addClass("energy-chat__contact-avatar is-initials energy-chat__group-avatar").text(initials(data.name));
                    $avatar.appendTo($row);
                    var $meta = $("<span>").addClass("energy-chat__contact-meta");
                    $("<span>").addClass("energy-chat__contact-name").text(data.name).appendTo($meta);
                    $("<span>").addClass("energy-chat__contact-status").text((data.memberCount || 0) + " üye").appendTo($meta);
                    $meta.appendTo($row);
                    return $row;
                }
            }).dxList("instance");

            // ----- Tab switching ------------------------------------------------
            function switchTab(tab) {
                var groups = tab === "groups";
                $tabContacts.toggleClass("is-active", !groups);
                $tabGroups.toggleClass("is-active", groups);
                $paneContacts.prop("hidden", groups);
                $paneGroups.prop("hidden", !groups);
                if (groups) { loadGroups(); loadInvites(); }
            }
            $tabContacts.on("click", function () { switchTab("contacts"); });
            $tabGroups.on("click", function () { switchTab("groups"); });

            // ----- Create-group popup ------------------------------------------
            function buildMemberPicker($container, preselected) {
                return $("<div>").appendTo($container).dxList({
                    dataSource: state.contacts,
                    keyExpr: "id",
                    height: 260,
                    searchEnabled: true,
                    searchExpr: ["fullName", "userName"],
                    showSelectionControls: true,
                    selectionMode: "multiple",
                    selectedItemKeys: preselected || [],
                    itemTemplate: function (d) {
                        return $("<span>").text(d.fullName || d.userName);
                    }
                }).dxList("instance");
            }

            var groupPopup = $groupPopup.dxPopup({
                title: "Yeni grup",
                width: 380, height: 480, hideOnOutsideClick: true, visible: false,
                contentTemplate: function (content) {
                    var $c = $(content);
                    $("<label>").addClass("energy-chat__field-label").text("Grup adı").appendTo($c);
                    var nameBox = $("<div>").appendTo($c).dxTextBox({ placeholder: "Grup adı" }).dxTextBox("instance");
                    $("<label>").addClass("energy-chat__field-label").text("Üyeler").appendTo($c);
                    var picker = buildMemberPicker($c, []);
                    var $btn = $("<div>").css("margin-top", "12px").appendTo($c);
                    $btn.dxButton({
                        text: "Oluştur", type: "default", width: "100%",
                        onClick: function () {
                            var name = (nameBox.option("value") || "").trim();
                            if (!name) { if (window.AppNotify) { window.AppNotify.error("Grup adı gerekli"); } return; }
                            postJson("/chat/groups", { name: name, memberUserIds: picker.option("selectedItemKeys") || [] })
                                .done(function () {
                                    groupPopup.hide();
                                    loadGroups();
                                    if (window.AppNotify) { window.AppNotify.success("Grup oluşturuldu"); }
                                })
                                .fail(function () { if (window.AppNotify) { window.AppNotify.error("Hata"); } });
                        }
                    });
                }
            }).dxPopup("instance");
            $newGroupBtn.on("click", function () { groupPopup.show(); });

            // ----- Members / invite popup --------------------------------------
            var membersPopup = $membersPopup.dxPopup({
                title: "Grup üyeleri", width: 380, height: 480, hideOnOutsideClick: true, visible: false
            }).dxPopup("instance");

            $groupManage.on("click", function () {
                if (!state.groupId) { return; }
                var gid = state.groupId;
                membersPopup.option("contentTemplate", function (content) {
                    var $c = $(content);
                    $("<div>").addClass("energy-chat__field-label").text("Üyeler").appendTo($c);
                    var $members = $("<div>").appendTo($c);
                    $.getJSON("/chat/groups/" + gid + "/members").done(function (list) {
                        $members.dxList({
                            dataSource: list || [],
                            height: 180,
                            itemTemplate: function (d) {
                                var label = (d.fullName || d.userName) + (d.isOwner ? " (sahip)" : (d.status === 0 ? " (bekliyor)" : ""));
                                return $("<span>").text(label);
                            }
                        });
                    });
                    $("<div>").addClass("energy-chat__field-label").text("Üye ekle").appendTo($c);
                    var picker = buildMemberPicker($c, []);
                    $("<div>").css("margin-top", "12px").appendTo($c).dxButton({
                        text: "Davet et", type: "default", width: "100%",
                        onClick: function () {
                            var ids = picker.option("selectedItemKeys") || [];
                            if (ids.length === 0) { return; }
                            postJson("/chat/groups/" + gid + "/invite", { userIds: ids })
                                .done(function () { membersPopup.hide(); if (window.AppNotify) { window.AppNotify.success("Davet gönderildi"); } })
                                .fail(function () { if (window.AppNotify) { window.AppNotify.error("Hata"); } });
                        }
                    });
                });
                membersPopup.show();
            });

            // ----- Message actions: reply / delete / forward / react ----------
            function cancelReply() {
                state.replyTo = null;
                $replyBar.prop("hidden", true);
            }
            function startReply(id) {
                var item = state.items.filter(function (i) { return i.id === id; })[0];
                if (!item) { return; }
                state.replyTo = {
                    id: id,
                    text: item.text || (item.attachment ? item.attachment.name : ""),
                    sender: (item.author && item.author.name) || ""
                };
                $replySender.text(state.replyTo.sender);
                $replyText.text(state.replyTo.text);
                $replyBar.prop("hidden", false);
            }
            $replyCancel.on("click", cancelReply);

            function deleteMessage(id) {
                if (!window.confirm("Mesaj silinsin mi?")) { return; }
                var token = $("meta[name='csrf-token']").attr("content");
                $.ajax({
                    url: "/chat/messages/" + id, method: "DELETE",
                    headers: token ? { "RequestVerificationToken": token } : {}
                });
                // The bubble is removed when the MessageDeleted event arrives.
            }
            function reactMessage(id, emoji) {
                postJson("/chat/messages/" + id + "/react", { emoji: emoji });
                // Reaction chips update when the MessageReacted event arrives.
            }
            function removeItem(id) {
                setItems(state.items.filter(function (i) { return i.id !== id; }));
            }
            function applyReactionUpdate(msg) {
                for (var i = 0; i < state.items.length; i++) {
                    if (state.items[i].id === msg.id) {
                        var copy = state.items.slice();
                        copy[i] = toChatItem(msg, me);
                        setItems(copy);
                        return;
                    }
                }
            }
            function markOwnRead(readerId) {
                if (state.mode !== "direct" || !state.peerId) { return; }
                if (String(readerId).toLowerCase() !== String(state.peerId).toLowerCase()) { return; }
                var changed = false;
                state.items.forEach(function (it) {
                    if (it.meta && it.meta.isMine && !it.meta.isRead) { it.meta.isRead = true; changed = true; }
                });
                if (changed) { chat.option("items", state.items.slice()); }
            }

            // Delegated handler for all in-bubble action buttons.
            $("#chat-window").on("click", "[data-chat-action]", function (e) {
                e.preventDefault();
                e.stopPropagation();
                var $b = $(this);
                var action = $b.attr("data-chat-action");
                var id = $b.attr("data-msg-id");
                if (action === "palette") { $b.toggleClass("is-open"); return; }
                if (action === "react") { reactMessage(id, $b.attr("data-emoji")); }
                else if (action === "reply") { startReply(id); }
                else if (action === "forward") { openForward(id); }
                else if (action === "delete") { deleteMessage(id); }
            });

            // ----- Forward popup ----------------------------------------------
            var forwardPopup = $forwardPopup.dxPopup({
                title: "İlet", width: 340, height: 480, hideOnOutsideClick: true, visible: false
            }).dxPopup("instance");

            function doForward(id, target) {
                postJson("/chat/messages/" + id + "/forward", target).done(function () {
                    forwardPopup.hide();
                    if (window.AppNotify) { window.AppNotify.success("İletildi"); }
                });
            }
            function openForward(id) {
                forwardPopup.option("contentTemplate", function (content) {
                    var $c = $(content);
                    $("<div>").addClass("energy-chat__field-label").text("Kişiler").appendTo($c);
                    $("<div>").appendTo($c).dxList({
                        dataSource: state.contacts, keyExpr: "id", height: 170, searchEnabled: true, searchExpr: ["fullName", "userName"],
                        itemTemplate: function (d) { return $("<span>").text(d.fullName || d.userName); },
                        onItemClick: function (e) { doForward(id, { recipientId: e.itemData.id }); }
                    });
                    $("<div>").addClass("energy-chat__field-label").text("Gruplar").appendTo($c);
                    $("<div>").appendTo($c).dxList({
                        dataSource: state.groups, keyExpr: "id", height: 130,
                        itemTemplate: function (d) { return $("<span>").text(d.name); },
                        onItemClick: function (e) { doForward(id, { groupId: e.itemData.id }); }
                    });
                });
                forwardPopup.show();
            }

            // ----- Voice call (WebRTC) ----------------------------------------
            var $call = $("#chat-call");
            var $callName = $("#chat-call-name");
            var $callState = $("#chat-call-state");
            var $callAvatar = $("#chat-call-avatar");
            var $callAccept = $("#chat-call-accept");
            var $callHangup = $("#chat-call-hangup");
            var callAudio = document.getElementById("chat-call-audio");
            var ICE = [{ urls: "stun:stun.l.google.com:19302" }];
            var call = { pc: null, peerId: null, stream: null, pendingOffer: null };

            function showCall(name, stateText, incoming) {
                $callName.text(name || "");
                $callState.text(stateText || "");
                $callAvatar.text(initials(name || "?"));
                $callAccept.prop("hidden", !incoming);
                $call.prop("hidden", false);
            }
            function hideCall() { $call.prop("hidden", true); }

            function cleanupCall() {
                if (call.pc) { try { call.pc.close(); } catch (e) { /* noop */ } }
                if (call.stream) { call.stream.getTracks().forEach(function (t) { try { t.stop(); } catch (e) { } }); }
                call = { pc: null, peerId: null, stream: null, pendingOffer: null };
                if (callAudio) { callAudio.srcObject = null; }
                hideCall();
            }

            function newPeerConnection(otherId) {
                var pc = new RTCPeerConnection({ iceServers: ICE });
                pc.onicecandidate = function (ev) {
                    if (ev.candidate) { ENERGY.sendIce(otherId, ev.candidate); }
                };
                pc.ontrack = function (ev) {
                    if (callAudio) { callAudio.srcObject = ev.streams[0]; }
                    $callState.text("Bağlandı");
                };
                pc.onconnectionstatechange = function () {
                    if (pc.connectionState === "failed" || pc.connectionState === "disconnected") { cleanupCall(); }
                };
                return pc;
            }

            function getMic() {
                if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
                    return Promise.reject(new Error("unsupported"));
                }
                return navigator.mediaDevices.getUserMedia({ audio: true });
            }

            function startCall() {
                if (state.mode !== "direct" || !state.peerId) { return; }
                var peerId = state.peerId;
                var peerName = (state.peer && (state.peer.fullName || state.peer.userName)) || "";
                getMic().then(function (stream) {
                    call.stream = stream;
                    call.peerId = peerId;
                    call.pc = newPeerConnection(peerId);
                    stream.getTracks().forEach(function (t) { call.pc.addTrack(t, stream); });
                    showCall(peerName, "Aranıyor…", false);
                    return call.pc.createOffer().then(function (offer) {
                        return call.pc.setLocalDescription(offer).then(function () {
                            ENERGY.callUser(peerId, me.name, offer);
                        });
                    });
                }).catch(function () {
                    if (window.AppNotify) { window.AppNotify.error("Mikrofona erişilemedi."); }
                    cleanupCall();
                });
            }

            function acceptCall() {
                if (!call.pendingOffer || !call.peerId) { return; }
                var offer = call.pendingOffer;
                getMic().then(function (stream) {
                    call.stream = stream;
                    call.pc = newPeerConnection(call.peerId);
                    stream.getTracks().forEach(function (t) { call.pc.addTrack(t, stream); });
                    return call.pc.setRemoteDescription(new RTCSessionDescription(offer))
                        .then(function () { return call.pc.createAnswer(); })
                        .then(function (answer) {
                            return call.pc.setLocalDescription(answer).then(function () {
                                ENERGY.answerCall(call.peerId, answer);
                                $callAccept.prop("hidden", true);
                                $callState.text("Bağlanıyor…");
                            });
                        });
                }).catch(function () {
                    if (window.AppNotify) { window.AppNotify.error("Mikrofona erişilemedi."); }
                    ENERGY.endCall(call.peerId);
                    cleanupCall();
                });
            }

            function hangup() {
                if (call.peerId) { ENERGY.endCall(call.peerId); }
                cleanupCall();
            }

            $callBtn.on("click", startCall);
            $callAccept.on("click", acceptCall);
            $callHangup.on("click", hangup);

            if (typeof ENERGY.onCall === "function") {
                ENERGY.onCall(function (ev) {
                    var d = ev.data || {};
                    var from = d.fromUserId;
                    if (ev.type === "offer") {
                        if (call.pc) { ENERGY.endCall(from); return; } // already in a call → reject
                        call.peerId = from;
                        call.pendingOffer = d.offer;
                        showCall(d.callerName || "", "Gelen arama…", true);
                    } else if (ev.type === "answer") {
                        if (call.pc) { call.pc.setRemoteDescription(new RTCSessionDescription(d.answer)).catch(function () { }); $callState.text("Bağlanıyor…"); }
                    } else if (ev.type === "ice") {
                        if (call.pc && d.candidate) { call.pc.addIceCandidate(new RTCIceCandidate(d.candidate)).catch(function () { }); }
                    } else if (ev.type === "ended") {
                        cleanupCall();
                    }
                });
            }

            // ----- Realtime: deletions / reactions / read-receipts ------------
            if (typeof ENERGY.onMessageDeleted === "function") {
                ENERGY.onMessageDeleted(function (p) {
                    if (!p || !p.id) { return; }
                    removeItem(p.id);
                    loadContacts(); loadGroups();
                });
            }
            if (typeof ENERGY.onMessageReacted === "function") {
                ENERGY.onMessageReacted(function (msg) {
                    if (msg && msg.id) { applyReactionUpdate(msg); }
                });
            }
            if (typeof ENERGY.onMessagesRead === "function") {
                ENERGY.onMessagesRead(function (p) {
                    if (p && p.readerId) { markOwnRead(p.readerId); }
                });
            }

            loadContacts().done(function () { loadInvites(); });

            // Real-time: append messages that belong to the open conversation and
            // refresh the contact ordering/badges as traffic arrives.
            if (window.EnergyChat && window.EnergyChat.subscribe) {
                window.EnergyChat.subscribe(function (m) {
                    if (m.groupId) {
                        // Group message: append if its group is open, refresh group list.
                        if (state.mode === "group" && m.groupId === state.groupId) {
                            appendMessage(m);
                        }
                        loadGroups();
                        return;
                    }

                    var inOpenConversation = state.mode === "direct" && state.peerId &&
                        (m.senderId === state.peerId || m.recipientId === state.peerId);

                    if (inOpenConversation) {
                        appendMessage(m);
                        if (m.senderId === state.peerId) {
                            setPeerTyping(false);
                            markRead(state.peerId);
                        }
                    }
                    // Refresh the sidebar to reflect new last-message ordering/unread.
                    loadContacts();
                });
            }

            // Group invitations / roster changes.
            if (typeof ENERGY.onGroupInvite === "function") {
                ENERGY.onGroupInvite(function () { loadInvites(); });
            }
            if (typeof ENERGY.onGroupChanged === "function") {
                ENERGY.onGroupChanged(function () { loadGroups(); });
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
