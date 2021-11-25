<?php
defined('_JEXEC') or die( 'Restricted access' );
        $locations=array(); 
		
		$sqlmaparray = "SELECT * 
			  FROM smartapp_farmerdetails ";
					$database->setQuery($sqlmaparray);
					$resultmaparray = $database->query($sqlmaparray);
					while($row= mysqli_fetch_assoc($resultmaparray)){
					$smartappid = $row[smartappID];
					$latitude = $row[lati];
					$longitude = $row[longi];
					$noofponds = $row[noPonds];
					$species = $row[typeofFish];
					$icon = $row[icon];
					
		$sqlmaparray2 = "SELECT name  
			  FROM smart_users
					WHERE (id = '$smartappid')";
					$database->setQuery($sqlmaparray2);
					$resultmaparray2 = $database->query($sqlmaparray2);
					while($row= mysqli_fetch_assoc($resultmaparray2)){
					$name = $row[name];
}

            /* Each row is added as a new array */
            $locations[]=array( 'name'=>$name, 'lat'=>$latitude, 'lng'=>$longitude, 'lnk'=>$icon );
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
            '<p><?php echo $locations[$i]['name'];?></p>',
            <?php echo $locations[$i]['lat'];?>,
            <?php echo $locations[$i]['lng'];?>,
			'<?php echo $locations[$i]['lnk'];?>',
			0
        ]<?php if($j!=sizeof($locations))echo ","; }?>
    ];
    var origin = new google.maps.LatLng(locations[0][2], locations[0][3]);

    function initialize() {
      var mapOptions = {
        zoom: 5,
        center: {lat:-30.709706, lng:24.681774}
      };

      map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

        infowindow = new google.maps.InfoWindow();

        for(i=0; i<locations.length; i++) {
            var position = new google.maps.LatLng(locations[i][2], locations[i][3]);
			var iconString = (locations[i][4]);
			alert (iconString);
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

        locate(0);

    }

    function locate(marker_id) {
        var myMarker = Markers[marker_id];
        var markerPosition = myMarker.getPosition();
        //map.setCenter(markerPosition);
        google.maps.event.trigger(myMarker, 'click');
    }

    google.maps.event.addDomListener(window, 'load', initialize);

    </script>
	<style>
#map-canvas{
	width:100%;
   height:410px;
}
</style>
    <body id="map-canvas">