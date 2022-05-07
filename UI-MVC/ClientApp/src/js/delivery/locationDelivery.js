const apiKey = 'AIzaSyAo3GLZXvtlLy9NL0pIf1pdt7OA1CHuPm0'

const taskId = document.querySelector('#taskId').textContent
const photoQuestionId = document.querySelector('#photoQuestionId').textContent

const map = document.querySelector('#map')
if (map !== null) {
    addGoogleMapsScript()
}

/* Create a script element for Google Maps API */
function addGoogleMapsScript() {
    let js_file = document.createElement('script');
    js_file.type = "text/javascript";
    js_file.src = 'https://maps.googleapis.com/maps/api/js?callback=initMapCallback&key=' + apiKey
    js_file.async = false
    document.getElementsByTagName('head')[0].appendChild(js_file);
}

/* Create maps */
function initMapCallback() {
    getLocationsByPhotoQuestion(photoQuestionId)
}

/* End of Create maps */

// Adds initMapCallback function to global scope
window.initMapCallback = initMapCallback;

/* Get functions */
function getLocationsByPhotoQuestion(photoQuestionId) {
    fetch('/api/Tasks/' + taskId + "/PhotoQuestions/" + photoQuestionId + '/Locations', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(data => {
                loadLocations(data)
            })
        }
    })
}

/* End of Get functions */

/* Load functions */
function loadLocations(locations) {
    const mapDiv = document.querySelector(`#map`)
    const mapCanvas = mapDiv.querySelector('#mapCanvas')
    const latLng = {lat: locations[0].latitude, lng: locations[0].longitude}

    const googleMap = loadMap(mapCanvas, latLng)
    loadMarkers(locations, googleMap)
}

function loadMap(mapCanvas, latLng) {
    return new google.maps.Map(mapCanvas, {
        center: latLng,
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    })
}

function loadMarkers(locations, googleMap) {
    for (let location of locations) {
        addMarker(googleMap, {lat: location.latitude, lng: location.longitude}, location.radius)
    }
}

/* end of Load functions */

/* Add functions */
function addMarker(map, location, radius) {
    let marker = new google.maps.Marker({
        position: location,
        map: map,
        draggable: false
    })
    marker.setMap(map)

    // Add circle
    let circle = new google.maps.Circle({
        map: map,
        radius: radius * 1000,
        fillColor: '#AA0000'
    });
    circle.bindTo('center', marker, 'position');
}

/* End of Add functions */
