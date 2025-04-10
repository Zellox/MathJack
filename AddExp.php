<?php
    require_once "Helper.php";

    $helper = new Helper();
    Helper::connect();

    $user = isset($_GET['user']) ? $_GET['user'] : null;
    $exp = isset($_GET['exp']) ? $_GET['exp'] : 0;

    if(isset($user) && isset($exp) && $user != "" && $exp != "")
    {
        $expAdded = $helper->AddExp($user,$exp);
        return "<p>$exp ajoutées</p>";
    }else
    {
        return "<p>entrez un nom d'utilisateur</p>";
    }
?>
