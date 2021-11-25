<?php
defined('_JEXEC') or die( 'Restricted access' );
?>
    <script type="text/javascript" src="./includes/custom/js/jquery.js"></script>
	<script type="text/javascript" src="./includes/custom/js/jquery-ui.js"></script>
	
<?php
$lat=(isset($_GET['lat']))?$_GET['lat']:'';
$long=(isset($_GET['long']))?$_GET['long']:'';
$user = & JFactory::getUser();
$id = $user->id;

	if(isset($_POST['submitted'])) 
	{
		// check that user does not exist before creating a new user for those users that double click the submit button
		$database =& JFactory::getDBO();
			  $sqlcount = "SELECT province, COUNT( smartappID ) AS smartappid
FROM smartapp_farmerdetails
WHERE (
smartappID =  '$id'
)AND (
province IS NOT NULL
)";
$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					while ($row=mysqli_fetch_assoc($resultcount)){
		$idcount = $row['smartappid'];
					}
if ($idcount > 0){
	JFactory::getApplication()->enqueueMessage('This user account already exists.','info');
}else{
		
		JFactory::getApplication()->enqueueMessage('Thanks. Your information has been saved.','success');
		
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
					if($typeoffish == ""){
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
					if($typeoffish4 == ""){
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
					if($typeoffish5 == ""){
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

if ($typeoffish > "" && $idcount == 0){
// Update the species table
$database6 =& JFactory::getDBO();
	$sql6 = "INSERT INTO smartapp_species (smartappID, species)
	VALUES ('$id', '$typeoffish')"; 
	$database6->setQuery($sql6);
	$database6->query();
}

if ($typeoffish4 > ""&& $idcount == 0){
// Update the species table
$database7 =& JFactory::getDBO();
	$sql7 = "INSERT INTO smartapp_species (smartappID, species)
	VALUES ('$id', '$typeoffish4')"; 
	$database7->setQuery($sql7);
	$database7->query();
}

if ($typeoffish5 > ""&& $idcount == 0){
// Update the species table
$database8 =& JFactory::getDBO();
	$sql8 = "INSERT INTO smartapp_species (smartappID, species)
	VALUES ('$id', '$typeoffish5')"; 
	$database8->setQuery($sql8);
	$database8->query();
$database8->close();
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
					
$database =& JFactory::getDBO();
			  $sqlgroup = "SELECT group_id 
FROM smart_user_usergroup_map
WHERE (
user_id =  '$id'
)";
$database->setQuery($sqlgroup);
					$resultgroup = $database->query($sqlgroup);
					while ($row=mysqli_fetch_assoc($resultgroup)){
		$group = $row['group_id'];
					}
	if ($group == 17){
		header('Location: ./index.php/supplier-details');
	}else{
		header('Location: ./index.php');
	}
	}

if (isset($_GET['lat'])&isset($_GET['long'])){
	$vis = "hidden";
	echo "We were able to retrieve your co-ordinates:<br>
	<b>Latitude:</b> ";
	echo $lat;
	echo "<br><b>Longitude:</b> ";
	echo $long;
	

}else{
	echo "We could not retrieve your co-ordinates.<br>
	Please enter them manually if you are able to.";
}

// update the user status
	$database =& JFactory::getDBO();
					$sqluser = "UPDATE smartapp_farmerdetails
SET 
	lati = '$latitude',
    longi = '$longitude'
WHERE
    smartappID = '$id'";
					$database->setQuery($sqluser);
					$database->query();
	}
?>
<p>
<a class="jcepopup zoom-left" href="index.php?option=com_content&amp;view=article&amp;id=34:information-farmer-details&amp;catid=2:uncategorised" type="text/html" data-mediabox="1">Help</a></p>
	
	<tr>
		<td>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<table style="width: 100%">
<tr>
<td>
<?php
$database =& JFactory::getDBO();
	$sql = "SELECT * FROM smart_lkp_province
	ORDER BY province ASC"; 
	$database->setQuery($sql);
	$result = $database->query($sql);
$database->close();

echo'<select name="province" style="color: grey;" required>';
echo'<option value="" selected disabled>Province</option>';
while ($row=mysqli_fetch_assoc($result)){
	echo"<option> $row[province] </option>";
$province = $row[province];
}
echo'</select>';
?>
</td>
</tr>
<tr>
		<td><input id="nearesttown" name="nearesttown" type="text" placeholder="Nearest Town" /></td>
	</tr>
 <tr>
<td>
<?php
$database2 =& JFactory::getDBO();
	$sql2 = "SELECT * FROM smart_lkp_noofponds"; 
	$database2->setQuery($sql2);
	$result2 = $database2->query($sql2);
$database2->close();

echo'<select name="noofponds" style="color: grey;">';
echo'<option selected disabled>Number of Ponds</option>';
while ($row=mysqli_fetch_assoc($result2)){
	echo"<option> $row[no_of_ponds] </option>";
$noofponds = $row[no_of_ponds];
}
echo'</select>';
?>
	</td>
	</tr>
	<tr>
		<td <?php echo $vis; ?>><input name="latitude" type="text" placeholder="Latitude" value = "<?php echo $lat; ?>" /></td>
	</tr>
	<tr>
		<td <?php echo $vis; ?>><input name="longitude" type="text" placeholder="Longitude" value = "<?php echo $long; ?>" /></td>
	</tr>
	<tr>
<td>
<?php
$database3 =& JFactory::getDBO();
	$sql3 = "SELECT * FROM smart_lkp_fishtype"; 
	$database3->setQuery($sql3);
	$result3 = $database3->query($sql3);
$database3->close();

echo'<select name="typeoffish" style="color: grey;" required>';
echo'<option value="" selected disabled>Species of Fish</option>';
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
echo'<option selected disabled>Species of Fish</option>';
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
echo'<option selected disabled>Species of Fish</option>';
while ($row=mysqli_fetch_assoc($result5)){
	echo"<option> $row[type_of_fish] </option>";
$typeoffish5 = $row[type_of_fish];
}
echo'</select>';

?>
	</td>
	</tr>
	
	<tr>
		<td colspan="2">
<input class="btn btn-light-green" name="submitted" type="submit" value="Submit" /></td>
	</tr>
</table>
</form>
<script type="text/javascript">
$(function() 
{
 $( "#nearesttown" ).autocomplete({
  source: 'https://buna.africa/includes/custom/ajax/ajaxtowns.php'
 });
});
</script>

<script>
                setTimeout(function() {
                    jQuery('#system-message-container').fadeOut('slow');
                }, 3000);
            </script>