<?php
$fp = fopen("test.txt", "a");
$val = $_POST['key1'];
fwrite( $fp,$val );
fclose( $fp );
?>