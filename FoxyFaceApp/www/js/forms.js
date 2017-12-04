
var forms = {
    initialize: function () {
        console.log("hi");
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },

    onDeviceReady: function () {
        console.log("hi");
        createLoginForm();
        createRegisterForm();
    }
};

forms.initialize();


function createLoginForm() {
    console.log("creating login form");
    $("#loginForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
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



function createRegisterForm() {
    console.log("creating register form");
    $("#registerForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
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




function showError(data) {
    if (data.hasOwnProperty("error")) {
        M.toast({html: data.error.description});
        return true;
    }
    return false;
}