<?php
defined('_JEXEC') or die( 'Restricted access' );
		$sizeofpondsq = addslashes($_POST['sizeofpondsq']);
		$targetweightsq = addslashes($_POST['targetweightsq']);
		
if(isset($_POST['calcstockpond'])){	
		$sizeofpondsq = ($sizeofpondsq);
		$targetweightsq = ($targetweightsq);
		//$pondcalc = ($targetweightsq*$sizeofpondsq);
		$pondstockdensity = (1.5*1000)/$targetweightsq*$sizeofpondsq;
	}
	
if(isset($_POST['calcstocktank'])){	
		$sizeofpondsq = ($sizeofpondsq);
		$targetweightsq = ($targetweightsq);
		//$pondcalc = ($targetweightsq*$sizeofpondsq);
		$tankstockdensity = (15*1000)/$targetweightsq*$sizeofpondsq;
	}
	
?>
<br>
<table style="width:50%;" class="table table-striped">
<tr>
<td>
Outdoor ponds
</td>
<td>
Stocking density =
</td>
<td>
1.5 kg/m2
</td>
</tr>
<tr>
<td>
Recirc Tanks
</td>
<td>
Stocking density =
</td>
<td>
15 kg/m3
</td>
</tr>
<tr>
<td>
Final weight
</td>
<td>
Final target weight =
</td>
<td>
600 g
</td>
</tr>
</table>
<br>

<table>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>#rectangular" enctype="multipart/form-data">
	<div class="row-fluid" >
<tr>
<th>Outdoor ponds</th>
</tr>
<tr>
		<td><input name="sizeofpondsq" type="text" placeholder="m2" value="<?php echo $sizeofpondsq; ?>"/>
		You can work out the volume of your pond using the <a href="index.php?option=com_content&amp;view=article&amp;id=37&amp;Itemid=209">volume calculator</a></td>
	</tr>
	<tr>
		<td><input name="targetweightsq" type="text" placeholder="Target weight (g)" value="<?php echo $targetweightsq; ?>" /><a style="font-size: 20px; color:grey; text-decoration:none;"> </a>
		Follow the guide in the manual for <a href="./index.php/training-manual-pc?start=10#count">counting and weighing fish</a> in a pond.</td>
	</tr>
	
<?php
	if(isset($_POST['calcstockpond'])) 
	{		
	?>
	<tr><td style="font-size: 20px; color:grey;">
	Number of fish = <?php echo $pondstockdensity; ?></td></tr>
	<tr><td><?php echo $pondstockdensity; ?> fish of <?php echo $targetweightsq; ?>g can be stocked in this pond.</td></tr>
	<?php
	}
	?>
	</table><br>
	<p><input class="btn btn-light-green" type="submit" name="calcstockpond" id="submitted" value="Calculate"></p>
	</form>
	
	<table>
	<hr>
<form autocomplete="off" class="form-validate" method="POST" action="<?php $_SERVER['PHP_SELF'];?>#rectangular" enctype="multipart/form-data">
	<div class="row-fluid" >
<tr>
<th>Recirc tanks</th>
</tr>
<tr>
		<td><input name="sizeofpondsq" type="text" placeholder="m3" value="<?php echo $sizeofpondsq; ?>"/>
		You can work out the volume of your pond using the <a href="index.php?option=com_content&amp;view=article&amp;id=37&amp;Itemid=209">volume calculator</a></td>
	</tr>
	<tr>
		<td><input name="targetweightsq" type="text" placeholder="Target weight (g)" value="<?php echo $targetweightsq; ?>" /><a style="font-size: 20px; color:grey; text-decoration:none;"> </a>
		Follow the guide in the manual for <a href="./index.php/training-manual-pc?start=10#count">counting and weighing fish</a> in a pond.</td>
	</tr>
	
<?php
	if(isset($_POST['calcstocktank'])) 
	{		
	?>
	<tr><td style="font-size: 20px; color:grey;">
	Number of fish = <?php echo $tankstockdensity; ?></td></tr>
	<tr><td><?php echo $tankstockdensity; ?> fish of <?php echo $targetweightsq; ?>g can be stocked in this pond.</td></tr>
	<?php
	}
	?>
	</table><br>
	<p><input class="btn btn-light-green" type="submit" name="calcstocktank" id="submitted" value="Calculate"></p>
	</form>

