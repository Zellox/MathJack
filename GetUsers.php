<?php
    require_once "Helper.php";

    $helper = new Helper();
    Helper::connect();

    if(isset($_GET["id"]) && $_GET["id"] > 0)
    {
        $id = $_GET["id"];
        $users = $helper->GetUsers($id);
    }
    else
    {
        $users = $helper->GetUsers();
    }

    echo $users;
?>