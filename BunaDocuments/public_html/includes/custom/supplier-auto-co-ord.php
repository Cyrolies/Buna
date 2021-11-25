<?php
defined('_JEXEC') or die( 'Restricted access' );
$lat=(isset($_GET['lat']))?$_GET['lat']:'';
$long=(isset($_GET['long']))?$_GET['long']:'';
$user = & JFactory::getUser();
$id = $user->id;

// update the user status
	$database =& JFactory::getDBO();
					$sqluser = "UPDATE smartapp_users 
SET 
	lati = '$lat',
    longi = '$long'
WHERE
    smartappID = '$id'";
					$database->setQuery($sqluser);
					$database->query();

echo $lat;?> --- Lat<br><?php
echo $long;?> --- Long<br><?php
?>