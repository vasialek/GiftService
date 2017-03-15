var GsMap = {

    _ymapStaticUrl: "https://static-maps.yandex.ru/1.x/",
    _gmapStaticUrl: "https://maps.googleapis.com/maps/api/staticmap",

    bindToLatLng: function (selector) {
        var self = this;
        $(selector).click(function (e) {
            e.preventDefault();
            var latLng = $(e.target).attr("data-latlng");
            //console.log("Lat. lang of POS is: " + latLng);
            if (latLng != "") {
                var title = $(e.target).attr("data-title");
                if (title == null) {
                    title = "Map";
                }

                $("#MapImage").html("<img width=\"500\" height=\"450\" src=\"" + self.prepareMapLink(latLng, 15, "ymap") + "\">");
                $("#MapTitle").html(title);
                $("#MapDiv").modal();
            }
        });
    },
    
    prepareMapLink: function (latLng, zoom, mapType) {
        if (latLng != null) {
            if (mapType == "ymap") {
                return this._ymapStaticUrl + "?lang=lt&ll=" + latLng + "&size=500,450&z=" + zoom + "&l=map&pt=" + latLng + ",flag";
            }
            if (mapType == "gmap") {
                return this._ymapStaticUrl + "?center=" + latLng + "&size=500x450&z=" + zoom;
            }

        }
        return "";
    }
}
