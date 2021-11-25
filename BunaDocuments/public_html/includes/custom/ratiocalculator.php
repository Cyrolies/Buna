<?php
defined('_JEXEC') or die( 'Restricted access' );

if(isset($_POST['calculateratio'])) 
	{	$age = addslashes($_POST['age']);
		$length = addslashes($_POST['length']);
		$weight = addslashes($_POST['weight']);
	}
///// This should be in a loop but was created while people were in the field with minimal info ////
///// Needs to be corrected ////
///// By age ////	
	if ((($age > 0) AND ($age < 1)) AND $length == 0 AND $weight == 0){
	$length = 5;
	$weight = 2;
	$bm = 4;
	$feed = 0.08;
	}
	
	if ((($age >= 1) AND ($age < 2)) AND $length == 0 AND $weight == 0){
	$length = 25;
	$weight = 10;
	$bm = 4;
	$feed = 0.40;
	}
	
	if ((($age >= 2) AND ($age < 3)) AND $length == 0 AND $weight == 0){
	$length = 45;
	$weight = 20;
	$bm = 3.8;
	$feed = 0.76;
	}
	
	if ((($age >= 3) AND ($age < 4)) AND $length == 0 AND $weight == 0){
	$length = 70;
	$weight = 40;
	$bm = 3.5;
	$feed = 1.40;
	}
	
	if ((($age >= 4) AND ($age < 5)) AND $length == 0 AND $weight == 0){
	$length = 100;
	$weight = 80;
	$bm = 3.2;
	$feed = 2.56;
	}
	
	if ((($age >= 5) AND ($age < 6)) AND $length == 0 AND $weight == 0){
	$length = 130;
	$weight = 140;
	$bm = 3.0;
	$feed = 4.20;
	}
	
	if ((($age >= 6) AND ($age < 7)) AND $length == 0 AND $weight == 0){
	$length = 170;
	$weight = 210;
	$bm = 1.5;
	$feed = 3.15;
	}
	
	if ((($age >= 7) AND ($age < 8)) AND $length == 0 AND $weight == 0){
	$length = 200;
	$weight = 280;
	$bm = 1.5;
	$feed = 4.20;
	}
	
	if ((($age >= 8) AND ($age < 9)) AND $length == 0 AND $weight == 0){
	$length = 230;
	$weight = 380;
	$bm = 1.0;
	$feed = 3.80;
	}
	
	if ((($age >= 9) AND ($age < 10)) AND $length == 0 AND $weight == 0){
	$length = 260;
	$weight = 480;
	$bm = 0.8;
	$feed = 3.84;
	}
	
	if ($age >= 10 AND $length == 0 AND $weight == 0){
	$length = 280;
	$weight = 600;
	$bm = 0.8;
	$feed = 4.80;
	}
///////
///// By Length ////
	if ((($length > 0) AND ($length < 25)) AND $age == 0 AND $weight == 0){
	$age = "1";
	$weight = 2;
	$bm = 4;
	$feed = 0.08;
	}
	
	if ((($length >= 25) AND ($length < 45)) AND $age == 0 AND $weight == 0){
	$age = "1 - 2";
	$weight = 10;
	$bm = 4;
	$feed = 0.40;
	}
	
	if ((($length >= 45) AND ($length < 70)) AND $age == 0 AND $weight == 0){
	$age = "2 - 3";
	$weight = 20;
	$bm = 3.8;
	$feed = 0.76;
	}
	
	if ((($length >= 70) AND ($length < 100)) AND $age == 0 AND $weight == 0){
	$age = "3 - 4";
	$weight = 40;
	$bm = 3.5;
	$feed = 1.40;
	}
	
	if ((($length >= 100) AND ($length < 130)) AND $age == 0 AND $weight == 0){
	$age = "4 - 5";
	$weight = 80;
	$bm = 3.2;
	$feed = 2.56;
	}
	
	if ((($length >= 130) AND ($length < 170)) AND $age == 0 AND $weight == 0){
	$age = "5 - 6";
	$weight = 140;
	$bm = 3.0;
	$feed = 4.20;
	}
	
	if ((($length >= 170) AND ($length < 200)) AND $age == 0 AND $weight == 0){
	$age = "6 - 7";
	$weight = 210;
	$bm = 1.5;
	$feed = 3.15;
	}
	
	if ((($length >= 200) AND ($length < 230)) AND $age == 0 AND $weight == 0){
	$age = "7 - 8";
	$weight = 280;
	$bm = 1.5;
	$feed = 4.20;
	}
	
	if ((($length >= 230) AND ($length < 260)) AND $age == 0 AND $weight == 0){
	$age = "8 - 9";
	$weight = 380;
	$bm = 1.0;
	$feed = 3.80;
	}
	
	if ((($length >= 260) AND ($length < 280)) AND $age == 0 AND $weight == 0){
	$age = "9 - 10";
	$weight = 480;
	$bm = 0.8;
	$feed = 3.84;
	}
	

	if ($length > 280 AND $age == 0 AND $weight == 0){
	$age = "> 10";
	$weight = " > 600";
	$bm = 0.8;
	$feed = 4.80;
	}
///////
///// By Weight ////
	if ((($weight > 0) AND ($weight < 10)) AND $age == 0 AND $length == 0){
	$length = 5;
	$age = "0 - 1";
	$bm = 4;
	$feed = 0.08;
	}
	
	if ((($weight >= 10) AND ($weight < 20)) AND $age == 0 AND $length == 0){
	$length = 25;
	$age = "1 - 2";
	$bm = 4;
	$feed = 0.40;
	}
	
	if ((($weight >= 20) AND ($weight < 40)) AND $age == 0 AND $length == 0){
	$length = 45;
	$age = "2 - 3";
	$bm = 3.8;
	$feed = 0.76;
	}
	
	if ((($weight >= 40) AND ($weight < 80)) AND $age == 0 AND $length == 0){
	$length = 70;
	$age = "3 - 4";
	$bm = 3.5;
	$feed = 1.40;
	}
	
	if ((($weight >= 80) AND ($weight < 140)) AND $age == 0 AND $length == 0){
	$length = 100;
	$age = "4 - 5";
	$bm = 3.2;
	$feed = 2.56;
	}
	
	if ((($weight >= 140) AND ($weight < 210)) AND $age == 0 AND $length == 0){
	$length = 130;
	$age = "5 - 6";
	$bm = 3.0;
	$feed = 4.20;
	}
	
	if ((($weight >= 210) AND ($weight < 280)) AND $age == 0 AND $length == 0){
	$length = 170;
	$age = "6 - 7";
	$bm = 1.5;
	$feed = 3.15;
	}
	
	if ((($weight >= 280) AND ($weight < 380)) AND $age == 0 AND $length == 0){
	$length = 200;
	$age = "7 - 8";
	$bm = 1.5;
	$feed = 4.20;
	}
	
	if ((($weight >= 380) AND ($weight < 480)) AND $age == 0 AND $length == 0){
	$length = 230;
	$age = "8 - 9";
	$bm = 1.0;
	$feed = 3.80;
	}
	
	if ((($weight >= 480) AND ($weight < 600)) AND $age == 0 AND $length == 0){
	$length = 260;
	$age = "9 - 10";
	$bm = 0.8;
	$feed = 3.84;
	}
	
	if ($weight > 600 AND $age == 0 AND $length == 0){
	$length = 280;
	$age = "> 10";
	$bm = 0.8;
	$feed = 4.80;
	}
	///// END /////
?>
<br>
<table>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>#rectangular" enctype="multipart/form-data">
	<div class="row-fluid" >

<tr>
		<td><input name="age" type="text" placeholder="Age (weeks)" value="" /><a style="font-size: 20px; color:grey; text-decoration:none;"></a></td>
	</tr>
	<tr>
		<td><input name="length" type="text" placeholder="Length (mm)" value="" /><a style="font-size: 20px; color:grey; text-decoration:none;"></a></td>
	</tr>
		<tr>
		<td><input name="weight" type="text" placeholder="weight (g)" value="" /><a style="font-size: 20px; color:grey; text-decoration:none;"></a></td>
	</tr>
	
<?php
	if(isset($_POST['calculateratio'])) 
	{
	?>
	<td style="font-size: 20px; color:grey;">
	Your fish should be<br></td>
	<tr>
		<td>Age:</td><td><?php echo $age; ?></td><td>Weeks old</td>
	</tr>
	<tr>
		<td>Weight:</td><td><?php echo $weight; ?></td><td>Grams</td>
	</tr>
	<tr>
		<td>Length:</td><td><?php echo $length; ?></td><td>Millimeters</td>
	</tr>
	<tr><td>&nbsp;</td></tr>
	<td style="font-size: 20px; color:grey;">
	Your fish should be fed<br></td>
	<tr>
		<td>Percentage of its body mass:</td><td><?php echo $bm; ?></td><td>%</td>
	</tr>
	<tr>
		<td>Grams of food per fish:</td><td><?php echo $feed; ?></td><td>Grams</td>
	</tr>
	<?php
	}
	?>
	</table><br>
	<p><input class="btn btn-light-green" type="submit" name="calculateratio" id="submitted" value="Calculate"></p>
	</form>

