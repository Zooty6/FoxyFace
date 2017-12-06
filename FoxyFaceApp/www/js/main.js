$(document).ready(function () {
    setupMaterialize();
    setupNav();
});

function setupMaterialize() {
    // setup sidenav button
    $('.sidenav').sidenav();
    // setup materialbox
    $('.materialboxed').materialbox();
    // setup collapsibles
    $('.collapsible').collapsible();
    // setup FABs
    $('.fixed-action-btn').floatingActionButton({
        direction: 'top', // Direction menu comes out
        hoverEnabled: false, // Hover enabled
        toolbarEnabled: false // Toolbar transition enabled
    });
    // setup modals
    $('.modal').modal();
}

function setupNav() {
    // get nav tag
    var nav = $('nav').sidenav();
    
    // get local storage
    var storage = window.localStorage;
    
    // display different elements in the nav depending on the current state (if we are logged in (= we have a token) or not (= we dont have a token))
    if (storage.getItem("token") === null) {
        nav.find("ul").append("<li><a href='register.html'>Register</a></li>");
        nav.find("ul").append("<li><a href='login.html'>Login</a></li>");
    } else {
        nav.find("ul").append("<li><a href='browse.html'>Browse</a></li>");
        nav.find("ul").append("<li><a href='post.html'>Submit</a></li>");
        nav.find("ul").append("<li><a href='logout.html'>Logout</a></li>");
    }
}

// returns true if we displayed an error
function showError(data) {
    // shows error if there is one, "data" is the result we receive from the server 
    if (data.hasOwnProperty("error")) {
        
        // if this is a invalid token error
        if (data.error.errorId === 2) {
            setTimeout(function () {
                // remove the token from local storage
                var storage = window.localStorage;
                storage.removeItem("token");
                // and redirect back to index.html
                window.location = "index.html";
            }, 2000); // but only after 2 seconds, so the user sees the toast message
        }
        
        M.toast({html: data.error.description}); // displays the toast message
        return true;
    }
    return false;
}