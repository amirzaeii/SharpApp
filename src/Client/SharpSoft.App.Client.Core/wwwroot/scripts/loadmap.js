var map;
export function load_map(raw) {    
    var geojson_data = JSON.parse(String(raw));
    //var geolocation = JSON.parse(String(currentlocation));

    //console.log(geolocation);
    map = L.map('map').setView([35.55625, 45.441599], 13);

    var isdarkMode = BitTheme.get() == 'dark';
    if (isdarkMode) {
        const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.sharpsoftco.com">SharpSoftCompany</a>',
            className: 'map-tiles'
        }).addTo(map);
    }
    else {
        const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.sharpsoftco.com">SharpSoftCompany</a>',
        }).addTo(map);
    }
  
   
    for (var geojson_item of geojson_data) {
        L.circle([geojson_item.Latitude, geojson_item.Longitude],
            {
                stroke: false,
                fillColor: '#00e0ff',
                fillOpacity: 0.3,
                radius: geojson_item.Radius
            } ).addTo(map);
    }



    return "";
}

export function refreshGeoLocation(currentlocation) {
    var geolocation = JSON.parse(String(currentlocation));

    console.log(geolocation);
    map.setView(geolocation, 30);
    
    var markerGroup = L.layerGroup().addTo(map);
    L.marker(geolocation).addTo(markerGroup).bindPopup('now you are here!',
        {
            permanent: true,
            className: "my-label",
            offset: [0, 0]
        });
}
export function canUseGeoLocation() {
    console.log(!!navigator.geolocation)
    return !!navigator.geolocation;
}

export async function getCurrentPosition() {
    const { coords }  = await new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject)
    });
    return [coords.latitude , coords.longitude];
}