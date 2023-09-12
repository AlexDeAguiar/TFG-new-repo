<?php
    $servername = "localhost"; //url del servidor
    $username   = "root"; //no tocar por el momento
    $password   = ""; //no tocar por el momento
    $dbname     = $_POST["database"];
    $query      = $_POST["query"]; 


    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Check connection
    if ($conn->connect_error) { die("Connection failed: " . $conn->connect_error); }

    //queries part: ====================================================================
    //print_r($conn->query($query)->fetch_all());
    //*
    $result = $conn->query($query);
    
    $conn->query("UPDATE users SET LastActivityTime = CAST('" . $_POST["lastActivityT"]; . "' AS DATETIME) WHERE Username = '" . $_POST["loginUsername"] . "'");
    while($row = $result->fetch_assoc()){ print_r($row); }
    //*/
    $conn->close();
?>