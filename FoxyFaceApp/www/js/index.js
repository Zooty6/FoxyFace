$(document).ready(function () {
    var storage = window.localStorage;
    if (storage.getItem("token") !== null) {
        window.location = "browse.html";
    }
});

