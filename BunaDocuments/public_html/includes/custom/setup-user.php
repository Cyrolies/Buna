<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;

	if(isset($_POST['submitted'])) 
	{
		$category = addslashes($_POST['category']);
		
	// update the user status
	$database =& JFactory::getDBO();
					$sqluser = "UPDATE smartapp_users 
					SET setupStatus = '1', accountType = '$category'
					WHERE smartappID = '$id'";
					$database->setQuery($sqluser);
					$database->query();
	
	if ($category == "Farmer"){
				// Set user to correct group
	$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smart_user_usergroup_map
SET 
	group_id = '13'
WHERE
    user_id = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
	header("Location: ../farmer-auto-co-ord");
	}
	
	if ($category == "FarmerSupplier"){
				// Set user to correct group
	$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smart_user_usergroup_map
SET 
	group_id = '17'
WHERE
    user_id = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
	header("Location: ../farmer-auto-co-ord");
	}
	
	if ($category == "Supplier"){
		// Set user to correct group
	$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smart_user_usergroup_map
SET 
	group_id = '14'
WHERE
    user_id = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
	header("Location: ../supplier-details");
	}
	if ($category == "Official"){
		// Set user to correct group
	$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smart_user_usergroup_map
SET 
	group_id = '15'
WHERE
    user_id = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
	header("Location: ../official-details");
	}
	}
?>
	<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
	<div class="row-fluid" >
		<div class="span3"><label><input style="margin-left:10px; margin-right:5px;" type="radio" name="category" <?php if (isset($category) && $category == 'Farmer') echo 'checked'; ?> value="Farmer" required>Farmer</label></div>
		<div class="span3"><label><input style="margin-left:10px; margin-right:5px;" type="radio" name="category" <?php if (isset($category) && $category == 'Supplier') echo 'checked'; ?> value="Supplier" required>Supplier</label></div>
		<div class="span3"><label><input style="margin-left:10px; margin-right:5px;" type="radio" name="category" <?php if (isset($category) && $category == 'FarmerSupplier') echo 'checked'; ?> value="FarmerSupplier" required>Farmer and Supplier</label></div>
		<div class="span3"><label><input style="margin-left:10px; margin-right:5px;" type="radio" name="category" <?php if (isset($category) && $category == 'Official') echo 'checked'; ?> value="Official" required>Official</label></div>
</div>

<input class="btn btn-light-green" type="submit" name="submitted" id="submitted" value="Submit" onclick="showDiv()" >


