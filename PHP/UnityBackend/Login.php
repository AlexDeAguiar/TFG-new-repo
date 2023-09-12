<?php
    $servername = "localhost"; //url del servidor
    $username = "root"; //no tocar por el momento
    $password = ""; //no tocar por el momento

    $loginUsername = $_POST["loginUsername"];
    $loginPassword = $_POST["loginPassword"];
    $lastActivityT = $_POST["lastActivityT"];
    $dbname        = $_POST["database"];
    $query         = $_POST["signInQuery"];


    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Check connection
    if ($conn->connect_error) { die("Connection failed: " . $conn->connect_error); }

    //queries part: ====================================================================
    $result = $conn->query($query);
    
    if ($result->num_rows > 0) {
      $conn->query("UPDATE users SET LastActivityTime = CAST('" . $lastActivityT . "' AS DATETIME) WHERE Username = '" . $loginUsername . "'");
      // output data of each row
      while($row = $result->fetch_assoc()) {
        if($row["Password"] == $loginPassword){ print_r($row); }
        else{ echo "Wrong Credentials"; }
      }
    } 
    else { echo "Inexistent Username"; }
    $conn->close();
?>