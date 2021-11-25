<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;

	if(isset($_POST['submitted'])) 
	{
		$province = addslashes($_POST['province']);
		$nearesttown = addslashes($_POST['nearesttown']);
		$noofponds = addslashes($_POST['noofponds']);
		$typeoffish = addslashes($_POST['typeoffish']);
		$typeoffish4 = addslashes($_POST['typeoffish4']);
		$typeoffish5 = addslashes($_POST['typeoffish5']);
		$latitude = addslashes($_POST['latitude']);
		$longitude = addslashes($_POST['longitude']);
		//Type of icon goes here
					
					if($typeoffish == "Unknown"){
						$icon = "https://buna.africa/images/icons/unknown.png";
					}
					if(empty($typeoffish)){
						$icon = "https://buna.africa/images/icons/unknown.png";
					}
					if($typeoffish == "Other"){
						$icon = "https://buna.africa/images/icons/other.png";
					}					
					if( $typeoffish == "Tilapia"){
						$icon = "https://buna.africa/images/icons/tilapia.png";
					}
					if( $typeoffish == "Catfish"){
						$icon = "https://buna.africa/images/icons/catfish.png";
					}
					if( $typeoffish == "Trout"){
						$icon = "https://buna.africa/images/icons/trout.png";
					}
					if( $typeoffish == "Koi"){
						$icon = "https://buna.africa/images/icons/koi.png";
					}
					if( $typeoffish == "Abalone"){
						$icon = "https://buna.africa/images/icons/abalone.png";
					}
					if( $typeoffish == "Mussel"){
						$icon = "https://buna.africa/images/icons/mussel.png";
					}
					if( $typeoffish == "Oyster"){
						$icon = "https://buna.africa/images/icons/oyster.png";
					}
					if( $typeoffish == "Shrimp"){
						$icon = "https://buna.africa/images/icons/shrimp
						.png";
					}
					
					
					if($typeoffish4 == "Unknown"){
						$icon4 = "https://buna.africa/images/icons/unknown.png";
					}
					if($typeoffish4 === NULL){
						$icon = "https://buna.africa/images/icons/unknown.png";
					}
					if($typeoffish4 == "Other"){
						$icon4 = "https://buna.africa/images/icons/other.png";
					}					
					if( $typeoffish4 == "Tilapia"){
						$icon4 = "https://buna.africa/images/icons/tilapia.png";
					}
					if( $typeoffish4 == "Catfish"){
						$icon4 = "https://buna.africa/images/icons/catfish.png";
					}
					if( $typeoffish4 == "Trout"){
						$icon4 = "https://buna.africa/images/icons/trout.png";
					}
					if( $typeoffish4 == "Koi"){
						$icon4 = "https://buna.africa/images/icons/koi.png";
					}
					if( $typeoffish4 == "Abalone"){
						$icon4 = "https://buna.africa/images/icons/abalone.png";
					}
					if( $typeoffish4 == "Mussel"){
						$icon4 = "https://buna.africa/images/icons/mussel.png";
					}
					if( $typeoffish4 == "Oyster"){
						$icon4 = "https://buna.africa/images/icons/oyster.png";
					}
					if( $typeoffish4 == "Shrimp"){
						$icon4 = "https://buna.africa/images/icons/shrimp
						.png";
					}
					
					
					if($typeoffish5 == "Unknown"){
						$icon5 = "https://buna.africa/images/icons/unknown.png";
					}
					if($typeoffish5 === NULL){
						$icon = "https://buna.africa/images/icons/unknown.png";
					}
					if($typeoffish5 == "Other"){
						$icon5 = "https://buna.africa/images/icons/other.png";
					}					
					if( $typeoffish5 == "Tilapia"){
						$icon5 = "https://buna.africa/images/icons/tilapia.png";
					}
					if( $typeoffish5 == "Catfish"){
						$icon5 = "https://buna.africa/images/icons/catfish.png";
					}
					if( $typeoffish5 == "Trout"){
						$icon5 = "https://buna.africa/images/icons/trout.png";
					}
					if( $typeoffish5 == "Koi"){
						$icon5 = "https://buna.africa/images/icons/koi.png";
					}
					if( $typeoffish5 == "Abalone"){
						$icon5 = "https://buna.africa/images/icons/abalone.png";
					}
					if( $typeoffish5 == "Mussel"){
						$icon5 = "https://buna.africa/images/icons/mussel.png";
					}
					if( $typeoffish5 == "Oyster"){
						$icon5 = "https://buna.africa/images/icons/oyster.png";
					}
					if( $typeoffish5 == "Shrimp"){
						$icon5 = "https://buna.africa/images/icons/shrimp
						.png";
					}
		// update the user status
	$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smartapp_farmerdetails 
SET 
	province = '$province',
	nearestTown = '$nearesttown',
	noPonds = '$noofponds',
	typeofFish = '$typeoffish',
	typeofFish4 = '$typeoffish4',
	typeofFish5 = '$typeoffish5',
	lati = '$latitude',
	longi = '$longitude',
	icon = '$icon',
	icon4 = '$icon4',
	icon5 = '$icon5'
WHERE
    smartappID = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
					
	if ($sqluserupdate){
		JFactory::getApplication()->enqueueMessage('Thanks. Your information has been updated.','success');
	}
	}

// update the user profile
		$sqlstatus = "SELECT * FROM smartapp_farmerdetails
					WHERE (smartappID = '$id')"; 
						$database->setQuery($sqlstatus);
						$resultstatus = $database->query($sqlstatus);
					$database->close();

					while ($row=mysqli_fetch_assoc($resultstatus)){
								 $setupstatus = $row['setupStatus'];
		$province = $row['province'];
		$nearesttown = $row['nearestTown'];
		$noofponds = $row['noPonds'];
		$typeoffish = $row['typeofFish'];
		$typeoffish4 = $row['typeofFish4'];
		$typeoffish5 = $row['typeofFish5'];
		$latitude = $row['lati'];
		$longitude = $row['longi'];
								 }
								 
// update the species table
$sqldel = "DELETE FROM smartapp_species WHERE smartappID = '$id'"; 
						$database->setQuery($sqldel);
						$resultdel = $database->query($sqldel);
					$database->close();
					
if ($typeoffish > ""){
// Update the species table
$database6 =& JFactory::getDBO();
	$sql6 = "INSERT INTO smartapp_species (smartappID, species)
	VALUES ('$id', '$typeoffish')"; 
	$database6->setQuery($sql6);
	$database6->query();
}

if ($typeoffish4 > ""){
// Update the species table
$database7 =& JFactory::getDBO();
	$sql7 = "INSERT INTO smartapp_species (smartappID, species)
	VALUES ('$id', '$typeoffish4')"; 
	$database7->setQuery($sql7);
	$database7->query();
}

if ($typeoffish5 > ""){
// Update the species table
$database8 =& JFactory::getDBO();
	$sql8 = "INSERT INTO smartapp_species (smartappID, species)
	VALUES ('$id', '$typeoffish5')"; 
	$database8->setQuery($sql8);
	$database8->query();
$database8->close();
}			



?>
<p>
<a class="jcepopup zoom-left" href="index.php?option=com_content&amp;view=article&amp;id=34:information-farmer-details&amp;catid=2:uncategorised" type="text/html" data-mediabox="1">Help</a></p>
	
	<tr>
		<td>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<table style="width: 100%">
<tr>
<td style="color: #999999;">
<label>Province</label>
<?php
$database =& JFactory::getDBO();
	$sql = "SELECT * FROM smart_lkp_province
	ORDER BY province ASC"; 
	$database->setQuery($sql);
	$result = $database->query($sql);
$database->close();

echo'<select name="province" style="color: grey;">';
?><option selected><?php echo $province; ?></option><?php
while ($row=mysqli_fetch_assoc($result)){
	echo"<option> $row[province] </option>";
$province = $row[province];
}
echo'</select>';
?>
</td>
</tr>
<tr>
<td style="color: #999999;"><label>Nearest Town</label>
		<input name="nearesttown" type="text" placeholder="Nearest Town" value="<?php echo $nearesttown; ?>" /></td>
	</tr>
 <tr>
<td style="color: #999999;">
<label>Number of ponds</label>
<?php
$database2 =& JFactory::getDBO();
	$sql2 = "SELECT * FROM smart_lkp_noofponds"; 
	$database2->setQuery($sql2);
	$result2 = $database2->query($sql2);
$database2->close();

echo'<select name="noofponds" style="color: grey;">';
?><option selected><?php echo $noofponds; ?></option><?php
while ($row=mysqli_fetch_assoc($result2)){
	echo"<option> $row[no_of_ponds] </option>";
$noofponds = $row[no_of_ponds];
}
echo'</select>';
?>
	</td>
	</tr>
		<tr>
		<td style="color: #999999;">
		<label>Latitude</label>
		<input name="latitude" type="text" placeholder="Latitude" value = "<?php echo $latitude; ?>" /></td>
	</tr>
	<tr>
		<td style="color: #999999;">
		<label>Longitude</label>
		<input name="longitude" type="text" placeholder="Longitude" value = "<?php echo $longitude; ?>" /></td>
	</tr>
	<tr>
<tr>
<td style="color: #999999;">
<label>Species of fish</label>
<?php
$database3 =& JFactory::getDBO();
	$sql3 = "SELECT * FROM smart_lkp_fishtype
	ORDER BY type_of_fish ASC"; 
	$database3->setQuery($sql3);
	$result3 = $database3->query($sql3);
$database3->close();

echo'<select name="typeoffish" style="color: grey;" required>';
?><option selected><?php echo $typeoffish; ?></option><?php
while ($row=mysqli_fetch_assoc($result3)){
	echo"<option> $row[type_of_fish] </option>";
$typeoffish = $row[type_of_fish];
}
echo'</select>';
?>
	</td>
	</tr>
<tr>
<td>
<?php
$database4 =& JFactory::getDBO();
	$sql4 = "SELECT * FROM smart_lkp_fishtype"; 
	$database4->setQuery($sql4);
	$result4 = $database4->query($sql4);
$database4->close();

echo'<select name="typeoffish4" style="color: grey;">';
?><option selected><?php echo $typeoffish4; ?></option><?php
while ($row=mysqli_fetch_assoc($result4)){
	echo"<option> $row[type_of_fish] </option>";
$typeoffish4 = $row[type_of_fish];
}
echo'</select>';
?>
	</td>
	</tr>
	
<tr>
<td>
<?php
$database5 =& JFactory::getDBO();
	$sql5 = "SELECT * FROM smart_lkp_fishtype"; 
	$database5->setQuery($sql5);
	$result5 = $database5->query($sql5);
$database5->close();

echo'<select name="typeoffish5" style="color: grey;">';
?><option selected><?php echo $typeoffish5; ?></option><?php
while ($row=mysqli_fetch_assoc($result5)){
	echo"<option> $row[type_of_fish] </option>";
$typeoffish5 = $row[type_of_fish];
}
echo'</select>';
?>
</tr>
<tr>
		<td colspan="2">
<input class="btn btn-light-green" name="submitted" type="submit" value="Submit" /></td>
	</tr>
</table>
</form>
<script>
                setTimeout(function() {
                    jQuery('#system-message-container').fadeOut('slow');
                }, 3000);
            </script>