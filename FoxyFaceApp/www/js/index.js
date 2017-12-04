$(document).ready(function () {
    // redirect to browse if token exists
    var storage = window.localStorage;
    if (storage.getItem("token") !== null) {
        window.location = "browse.html";
    }
});

