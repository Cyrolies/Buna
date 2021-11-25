<?php
defined('_JEXEC') or die( 'Restricted access' );
		$s1nooffish = addslashes($_POST['s1nooffish']);
		$s1weightoffish = addslashes($_POST['s1weightoffish']);
		$s2nooffish = addslashes($_POST['s2nooffish']);
		$s2weightoffish = addslashes($_POST['s2weightoffish']);
		$s3nooffish = addslashes($_POST['s3nooffish']);
		$s3weightoffish = addslashes($_POST['s3weightoffish']);
if(isset($_POST['calccount'])){ 
	$avgweightoffish = ($s1weightoffish+$s2weightoffish+$s1weightoffish)/($s1nooffish+$s2nooffish+$s3nooffish);
	$totweightoffish = ($s1weightoffish+$s2weightoffish+$s1weightoffish)/100;
	$totnooffish = ($totweightoffish*1000)/$avgweightoffish;
	?>
	<table>
	<tr>&nbsp;</tr>
	<tr>
	<td style="font-size: 20px; color:grey;">Average weight of fish (g) = <?php echo $avgweightoffish; ?><a style="font-size: 20px; color:grey; text-decoration:none;"> g </a></td>
	</tr>
		<tr>
	<td style="font-size: 20px; color:grey;">Total weight of fish (kg) = <?php echo $totweightoffish; ?><a style="font-size: 20px; color:grey; text-decoration:none;"> kg</a></td>
	</tr>
		<tr>
	<td style="font-size: 20px; color:grey;">Number of fish = <?php echo $totnooffish; ?><a style="font-size: 20px; color:grey; text-decoration:none;"> </a></td>
	</tr>
	</table>
	<?php
	}
	?>

<table>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
	<div class="row-fluid" >
<td>&nbsp;</td>
<tr>
		<td><input name="s1nooffish" type="text" placeholder="Sample 1 number of fish" value="<?php echo $s1nooffish; ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> </a></td>
		<td><input name="s1weightoffish" type="text" placeholder="Sample 1 weight of fish" value="<?php echo $s1weightoffish; ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> (g) </a></td>
		
	</tr>
<tr>
		<td><input name="s2nooffish" type="text" placeholder="Sample 2 number of fish" value="<?php echo $s2nooffish; ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> </a></td>
		<td><input name="s2weightoffish" type="text" placeholder="Sample 2 weight of fish" value="<?php echo $s2weightoffish; ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> (g) </a></td>
		
	</tr>
<tr>
		<td><input name="s3nooffish" type="text" placeholder="Sample 3 number of fish" value="<?php echo $s3nooffish; ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> </a></td>
		<td><input name="s3weightoffish" type="text" placeholder="Sample 3 weight of fish" value="<?php echo $s3weightoffish ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> (g) </a></td>
		
	</tr>
	<td>&nbsp </td>
	
		</table><br>
	<p><input class="btn btn-light-green" type="submit" name="calccount" id="submitted" value="Calculate"></p>
	</form>