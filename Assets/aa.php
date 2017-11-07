<?php
$fp = fopen('ar.dat', 'r');//受信よーい
  $point = fgets($fp);
fclose($fp);
$count = $_POST['count'];
$fp = fopen('ar.dat', 'w');//書き込みよーい
$point = $point + $count;
fputs($fp, $point);
fclose($fp);
?>