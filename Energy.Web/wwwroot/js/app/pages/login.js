(function (window, $) {
    "use strict";

    function init(options) {
        options = options || {};
        var l = window.AppL10n.auth;
        var formData = { userName: "", password: "", rememberMe: false };

        var form = $("#energy-login-form-host").dxForm({
            formData: formData,
            labelLocation: "top",
            showValidationSummary: false,
            items: [
                {
                    dataField: "userName",
                    label: { text: l.userNameOrEmail },
                    editorOptions: { stylingMode: "outlined", mode: "text", inputAttr: { autocomplete: "username" } },
                    validationRules: [{ type: "required", message: l.fieldRequired }]
                },
                {
                    dataField: "password",
                    label: { text: l.password },
                    editorType: "dxTextBox",
                    editorOptions: {
                        stylingMode: "outlined",
                        mode: "password",
                        inputAttr: { autocomplete: "current-password" },
                        buttons: [{
                            name: "togglePwd",
                            location: "after",
                            options: {
                                icon: "eyeopen",
                                stylingMode: "text",
                                hint: l.showPassword,
                                onClick: function (e) {
                                    var ed = form.getEditor("password");
                                    var visible = ed.option("mode") === "password";
                                    ed.option("mode", visible ? "text" : "password");
                                    e.component.option("icon", visible ? "close" : "eyeopen");
                                    e.component.option("hint", visible ? l.hidePassword : l.showPassword);
                                }
                            }
                        }]
                    },
                    validationRules: [{ type: "required", message: l.fieldRequired }]
                },
                {
                    dataField: "rememberMe",
                    label: { visible: false },
                    editorType: "dxCheckBox",
                    editorOptions: { text: l.rememberMe }
                },
                {
                    itemType: "button",
                    horizontalAlignment: "stretch",
                    buttonOptions: {
                        text: l.signIn,
                        type: "default",
                        stylingMode: "contained",
                        useSubmitBehavior: false,
                        onClick: function () { submit(); }
                    }
                }
            ]
        }).dxForm("instance");

        function submit() {
            var result = form.validate();
            if (!result.isValid) { return; }

            window.AppLoading && window.AppLoading.begin();
            $("#energy-login-form input[name='UserNameOrEmail']").remove();
            $("#energy-login-form input[name='Password']").remove();
            $("#energy-login-form input[name='RememberMe']").remove();
            $("#energy-login-form")
                .append('<input type="hidden" name="UserNameOrEmail" />')
                .append('<input type="hidden" name="Password" />')
                .append('<input type="hidden" name="RememberMe" />');
            $("#energy-login-form input[name='UserNameOrEmail']").val(formData.userName);
            $("#energy-login-form input[name='Password']").val(formData.password);
            $("#energy-login-form input[name='RememberMe']").val(formData.rememberMe ? "true" : "false");

            window.AppHttp.postForm("/account/login", document.getElementById("energy-login-form"))
                .then(function (payload) {
                    if (payload && payload.ok) {
                        window.location.href = payload.redirect || "/";
                    } else {
                        window.AppNotify.error((payload && payload.message) || l.invalidCredentials);
                    }
                })
                .catch(function (err) { window.AppNotify.fromHttpError(err); })
                .finally(function () { window.AppLoading && window.AppLoading.end(); });
        }

        // Enter to submit
        $("#energy-login-form-host").on("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                submit();
            }
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Login = { init: init };
})(window, jQuery);
