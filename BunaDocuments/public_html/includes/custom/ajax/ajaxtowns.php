<?php
	define (DB_USER, "midnight_db");
	define (DB_PASSWORD, "96y685js9w");
	define (DB_DATABASE, "midnight_db");
	define (DB_HOST, "localhost");

	$mysqli = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

	$sql = "SELECT Town FROM smart_lkp_towns 
			WHERE Town LIKE '%".$_GET['term']."%'
			LIMIT 5"; 
	$result = $mysqli->query($sql);

	$json = [];

	while($row = $result->fetch_assoc()){
	     $json[] = $row['Town'];
	}
	echo json_encode($json);
?>