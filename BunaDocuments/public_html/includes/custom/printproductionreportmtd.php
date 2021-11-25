<?php
defined('_JEXEC') or die( 'Restricted access' );
$document = JFactory::getDocument();
JHtml::_('behavior.formvalidator');
$user = JFactory::getUser();
$userid = ($user->id);
$firstname = ($user->name);
$todaydate = date('d-m-Y H:i:s');
$firstdayofmonth = date('Y-m-01 H:i:s');

$format_enddate  = date('Y-m-d', strtotime($todaydate));
$format_startdate  = date('Y-m-d', strtotime($firstdayofmonth));

$database =& JFactory::getDBO();
	$sql = "SELECT userID,
	entrydate,
		species,
       kgharvested,
       kgsold,
       income
  FROM smartapp_farmer_submissions
  WHERE (entrydate >= '$format_startdate'
 AND entrydate <= '$format_enddate')
ORDER BY userID ASC, species ASC"; 
	$database->setQuery($sql);
	$result = $database->query($sql);
$database->close();
?>
<body style="background-color: #FFFFFF; background-image: none !important;">
<br><br>
<table class="table">

  <thead class="thead">
    <tr>
      <th scope="col" colspan="6">Production Report - All Farms Month to Date</th>
    </tr>
    <tr>
      <th scope="col">UserID|Farm</th>
	  <th scope="col">Date</th>
      <th scope="col">Species</th>
      <th scope="col">Kg Harvested</th>
      <th scope="col">Kg Sold</th>
	  <th scope="col"><center>Income</center></th>
    </tr>
  </thead>

  <?php
while ($row=mysqli_fetch_assoc($result)){
$userid = $row['userID'];
$entrydate = $row['entrydate'];
$species = $row['species'];
$kgharvested = $row['kgharvested'];
$kgsold = $row['kgsold'];
$income = $row['income'];
$date = date('d-m-Y', strtotime($entrydate));
?>


  <tbody>
    <tr>
      <td scope="col"><?php echo $userid ?></td>
	  <td scope="col"><?php echo $date ?></td>
      <td scope="col"><?php echo $species ?></td>
      <td scope="col"><?php echo $kgharvested ?></td>
	  <td scope="col"><?php echo $kgsold ?></td>
	  <td scope="col"><center><?php echo $income ?></center></td>

<?php
}
?>
    </tr>  
  </tbody>
</table>
<?php
date_default_timezone_set('Africa/Johannesburg');
?>
<p class="reportfoot">Printed Date: <?php echo date('d/m/Y'); ?> | Time: <?php echo date('h:i a'); ?></p>
</body>
<script>
setTimeout(function() {
    $('#system-message-container').fadeOut('fast');
}, 2000);
</script>