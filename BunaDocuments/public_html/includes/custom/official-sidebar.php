<?php
defined('_JEXEC') or die( 'Restricted access' );
?>
<table class="table table-striped" style="text-align: center; width: 100%;">
		<tr>
		<td class="rounded-corners" style="text-align: center; color: #FFFFFF; background-color: #6D850A;" colspan="2">By Province</td>
	</tr>
	<tr>
		<th  style="width: 50%; text-align: left;">Province</th>
		<th  style="width: 50%; text-align: center;">Registered Farmers</th>
	</tr>
<?php
// start ec
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS easterncapecount
FROM smartapp_farmerdetails
WHERE (
province =  'Eastern Cape'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$easterncapecount = $row['easterncapecount'];
		$easterncape = "Eastern Cape";
		$easterncapepage = "eastern-cape";
					}

$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS noeasterncapecount
FROM smartapp_farmerdetails
WHERE (
province =  'Eastern Cape'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$noeasterncapecount = $row['noeasterncapecount'];
		$noeasterncape = "No GPS available";
		}
					
						
// start free state
			  $sqlcount = "SELECT COUNT( province ) AS freestatecount
FROM smartapp_farmerdetails
WHERE (
province =  'Free State'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$freestatecount = $row['freestatecount'];
		$freestate = "Free State";
		$freestatepage = "free-state";
					}

$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nofreestatecount
FROM smartapp_farmerdetails
WHERE (
province =  'Free State'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nofreestatecount = $row['nofreestatecount'];
		$nofreestate = "No GPS available";
		}

// start gauteng
			  $sqlcount = "SELECT COUNT( province ) AS gautengcount
FROM smartapp_farmerdetails
WHERE (
province =  'Gauteng'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$gautengcount = $row['gautengcount'];
		$gauteng = "Gauteng";
		$gautengpage = "gauteng";
					}

$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nogautengcount
FROM smartapp_farmerdetails
WHERE (
province =  'Gauteng'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nogautengcount = $row['nogautengcount'];
		$nogauteng = "No GPS available";
		}

// start kzn
			  $sqlcount = "SELECT COUNT( province ) AS kwazulunatalcount
FROM smartapp_farmerdetails
WHERE (
province =  'KwaZulu-Natal'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$kwazulunatalcount = $row['kwazulunatalcount'];
		$kwazulunatal = "KwaZulu-Natal";
		$kwazulunatalpage = "kwazulu-natal";
					}
					
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nokwazulunatalcount
FROM smartapp_farmerdetails
WHERE (
province =  'KwaZulu-Natal'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nokwazulunatalcount = $row['nokwazulunatalcount'];
		$nokwazulunatal = "No GPS available";
		}					
// start limpopo
			  $sqlcount = "SELECT COUNT( province ) AS limpopocount
FROM smartapp_farmerdetails
WHERE (
province =  'Limpopo'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$limpopocount = $row['limpopocount'];
		$limpopo = "Limpopo";
		$limpopopage = "limpopo";
		
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nolimpopocount
FROM smartapp_farmerdetails
WHERE (
province =  'Limpopo'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nolimpopocount = $row['nolimpopocount'];
		$nolimpopo = "No GPS available";
		}		
					}
// start mpumalanga
			  $sqlcount = "SELECT COUNT( province ) AS mpumalangacount
FROM smartapp_farmerdetails
WHERE (
province =  'Mpumalanga'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$mpumalangacount = $row['mpumalangacount'];
		$mpumalanga = "Mpumalanga";
		$mpumalangapage = "mpumalanga";
					}
					
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nompumalangacount
FROM smartapp_farmerdetails
WHERE (
province =  'Mpumalanga'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nompumalangacount = $row['nompumalangacount'];
		$nompumalanga = "No GPS available";
		}
// start north west
			  $sqlcount = "SELECT COUNT( province ) AS northwestcount
FROM smartapp_farmerdetails
WHERE (
province =  'North West'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$northwestcount = $row['northwestcount'];
		$northwest = "North West";
		$northwestpage = "north-west";
					}

$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nonorthwestcount
FROM smartapp_farmerdetails
WHERE (
province =  'North West'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nonorthwestcount = $row['nonorthwestcount'];
		$nonorthwest = "No GPS available";
		}
// start northern cape
			  $sqlcount = "SELECT COUNT( province ) AS northerncapecount
FROM smartapp_farmerdetails
WHERE (
province =  'Northern Cape'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$northerncapecount = $row['northerncapecount'];
		$northerncape = "Northern Cape";
		$northerncapepage = "northern-cape";
					}
					
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nonortherncapecount
FROM smartapp_farmerdetails
WHERE (
province =  'Northern Cape'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nonortherncapecount = $row['nonortherncapecount'];
		$nonortherncape = "No GPS available";
		}					
// start western cape
			  $sqlcount = "SELECT COUNT( province ) AS westerncapecount
FROM smartapp_farmerdetails
WHERE (
province =  'Western Cape'
)
AND (
lati <>0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$westerncapecount = $row['westerncapecount'];
		$westerncape = "Western Cape";
		$westerncapepage = "western-cape";
					}					
					
$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT( province ) AS nowesterncapecount
FROM smartapp_farmerdetails
WHERE (
province =  'Eastern Cape'
)
AND (
lati = 0
)"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$nowesterncapecount = $row['nowesterncapecount'];
		$nowesterncape = "No GPS available";
		}					
						
													 
								 ?>
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/eastern-cape"><?php echo $easterncape; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $noeasterncape; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $easterncapecount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $noeasterncapecount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/free-state"><?php echo $freestate; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nofreestate; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $freestatecount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nofreestatecount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/gauteng"><?php echo $gauteng; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nogauteng; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $gautengcount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nogautengcount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/kwazulu-natal"><?php echo $kwazulunatal; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nokwazulunatal; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $kwazulunatalcount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nokwazulunatalcount; ?></p></td>
	</tr>

	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/limpopo"><?php echo $limpopo; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nolimpopo; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $limpopocount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nolimpopocount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/mpumalanga"><?php echo $mpumalanga; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nompumalanga; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $mpumalangacount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nompumalangacount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/north-west"><?php echo $northwest; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nonorthwest; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $northwestcount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nonorthwestcount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/western-cape"><?php echo $westerncape; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nowesterncape; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $westerncapecount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nowesterncapecount; ?></p></td>
	</tr>
	
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/northern-cape"><?php echo $northerncape; ?></a>
		<p style="font-size: x-small; color: #333333;"><?php echo $nonortherncape; ?></p></td>
		<td style="width: 50%; text-align: center;"><?php echo $northerncapecount; ?>
		<p style="font-size: x-small; color: #333333;"><?php echo $nonortherncapecount; ?></p></td>
	</tr>


									<?php
								 

								 $database->close();
	
	$database2 =& JFactory::getDBO();
			  $sqlcount2 = "SELECT COUNT(smartappID) AS farmercount
  FROM smartapp_users
 WHERE accountType = 'Farmer' "; 
					$database2->setQuery($sqlcount2);
					$resultcount2 = $database2->query($sqlcount2);
					while ($row=mysqli_fetch_assoc($resultcount2)){
					$farmercount = $row['farmercount'];
					}
?>
<tr>
		<td style="width: 50%; text-align: left;"><b>TOTAL</b></td>
		<td style="width: 50%; text-align: center;"><?php echo $farmercount; ?></td>
	</tr>
</table>

<table class="table table-striped" style="text-align: center; width: 100%;">
	<tr>
		<td class="rounded-corners" style="text-align: center; color: #FFFFFF; background-color: #6D850A;" colspan="2">By Species</td>
	</tr>
	<tr>
		<th  style="width: 50%; text-align: left;">Species</th>
		<th  style="width: 50%; text-align: center;">Registered Farmers</th>
	</tr>
<?php
$database3 =& JFactory::getDBO();
			  $sqlcount3 = "
SELECT species, (COUNT(species)) AS fishcount
  FROM smartapp_species
GROUP BY species
ORDER BY species ASC"; 
					$database3->setQuery($sqlcount3);
					$resultcount3 = $database->query($sqlcount3);
					while ($row=mysqli_fetch_assoc($resultcount3)){
					$species = $row['species'];
					$fishcount = $row['fishcount'];
					
					if($species == ""){
						$species = "No species given";
					}
					if($species == "Unknown"){
						$speciespage = "unknown";
					}
					if($species == "Other"){
						$speciespage = "other";
					}					
					if( $species == "Tilapia"){
						$speciespage = "tilapia";
					}
					if( $species == "Catfish"){
						$speciespage = "catfish";
					}
					if( $species == "Trout"){
						$speciespage = "trout";
					}
					if( $species == "Koi"){
						$speciespage = "koi";
					}
					if( $species == "Abalone"){
						$speciespage = "abalone";
					}
					if( $species == "Mussel"){
						$speciespage = "mussel";
					}
					if( $species == "Oyster"){
						$speciespage = "oyster";
					}
					if( $species == "Shrimp"){
						$speciespage = "shrimp";
					}
	if ($species == "No species given"){
		?>
		<tr>
		<td style="width: 50%; text-align: left;"><?php echo $species; ?></td>
		<td style="width: 50%; text-align: center;"><?php echo $fishcount; ?></td>
	</tr>
	<?php
	} else {
								 
								 ?>
	<tr>
		<td style="width: 50%; text-align: left;"><a href="./index.php/<?php echo $speciespage; ?>"><?php echo $species; ?></a></td>
		<td style="width: 50%; text-align: center;"><?php echo $fishcount; ?></td>
	</tr>


									<?php
	}								 
}
	 $database3->close();
?>

