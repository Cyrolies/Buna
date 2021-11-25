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
$expensetype = addslashes($_POST['expensetype']);
$expensedetail = addslashes($_POST['expensedetail']);
$expenseamnt = addslashes($_POST['expenseamnt']);

$database =& JFactory::getDBO();
	$sql = "INSERT INTO smartapp_farmer_expenses (userid,entrydate,expensetype,expensedetail,expenseamnt) 
	VALUES ('$userid','$entrydate','$expensetype','$expensedetail','$expenseamnt')";
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
		$sqlexpenseident = "SELECT * FROM smart_lkp_expensetype"; 
		$database->setQuery($sqlexpenseident);
		$resultexpenseident = $database->query($sqlexpenseident);
	$database->close();

	echo'<select name="expensetype" style="color: grey; width:250px;" required>';
	echo'<option selected disabled></option>';
	while ($row=mysqli_fetch_assoc($resultexpenseident)){
		echo"<option> $row[type_of_expense] </option>";
	$expense = $row[type_of_expense];
	}
	echo'</select>';
	?>
			
	</span>
     </div>
   </div>
</div>
	

	<div><input class="required" type="text" name="expensedetail" style="width:250px;" placeholder="Expense detail" /></div>
	<div><input class="required" step="0.50" min="0" type="number" name="expenseamnt"  style="width:250px;" placeholder="Amount" /></div>
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