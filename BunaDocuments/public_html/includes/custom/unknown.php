<?php
defined('_JEXEC') or die( 'Restricted access' );
?>
<table style="float: left;" cellpadding="3">
<tbody>
<tr>
<td colspan="1"><strong><span style="font-size: 8pt;">Dashboard Menu</span></strong></td>
<td colspan="9"><strong><span style="font-size: 8pt;">Legend</span></strong></td>
</tr>
<tr>
<td><center>{loadmoduleid 148}</center></td>
<td style="text-align: center;"><a href="./index.php/abalone"><img src="images/icons/abalone.png" alt="abalone" /></a></td>
<td style="text-align: center;"><a href="./index.php/catfish"><img src="images/icons/catfish.png" alt="catfish" /></a></td>
<td style="text-align: center;"><a href="./index.php/koi"><img src="images/icons/koi.png" alt="koi" /></a></td>
<td style="text-align: center;"><a href="./index.php/mussel"><img src="images/icons/mussel.png" alt="mussel" /></a></td>
<td style="text-align: center;"><a href="./index.php/oyster"><img src="images/icons/oyster.png" alt="oyster" /></a></td>
<td style="text-align: center;"><a href="./index.php/shrimp"><img src="images/icons/shrimp.png" alt="shrimp" /></a></td>
<td style="text-align: center;"><a href="./index.php/tilapia"><img src="images/icons/tilapia.png" alt="tilapia" /></a></td>
<td style="text-align: center;"><a href="./index.php/trout"><img src="images/icons/trout.png" alt="trout" /></a></td>
<td style="text-align: center;"><a href="./index.php/unknown"><img src="images/icons/unknown.png" alt="unknown" /></a></td>
</tr>
<tr>
<td></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Abalone</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Catfish</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Koi</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Mussel</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Oyster</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Shrimp</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Tilapia</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Trout</span></td>
<td style="text-align: center;"><span style="font-size: 8pt;">Unknown</span></td>
</tr>
</tbody>
</table>
<br><br>
<?php
// check if there is data to display
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( species ) AS species
FROM smartapp_species
WHERE (
species =  ''
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$speciescount = $row['species'];
					}
	if($speciescount == 0){
		?><br><br><br><br><?php echo "There is no data to display.";?></p><?php
	}
// Get the data from the database table for the first data set
$sqlmaparray = "SELECT * 
			  FROM smartapp_farmerdetails 
			  WHERE (typeofFish IS NULL OR typeofFish = '')";
					$database->setQuery($sqlmaparray);
					$resultmaparray = $database->query($sqlmaparray);
					while($row= mysqli_fetch_assoc($resultmaparray)){
					$smartappid = $row[smartappID];
					$latitude = $row[lati];
					$longitude = $row[longi];
					$noofponds = $row[noPonds];
					$species = $row[typeofFish];
					$icon = "https://buna.africa/images/icons/unknown.png";
					
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
        zoom: 5,
        center: {lat:-30.709706, lng:24.681774}
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