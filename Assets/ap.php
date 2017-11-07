<?php
$fp = fopen('ar.dat', 'r');//受信よーい　
  $point = fgets($fp);
fclose($fp);
print($point);
?>