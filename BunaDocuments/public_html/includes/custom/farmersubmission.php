<?php
defined('_JEXEC') or die( 'Restricted access' );
$document = JFactory::getDocument();
JHtml::_('behavior.formvalidator');
JHTML::_('behavior.calendar');
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
	$sql = "INSERT INTO smartapp_farmer_submissions (userid,entrydate,species,kgharvested,kgsold,income) 
	VALUES ('$userid','$entrydate','$species','$kgharvested','$kgsold','$income')";
	$database->setQuery($sql);
	$database->query();
$database->close();

if ($sql){
	JFactory::getApplication()->enqueueMessage('Thanks. Your information has been saved.','success');
	 	}else{
	JFactory::getApplication()->enqueueMessage('Sorry. Your information could not be saved. Please inform the website administrator.','error');
}
}
?>

<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
	<p>
	<label>You are entering a monthly submission as<b> <?php echo $firstname ?></b>.<br> Your farm/user id is:<b> <?php echo $userid ?></b></label>  
	<div>
	<?php 
	echo JHTML::calendar(date("Y-m-d"),'entrydate', 'Entry Date', '%Y-%m-%d',array('size'=>'8','maxlength'=>'10','class'=>' validate[\'required\']',));
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

	echo'<select name="species" style="color: grey; width:250px;" required>';
	echo'<option selected disabled></option>';
	while ($row=mysqli_fetch_assoc($resultjobident)){
		echo"<option> $row[type_of_fish] </option>";
	$species = $row[type_of_fish];
	}
	echo'</select>';
	?>
			
	</span>
     </div>
   </div>
</div>
	

	<div><input class="required" step="0.50" min="0" type="number" name="kgharvested" style="width:250px;" placeholder="Kg harvested" /></div>
	<div><input class="required" step="0.50" min="0" type="number" name="kgsold"  style="width:250px;" placeholder="Kg sold" /></div>
	<div><input type="number" step="0.01" min="0" name="income" placeholder="Sales value" style="width:250px;" /></div>
	<div>&nbsp </div>
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