<?php
defined('_JEXEC') or die( 'Restricted access' );
// Get the data from the database table for the first data set
$sqlmaparray = "SELECT * 
			  FROM smartapp_farmerdetails
				WHERE (province = 'North West')";
					$database->setQuery($sqlmaparray);
					$resultmaparray = $database->query($sqlmaparray);
					while($row= mysqli_fetch_assoc($resultmaparray)){
					$smartappid = $row[smartappID];
					$latitude = $row[lati];
					$longitude = $row[longi];
					$noofponds = $row[noPonds];
					$species = $row[typeofFish];
					$icon = $row[icon];
					
					if($noofponds == 0){
						$noofponds = "Unknown";
					}
					if($species == ""){
						$species = "Unknown";
					}
					if($icon == ""){
						$icon = "https://buna.africa/images/icons/unknown.png";
					}
					
// Get the data from the database table for the second data set					
$sqlmaparray2 = "SELECT name  
			  FROM smart_users
					WHERE (id = '$smartappid')";
					$database->setQuery($sqlmaparray2);
					$resultmaparray2 = $database->query($sqlmaparray2);
					while($row= mysqli_fetch_assoc($resultmaparray2)){
					$name = $row[name];
}
//Create an array with the two datasets
					$locations[] = array( 'name'=>$name, 'lat'=>$latitude, 'lng'=>$longitude, 'ponds'=>$noofponds, 'species'=>$species, 'icon'=>$icon );
					}
					?>

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAvVEWeV5RcDhYE3DZyjLKhp1uukk0OuxY"></script> 
    <script type="text/javascript">
	var map;
    var Markers = {};
    var infowindow;
	var locations = [
        <?php for($i=0;$i<sizeof($locations);$i++){ $j=$i+1;?>
        [
            'Farmers',
            '<p><b><?php echo $locations[$i]['name'];?></b><br>No. of ponds: <?php echo $locations[$i]['ponds'];?><br>Species: <?php echo $locations[$i]['species'];?><br>Latitude: <?php echo $locations[$i]['lat'];?><br>Longitude: <?php echo $locations[$i]['lng'];?></p>',
            <?php echo $locations[$i]['lat'];?>,
            <?php echo $locations[$i]['lng'];?>,
			'<?php echo $locations[$i]['icon'];?>',
            0
        ]<?php if($j!=sizeof($locations))echo ","; }?>
		];	
	var origin = new google.maps.LatLng(locations[0][2], locations[0][3]);

    function initialize() {
      var mapOptions = {
        zoom: 6.3,
        center: {lat:-26.465659, lng:25.471169}
      };
      map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
        infowindow = new google.maps.InfoWindow();
        for(i=0; i<locations.length; i++) {
			var position = new google.maps.LatLng(locations[i][2], locations[i][3]);
			var iconString = (locations[i][4]);
			var marker = new google.maps.Marker({
                position: position,
                map: map,
				icon: iconString
            });
            google.maps.event.addListener(marker, 'click', (function(marker, i) {
                return function() {
                    infowindow.setContent(locations[i][1]);
                    infowindow.setOptions({maxWidth: 200});
                    infowindow.open(map, marker);
                }
            }) (marker, i));
			Markers[locations[i][4]] = marker;
                    }
    }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
	<style>
#map-canvas{
	width:100%;
   height:410px;
}
</style>
    <div id="map-canvas"></div>					