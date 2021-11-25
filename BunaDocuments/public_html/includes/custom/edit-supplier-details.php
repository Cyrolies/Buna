<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;

	if(isset($_POST['submitted'])) 
	{
		include_once('./includes/custom/upload.php');
		$businessname = addslashes($_POST['businessname']);
		$typeofservice = addslashes($_POST['typeofservice']);
		$email = addslashes($_POST['email']);
		$contact = addslashes($_POST['contact']);
		$logoname = addslashes($_POST['logoname']);
		$province = addslashes($_POST['province']);
		
		// update the supplier status
	$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smartapp_supplierdetails 
SET 
	businessname = '$businessname',
	typeofservice = '$typeofservice',
	email = '$email',
	contact = '$contact',
	province = '$province'
WHERE
    smartappID = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
					
	if ($sqluserupdate){
		JFactory::getApplication()->enqueueMessage('Thanks. Your information has been updated.','success');
	}
	}

// update the user profile
		$sqlstatus = "SELECT * FROM smartapp_supplierdetails
					WHERE (smartappID = '$id')"; 
						$database->setQuery($sqlstatus);
						$resultstatus = $database->query($sqlstatus);
					$database->close();

					while ($row=mysqli_fetch_assoc($resultstatus)){
					$businessname = $row['businessname'];
		$typeofservice = $row['typeofservice'];
		$email = $row['email'];
		$contact = $row['contact'];
		$logoname = $row['logoname'];
		$province = $row['province'];
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
		<label>Business name</label>
		<input name="businessname" type="text" placeholder="Business Name" value="<?php echo $businessname; ?>" /></td>
	</tr>
 <tr>
<tr>
		<td style="color: #999999;">
		<label>Type of service</label>
		<input name="typeofservice" type="text" placeholder="Type Of Service" value="<?php echo $typeofservice; ?>" /></td>
	</tr>
 <tr>
		<td style="color: #999999;">
		<label>Email address</label>
		<input name="email" type="text" placeholder="Email" value="<?php echo $email; ?>" /></td>
	</tr>
 <tr>
<tr>
		<td style="color: #999999;">
		<label>Contact number</label>
		<input name="contact" type="tel" placeholder="Contact Number" value="<?php echo $contact; ?>" /></td>
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
	<tr>
		<td colspan="2">
		
<div class="container"><a class="legend" style="color: #3c821f; font-size: 18px; text-decoration: none;">Your current logo is:</a>
<div><img height="200" width="200" src="<?php echo $logoname; ?>" value="<?php echo $logoname; ?>" /></p>
</div>
<div class="row-fluid" >
	<div class="span3"><input type="file" name="PhotoUpload" id="PhotoUpload" /></div>
</div>
		
<input class="btn btn-light-green" name="submitted" type="submit" value="Submit" /></td>
	</tr>
</table>
</form>

<script>
                setTimeout(function() {
                    jQuery('#system-message-container').fadeOut('slow');
                }, 3000);
            </script>