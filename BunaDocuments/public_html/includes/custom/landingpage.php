<?php
defined('_JEXEC') or die( 'Restricted access' );
if(!isset($_GET['r'])) 
{ echo "<script language=\"JavaScript\">
 document.location=\"$PHP_SELF?r=1&width=\"+screen.width+\"&Height=\"+screen.height;</script>"; 
} 
$width = addslashes($_GET['width']);
$height = addslashes($_GET['Height']);

// Settings if user is using a mobile phone
if ($width < 427){
	echo "<script language=\"JavaScript\"> document.location=\"./index.php/m-landing?r=1&width=\"+screen.width+\"&Height=\"+screen.height;</script>";
			}
//Settings if user is using a tablet			
if (($width > 767 )&($width < 981)){
	echo "<script language=\"JavaScript\"> document.location=\"./index.php/t-landing?r=1&width=\"+screen.width+\"&Height=\"+screen.height;</script>";
			}
			
//Settings if user is using a pc			
if ($width > 980){
	echo "<script language=\"JavaScript\"> document.location=\"./index.php/p-landing?r=1&width=\"+screen.width+\"&Height=\"+screen.height;</script>";
			}
			
?>