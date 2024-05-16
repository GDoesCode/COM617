window.initializeBingMaps = function (cb) {
    if (Microsoft && Microsoft.Maps) {
        var map = new Microsoft.Maps.Map('#map', {
            credentials: 'ApXUlPc2Bp6Pm9sbrCr5URzTiJxpa3ORFy-jrPgtecmQXCkGOcZ7i820Lp97gpNr'
        });

        Microsoft.Maps.Events.addHandler(map, 'click', async function (e) {
            var point = new Microsoft.Maps.Point(e.getX(), e.getY());
            var location = e.target.tryPixelToLocation(point);

            // Send location data back to Blazor code
            await cb.invokeMethodAsync('OnMapClicked', location.latitude, location.longitude);
        });
    } else {
        console.error('Microsoft.Maps namespace is not available. Bing Maps SDK may not be loaded.');
    }
}