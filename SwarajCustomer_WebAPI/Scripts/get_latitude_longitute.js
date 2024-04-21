function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    } else {
        displayMessage('Geolocation is not supported by this browser.', 'warning');
    }
}

function showPosition(position) {
    sessionStorage.removeItem("Latitude");
    sessionStorage.removeItem("Longitude");

    sessionStorage.setItem("Latitude", parseFloat(position.coords.latitude).toFixed(8));
    sessionStorage.setItem("Longitude", parseFloat(position.coords.longitude).toFixed(8));

}

function showError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            displayMessage('User denied the request for Geolocation.', 'warning');
            break;
        case error.POSITION_UNAVAILABLE:
            displayMessage('Location information is unavailable.', 'warning');
            break;
        case error.TIMEOUT:
            displayMessage('The request to get user location timed out.', 'warning');
            break;
        case error.UNKNOWN_ERROR:
            displayMessage('An unknown error occurred.', 'warning');
            break;
    }
}


