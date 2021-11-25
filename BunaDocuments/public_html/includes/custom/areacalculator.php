<?php
defined('_JEXEC') or die( 'Restricted access' );
$length = addslashes($_POST['length']);
$breadth = addslashes($_POST['breadth']);

if(isset($_POST['calculatearea'])) 
	{
		$area = $length*$breadth;
	}
	
?>
<table style="width: 100%;">
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
	<div class="row-fluid" >

<tr>
		<td><input name="length" type="text" placeholder="Length (M)" value="<?php echo $length; ?>"/></td>
	</tr>
	<tr>
		<td><input name="breadth" type="text" placeholder="Breadth (M)" value="<?php echo $breadth; ?>" /></td>
	</tr>
	<?php
	if(isset($_POST['calculatearea'])) 
	{
	?>
	<td style="font-size: 20px; color:grey;">Area = <?php echo $area; ?> M&sup2</td>
	<?php 
	}
	?>
</table><br>
<p><input class="btn btn-light-green" type="submit" name="calculatearea" id="submitted" value="Calculate"></p>