var app = {
    initialize: function() {
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },
    
    onDeviceReady: function() {
        var foxyFaceSidenav = document.querySelector('.sidenav');
        new M.Sidenav(foxyFaceSidenav);
    }
};

app.initialize();
