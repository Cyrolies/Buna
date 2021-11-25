<?php
defined('_JEXEC') or die( 'Restricted access' );
		$nooffish = addslashes($_POST['nooffish']);
		$avgweight = addslashes($_POST['avgweight']);
if(isset($_POST['calculateproduction'])){ 
$totalweight = ($avgweight*$nooffish/1000);
$feedratio = 0.03; //3% of mass
$feedperday = ($totalweight*$feedratio);
$twofeedsperday = ($feedperday/2*1000);
$feedperfish = ($feedperday/$nooffish*1000);
	
	?>
	<table>
	<tr>&nbsp;</tr>
	<tr>
	<td style="font-size: 20px; color:grey;">Feed per day (kg) = <?php echo $feedperday; ?><a style="font-size: 20px; color:grey; text-decoration:none;"> kg </a></td>
	</tr>
		<tr>
	<td style="font-size: 20px; color:grey;">Two feeds per day (g) = <?php echo $twofeedsperday; ?><a style="font-size: 20px; color:grey; text-decoration:none;"> g </a></td>
	</tr>
		<tr>
	<td style="font-size: 20px; color:grey;">Feed per fish (g) = <?php echo $feedperfish; ?><a style="font-size: 20px; color:grey; text-decoration:none;"> g </a></td>
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
		<td><input name="nooffish" type="text" placeholder="Number of fish" value="<?php echo $nooffish; ?>" required /><a style="font-size: 20px; color:grey; text-decoration:none;"> per pond </a></td>
	</tr>
	<tr>
		<td><input name="avgweight" type="text" placeholder="Avg weight (g)" value="<?php echo $avgweight; ?>" /><a style="font-size: 20px; color:grey; text-decoration:none;"> per fish </a></td>
	</tr>
	<td>&nbsp </td>
	
		</table><br>
	<p><input class="btn btn-light-green" type="submit" name="calculateproduction" id="submitted" value="Calculate"></p>
	</form>