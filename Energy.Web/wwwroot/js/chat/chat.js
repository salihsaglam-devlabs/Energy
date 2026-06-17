/*
 * Chat sayfası — gerçek zamanlı sohbet ekranının tam istemci uygulaması.
 *
 * Sorumluluk:
 *   - Kişiler ve gruplar kenar çubuğunu, varsayılan "Sohbetler" karşılama ekranını ve
 *     aktif konuşma penceresini (DevExtreme dxChat) yönetir.
 *   - Doğrudan ve grup mesajlarını gönderir/alır; metin, dosya ve sesli mesaj eklerini
 *     (Base64) destekler; yanıtlama, iletme, silme ve emoji tepkilerini ele alır.
 *   - Gerçek zamanlı olayları (EnergyChat/SignalR) işler: yeni mesaj, yazıyor göstergesi,
 *     çevrimiçi durum, okundu bilgileri, grup davet/değişiklik/silme.
 *   - Grup yönetimini sağlar: oluşturma, üye davet etme/çıkarma, yönetici atama/kaldırma.
 *   - WebRTC üzerinden sesli arama sinyalleşmesini (teklif/yanıt/ICE/sonlandırma) yürütür.
 *   - Mobil çekmece, sesli mesaj kaydedici ve duyarlı (responsive) davranışları içerir.
 *
 * Genel API: window.AppPages.Chat.init().
 */
(function (window, $) {
    "use strict";

    window.AppPages = window.AppPages || {};

    // Base64 gidiş-dönüşünü makul tutmak için ek boyutunu sınırla (10 MB).
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
        // Baloncuğun alıntıları, tepkileri, okundu bilgilerini ve mesaj bazlı eylem
        // menüsünü oluşturabilmesi için tüm üst veriyi taşı.
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

    // Bir mesaj baloncuğunun gövdesini oluşturur: isteğe bağlı yanıt alıntısı, metin,
    // isteğe bağlı ek, tepki çipleri, okundu bilgisi tikleri ve eylem menüsü.
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

        // Tepki çipleri.
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

        // Kendi doğrudan mesajların için okundu bilgisi tikleri.
        if (meta.isMine && !meta.groupId) {
            $("<span>")
                .addClass("energy-chat__ticks" + (meta.isRead ? " is-read" : ""))
                .html(meta.isRead ? '<i class="fa-solid fa-check-double"></i>' : '<i class="fa-solid fa-check"></i>')
                .appendTo($wrap);
        }

        // Mesaj bazlı eylem araç çubuğu.
        var $tools = $("<div>").addClass("energy-chat__msg-tools");
        var $react = $("<button>").addClass("energy-chat__msg-action")
            .attr({ "data-chat-action": "palette", "data-msg-id": meta.id, title: (labels.actionReact || "Tepki") })
            .append($("<i>").addClass("fa-regular fa-face-smile"));
        var $palette = $("<span>").addClass("energy-chat__palette");
        REACTION_EMOJIS.forEach(function (e) {
            $("<button>").addClass("energy-chat__palette-emoji")
                .attr({ "data-chat-action": "react", "data-emoji": e, "data-msg-id": meta.id })
                .text(e).appendTo($palette);
        });
        $react.append($palette).appendTo($tools);
        $("<button>").addClass("energy-chat__msg-action")
            .attr({ "data-chat-action": "reply", "data-msg-id": meta.id, title: (labels.actionReply || "Yanıtla") })
            .append($("<i>").addClass("fa-solid fa-reply")).appendTo($tools);
        $("<button>").addClass("energy-chat__msg-action")
            .attr({ "data-chat-action": "forward", "data-msg-id": meta.id, title: (labels.actionForward || "İlet") })
            .append($("<i>").addClass("fa-solid fa-share")).appendTo($tools);
        if (meta.isMine) {
            $("<button>").addClass("energy-chat__msg-action is-danger")
                .attr({ "data-chat-action": "delete", "data-msg-id": meta.id, title: (labels.actionDelete || "Sil") })
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
                mode: "direct",       // "direct" | "group" (doğrudan | grup)
                peerId: null, peer: null,
                groupId: null, group: null,
                items: [], contacts: [], groups: [], invites: [],
                replyTo: null         // { id, text, sender }
            };
            var contactList, chat, groupList;
            var typingDebounce = null;     // giden "yazıyor" bildirimlerini kısıtlar
            var peerTypingTimer = null;    // gelen yazıyor bayrağını otomatik temizler

            var $windowWrap = $(".energy-chat__window-wrap");
            var $welcome = $("#chat-welcome");
            var $welcomeRecent = $("#chat-welcome-recent");
            var $attachBtn = $("#chat-attach-btn");
            var $attachInput = $("#chat-attach-input");
            var $peer = $("#chat-peer");
            var $peerAvatar = $peer.find(".energy-chat__peer-avatar");
            var $peerName = $peer.find(".energy-chat__peer-name");
            var $peerStatus = $peer.find(".energy-chat__peer-status");

            // Mobil çekmece (drawer) + ses kaydedici öğeleri.
            var $sidebar = $("#chat-sidebar");
            var $backdrop = $("#chat-backdrop");
            var $drawerToggle = $("#chat-drawer-toggle");
            var $drawerClose = $("#chat-drawer-close");
            var $voiceBtn = $("#chat-voice-btn");
            var $recorder = $("#chat-recorder");
            var $recTime = $("#chat-recorder-time");
            var $recCancel = $("#chat-rec-cancel");
            var $recSend = $("#chat-rec-send");

            // Gruplar arayüz öğeleri.
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

            // Yanıt çubuğu + arama öğeleri.
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

            // ----- Karşı taraf başlığı (ad + çevrimiçi/çevrimdışı + yazıyor) -----------------
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
                updateCallAvailability(online);
            }

            // Arama butonunu yalnızca karşı taraf çevrimiçiyse etkin tutar.
            function updateCallAvailability(online) {
                if (state.mode !== "direct") { return; }
                $callBtn
                    .prop("disabled", !online)
                    .toggleClass("is-disabled", !online)
                    .attr("title", online ? "Sesli ara" : "Kullanıcı çevrimdışı");
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
                // dxChat'in yerel "yazıyor" baloncuğu.
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
                    // Kişideki okunmamış rozetini yerel olarak temizle.
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
                hideWelcome();
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

            // ----- Hedef yardımcıları (doğrudan karşı taraf vs grup) ------------------------
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

            // ----- Varsayılan "Sohbetler" karşılama ekranı ---------------------------
            // Hiçbir konuşma açık olmadığında gösterilir; böylece ana alan asla boş
            // kalmaz (ayrıca hiç sohbet yokken mobildeki boş görüntülenme sorununu da giderir).
            function hideWelcome() { if ($welcome.length) { $welcome.prop("hidden", true); } }
            function showWelcome() {
                state.mode = "direct";
                state.peerId = null; state.peer = null;
                state.groupId = null; state.group = null;
                $windowWrap.prop("hidden", true);
                $groupManage.prop("hidden", true);
                $callBtn.prop("hidden", true);
                if ($welcome.length) { $welcome.prop("hidden", false); }
                renderRecent();
            }
            function renderRecent() {
                if (!$welcomeRecent.length) { return; }
                $welcomeRecent.empty();
                var recents = [];
                (state.contacts || []).forEach(function (c) {
                    if (c.lastMessageAt) {
                        recents.push({ type: "direct", id: c.id, name: c.fullName || c.userName, at: c.lastMessageAt, hasImage: c.hasProfileImage });
                    }
                });
                (state.groups || []).forEach(function (g) {
                    recents.push({ type: "group", id: g.id, name: g.name, at: g.lastMessageAt || 0, group: g });
                });
                recents.sort(function (a, b) { return new Date(b.at || 0) - new Date(a.at || 0); });
                recents = recents.slice(0, 6);
                if (recents.length === 0) { return; }
                $("<div>").addClass("energy-chat__welcome-recent-title").text(labels.recentChats || "Son sohbetler").appendTo($welcomeRecent);
                recents.forEach(function (r) {
                    var $row = $("<button>").attr("type", "button").addClass("energy-chat__welcome-recent-item");
                    var $avatar = $("<span>").addClass("energy-chat__contact-avatar");
                    if (r.type === "direct" && r.hasImage) {
                        $("<img>").attr({ src: avatarUrlFor(r.id), alt: r.name }).appendTo($avatar);
                    } else {
                        $avatar.addClass("is-initials").text(initials(r.name));
                        if (r.type === "group") { $avatar.addClass("energy-chat__group-avatar"); }
                    }
                    $avatar.appendTo($row);
                    $("<span>").addClass("energy-chat__welcome-recent-name").text(r.name).appendTo($row);
                    $row.on("click", function () {
                        if (r.type === "group") { openGroup(r.group); } else { openConversation(r.id); }
                    });
                    $row.appendTo($welcomeRecent);
                });
            }

            // Bir mesaj gönderir (metin ve/veya ek). Kaydedilen satır hemen eklenir;
            // böylece gönderen, SignalR/yenilemeyi beklemeden onu görür; gerçek zamanlı
            // yansıma id ile yinelenenlerden ayıklanır.
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
                    // BaseResponse.IsSuccess JSON'da "success" olarak serileştirilir
                    // ([JsonPropertyName("success")]); önce onu oku.
                    var ok = envelope && (envelope.success || envelope.isSuccess || envelope.IsSuccess);
                    if (ok && msg && messageBelongsHere(msg)) {
                        appendMessage(msg);
                    }
                }).fail(function () {
                    if (window.AppNotify) { window.AppNotify.error(labels.error || "Error"); }
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

            // Kaydedilen blob'lar (sesli mesajlar) için ortak gönderici. Dosya ekleriyle
            // aynı boru hattı: mevcut /chat/messages uç noktası üzerinden Base64.
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
                    // Avatarın üzerine yerleştirilen çevrimiçi durum noktası.
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
                    // DevExtreme önce şablon verisini, en son da kapsayıcı öğeyi geçer;
                    // her iki çağrı biçimine de tolerans göster.
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

            // Giden yazıyor bildirimlerini kısıtla (throttle); her zaman sonda bir "durdu" gönder.
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

            // Dosya paylaşımı: sohbetin yanındaki bir düğme dosya seçiciyi açar ve
            // seçilen dosyayı bir ek mesajı olarak gönderir.
            $attachBtn.on("click", function () { $attachInput.trigger("click"); });
            $attachInput.on("change", function () {
                var file = this.files && this.files[0];
                if (file) { sendAttachment(file); }
                $(this).val("");
            });

            // ----- Mobil çekmece (kişiler kayar paneli) ----------------------------
            function openDrawer() {
                $sidebar.addClass("is-open");
                $backdrop.prop("hidden", false);
            }
            function closeDrawer() {
                $sidebar.removeClass("is-open");
                $backdrop.prop("hidden", true);
            }
            $drawerToggle.on("click", openDrawer);
            $("#chat-welcome-toggle").on("click", openDrawer);
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
                    recorder.stream.getTracks().forEach(function (t) { try { t.stop(); } catch (e) { /* işlem yok */ } });
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
                    $("<button>").addClass("energy-chat__icon-btn is-primary").attr("title", labels.acceptCall || "Kabul et")
                        .append($("<i>").addClass("fa-solid fa-check"))
                        .on("click", function () { respondInvite(inv.groupId, true); })
                        .appendTo($actions);
                    $("<button>").addClass("energy-chat__icon-btn is-danger").attr("title", labels.rejectCall || "Reddet")
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
                noDataText: (labels.noGroups || "Grup yok"),
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

            // ----- Grup oluşturma açılır penceresi ------------------------------------------
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
                title: (labels.newGroup || "Yeni grup"),
                width: 380, height: 480, hideOnOutsideClick: true, visible: false,
                contentTemplate: function (content) {
                    var $c = $(content);
                    $("<label>").addClass("energy-chat__field-label").text(labels.groupName || "Grup adı").appendTo($c);
                    var nameBox = $("<div>").appendTo($c).dxTextBox({ placeholder: (labels.groupName || "Grup adı") }).dxTextBox("instance");
                    $("<label>").addClass("energy-chat__field-label").text(labels.members || "Üyeler").appendTo($c);
                    var picker = buildMemberPicker($c, []);
                    var $btn = $("<div>").css("margin-top", "12px").appendTo($c);
                    $btn.dxButton({
                        text: (labels.createGroup || "Oluştur"), type: "default", width: "100%",
                        onClick: function () {
                            var name = (nameBox.option("value") || "").trim();
                            if (!name) { if (window.AppNotify) { window.AppNotify.error(labels.groupNameRequired || "Grup adı gerekli"); } return; }
                            postJson("/chat/groups", { name: name, memberUserIds: picker.option("selectedItemKeys") || [] })
                                .done(function () {
                                    groupPopup.hide();
                                    loadGroups();
                                    if (window.AppNotify) { window.AppNotify.success(labels.groupCreated || "Grup oluşturuldu"); }
                                })
                                .fail(function () { if (window.AppNotify) { window.AppNotify.error(labels.error || "Hata"); } });
                        }
                    });
                }
            }).dxPopup("instance");
            $newGroupBtn.on("click", function () { groupPopup.show(); });

            // ----- Üyeler / davet açılır penceresi --------------------------------------
            // Antiforgery jetonunu taşıyan genel JSON ajax yardımcısı (POST/DELETE).
            function ajaxJson(url, method, body) {
                var token = $("meta[name='csrf-token']").attr("content");
                return $.ajax({
                    url: url, method: method, contentType: "application/json",
                    headers: token ? { "RequestVerificationToken": token } : {},
                    data: body ? JSON.stringify(body) : undefined
                });
            }

            // Geçerli kullanıcı, açık grubun sahibiyse veya yöneticisiyse onu yönetir.
            function canManageGroup() {
                return !!(state.group && (state.group.isOwner || state.group.isAdmin));
            }
            function setGroupAdmin(gid, userId, isAdmin) {
                return ajaxJson("/chat/groups/" + gid + "/members/" + userId + "/admin", "POST", { isAdmin: isAdmin });
            }
            function removeGroupMember(gid, userId) {
                return ajaxJson("/chat/groups/" + gid + "/members/" + userId, "DELETE");
            }
            function deleteGroup(gid) {
                return ajaxJson("/chat/groups/" + gid, "DELETE");
            }
            // Verilen (artık çıkarılmış/silinmiş) grup açıksa konuşma penceresini kapatır.
            function closeGroupView(gid) {
                if (state.mode === "group" && state.groupId === gid) {
                    state.mode = "direct";
                    state.groupId = null; state.group = null;
                    state.peerId = null; state.peer = null;
                    $windowWrap.prop("hidden", true);
                    $groupManage.prop("hidden", true);
                }
            }

            var membersPopup = $membersPopup.dxPopup({
                title: (labels.groupMembers || "Grup üyeleri"), width: 380, height: 520, hideOnOutsideClick: true, visible: false
            }).dxPopup("instance");

            $groupManage.on("click", function () {
                if (!state.groupId) { return; }
                var gid = state.groupId;
                var manage = canManageGroup();
                membersPopup.option("contentTemplate", function (content) {
                    var $c = $(content);
                    $("<div>").addClass("energy-chat__field-label").text(labels.members || "Üyeler").appendTo($c);
                    var $members = $("<div>").addClass("energy-chat__members").appendTo($c);

                    function loadMembers() {
                        $members.empty();
                        $.getJSON("/chat/groups/" + gid + "/members").done(function (list) {
                            (list || []).forEach(function (d) {
                                var $row = $("<div>").addClass("energy-chat__member-row");
                                var role = d.isOwner ? (labels.roleOwner || "Sahip") : (d.isAdmin ? (labels.roleAdmin || "Yönetici") : (d.status === 0 ? (labels.rolePending || "Bekliyor") : (labels.roleMember || "Üye")));
                                $("<span>").addClass("energy-chat__member-name").text(d.fullName || d.userName).appendTo($row);
                                $("<span>").addClass("energy-chat__member-role").text(role).appendTo($row);
                                // Sahip/yöneticiler diğer kabul edilmiş, sahip olmayan üyeleri yönetir.
                                if (manage && !d.isOwner && d.status === 1) {
                                    var $actions = $("<span>").addClass("energy-chat__member-actions").appendTo($row);
                                    $("<button>").addClass("energy-chat__icon-btn")
                                        .attr("title", d.isAdmin ? (labels.removeAdmin || "Yöneticiliği kaldır") : (labels.makeAdmin || "Yönetici yap"))
                                        .append($("<i>").addClass(d.isAdmin ? "fa-solid fa-user-shield" : "fa-regular fa-user"))
                                        .on("click", function () {
                                            setGroupAdmin(gid, d.userId, !d.isAdmin)
                                                .done(loadMembers)
                                                .fail(function () { if (window.AppNotify) { window.AppNotify.error(labels.error || "Hata"); } });
                                        }).appendTo($actions);
                                    $("<button>").addClass("energy-chat__icon-btn is-danger")
                                        .attr("title", labels.removeFromGroup || "Gruptan çıkar")
                                        .append($("<i>").addClass("fa-solid fa-user-minus"))
                                        .on("click", function () {
                                            if (!window.confirm(labels.confirmRemoveMember || "Üye gruptan çıkarılsın mı?")) { return; }
                                            removeGroupMember(gid, d.userId)
                                                .done(function () { loadMembers(); loadGroups(); })
                                                .fail(function () { if (window.AppNotify) { window.AppNotify.error(labels.error || "Hata"); } });
                                        }).appendTo($actions);
                                }
                                $row.appendTo($members);
                            });
                        });
                    }
                    loadMembers();

                    if (manage) {
                        $("<div>").addClass("energy-chat__field-label").text(labels.addMember || "Üye ekle").appendTo($c);
                        var picker = buildMemberPicker($c, []);
                        $("<div>").css("margin-top", "12px").appendTo($c).dxButton({
                            text: (labels.inviteToGroup || "Davet et"), type: "default", width: "100%",
                            onClick: function () {
                                var ids = picker.option("selectedItemKeys") || [];
                                if (ids.length === 0) { return; }
                                postJson("/chat/groups/" + gid + "/invite", { userIds: ids })
                                    .done(function () { if (window.AppNotify) { window.AppNotify.success(labels.inviteSent || "Davet gönderildi"); } loadMembers(); })
                                    .fail(function () { if (window.AppNotify) { window.AppNotify.error(labels.error || "Hata"); } });
                            }
                        });
                        $("<div>").css("margin-top", "16px").appendTo($c).dxButton({
                            text: (labels.deleteGroup || "Grubu sil"), type: "danger", width: "100%", icon: "trash",
                            onClick: function () {
                                if (!window.confirm(labels.confirmDeleteGroup || "Grup kalıcı olarak silinsin mi?")) { return; }
                                deleteGroup(gid).done(function () {
                                    membersPopup.hide();
                                    if (window.AppNotify) { window.AppNotify.success(labels.groupDeleted || "Grup silindi"); }
                                    closeGroupView(gid);
                                    loadGroups();
                                }).fail(function () { if (window.AppNotify) { window.AppNotify.error(labels.error || "Hata"); } });
                            }
                        });
                    }
                });
                membersPopup.show();
            });

            // ----- Mesaj eylemleri: yanıtla / sil / ilet / tepki ver ----------
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
                if (!window.confirm(labels.confirmDeleteMessage || "Mesaj silinsin mi?")) { return; }
                var token = $("meta[name='csrf-token']").attr("content");
                $.ajax({
                    url: "/chat/messages/" + id, method: "DELETE",
                    headers: token ? { "RequestVerificationToken": token } : {}
                });
                // Baloncuk, MessageDeleted olayı geldiğinde kaldırılır.
            }
            function reactMessage(id, emoji) {
                postJson("/chat/messages/" + id + "/react", { emoji: emoji });
                // Tepki çipleri, MessageReacted olayı geldiğinde güncellenir.
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

            // ----- İletme açılır penceresi ----------------------------------------------
            var forwardPopup = $forwardPopup.dxPopup({
                title: (labels.actionForward || "İlet"), width: 340, height: 480, hideOnOutsideClick: true, visible: false
            }).dxPopup("instance");

            function doForward(id, target) {
                postJson("/chat/messages/" + id + "/forward", target).done(function () {
                    forwardPopup.hide();
                    if (window.AppNotify) { window.AppNotify.success(labels.messageForwarded || "İletildi"); }
                });
            }
            function openForward(id) {
                forwardPopup.option("contentTemplate", function (content) {
                    var $c = $(content);
                    $("<div>").addClass("energy-chat__field-label").text(labels.contacts || "Kişiler").appendTo($c);
                    $("<div>").appendTo($c).dxList({
                        dataSource: state.contacts, keyExpr: "id", height: 170, searchEnabled: true, searchExpr: ["fullName", "userName"],
                        itemTemplate: function (d) { return $("<span>").text(d.fullName || d.userName); },
                        onItemClick: function (e) { doForward(id, { recipientId: e.itemData.id }); }
                    });
                    $("<div>").addClass("energy-chat__field-label").text(labels.groups || "Gruplar").appendTo($c);
                    $("<div>").appendTo($c).dxList({
                        dataSource: state.groups, keyExpr: "id", height: 130,
                        itemTemplate: function (d) { return $("<span>").text(d.name); },
                        onItemClick: function (e) { doForward(id, { groupId: e.itemData.id }); }
                    });
                });
                forwardPopup.show();
            }

            // ----- Sesli arama (WebRTC) ----------------------------------------
            var $call = $("#chat-call");
            var $callName = $("#chat-call-name");
            var $callState = $("#chat-call-state");
            var $callAvatar = $("#chat-call-avatar");
            var $callAccept = $("#chat-call-accept");
            var $callHangup = $("#chat-call-hangup");
            var callAudio = document.getElementById("chat-call-audio");
            var ICE = [{ urls: "stun:stun.l.google.com:19302" }];
            var call = { pc: null, peerId: null, stream: null, pendingOffer: null, pendingCandidates: [], remoteSet: false };

            function showCall(name, stateText, incoming) {
                $callName.text(name || "");
                $callState.text(stateText || "");
                $callAvatar.text(initials(name || "?"));
                $callAccept.prop("hidden", !incoming);
                $call.prop("hidden", false);
            }
            function hideCall() { $call.prop("hidden", true); }

            function cleanupCall() {
                if (call.pc) { try { call.pc.close(); } catch (e) { /* işlem yok */ } }
                if (call.stream) { call.stream.getTracks().forEach(function (t) { try { t.stop(); } catch (e) { } }); }
                call = { pc: null, peerId: null, stream: null, pendingOffer: null, pendingCandidates: [], remoteSet: false };
                if (callAudio) { callAudio.srcObject = null; }
                hideCall();
            }

            // Uzak açıklama (remote description) henüz ayarlanmadan veya bağlantı (pc)
            // oluşturulmadan önce gelen ICE adaylarını kuyruğa alır; aksi hâlde erken
            // (özellikle host) adaylar kaybolur ve aranan tarafta ICE asla tamamlanmaz
            // ("Bağlanıyor…" durumunda takılı kalır).
            function addOrQueueIce(candidate) {
                if (!candidate) { return; }
                if (call.pc && call.remoteSet) {
                    call.pc.addIceCandidate(new RTCIceCandidate(candidate)).catch(function () { /* yok say */ });
                } else {
                    call.pendingCandidates.push(candidate);
                }
            }

            // Uzak açıklama ayarlandıktan sonra kuyruktaki adayları uygula.
            function flushCandidates() {
                if (!call.pc || !call.remoteSet) { return; }
                var pending = call.pendingCandidates || [];
                call.pendingCandidates = [];
                pending.forEach(function (c) {
                    try { call.pc.addIceCandidate(new RTCIceCandidate(c)).catch(function () { /* yok say */ }); }
                    catch (e) { /* yok say */ }
                });
            }

            function newPeerConnection(otherId) {
                var pc = new RTCPeerConnection({ iceServers: ICE });
                pc.onicecandidate = function (ev) {
                    if (ev.candidate) { ENERGY.sendIce(otherId, ev.candidate); }
                };
                pc.ontrack = function (ev) {
                    if (callAudio) {
                        callAudio.srcObject = ev.streams[0];
                        // Bazı tarayıcılarda otomatik oynatma engeli olabilir; sessizce dene.
                        if (typeof callAudio.play === "function") { callAudio.play().catch(function () { /* yok say */ }); }
                    }
                };
                // Bağlantı durumunu tek kaynaktan yönet: yalnızca ICE/DTLS gerçekten
                // kurulduğunda "Bağlandı" yaz; başarısızlıkta aramayı temizle.
                pc.onconnectionstatechange = function () {
                    var st = pc.connectionState;
                    if (st === "connected") { $callState.text(labels.callConnected || "Bağlandı"); }
                    else if (st === "failed" || st === "disconnected") { cleanupCall(); }
                };
                // connectionState'i desteklemeyen tarayıcılar için ICE durumu yedeği.
                pc.oniceconnectionstatechange = function () {
                    var st = pc.iceConnectionState;
                    if (st === "connected" || st === "completed") { $callState.text(labels.callConnected || "Bağlandı"); }
                    else if (st === "failed") { cleanupCall(); }
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
                // Arama yalnızca karşı taraf çevrimiçiyse başlatılabilir.
                if (!isOnline(peerId)) {
                    if (window.AppNotify) { window.AppNotify.warning(labels.callNeedsOnline || "Arama yapabilmek için kullanıcının çevrimiçi olması gerekir."); }
                    return;
                }
                var peerName = (state.peer && (state.peer.fullName || state.peer.userName)) || "";
                getMic().then(function (stream) {
                    call.stream = stream;
                    call.peerId = peerId;
                    call.remoteSet = false;
                    call.pendingCandidates = [];
                    call.pc = newPeerConnection(peerId);
                    stream.getTracks().forEach(function (t) { call.pc.addTrack(t, stream); });
                    showCall(peerName, "Aranıyor…", false);
                    return call.pc.createOffer().then(function (offer) {
                        return call.pc.setLocalDescription(offer).then(function () {
                            ENERGY.callUser(peerId, me.name, offer);
                        });
                    });
                }).catch(function () {
                    if (window.AppNotify) { window.AppNotify.error(labels.micNotAccessible || "Mikrofona erişilemedi."); }
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
                        .then(function () {
                            // Teklif (uzak açıklama) ayarlandı: kuyruğa alınmış erken ICE
                            // adaylarını şimdi uygula.
                            call.remoteSet = true;
                            flushCandidates();
                            return call.pc.createAnswer();
                        })
                        .then(function (answer) {
                            return call.pc.setLocalDescription(answer).then(function () {
                                ENERGY.answerCall(call.peerId, answer);
                                $callAccept.prop("hidden", true);
                                $callState.text(labels.calling || "Bağlanıyor…");
                            });
                        });
                }).catch(function () {
                    if (window.AppNotify) { window.AppNotify.error(labels.micNotAccessible || "Mikrofona erişilemedi."); }
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
                        if (call.pc) { ENERGY.endCall(from); return; } // zaten bir aramada → reddet
                        call.peerId = from;
                        call.pendingOffer = d.offer;
                        showCall(d.callerName || "", "Gelen arama…", true);
                        // Zil çal (hesap bazlı arama sesi tercihine uyar).
                        if (window.EnergyUserSettings && typeof window.EnergyUserSettings.beep === "function") {
                            window.EnergyUserSettings.beep("call");
                        }
                    } else if (ev.type === "answer") {
                        if (call.pc) {
                            $callState.text(labels.calling || "Bağlanıyor…");
                            call.pc.setRemoteDescription(new RTCSessionDescription(d.answer))
                                .then(function () {
                                    // Yanıt (uzak açıklama) ayarlandı: bekleyen ICE adaylarını boşalt.
                                    call.remoteSet = true;
                                    flushCandidates();
                                })
                                .catch(function () { });
                        }
                    } else if (ev.type === "ice") {
                        if (d.candidate) { addOrQueueIce(d.candidate); }
                    } else if (ev.type === "ended") {
                        cleanupCall();
                    }
                });
            }

            // ----- Gerçek zamanlı: silmeler / tepkiler / okundu bilgileri ------------
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

            // Varsayılan "Sohbetler" ekranını HEMEN göster (herhangi bir AJAX'tan önce);
            // böylece ana alan asla boş görüntülenmez — özellikle mobilde ve kullanıcının
            // henüz hiç konuşması olmadığında.
            showWelcome();
            loadContacts().done(function () { loadInvites(); loadGroups(); renderRecent(); });

            // Gerçek zamanlı: açık konuşmaya ait mesajları ekle ve trafik geldikçe
            // kişi sıralamasını/rozetlerini yenile.
            if (window.EnergyChat && window.EnergyChat.subscribe) {
                window.EnergyChat.subscribe(function (m) {
                    if (m.groupId) {
                        // Grup mesajı: grubu açıksa ekle, grup listesini yenile.
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
                    // Yeni son-mesaj sıralamasını/okunmamışı yansıtmak için kenar çubuğunu yenile.
                    loadContacts();
                });
            }

            // Grup davetleri / üye listesi değişiklikleri.
            if (typeof ENERGY.onGroupInvite === "function") {
                ENERGY.onGroupInvite(function () { loadInvites(); });
            }
            if (typeof ENERGY.onGroupChanged === "function") {
                ENERGY.onGroupChanged(function () { loadGroups(); });
            }
            if (typeof ENERGY.onGroupDeleted === "function") {
                ENERGY.onGroupDeleted(function (p) {
                    if (p && p.groupId) { closeGroupView(p.groupId); }
                    loadGroups();
                });
            }

            // Çevrimiçi/çevrimdışı durum: bir karşı taraf bağlandığında veya bağlantısı
            // kesildiğinde kişi etiketlerini ve konuşma başlığını yeniden çiz.
            if (typeof ENERGY.onPresence === "function") {
                ENERGY.onPresence(function (p) {
                    try { contactList.repaint(); } catch (e) { /* liste henüz hazır değil */ }
                    if (state.peer && (p.snapshot || p.userId === String(state.peer.id).toLowerCase())) {
                        renderPeerStatus();
                    }
                });
            }

            // Geçerli konuşma karşı tarafından gelen yazıyor göstergeleri.
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
