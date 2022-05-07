const apiKey = 'AIzaSyAo3GLZXvtlLy9NL0pIf1pdt7OA1CHuPm0'

const taskId = document.querySelector('#TaskId').value

let markersMap = new Map()
let circlesMap = new Map()

/* Create a script element for Google Maps API */
export function addGoogleMapsScript() {
    let js_file = document.createElement('script');
    js_file.type = "text/javascript";
    js_file.src = 'https://maps.googleapis.com/maps/api/js?callback=initMapCallback&key=' + apiKey
    js_file.async = false
    document.getElementsByTagName('head')[0].appendChild(js_file);
}
/* End of Create a script element for Google Maps API */

/* Create maps */
export async function initMapCallback() {
    const defaultPos = {lat: 51.2186, lng: 4.4014}

    const mapDivs = document.querySelectorAll('[id *= "map_"]')

    for (let mapDiv of mapDivs) {
        const mapCanvas = mapDiv.querySelector('#mapCanvas')
        loadMap(mapCanvas, defaultPos)
    }

    const radiusInputs = document.querySelectorAll('#radius')
    for (let radiusInput of radiusInputs) {
        let photoQuestionId = radiusInput.closest('[id *= "map_"]').id.split('_')[1]

        radiusInput.addEventListener('input', (e) => {
            updateCircles(photoQuestionId, e.currentTarget.value)
        })
    }

    getAllLocations()
}
/* End of Create maps */

// Adds initMapCallback function to global scope
window.initMapCallback = initMapCallback;

/* Get functions */
function getAllLocations() {
    fetch('/api/Tasks/' + taskId + "/PhotoQuestions", {
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


/* Add functions */
function addLocation(latLng, radius, googleMap, photoQuestionId) {
    let location = {latitude: latLng.lat(), longitude: latLng.lng(), radius: radius}

    fetch('/api/Tasks/' + taskId + '/PhotoQuestions/' + photoQuestionId + '/Locations', {
        method: 'POST',
        body: JSON.stringify(location),
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            response.json().then(data => {
                addMarker(googleMap, latLng, data.locationId)
            })
        } else {
            throw new Error();
        }
    }).catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })
}

function addMarker(map, location, locationId) {
    let mapDiv = map.getDiv().parentElement
    let photoQuestionId = mapDiv.id.split('_')[1]

    let marker = new google.maps.Marker({
        position: location,
        map: map,
        draggable: true
    })
    marker.setMap(map)

    // Get radius from input
    let radius = mapDiv.querySelector('#radius').value 
    if (radius === null){
        radius = 1
    }

    // Add circle
    let circle = new google.maps.Circle({
        map: map,
        radius: radius * 1000,
        fillColor: '#AA0000'
    });
    circle.bindTo('center', marker, 'position');

    // Remove marker when clicking on the marker
    google.maps.event.addListener(marker, 'click', () => {
        removeLocation(marker, photoQuestionId)
        removeMarker(marker)
        removeCircle(circle)
    })

    // Update location when drag ends
    marker.addListener('dragend', 
            e => updateLocation(marker, e.latLng, photoQuestionId, radius));

    // Adding marker with locationId as a key value pair to markers map
    markersMap.set(marker, { photoQuestionId, locationId })

    // Adding circle with photoQuestionId as a key value pair to circles map
    circlesMap.set(circle, photoQuestionId)
}

export function addMapDivs(questions){
    for (let question of questions){
        addMapDiv(question)
    }
}



    //tooltip?
    // const spanDiv = document.createElement('span')
    // spanDiv.textContent = 'Klik op de map om een locatie toe te voegen, klik er nog eens op om deze te verwijderen. ' +
    //    'Je kunt de locatie verplaatsen door te slepen.'
    //
    // // spanDiv.innerHTML= `<div class="tooltip">
    // //                     <img src="/resources/Icons/questionmark.svg" alt="extra info">
    // //                     <span class="tooltiptext">
    // //                         Klik op de map om een locatie toe te voegen, klik er nog eens op om deze te verwijderen.
    // //                         Je kan de locatie verplaatsen door te slepen.
    // //                     </span>
    // //                     </div>`
    // questionInfoDiv.appendChild(spanDiv)

function addMapDiv(question) {
    const questionInfoDiv = document.querySelector(`#PhotoQuestionMap_${question.photoQuestionId}`)
    const mapDiv = document.createElement('div')
    mapDiv.id = `map_${question.photoQuestionId}`
    mapDiv.innerHTML = `
        <div class="radius-info">
            <div class="form-group">
            <div class="radius-info">
            <label>Radius (km): </label>
            <div class="tooltip">
                   <img  src="/resources/Icons/questionmark.svg" alt="extra info">
                   <span class="tooltiptext">
                          Klik op de map om een locatie toe te voegen, De radius bepaalt de grootte van de cirkel
                    </span>
            </div>
            </div>
            <input class="form-control" id="radius" type="number" placeholder="1" value="1"/>
            </div>
        
            <div class="tooltip">
                   <img class="info-map" src="/resources/Icons/questionmark.svg" alt="extra info">
                   <span class="tooltiptext">
                          Klik op de map om een locatie toe te voegen, klik er nog eens op om deze te verwijderen.
                           Je kan de locatie verplaatsen door te slepen.
                    </span>
            </div>
        </div>
        <div id="mapCanvas" class="map"></div>`
    questionInfoDiv.appendChild(mapDiv)
}
/* End of Add functions */

/* Remove functions */
function removeMarker(marker) {
    markersMap.delete(marker)
    marker.setMap(null)
}

function removeCircle(circle) {
    circlesMap.delete(circle)
    circle.unbindAll()
    circle.setMap(null);
}

function removeLocation(marker, photoQuestionId) {
    let locationId = parseInt(markersMap.get(marker).locationId)
    
    fetch('/api/Tasks/' + taskId + '/PhotoQuestions/' + photoQuestionId + '/Locations/' + locationId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            
        } else {
            throw new Error();
        }
    }).catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })

}
/* End of Remove functions */

/* Update functions */
function updateCircles(photoQuestionId, radius) {
    for (let [ckey, cvalue] of circlesMap.entries()) {
        if (cvalue === photoQuestionId) {
            updateCircleRadius(ckey, radius)
        }
    }
    
    for (let [mkey, mvalue] of markersMap.entries()) {
        if(mvalue.photoQuestionId === photoQuestionId) {
            updateLocation(mkey, mkey.getPosition(), photoQuestionId, radius)
        }
    }
}

function updateCircleRadius(circle, radius) {
    circle.setRadius(radius * 1000)
}

function updateLocation(marker, latLng, photoQuestionId, radius) {
    let locationId = parseInt(markersMap.get(marker).locationId)

    let location = {latitude: latLng.lat(), longitude: latLng.lng(), radius: radius}

    fetch('/api/Tasks/' + taskId + '/PhotoQuestions/' + photoQuestionId + '/Locations/' + locationId, {
        method: 'PUT',
        body: JSON.stringify(location),
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            
        } else {
            throw new Error();
        }
    }).catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })
}
/* End of Update functions */

/* Load functions */
export function loadLocations(questions) {
    for (let question of questions) {
        if (question.locations.length > 0) {
            const mapDiv = document.querySelector(`#map_${question.photoQuestionId}`)
            const mapCanvas = mapDiv.querySelector('#mapCanvas')
            
            const radiusInput = mapDiv.querySelector('#radius')
            radiusInput.value= question.locations[0].radius
            
            const latLng = {lat: question.locations[0].latitude, lng: question.locations[0].longitude}

            const googleMap = loadMap(mapCanvas, latLng)
            loadMarkers(question.locations, googleMap)
        }
    }
}

function loadMap(mapCanvas, latLng) {
    const photoQuestionId = mapCanvas.parentElement.id.split('_')[1]

    let map = new google.maps.Map(mapCanvas, {
        center: latLng,
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    })
    google.maps.event.addListener(map, 'click', (e) => {
        const radius = mapCanvas.parentElement.querySelector('#radius').value
        addLocation(e.latLng, radius, map, photoQuestionId)
    })
    return map
}

function loadMarkers(locations, googleMap) {
    for (let location of locations) {
        addMarker(googleMap, {lat: location.latitude, lng: location.longitude}, location.locationId)
    }
}
/* End of Load functions */