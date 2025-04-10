<?php
    class Helper
    {
        static private $hostname = 'localhost';
        static private $database = 'saes4';
        static private $login = 'root';
        static private $password = '';
        static private $tabUTF8 = array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8");
        static private $pdo;

        static public function pdo() {return self::$pdo;}
        static public function connect()  {
            $h = self::$hostname; $d = self::$database; $l = self::$login; $p = self::$password;
            try {
                self::$pdo = new PDO("mysql:host=$h;dbname=$d",$l,$p);
                self::$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            } catch(PDOException $e) {
                echo "Erreur de connexion : ".$e->getMessage()."<br>";
            }
        }

        function GetUsers($user=null)
        {
            // Construire la requête SQL de manière dynamique
            $sql = "SELECT * FROM users WHERE id > 0";

            // Si un ID est fourni, ajouter la condition AND id = ?
            if ($user !== null) {
                $sql .= " AND username = '$user' ";  // L'ID est déjà un entier, donc sans risque
            }

            $result = self::pdo()->prepare($sql);
            $result->execute();

            $d = $result->fetchAll(PDO::FETCH_ASSOC);
            return $this->ParseUsersJson($d);
        }

        function ParseUsersJson($array = array())
        {
            $r = array();
            foreach ($array as $key => $value) {
                $t = array(
                    "id" => intval($value["id"]),
                    "email" => (string)$value["email"],
                    "pass" => (string)$value["pass"],
                    "username" => (string)$value["username"],
                    "pieces" => intval($value["pieces"]),
                    "xp" => intval($value["exp"])
                );
                array_push($r, $t);
            }
            return json_encode($r);
        }

        function AddUser($email = null, $pass = null, $username = null, $pieces = 1000, $xp = 0)
        {
            $sql = "INSERT INTO `users` (`email`, `pass`, `username`, `exp`, `pieces`) 
            VALUES ('$email', '$pass', '$username', '$xp', '$pieces')";

            $result = self::pdo()->query($sql);

            $data = $result->fetchAll();

            return self::pdo()->lastInsertId();
        }

        function GetPieces($user = null)
        {
            $sql = "SELECT PIECES FROM users WHERE username = '$user'";

            $result = self::pdo()->query($sql);
            
            $d = $result->fetchAll(PDO::FETCH_ASSOC);

            return json_encode($d);
        }

        function GetExp($user = null)
        {
            $sql = "SELECT exp FROM users WHERE username = '$user'";

            $result = self::pdo()->query($sql);
            
            $d = $result->fetchAll(PDO::FETCH_ASSOC);

            return json_encode($d);
        }

        function AddPieces($user = null, $pieces)
        {
            // Convertir en entier les pièces envoyées
            $pieces = intval($pieces);
        
            // Décode la réponse JSON en tableau associatif
            $tabPieces = json_decode($this::GetPieces($user), true);
        
            // Vérifie si la réponse contient un tableau avec des éléments
            if (!empty($tabPieces) && isset($tabPieces[0]['PIECES'])) {
                $piecesBefore = intval($tabPieces[0]['PIECES']);  // Récupère la valeur de "PIECES" du premier élément
            } else {
                $piecesBefore = 0;  // Si "PIECES" n'est pas trouvé, initialiser à 0
            }
        
            // Additionne les pièces existantes avec celles à ajouter
            $pieces += $piecesBefore;
        
            // Si aucun utilisateur n'est spécifié, renvoie une erreur
            if ($user == null) {
                return "<p>Entrez un nom d'utilisateur</p>";
            } else {
                // Préparer et exécuter la mise à jour dans la base de données
                $sql = "UPDATE users
                        SET pieces = '$pieces'
                        WHERE username = '$user';";
                
                $result = self::pdo()->query($sql);
        
                // Récupérer les résultats (facultatif)
                $d = $result->fetchAll(PDO::FETCH_ASSOC);
        
                // Affichage des pièces avant et après l'ajout
                echo "$piecesBefore pièce avant, $pieces pièce après";
        
                // Retourne les résultats en JSON
                return json_encode($d);
            }
        }        


        function AddExp($user = null, $exp)
        {
            // Convertir en entier les exp envoyées
            $exp = intval($exp);
        
            // Décode la réponse JSON en tableau associatif
            $tabExp = json_decode($this::GetExp($user), true);
        
            // Vérifie si la réponse contient un tableau avec des éléments
            if (!empty($tabExp) && isset($tabExp[0]['exp'])) {
                $expBefore = intval($tabExp[0]['exp']);  // Récupère la valeur de "exp" du premier élément
            } else {
                $expBefore = 0;  // Si "exp" n'est pas trouvé, initialiser à 0
            }
        
            // Additionne les pièces existantes avec celles à ajouter
            $exp += $expBefore;
        
            // Si aucun utilisateur n'est spécifié, renvoie une erreur
            if ($user == null) {
                return "<p>Entrez un nom d'utilisateur</p>";
            } else {
                // Préparer et exécuter la mise à jour dans la base de données
                $sql = "UPDATE users
                        SET exp = '$exp'
                        WHERE username = '$user';";
                
                $result = self::pdo()->query($sql);
        
                // Récupérer les résultats (facultatif)
                $d = $result->fetchAll(PDO::FETCH_ASSOC);
        
                // Affichage des exp avant et après l'ajout
                echo "$expBefore exp avant, $exp exp après";
        
                // Retourne les résultats en JSON
                return json_encode($d);
            }
        }
        
        function GetLeaderboard()
        {
            // Créer la requête SQL pour récupérer tous les utilisateurs triés par le nombre de pièces de manière décroissante
            $sql = "SELECT username, pieces FROM users ORDER BY pieces DESC";
        
            // Exécuter la requête
            $result = self::pdo()->query($sql);
        
            // Vérifier si la requête a retourné des résultats
            if ($result && $result->rowCount() > 0) {
                // Récupérer les résultats sous forme de tableau associatif
                $leaderboard = $result->fetchAll(PDO::FETCH_ASSOC);
            
                // Ajouter un champ "rank" à chaque utilisateur
                foreach ($leaderboard as $index => &$user) {
                    $user['rang'] = $index + 1;  // Assigner le rang en fonction de l'index (1 pour le premier)
                }
            
                // Retourner le classement en JSON
                return json_encode($leaderboard);
            } else {
                // Si aucune donnée n'a été trouvée, retourner un message
                return json_encode(["message" => "Aucun joueur trouvé."]);
            }
        }

    }