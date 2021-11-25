<?php
defined('_JEXEC') or die( 'Restricted access' );

$user = & JFactory::getUser();
//Check guest property of the user object, if it is guest then the user  is not logged in.
if ($user->guest) {
echo "<div class='btn-container'>
  <div class='row'>
	<div class='column'>
	<div>
		<a href='./index.php/training-description' class='btn btn-large btn-training btn-white' role='button'><i class='icon-book'></i>  Training</a>
    </div>      
	  </div>
	  <div>&nbsp;</div>
	  <div class='column'>
      <div>
        <a href='./index.php/suppliers-description' class='btn btn-large btn-supplies btn-white' role='button'><i class='icon-shopping-basket'></i>  Suppliers</a>
      </div>
	  </div>
  </div>
  </div>
";
echo "Please create an account by clicking on the register button. This will give you access to all site resources.";?><br><?php
} else {
	// if the user is logged in
	// get the user id
$id = $user->id;
$name = $user->name;
	// check if the user has been set up in the smartapp_users table
		$database =& JFactory::getDBO();
			  $sqlcount = "SELECT COUNT(smartappID) 
			  FROM smartapp_users 
			  WHERE (smartappID = '$id')"; 
					$database->setQuery($sqlcount);
					$resultcount = $database->query($sqlcount);
					$rescount = mysqli_fetch_array($resultcount);
					$total = $rescount[0];
					$database->close();			
				if ($total == 0){
	
	// create the user if it does not exist
	$database =& JFactory::getDBO();
					$sqluser = "INSERT INTO smartapp_users (smartappID)
					VALUES ('$id')";
					$database->setQuery($sqluser);
					$database->query();
	}
					
	//	check if the user has set up user details
		$sqlstatus = "SELECT setupStatus FROM smartapp_users
					WHERE (smartappID = '$id')"; 
						$database->setQuery($sqlstatus);
						$resultstatus = $database->query($sqlstatus);
					$database->close();

					while ($row=mysqli_fetch_assoc($resultstatus)){
								 $setupstatus = $row['setupStatus'];
								 }
	// if user has not set up user details then prompt them to do so		
				if ($setupstatus == 0){
					echo "Hi"." ".$name." "."<br><br>Welcome!<br><br>
					We need some information from you to provide you with a personalized experience and dashboard.<br><br>
					You will be guided through a three step setup wizard.<br><br>
					You can update or change your information from your dashboard at any time after you log in.<br><br>";
					echo "<p><a href='./index.php/setup-user' ><button type='button' class='btn btn-info'>Let's Start</button></a></p>";
					echo "<p><a href='./index.php/training-manual-mobile' ><button type='button' class='btn btn-grey'>I will do it later</button></a></p>";
				}else{		
?>

<div>
<table style='width: 100%'>
	<tr>
		<td style='width: 40%; text-align: right;'><a href='./index.php/training-description' class='btn btn-large btn-training btn-white' role='button'><i class='icon-book'></i>  Training</a></td>
		<td style='width: 2%;'>&nbsp;</td>
		<td style='width: 40%; text-align: left;'><a href='./index.php/suppliers-description' class='btn btn-large btn-supplies btn-white' role='button'><i class='icon-shopping-basket'></i>  Suppliers</a></td>
	</tr>
</table>
  </div>
<?php
}
					
		}
		?>
