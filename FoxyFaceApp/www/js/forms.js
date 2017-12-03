

// Login Form
$(document).ready(function() {
    $("#loginForm").ajaxForm({
        success: function (data) {
            
            if (data.hasOwnProperty("error")) {
                alert(data.error.description);
            } else {
                var storage = window.localStorage;
                storage.setItem("token", data.token);   
            }
        },
        error: function () {
            alert("Couldn't send request...");
        },
        dataType: 'json'
    });
});
