$(document).ready(function () {
    // redirect if token doesn't exist
    var storage = window.localStorage;
    var token = storage.getItem("token");
    if (token === null) {
        window.location = "index.html";
    }

    // fetch images
    var amountOnPage = 10;
    
    var currentUrl = new URL(window.location);
    var currentPage = currentUrl.searchParams.get("page");
    if (currentPage === null) {
        currentPage = 1;
    }
    
    $.ajax({
        url: "http://localhost:5000/api/browse",
        data: {
            offset: amountOnPage * (currentPage-1),
            amount: amountOnPage,
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

                $(".pagination").pagination({
                    items: data.totalPosts,
                    itemsOnPage: amountOnPage,
                    currentPage: currentPage,
                    prevText:"<i class='material-icons'>chevron_left</i>",
                    nextText:"<i class='material-icons'>chevron_right</i>",
                    onPageClick: function (pageNumber, event) {
                        window.location = "browse.html?page=" + pageNumber
                    }
                });
            }
        },
        error: function () {
            showError({error: {description: "Couldn't connect to server"}});
        },
        dataType: "json"
    });

});


function createImage(post) {
    return  '<div class="card galleryItem hoverable">' +
                '<a href="view.html?id=' + post.id + '">' +
                    '<div class="card-image waves-effect waves-block waves-light">' +
                        '<img src="' + post.path  + 'thumbnail.jpeg">' +
                    '</div>' +
                '</a>' +
                '<div class="card-content">' +
                    '<span class="card-title activator grey-text text-darken-4 truncate"><i class="material-icons right">more_vert</i>' + post.title + '</span>' +
                '</div>' +
                '<div class="card-reveal">' + 
                    '<span class="card-title grey-text text-darken-4 truncate"><i class="material-icons right">close</i>' + post.title + '</span>' +
                    '<div class="divider"></div>' + 
                    '<p>' + post.description + '</p>' +
                '</div>' +
            '</div>';
}