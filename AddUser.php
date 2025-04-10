<?php
    require_once "Helper.php";

    $helper = new Helper();
    Helper::connect();

    $email = isset($_GET['email']) ? $_GET['email'] : null;
    $pass = isset($_GET['pass']) ? $_GET['pass'] : null;
    $username = isset($_GET['user']) ? $_GET['user'] : null;
    $xp = isset($_GET['xp']) ? $_GET['xp'] : 0;
    $pieces = isset($_GET['pieces']) ? $_GET['pieces'] : 1000;

    if(isset($email) && isset($pass) && isset($username) && $email != "" && $pass != "" && $username != "")
    {
        $user = $helper->AddUser($email, md5($pass), $username);
    }elseif (isset($email) && isset($username) && isset($pass) && isset($xp) && isset($pieces) && $username != "" && $pass != "" && $pieces != "" && $email != "" && $xp != "")
    {
        $user = $helper->AddUser($email, md5($pass),$username, $pieces, $xp);
    }
    else{
        $user = "0";
    }

    echo $user;
?>