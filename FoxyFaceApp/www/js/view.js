$(document).ready(function () {
    // redirect if token doesn't exist
    var storage = window.localStorage;
    var token = storage.getItem("token");
    if (token === null) {
        window.location = "index.html";
    }

    // get image id from url or redirect to index.html if there isn't one
    var currentUrl = new URL(window.location);
    var imageId = currentUrl.searchParams.get("id");
    if (imageId === null) {
        window.location = "index.html";
    }

    $.ajax({
        url: "https://foxyfaceapi.azurewebsites.net/api/post",
        data: {
            postId: imageId,
            token: token
        },
        success: function (data) {
            if (!showError(data)) {
                
                // setup everything once we received the data of the post
                createImage(data);
                createComments(data);
                createRatings(data);
                
                createFabs(data);

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

function createFabs(data) {
    $("#shareButton").click(function () {

        // this is the complete list of currently supported params you can pass to the plugin (all optional)
        var options = {
            message: data.title, // not supported on some apps (Facebook, Instagram)
            subject: data.title, // fi. for email
            files:[], // an array of filenames either locally or remotely
            url: data.path,
            chooserTitle: 'Pick an app' // Android only, you can override the default share sheet title
        };

        var onSuccess = function(result) {
            console.log("Share completed? " + result.completed); // On Android apps mostly return false even while it's true
            console.log("Shared to app: " + result.app); // On Android result.app is currently empty. On iOS it's empty when sharing is cancelled (result.completed=false)
        };

        var onError = function(msg) {
            M.toast({html: "Sharing failed: " + msg});
        };

        window.plugins.socialsharing.shareWithOptions(options, onSuccess, onError);        
    });
}

function createCommentForm(token, imageId) {    
    $("#commentForm").ajaxForm({
        success: function (data) {
            if (!showError(data)) {
                // after weve posted a comment, reload
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
                // after weve posted a rating, reload
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
    // gets the view card div
    var card = $("#viewCard");

    // finds the image
    var image = card.find("img");
    // sets the image attributes accordingly
    image.attr("src", data.path);
    image.attr("alt", data.title);

    // finds the card title div
    var title = card.find(".card-title");
    // sets the title
    title.text(data.title);

    // gets the card author div
    var author = card.find(".card-author");
    // sets the author
    author.text(data.user.username);

    // gets the card description div
    var description = card.find(".card-description");
    // sets the description
    description.text(data.description);

    // gets the card action div
    var action = $(".card-action");
    // count all stars of every rating
    var totalStars = 0;
    for (var ratingsIndex in data.ratings) {
        var rating = data.ratings[ratingsIndex];
        totalStars += rating.stars;
    }
    // calculate average
    var avgStars = totalStars / data.ratings.length;
    // generate the stars text and append that to the action div
    action.html(calculateStars(Math.floor(avgStars)));
}

function createComments(data) {
    // get the comments div
    var comments = $("#comments");
    // and append each comment to it
    for (var commentIndex in data.comments) {
        comments.append(createComment(data.comments[commentIndex]));
    }
}

function createRatings(data) {
    // get the ratings div
    var ratings = $("#ratings");
    // and append each comment to it
    for (var ratingsIndex in data.ratings) {
        ratings.append(createRating(data.ratings[ratingsIndex]));
    }
}

function createComment(comment) {
    // returns a materialize card with the comment as content
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
    
    // returns a materialize card with rating as content 
    return  '<div class="card">' +
        '<div class="card-content">' +
        '<span class="card-title">' + rating.user.value.username + '</span>' +
            '<p>' +
                calculateStars(rating.stars) +
            '</p>' +
        '</div>' +
        '</div>'
}

function calculateStars(stars) {
    // generates a string for the rating
    // first add n amount of <stars> as star icons
    var ret = "";
    for (var i = 0; i < stars; i++) {
        ret += "<i class='material-icons'>star</i>";
    }
    // and then fill it up to 5 total with outlined stars
    for (i = 0; i < 5 - stars; i++) {
        ret += "<i class='material-icons'>star_border</i>";
    }
    return ret;
}