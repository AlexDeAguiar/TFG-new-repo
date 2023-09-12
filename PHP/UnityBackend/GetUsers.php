<?php
    $servername = "localhost"; //url del servidor
    $username = "root"; //no tocar por el momento
    $password = ""; //no tocar por el momento
    $dbname = "unitybackendtutorial";
    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Check connection
    if ($conn->connect_error) { die("Connection failed: " . $conn->connect_error); }
    echo "Connected successfully, showing the users list: <br><br>";


    //queries part: ====================================================================
    $sql = "SELECT u.Username, t.UserType FROM users u JOIN usertypes t ON u.UserType = t.ID;";
    $result = $conn->query($sql);
    
    if ($result->num_rows > 0) {
      // output data of each row
      while($row = $result->fetch_assoc()) {
        echo "Username: " . $row["Username"]. "<br>User type: " . $row["UserType"]. "<br><br>";
      }
    } 
    else { echo "0 results"; }
    $conn->close();
?>