$(document).ready(function () {
    createSubmitForm();    
});

function createSubmitForm() {
    var storage = window.localStorage;
    var token = storage.getItem("token");
    if (token === null) {
        window.location = "index.html";
    }
    console.log("creating post form");
    $("#submitForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                var postId = data.postId;
                window.location = "view.html?id=" + postId;
            }
        },
        error: function () {
            alert("Couldn't send request...");
        },
        dataType: 'json',
        data: {            
            token: token
        }
    });
}