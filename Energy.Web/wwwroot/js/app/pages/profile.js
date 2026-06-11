(function (window, $) {
    "use strict";

    window.AppPages = window.AppPages || {};

    function readJson($el, attr, fallback) {
        try {
            var raw = $el.attr(attr);
            return raw ? JSON.parse(raw) : fallback;
        } catch (e) {
            return fallback;
        }
    }

    function buildAvatar($container, initials, hasProfileImage) {
        $container.empty();
        var src = "/profile/image?v=" + Date.now();
        var $wrapper = $("<div>").addClass("energy-profile__avatar");
        var $fallback = $("<span>").addClass("energy-profile__avatar-initials").text(initials);

        if (hasProfileImage) {
            var $img = $("<img>")
                .attr("src", src)
                .attr("alt", "")
                .on("error", function () { $img.remove(); $wrapper.append($fallback); });
            $wrapper.append($img);
        } else {
            $wrapper.append($fallback);
        }
        $container.append($wrapper);
    }

    function buildPersonalFormItems(labels) {
        return [
            { dataField: "firstName", label: { text: labels.firstName }, editorOptions: { readOnly: true } },
            { dataField: "lastName", label: { text: labels.lastName }, editorOptions: { readOnly: true } },
            { dataField: "email", label: { text: labels.email }, editorOptions: { readOnly: true } },
            { dataField: "phoneNumber", label: { text: labels.phoneNumber }, editorOptions: { readOnly: true } }
        ];
    }

    function buildAccountFormItems(labels) {
        return [
            { dataField: "userName", label: { text: labels.userName }, editorOptions: { readOnly: true } },
            {
                dataField: "isActive",
                label: { text: labels.isActive },
                editorType: "dxSwitch",
                editorOptions: { readOnly: true, switchedOnText: labels.active, switchedOffText: labels.inactive }
            },
            { dataField: "emailConfirmed", label: { text: labels.emailConfirmed }, editorType: "dxSwitch", editorOptions: { readOnly: true } },
            { dataField: "twoFactorEnabled", label: { text: labels.twoFactorEnabled }, editorType: "dxSwitch", editorOptions: { readOnly: true } },
            { dataField: "lockoutEnabled", label: { text: labels.lockoutEnabled }, editorType: "dxSwitch", editorOptions: { readOnly: true } }
        ];
    }

    function renderPersonalForm($container, profile, labels) {
        $container.dxForm({
            formData: profile,
            labelLocation: "top",
            colCount: 2,
            items: buildPersonalFormItems(labels)
        });
    }

    function renderAccountForm($container, profile, labels) {
        $container.dxForm({
            formData: profile,
            labelLocation: "top",
            colCount: 2,
            items: buildAccountFormItems(labels)
        });
    }

    function renderRolesGrid($container, profile, labels) {
        $container.dxDataGrid({
            dataSource: profile.roles || [],
            keyExpr: "id",
            showBorders: true,
            columnAutoWidth: true,
            noDataText: labels.noRoles,
            columns: [
                { dataField: "name", caption: labels.roleName },
                { dataField: "description", caption: labels.roleDescription }
            ]
        });
    }

    function renderPermissionsList($container, profile, labels) {
        var items = (profile.permissions || []).map(function (code) {
            return { code: code };
        });
        $container.dxDataGrid({
            dataSource: items,
            keyExpr: "code",
            showBorders: true,
            columnAutoWidth: true,
            noDataText: labels.noPermissions,
            searchPanel: { visible: true, width: 240 },
            paging: { pageSize: 25 },
            columns: [
                { dataField: "code", caption: labels.permissionCode }
            ]
        });
    }

    function renderTabs($container, profile, labels) {
        $container.dxTabPanel({
            animationEnabled: true,
            swipeEnabled: false,
            items: [
                {
                    title: labels.personalInfo,
                    template: function (_, __, element) {
                        var $form = $("<div>").appendTo(element);
                        renderPersonalForm($form, profile, labels);
                    }
                },
                {
                    title: labels.accountInfo,
                    template: function (_, __, element) {
                        var $form = $("<div>").appendTo(element);
                        renderAccountForm($form, profile, labels);
                    }
                },
                {
                    title: labels.roles,
                    template: function (_, __, element) {
                        var $grid = $("<div>").appendTo(element);
                        renderRolesGrid($grid, profile, labels);
                    }
                },
                {
                    title: labels.permissions,
                    template: function (_, __, element) {
                        var $grid = $("<div>").appendTo(element);
                        renderPermissionsList($grid, profile, labels);
                    }
                }
            ]
        });
    }

    function setupImage($layout, profile, labels, initials) {
        var $avatarBox = $("#profile-avatar-box");
        var $uploader = $("#profile-image-uploader");
        var $remove = $("#profile-image-remove");

        // The new backend does not expose a profile-image endpoint; when the
        // upload/remove DOM hooks are absent we only render the static avatar
        // (initials) and skip the rest of the widget setup.
        var hasUploadUi = $uploader.length > 0 && $remove.length > 0;

        var state = { hasProfileImage: !!profile.hasProfileImage };

        if (!hasUploadUi) {
            buildAvatar($avatarBox, initials, state.hasProfileImage);
            return;
        }

        var removeBtn;
        var refresh = function () {
            buildAvatar($avatarBox, initials, state.hasProfileImage);
            if (removeBtn) { removeBtn.option("disabled", !state.hasProfileImage); }
        };

        $uploader.dxFileUploader({
            uploadUrl: "/profile/image",
            uploadMode: "instantly",
            multiple: false,
            accept: "image/png,image/jpeg,image/gif,image/webp",
            allowedFileExtensions: [".png", ".jpg", ".jpeg", ".gif", ".webp"],
            maxFileSize: 2 * 1024 * 1024,
            name: "file",
            selectButtonText: labels.uploadImage,
            labelText: labels.imageHint,
            showFileList: false,
            onUploaded: function () {
                state.hasProfileImage = true;
                refresh();
                window.AppNotify && window.AppNotify.success(labels.imageUploaded);
            },
            onUploadError: function (e) {
                var msg = labels.imageUploaded;
                try {
                    var body = e.request && e.request.responseText ? JSON.parse(e.request.responseText) : null;
                    if (body && body.message) { msg = body.message; }
                } catch (err) { /* ignore */ }
                window.AppNotify && window.AppNotify.error(msg);
            }
        });

        removeBtn = $remove.dxButton({
            text: labels.removeImage,
            icon: "trash",
            type: "danger",
            stylingMode: "outlined",
            disabled: !state.hasProfileImage,
            onClick: function () {
                DevExpress.ui.dialog
                    .confirm(labels.confirmDelete, labels.removeImage)
                    .done(function (ok) {
                        if (!ok) return;
                        $.ajax({
                            url: "/profile/image/remove",
                            method: "POST",
                            credentials: "same-origin"
                        })
                            .done(function () {
                                state.hasProfileImage = false;
                                refresh();
                                window.AppNotify && window.AppNotify.success(labels.imageRemoved);
                            })
                            .fail(function (xhr) {
                                var msg = labels.imageRemoved;
                                try {
                                    var body = xhr.responseText ? JSON.parse(xhr.responseText) : null;
                                    if (body && body.message) { msg = body.message; }
                                } catch (err) { /* ignore */ }
                                window.AppNotify && window.AppNotify.error(msg);
                            });
                    });
            }
        }).dxButton("instance");

        refresh();
    }

    window.AppPages.Profile = {
        init: function () {
            var $layout = $(".energy-profile__layout");
            if ($layout.length === 0) return;

            var profile = readJson($layout, "data-profile", {});
            var labels = readJson($layout, "data-labels", {});
            var initials = $layout.attr("data-initials") || "?";

            renderTabs($("#profile-tabs"), profile, labels);
            setupImage($layout, profile, labels, initials);
        }
    };
})(window, jQuery);

