<?php
defined('_JEXEC') or die( 'Restricted access' );
$searchstr = addslashes($_POST['searchstr']);

if(isset($_POST['reset'])) 
	{ $searchstr = "";}
	?>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<table style="width: 100%">
	<tr>
		<td><input name="searchstr" type="text" placeholder="Search by name or service" value = "<?php echo $searchstr; ?>" /></td>
	</tr>
	<tr>
		<td colspan="2">
<input class="btn btn-light-green" name="searchbtn" type="submit" value="Search" />
<button name="reset" class="btn btn-dark-green" type="submit" formaction="./suppliers">Reset Search</button>
</td>
	</tr>
</table>
</form>
<?php

if(isset($_POST['searchbtn'])) 
	{
		$sql5 = "SELECT smartappID,
       businessname,
       typeofservice,
       email,
       contact,
	   logoname
  FROM smartapp_supplierdetails
 WHERE businessname LIKE '%$searchstr%'
 OR typeofservice LIKE '%$searchstr%'"; 
				$database->setQuery($sql5);
				$result5 = $database->query($sql5);
			$database->close();
	
			while ($row=mysqli_fetch_assoc($result5)){
				$smartappid = $row['smartappID'];
				$businessname = $row['businessname'];
				$typeofservice = $row['typeofservice'];
				$email = $row['email'];
				$logoname = $row['logoname'];
				if ($logoname == ""){
					$logoname = "./includes/custom/upload/facebook_1550134915558.jpg";
				}
?>
<table class="table table-striped" style="width: 100%">
	<tr style="width: 100%;">
		<td colspan="2"><img class="pull-left" height="80" width="80" src="<?php echo $logoname; ?>" value="<?php echo $logoname; ?>" /><b><?php echo $businessname; ?></b></td>
	</tr>
	<tr>
		<td style="width: 20%"><b>Service:</b></td>
		<td style="width: 80%"><?php echo $typeofservice; ?></td>
	</tr>
	<tr>
		<td style="width: 20%"><b>Email:</b></td>
		<td style="width: 80%"><?php echo $email; ?></td>
	</tr>
	<tr>
		<td style="width: 20%"><b>Contact:</b></td>
		<td style="width: 80%"><?php echo $contact; ?></td>
	</tr>
</table>
<br>
<?php
			}
	

}else{
$sql4 = "SELECT smartappID,accountType FROM smartapp_users
			WHERE accountType='Supplier'"; 
				$database->setQuery($sql4);
				$result4 = $database->query($sql4);
			$database->close();

			while ($row=mysqli_fetch_assoc($result4)){
				$smartappid = $row['smartappID'];
				$accounttype = $row['accountType'];
			
$sql3 = "SELECT businessname,
       typeofservice,
       email,
       contact,
       smartappID,
	   logoname
		FROM smartapp_supplierdetails
		WHERE (smartappID = '$smartappid')
		ORDER BY businessname DESC"; 
				$database->setQuery($sql3);
				$result3 = $database->query($sql3);
			$database->close();

			while ($row=mysqli_fetch_assoc($result3)){
				$smartappid = $row['smartappID'];
				$businessname = $row['businessname'];
				$typeofservice = $row['typeofservice'];
				$email = $row['email'];
				$contact = $row['contact'];
				$logoname = $row['logoname'];
				
				if ($logoname == ""){
					$logoname = "./includes/custom/upload/placeholder.jpg";
				}
?>
<table class="table table-striped" style="width: 100%">
	<tr style="width: 100%">
		<td colspan="2"><img class="pull-left" height="80" width="80" src="<?php echo $logoname; ?>" value="<?php echo $logoname; ?>" /><b><?php echo $businessname; ?></b></td>
	</tr>
	<tr>
		<td style="width: 20%"><b>Service:</b></td>
		<td style="width: 80%"><?php echo $typeofservice; ?></td>
	</tr>
	<tr>
		<td style="width: 20%"><b>Email:</b></td>
		<td style="width: 80%"><?php echo $email; ?></td>
	</tr>
	<tr>
		<td style="width: 20%"><b>Contact:</b></td>
		<td style="width: 80%"><?php echo $contact; ?></td>
	</tr>
</table>
<br>
<?php
			}
			}
	}
			?>