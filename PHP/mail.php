<?php
	// MivzakLive Mail Sender
	echo mail($_GET['to'],$_GET['subject'],$_GET['body']) ? "TRUE" : "FALSE";
?>