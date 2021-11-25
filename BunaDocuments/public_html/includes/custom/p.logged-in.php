<?php
defined('_JEXEC') or die( 'Restricted access' );

$user = & JFactory::getUser();
//Check guest property of the user object, if it is guest then the user  is not logged in.
if ($user->guest) {
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
		$sqlstatus = "SELECT setupStatus, accountType FROM smartapp_users
					WHERE (smartappID = '$id')"; 
						$database->setQuery($sqlstatus);
						$resultstatus = $database->query($sqlstatus);
					$database->close();

					while ($row=mysqli_fetch_assoc($resultstatus)){
								 $setupstatus = $row['setupStatus'];
								 $accounttype = $row['accountType'];
								 }
	// if user has not set up user details then prompt them to do so		
				if ($setupstatus == 0){
					echo "Hi"." ".$name." "."<br><br>Welcome!<br><br>
					We need some information from you to provide you with a personalized experience and dashboard.<br><br>
					You will be guided through a three step setup wizard.<br><br>
					You can update or change your information from your dashboard at any time after you log in.<br><br>";
					echo "<p><a href='./index.php/setup-user' ><button type='button' class='btn btn-light-green'>Let's Start</button></a></p>";
					echo "<p><a href='./index.php/training-manual-mobile' ><button type='button' class='btn btn-dark-green'>I will do it later</button></a></p>";
				}else{
					
	if ($accounttype == "Official"){
		header('Location: ./official-logged-in');
	}
					

}
	if ($setupstatus != 0){
		?>
		<div><br>
		<p>You are logged in to the site with a '<?php echo $accounttype; ?>' profile.</p>
		<p>As the site matures you will find more functionality on this page.</p>
		<p>Please navigate to your area of interest by using the menu above.</p>
		</div>
		<?php
	}		
}
?>
