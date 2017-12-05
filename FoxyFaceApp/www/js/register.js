$(document).ready(function () {
    createRegisterForm();
});

function createRegisterForm() {
    console.log("creating register form");
    $("#registerForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                var storage = window.localStorage;
                storage.setItem("token", data.token);
                window.location = "browse.html";
            }
        },
        error: function () {
            alert("Couldn't send request...");
        },
        dataType: 'json'
    });
}