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

                createImage(data);
                createComments(data);
                createRatings(data);
                
                createFabs();


                createCommentForm(token, imageId);
                createRatingForm(token, imageId);


                
            }
        },
        error: function () {
            showError({error: {description: "Couldn't connect to server"}});
        },
        dataType: "json"
    });
});

function createFabs() {
    $("#shareButton").click(function () {

    });
    
}

function createCommentForm(token, imageId) {    
    $("#commentForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                window.location.reload();
            }
        },
        error: function () {
            alert("Couldn't send request...");
        },
        dataType: 'json',
        data: {
            token: token, 
            postId: imageId
        }
    });
}

function createRatingForm(token, imageId) {
    $("#ratingForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                window.location.reload();
            }
        },
        error: function () {
            alert("Couldn't send request...");
        },
        dataType: 'json',
        data: {
            token: token,
            postId: imageId
        }
    });
}

function createImage(data) {
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


    var action = $(".card-action");
    var totalStars = 0;
    for (var ratingsIndex in data.ratings) {
        var rating = data.ratings[ratingsIndex];
        totalStars += rating.stars;
    }
    var avgStars = totalStars / data.ratings.length;
    action.html(calculateStars(avgStars));
}

function createComments(data) {
    var comments = $("#comments");
    for (var commentIndex in data.comments) {
        comments.append(createComment(data.comments[commentIndex]));
    }
}

function createRatings(data) {
    var ratings = $("#ratings");
    for (var ratingsIndex in data.ratings) {
        ratings.append(createRating(data.ratings[ratingsIndex]));
    }
}

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

function createRating(rating) {
    var stars = "";
    for (var i = 0; i < rating.stars; i++) {
        stars += "<i class='material-icons'>star</i>";
    }
    for (i = 0; i < 5 - rating.stars; i++) {
        stars += "<i class='material-icons'>star_border</i>";
    }
    
    
    
    return  '<div class="card">' +
        '<div class="card-content">' +
        '<span class="card-title">' + rating.user.value.username + '</span>' +
            '<p>' +
                stars +
            '</p>' +
        '</div>' +
        '</div>'
}

function calculateStars(stars) {
    var ret = "";
    for (var i = 0; i < stars; i++) {
        ret += "<i class='material-icons'>star</i>";
    }
    for (i = 0; i < 5 - stars; i++) {
        ret += "<i class='material-icons'>star_border</i>";
    }
    return ret;
}