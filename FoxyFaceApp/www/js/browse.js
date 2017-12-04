var app = {
    initialize: function() {
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },

    onDeviceReady: function() {
        var storage = window.localStorage;
        if (storage.getItem("token") === null) {
            window.location = "index.html"; 
        }
    }
};

app.initialize();
