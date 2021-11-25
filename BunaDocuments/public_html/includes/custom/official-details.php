<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;

$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT(smartappID) 
			  FROM smartapp_officialdetails 
			  WHERE (smartappID = '$id')"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					$rescount = mysqli_fetch_array($resultcount);
					$total = $rescount[0];
					$database->close();			
				if ($total == 0){
	
	// create the user if it does not exist
	$database =& JFactory::getDBO();
					$sqluser = "INSERT INTO smartapp_officialdetails (smartappID)
					VALUES ('$id')";
					$database->setQuery($sqluser);
					$database->query();
	}

	if(isset($_POST['submitted'])) 
	{
		JFactory::getApplication()->enqueueMessage('Thanks. Your information has been saved.','success');
		
		$officialname = addslashes($_POST['officialname']);
		$email = addslashes($_POST['email']);
		$contact = addslashes($_POST['contact']);
		
		// update the user status
	$database =& JFactory::getDBO();
					$sqlsupplierupdate = "UPDATE smartapp_officialdetails 
SET 
	officialname = '$officialname',
	email = '$email',
	contact = '$contact'
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
 
	<tr>
		<td><input name="officialname" type="text" placeholder="Name and surname" /></td>
	</tr>
	<tr>
		<td><input name="email" type="text" placeholder="Email" /></td>
	</tr>
	<tr>
		<td><input name="contact" type="text" placeholder="Contact Number" /></td>
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