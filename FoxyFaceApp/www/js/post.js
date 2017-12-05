$(document).ready(function () {
    createSubmitForm();
    createCameraButton();    
});

function createCameraButton() {
    $("#cameraButton").click(function () {
        navigator.camera.getPicture(function (file) {
            debugger;
            $(".fileButton").each(function () {
               $(this).val(file); 
            });
        },function (message) {  }, {});
    });
}

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