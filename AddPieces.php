<?php
    require_once "Helper.php";

    $helper = new Helper();
    Helper::connect();

    $user = isset($_GET['user']) ? $_GET['user'] : null;
    $pieces = isset($_GET['pieces']) ? $_GET['pieces'] : 0;

    if(isset($user) && isset($pieces) && $user != "" && $pieces != "")
    {
        $pieceAdded = $helper->AddPieces($user,$pieces);
        return "<p>$pieces ajoutées</p>";
    }else
    {
        return "<p>entrez un nom d'utilisateur</p>";
    }
?>
