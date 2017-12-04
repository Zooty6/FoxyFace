$(document).ready(function () {
    setupMaterialize();
    setupNav();
});

function setupMaterialize() {
    $('.sidenav').sidenav();
}

function setupNav() {
    var nav = $('nav').sidenav();
    
    var storage = window.localStorage;
    
    if (storage.getItem("token") === null) {
        nav.find("ul").append("<li><a href='register.html'>Register</a></li>");
        nav.find("ul").append("<li><a href='login.html'>Login</a></li>");
    } else {
        nav.find("ul").append("<li><a href='browse.html'>Browse</a></li>");
    }
    
}
