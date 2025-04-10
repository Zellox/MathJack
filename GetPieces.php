<?php
     require_once "Helper.php";

     $helper = new Helper();
     Helper::connect();
    
     $username = isset($_GET['user']) ? $_GET['user'] : null;
     if(isset($_GET["user"]) && $username != "")
     {
        $pieces = $helper->GetPieces($username);
        echo $pieces;
     }else
     {
        echo "<p>entrez un nom d'utilisateur</p>";
     }

     
?>     