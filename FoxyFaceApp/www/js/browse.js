$(document).ready(function () {
    // redirect if token doesn't exist
    var storage = window.localStorage;
    var token = storage.getItem("token");
    if (token === null) {
        window.location = "index.html";
    }

    // fetch images
    $.ajax({
        url: "http://localhost:5000/api/browse",
        data: {
            offset: 0,
            amount: 50,
            orderBy: "date",
            order: "desc",
            token: token
        },
        success: function (data) {
            if (!showError(data)) {
                var gallery = $("#galleryContainer");
                for (var index in data.posts) {
                    gallery.append(createImage(data.posts[index]));
                }
            }
        },
        error: function () {
            showError({error: {description: "Couldn't connect to server"}});
        },
        dataType: "json"
    });
});


function createImage(post) {
    return  '<div class="galleryItem">' + 
                '<a href="' + post.path + '">' +
                    '<div class="imageContainer">' +
                        '<img src="' + post.path + "thumbnail.jpeg" + '">' +
                    '</div>' +
                    '<div>' + post.title + '</div>' +
                '</a>' +
            '</div>';
}