<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;
// create the farmer details id
	$database =& JFactory::getDBO();
					$sqluser = "INSERT INTO smartapp_farmerdetails (smartappID)
					VALUES ('$id')";
					$database->setQuery($sqluser);
					$database->query();

$lat=(isset($_GET['lat']))?$_GET['lat']:'';
$lng=(isset($_GET['long']))?$_GET['long']:'';
?>

<p><button class="btn btn-light-green" onclick="getLocation()">Get co-ordinates</button></p>
<p>		<a href="./index.php/farmer-details" class="btn btn-dark-green" role="button">Next</a>
</p>

<p id="coord"></p>
<div class="centered">
    <p><i class="icon-spin icon-spinner" id="loaderdiv" style="display:none;"></i></p>
	<p id="loadertext" class="loadertext" style="display:none;">Attempting to retrieve co-ordinates ...</span></p>
</div>

	
<script>
var x = document.getElementById("coord");
function getLocation() {
	document.getElementById('loaderdiv').style.display = "block";
	document.getElementById('loadertext').style.display = "block";	
	if (navigator.geolocation) {
		var location_timeout = setTimeout("geolocFail()", 4000);
		navigator.geolocation.getCurrentPosition(redirectToPosition)
		clearTimeout(location_timeout);
		var lat = position.coords.latitude;
        var lng = position.coords.longitude;
	 function error() {
        clearTimeout(location_timeout);
        geolocFail();
    };
} else {
    // If there is no geolocation
	x.innerHTML = "Geolocation is not supported by this browser.";
    geolocFail();
}
}
function redirectToPosition(position) {
    window.location='farmer-details?lat='+position.coords.latitude+'&long='+position.coords.longitude;
}
</script>