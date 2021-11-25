<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;
// Get the users town
$sqlstatus = "SELECT * FROM smartapp_farmerdetails
					WHERE (smartappID = '$id')"; 
						$database->setQuery($sqlstatus);
						$resultstatus = $database->query($sqlstatus);
					$database->close();

					while ($row=mysqli_fetch_assoc($resultstatus)){
						$province = $row['province'];
						$nearesttown = $row['nearestTown'];
						 }
?>
<script src="https://www.yr.no/place/South_Africa/<?php echo $province; ?>/<?php echo $nearesttown; ?>/external_box_small.js"></script>
<noscript><a href="https://www.yr.no/place/South_Africa/<?php echo $province; ?>/<?php echo $nearesttown; ?>/">yr.no: Forecast for <?php echo $nearesttown; ?></a></noscript>