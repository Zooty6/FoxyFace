$(document).ready(function () {
    setupMaterialize();
    setupNav();
});

function setupMaterialize() {
    $('.sidenav').sidenav();
    $('.materialboxed').materialbox();
    $('.collapsible').collapsible();
}

function setupNav() {
    var nav = $('nav').sidenav();
    
    var storage = window.localStorage;
    
    if (storage.getItem("token") === null) {
        nav.find("ul").append("<li><a href='register.html'>Register</a></li>");
        nav.find("ul").append("<li><a href='login.html'>Login</a></li>");
    } else {
        nav.find("ul").append("<li><a href='browse.html'>Browse</a></li>");
        nav.find("ul").append("<li><a href='post.html'>Submit</a></li>");
        nav.find("ul").append("<li><a href='logout.html'>Logout</a></li>");
    }
}

function showError(data) {
    if (data.hasOwnProperty("error")) {
        
        if (data.error.errorId === 2) {
            setTimeout(function () {
                var storage = window.localStorage;
                storage.removeItem("token");
                window.location = "index.html";
            }, 2000);
        }
        
        M.toast({html: data.error.description});
        return true;
    }
    return false;
}