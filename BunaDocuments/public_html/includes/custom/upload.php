<?php
defined('_JEXEC') or die( 'Restricted access' );
$user = & JFactory::getUser();
$id = $user->id;
$target_dir = "/dev_wrc/includes/custom/upload/";
$target_file = basename($_FILES["PhotoUpload"]["name"]);
$uploadOk = 1;
$imageFileType = pathinfo($target_file,PATHINFO_EXTENSION);
// Check if image file is a actual image or fake image
if(isset($_POST["submitted"])) {
    $check = getimagesize($_FILES["PhotoUpload"]["tmp_name"]);
    if($check !== false) {
        //echo "File is an image - " . $check["mime"] . ".";
        $uploadOk = 1;
    } else {
		if ($check == ""){
		// do nothing
		}else{
        echo "Incorrect file type.";
		}
        $uploadOk = 0;
    }
}
// Check if file already exists
if (file_exists($target_file)) {
    echo "Sorry, a file with that name already exists.";
    $uploadOk = 0;
}
 // Check file size
elseif ($_FILES["PhotoUpload"]["size"] > 2000000) {
    echo "Sorry, your file is too large. Please use an image no larger than 2 Mb.";
    $uploadOk = 0;
 }
// Allow certain file formats
elseif($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
&& $imageFileType != "gif" && $imageFileType != "PNG" && $imageFileType != "" ) {
    echo "Sorry, only JPG, JPEG, PNG & GIF files are allowed.";
    $uploadOk = 0;
}
// Check if $uploadOk is set to 0 by an error
elseif ($uploadOk == 0 && $imageFileType != "") {
    echo "Sorry, your file was not uploaded.";
// if everything is ok, try to upload file
} else {
	$target_file = $_SERVER['DOCUMENT_ROOT'].$target_dir.$target_file;
    //echo "<br><br>";
    //echo $target_file;
    //echo "<br><br>";
	//echo $id;
	
    if (move_uploaded_file($_FILES["PhotoUpload"]["tmp_name"], $target_file)) {
		$logoname = $target_dir . basename( $_FILES["PhotoUpload"]["name"]);
		
		$database =& JFactory::getDBO();
					$sqluserupdate = "UPDATE smartapp_supplierdetails 
SET 
	logoname = '$logoname'
WHERE
    smartappID = '$id'";
					$database->setQuery($sqluserupdate);
					$database->query();
		
		echo "<div class='alert alert-success alert-dismissable'>
			<a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>
			<strong>Thanks!</strong>The file ". basename( $_FILES["PhotoUpload"]["name"]). " has been uploaded.
			</div>";
    } else {
		if ($imageFileType != ""){
        echo " Sorry, there was an error uploading your file.";
		}
    }
}
?>