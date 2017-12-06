$(document).ready(function () {
    // redirect if token doesn't exist
    var storage = window.localStorage;
    var token = storage.getItem("token");
    if (token === null) {
        window.location = "index.html";
    }

    // fetch image
    var currentUrl = new URL(window.location);
    var imageId = currentUrl.searchParams.get("id");
    if (imageId === null) {
        window.location = "index.html";
    }

    $.ajax({
        url: "http://localhost:5000/api/post",
        data: {
            postId: imageId,
            token: token
        },
        success: function (data) {
            if (!showError(data)) {
                var card = $("#viewCard");
                
                var image = card.find("img");
                image.attr("src", data.path);
                image.attr("alt", data.title);
                
                var title = card.find(".card-title");
                title.text(data.title);

                var author = card.find(".card-author");
                author.text(data.user.username);
                
                var description = card.find(".card-description");
                description.text(data.description);
                
                
                
                
                
                var comments = $("#comments");
                for (var commentIndex in data.comments) {
                    comments.append(createComment(data.comments[commentIndex]));   
                }
            }
        },
        error: function () {
            showError({error: {description: "Couldn't connect to server"}});
        },
        dataType: "json"
    });
});


function createComment(comment) {
    return  '<div class="card">' +
                '<div class="card-content">' +
                '<span class="card-title">' + comment.user.value.username + '</span>' +
                '<p>' +
                    comment.text +
                '</p>' +
                '</div>' +
            '</div>'
}