<?php
defined('_JEXEC') or die( 'Restricted access' );

if(isset($_POST['calculatevolume'])) 
	{
		$length = addslashes($_POST['length']);
		$breadth = addslashes($_POST['breadth']);
		$height = addslashes($_POST['height']);
		$rectvolume = $length*$breadth*$height;
	}
	
if(isset($_POST['calculatecircvolume'])) 
	{
		$diameter = addslashes($_POST['diameter']);
		$circheight = addslashes($_POST['circheight']);
		$radius = $diameter/2;
		$circvolume = 3.14*pow($radius,2)*$circheight;
	}
?>
<table style="width: 100%;">
<tr id="rectangular"><td><b>Rectangular or Square Ponds</b></td></tr>

<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>#rectangular" enctype="multipart/form-data">
	<div class="row-fluid" >

<tr>
		<td><input name="length" type="text" placeholder="Length (M)" value="<?php echo $length; ?>"/></td>
	</tr>
	<tr>
		<td><input name="breadth" type="text" placeholder="Breadth (M)" value="<?php echo $breadth; ?>" /></td>
	</tr>
	<tr>
		<td><input name="height" type="text" placeholder="Height (M)" value = "<?php echo $height; ?>" /></td>
	</tr>
	<?php
	if(isset($_POST['calculatevolume'])) 
	{
	?>
	<td style="font-size: 20px; color:grey;">Volume = <?php echo $rectvolume; ?> &#13221 or <?php echo $rectvolume*1000; ?> L</td>
	<?php 
	}
	?>
</table>
<p><input class="btn btn-light-green" type="submit" name="calculatevolume" id="submitted" value="Calculate"></p>

<hr>
<table>
<tr id="circular"><td><b>Circular Ponds</b></td></tr>

<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>#circular" enctype="multipart/form-data">
	<div class="row-fluid" >
<tr>
		<td><input name="diameter" type="text" placeholder="Diameter (M)" value="<?php echo $diameter; ?>"/>
		<a style="text-decoration: none;" class="jcepopup" href="images/Diameter.png" data-mediabox="1"></a></td>
	</tr>
	<tr>
		<td><input name="circheight" type="text" placeholder="Height (M)" value = "<?php echo $circheight; ?>" /></td>
	</tr>
	<?php
	if(isset($_POST['calculatecircvolume'])) 
	{
	?>
	<td style="font-size: 20px; color:grey;">Volume = <?php echo $circvolume; ?> &#13221 or <?php echo $circvolume*1000; ?> L</td>
	<?php 
	}
	?>

</table>

<p><input class="btn btn-light-green" type="submit" name="calculatecircvolume" id="submitted" value="Calculate"></p>
</form>
<br>
<table class=" rounded-corners" style="width: 100%; background-color: #7d3f19;">
<tbody>
<tr>
<td style="text-align: center;">&nbsp;<strong><span style="color: #ffffff; font-family: tahoma, arial, helvetica, sans-serif; font-size: 12pt;">Pond Area Calculator</span></strong></td>
</tr>
</tbody>
</table>
<br>
<?php
defined('_JEXEC') or die( 'Restricted access' );

if(isset($_POST['calculatearea'])) 
	{
		$length2 = addslashes($_POST['length2']);
		$breadth2 = addslashes($_POST['breadth2']);
		$area = $length2*$breadth2;
	}
	
?>
<table style="width: 100%;">
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
	<div class="row-fluid" >

<tr>
		<td><input name="length2" type="text" placeholder="Length (M)" value="<?php echo $length2; ?>"/></td>
	</tr>
	<tr>
		<td><input name="breadth2" type="text" placeholder="Breadth (M)" value="<?php echo $breadth2; ?>" /></td>
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