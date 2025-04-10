<?php
    require_once "Helper.php";

    $helper = new Helper();
    Helper::connect();

    $classement = $helper->GetLeaderboard();
    echo $classement;
?> 