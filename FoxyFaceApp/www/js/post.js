$(document).ready(function () {
    // redirect if we can't find a token
    var storage = window.localStorage;
    var token = storage.getItem("token");
    if (token === null) {
        window.location = "index.html";
    }
    
    createSubmitForm(token);
    createCameraButton();    
});

function createCameraButton() {
    // adds an event listener to the camera button
    $("#cameraButton").click(function () {
        // if we clicked it, then try to get a picture from the camera
        navigator.camera.getPicture(function (base64) {
            // afterwards set the file path of the image to the file input
            $("#cameraBase64").val(base64);
        },function (message) {  }, {
            destinationType: 0 // return base64 picture
        });
    });
}

function createSubmitForm(token) {
    console.log("creating post form");
    $("#submitForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                // after we've posted the image, redirect to the proper view html site of the new post.
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