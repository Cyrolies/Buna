<?php
defined('_JEXEC') or die( 'Restricted access' );
$document = JFactory::getDocument();
//$document->addStyleSheet('templates/jd_chicago/custom.css');
JHtml::_('behavior.formvalidator');

$recordid = $_GET['i'];
if(isset($_POST['submitted'])) {
$database =& JFactory::getDBO();

$sql = "DELETE FROM smartapp_farmer_submissions 
	WHERE farmersubmissionsID = '$recordid' ";
	$database->setQuery($sql);
		$database->query();
$database->close();

if ($sql){
	JFactory::getApplication()->enqueueMessage('The record has been deleted.','success');
	 	}else{
	JFactory::getApplication()->enqueueMessage('Sorry. The record could not be deleted. Please inform the website administrator.','error');
}
}

$database =& JFactory::getDBO();
		$sqljobcodes = "SELECT farmersubmissionsID,
		userID,
		entrydate,
		species,
       kgharvested,
       kgsold,
       income
  FROM smartapp_farmer_submissions
  WHERE farmersubmissionsID = '$recordid'"; 
		$database->setQuery($sqljobcodes);
		$resultjobcodes = $database->query($sqljobcodes);
	$database->close();

while ($row=mysqli_fetch_assoc($resultjobcodes)){
$farmersubmissionsid = $row['farmersubmissionsID'];
$userid = $row['userID'];
$entrydate = $row['entrydate'];
$species = $row['species'];
$kgharvested = $row['kgharvested'];
$kgsold = $row['kgsold'];
$income = $row['income'];
$date = date('d-m-Y', strtotime($entrydate));
}

echo $species; 
?>
<table style="width: 60%">
<tr style="color: #262626;"><b>You are about to delete the following record.</b><br>
This action cannot be reversed. Are you sure you want to delete it?</tr>
<br><br>
	<tr>
		<td><b>Record ID:</b></td>
		<td><?php echo $farmersubmissionsid; ?></td>
	</tr>
	<tr>
		<td><b>Farm|User ID:</b></td>
		<td><?php echo $userid; ?></td>
	</tr>
	<tr>
		<td><b>Entry Date:</b></td>
		<td><?php echo $entrydate; ?></td>
	</tr>
	<tr>
		<td><b>Species:</b></td>
		<td><?php echo $species; ?></td>
	</tr>
	<tr>
		<td><b>Kg Harvested:</b></td>
		<td><?php echo $kgharvested; ?></td>
	</tr>
	<tr>
		<td><b>Kg Sold:</b></td>
		<td><?php echo $kgsold; ?></td>
	</tr>
	<tr>
		<td><b>Income:</b></td>
		<td><?php echo $income; ?></td>
	</tr>
</table><br>


<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<div class="form-group">
<div>
    <input type="submit" name="submitted" class="btn btn-lg btn-danger" value="Delete"/>
	<input type="button" class="btn btn-lg btn-hg-blue" value="Back" onClick="window.location = './index.php/documents/reports/view-production-report'" />
</div>
</div>
</form>