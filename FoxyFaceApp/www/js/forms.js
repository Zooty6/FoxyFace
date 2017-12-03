var forms = {
    initialize: function() {
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },

    onDeviceReady: function() {
        $("#loginForm").ajaxForm({
            success: function (data) {
                if (data.hasOwnProperty("error")) {
                    M.toast({html: data.error.description});
                } else {
                    var storage = window.localStorage;
                    storage.setItem("token", data.token);
                    window.location = "index.html";
                }
            },
            error: function () {
                alert("Couldn't send request...");
            },
            dataType: 'json'
        });
    }
};

forms.initialize();
