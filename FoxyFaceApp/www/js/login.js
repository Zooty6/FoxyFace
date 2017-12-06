$(document).ready(function () {
    createLoginForm();
});

function createLoginForm() {
    console.log("creating login form");
    $("#loginForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                // sets the cookie we received
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
