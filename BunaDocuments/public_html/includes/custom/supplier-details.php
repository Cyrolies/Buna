<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;

$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT(smartappID) 
			  FROM smartapp_supplierdetails 
			  WHERE (smartappID = '$id')"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					$rescount = mysqli_fetch_array($resultcount);
					$total = $rescount[0];
					$database->close();			
				if ($total == 0){
	
	// create the user if it does not exist
	$database =& JFactory::getDBO();
					$sqluser = "INSERT INTO smartapp_supplierdetails (smartappID)
					VALUES ('$id')";
					$database->setQuery($sqluser);
					$database->query();
	}

	if(isset($_POST['submitted'])) 
	{
		JFactory::getApplication()->enqueueMessage('Thanks. Your information has been saved.','success');
		
		$businessname = addslashes($_POST['businessname']);
		$typeofservice = addslashes($_POST['typeofservice']);
		$email = addslashes($_POST['email']);
		$contact = addslashes($_POST['contact']);
		
		// update the user status
	$database =& JFactory::getDBO();
					$sqlsupplierupdate = "UPDATE smartapp_supplierdetails 
SET 
	businessname = '$businessname',
	typeofservice = '$typeofservice',
	email = '$email',
	contact = '$contact',
	province = '$province'
WHERE
    smartappID = '$id'";
					$database->setQuery($sqlsupplierupdate);
					$database->query();
					
	if ($sqlsupplierupdate){
		header('Location: ./index.php');
	}
	}
?>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<table style="width: 100%">
 
<p><i class="icon-info" style="color: #0075b4;">
<a class="jcepopup zoom-left" href="index.php?option=com_content&amp;view=article&amp;id=34:information-farmer-details&amp;catid=2:uncategorised" type="text/html" data-mediabox="1">Help</a></i></p>
	<tr>
		<td><input name="businessname" type="text" placeholder="Business Name" /></td>
	</tr>
	<tr>
	<td><label style="font-size: xx-small; color: #808080;">Please enter types of services offered seperated by a comma. Eg Feed, Consulting</label>
		<input name="typeofservice" type="text" placeholder="Type of Service" /></td>
	</tr>
		<tr>
		<td><input name="email" type="text" placeholder="Email Address" /></td>
	</tr>
		<tr>
		<td><input name="contact" type="tel" placeholder="Contact Number" /></td>
	</tr>
<tr>
<td>
<?php
$database =& JFactory::getDBO();
	$sql = "SELECT * FROM smart_lkp_province
	ORDER BY province ASC"; 
	$database->setQuery($sql);
	$result = $database->query($sql);
$database->close();

echo'<select name="province" style="color: grey;">';
echo'<option selected disabled>Province</option>';
while ($row=mysqli_fetch_assoc($result)){
	echo"<option> $row[province] </option>";
$province = $row[province];
}
echo'</select>';
?>
</td>
</tr>	
	<tr>
		<td colspan="2">
<input class="btn btn-warning" name="submitted" type="submit" value="Submit" /></td>
	</tr>
</table>
</form>
<script>
                setTimeout(function() {
                    jQuery('#system-message-container').fadeOut('slow');
                }, 3000);
            </script>