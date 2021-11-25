<?php
defined('_JEXEC') or die( 'Restricted access' );
$url = $_SERVER['REQUEST_URI']; 
$i = $_GET['i'];
$document = JFactory::getDocument();
JHtml::_('behavior.formvalidator');
$todaydate = date('d-m-Y H:i:s');
$firstdayofmonth = date('Y-m-01 H:i:s');

$recordid = $_GET['i'];
$user = JFactory::getUser();
$userid = ($user->id);
$firstname = ($user->name);
if(isset($_POST['submitted'])) {
$entrydate = addslashes($_POST['entrydate']);
$species = addslashes($_POST['species']);
$kgharvested = addslashes($_POST['kgharvested']);
$kgsold = addslashes($_POST['kgsold']);
$income = addslashes($_POST['income']);

$database =& JFactory::getDBO();

$sql = "UPDATE smartapp_farmer_submissions 
SET entrydate = '$entrydate',
species = '$species',
	kgharvested = '$kgharvested',
	kgsold = '$kgsold',
	income = '$income'
	WHERE farmersubmissionsID = '$i' ";
	$database->setQuery($sql);
		$database->query();
$database->close();

if ($sql){
	JFactory::getApplication()->enqueueMessage('Thanks. Your information has been updated.','success');
	 	}else{
	JFactory::getApplication()->enqueueMessage('Sorry. Your information could not be saved. Please inform the website administrator.','error');
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
  WHERE farmersubmissionsID = '$i'"; 
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

?>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
	<p>
	<label>You are updating a monthly submission as<b> <?php echo $firstname ?></b>.<br> Your farm/user id is:<b> <?php echo $userid ?></b></label>  
	<div>
	<?php 
	echo JHTML::calendar($entrydate,'entrydate', 'Entry Date', '%Y-%m-%d',array('size'=>'8','maxlength'=>'10','class'=>' validate[\'required\']',));
 ?>
</div>
	<div class="flex">
   <div>
     <div class="input-group">
	 <span class="input-append flex required">
        <?php
	$database =& JFactory::getDBO();
		$sqljobident = "SELECT * FROM smart_lkp_fishtype"; 
		$database->setQuery($sqljobident);
		$resultjobident = $database->query($sqljobident);
	$database->close();

	echo'<select name="species" style="color: grey; width:250px;">';
	echo'<option selected disabled></option>';
	while ($row=mysqli_fetch_assoc($resultjobident)){
		echo "<option>$row[type_of_fish]</option>";
		if($species === $row[type_of_fish]){
        echo "<option selected='selected' value='$species'>$species</option>";
    }		
	//$species = $row[type_of_fish];
	}
	echo'</select>';
	?>
			
	</span>
     </div>
   </div>
</div>

	<div><input class="required" step="0.50" min="0" type="number" name="kgharvested" style="width:250px;" value="<?php echo $kgharvested; ?>" /></div>
	<div><input class="required" step="0.50" min="0" type="number" name="kgsold"  style="width:250px;" value="<?php echo $kgsold; ?>" /></div>
	<div><input type="number" step="0.01" min="0" name="income" value="<?php echo $income; ?>" style="width:250px;" /></div>
	<div>&nbsp </div>
	<div><input type="hidden" name="farmersubmissionsid" value="<?php echo $i; ?>" /></div>
	<div><input class="btn btn-light-green" type="submit" name="submitted" id="submitted" value="Submit" onclick="showDiv()" ></div>
<div id="stat" name="stat"></div>
	

<script type="text/javascript">	
	function showDiv() {
   document.getElementById('stat').style.display = "block";
}

setTimeout(function() {
    $('#system-message-container').fadeOut('fast');
}, 2000);

</script>