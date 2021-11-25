<?php
defined('_JEXEC') or die( 'Restricted access' );
$document = JFactory::getDocument();
JHtml::_('behavior.formvalidator');
$user = JFactory::getUser();
$useridcurrent = ($user->id);
$firstname = ($user->name);
$todaydate = date('d-m-Y H:i:s');
$firstdayofmonth = date('Y-m-01 H:i:s');

$format_enddate  = date('Y-m-d', strtotime($todaydate));
$format_startdate  = date('Y-m-d', strtotime($firstdayofmonth));

//// START EXCEL EXPORT FUNCTION ////
if(isset($_POST["Export"])){
		 
      function cleanData(&$str)
  {
    if($str == 't') $str = 'TRUE';
    if($str == 'f') $str = 'FALSE';
    if(preg_match("/^0/", $str) || preg_match("/^\+?\d{8,}$/", $str) || preg_match("/^\d{4}.\d{1,2}.\d{1,2}/", $str)) {
      $str = "'$str";
    }
    if(strstr($str, '"')) $str = '"' . str_replace('"', '""', $str) . '"';
  }

  // filename for download
  $filename = "Production_Report_MTD_" . $todaydate . ".csv";

  header("Content-Disposition: attachment; filename=\"$filename\"");
  header("Content-Type: text/csv");

  $out = fopen("php://output", 'w');

  $flag = false;
  $database =& JFactory::getDBO();
		$sqljobcodes = "SELECT userID,
		entrydate,
		species,
       kgharvested,
       kgsold,
       income
  FROM smartapp_farmer_submissions
  WHERE (entrydate >= '$format_startdate'
 AND entrydate <= '$format_enddate')
ORDER BY userID ASC, species ASC"; 
		$database->setQuery($sqljobcodes);
		$resultjobcodes = $database->query($sqljobcodes);
	$database->close();


	while ($row=mysqli_fetch_assoc($resultjobcodes)){
    if(!$flag) {
      // display field/column names as first row
      fputcsv($out, array_keys($row), ',', '"');
      $flag = true;
    }
	
    array_walk($row, __NAMESPACE__ . '\cleanData');
    fputcsv($out, array_values($row), ',', '"');
  }

  fclose($out);
  exit; 
//// END EXCEL EXPORT FUNCTION ////  
 } else {
$database =& JFactory::getDBO();
	$sql = "SELECT farmersubmissionsID,
	userID,
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
$farmersubmissionsid = $row['farmersubmissionsID'];
$userid = $row['userID'];
$entrydate = $row['entrydate'];
$species = $row['species'];
$kgharvested = $row['kgharvested'];
$kgsold = $row['kgsold'];
$income = $row['income'];
$date = date('d-m-Y', strtotime($entrydate));
if ($useridcurrent == $userid){
	$visibility = "show";
}
if ($useridcurrent != $userid){
	$visibility = "hide";
}
?>


  <tbody>
    <tr>
      <td scope="col"><?php echo $userid ?></td>
	  <td scope="col"><?php echo $date ?></td>
      <td scope="col"><?php echo $species ?></td>
      <td scope="col"><?php echo $kgharvested ?></td>
	  <td scope="col"><?php echo $kgsold ?></td>
	  <td scope="col"><center><?php echo $income ?></center></td>
	  <td><div class = <?php echo $visibility; ?>><a type="button" class="btn btn-mini btn-block" href="./index.php/production-update?i=<?php echo $farmersubmissionsid ?>"> Edit</a></div>
	  <div class = <?php echo $visibility; ?>><a type="button" class="btn btn-mini btn-danger btn-block" href="./index.php/production-delete-record?i=<?php echo $farmersubmissionsid ?>"> Delete</a></div></td>

<?php
}
?>
    </tr>  
  </tbody>
</table>
<?php
////  END SUMMARY TRAVEL PER DATE ////
//// ----------- END -------------- ////
?>
 <div>
            <form class="form-horizontal" action="<?php $_SERVER['PHP_SELF'];?>" method="post" name="upload_excel"   
                      enctype="multipart/form-data">
                  <div class="form-group">
                            <div>
                                <input type="submit" name="Export" class="btn btn-hg-green" value="export to excel"/>

							<a type="button" class="btn btn-hg-blue" target="_blank" href="./index.php/print-production-report-mtd?tmpl=component"><i class="fa fa-print"></i> Print</a>
							</div>
                   </div>                    
            </form>  

 </div>
<?php
}
?>
<script>
setTimeout(function() {
    $('#system-message-container').fadeOut('fast');
}, 2000);
</script>