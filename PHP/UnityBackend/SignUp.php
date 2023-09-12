<?php
    $servername = "localhost"; //url del servidor
    $username = "root"; //no tocar por el momento
    $password = ""; //no tocar por el momento

    $loginUsername = $_POST["loginUsername"];
    $loginPassword = $_POST["loginPassword"];
    $loginUserType = $_POST["loginUserType"];
    $SIquery       = $_POST["signInQuery"];
    $SUquery       = $_POST["signUpQuery"];
    $dbname        = $_POST["database"];

    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Check connection
    if ($conn->connect_error) { die("Connection failed: " . $conn->connect_error); }

    //queries part: ====================================================================
    //$sql = "SELECT u.Username FROM users u WHERE u.Username = '" . $loginUsername . "'";

    $result = $conn->query($SIquery);
    
    if ($result->num_rows > 0) {
        //tell user that that name is already taken
        echo "Username is already taken";
    } 
    else {
        //Insert user and password into the database:
        //$sql2 = "INSERT INTO users (Username, Password, UserType) VALUES ('" . $loginUsername . "', '" . $loginPassword ."', " . $loginUserType .")";
    
        if ($conn->query($SUquery) === TRUE) {
            echo "New record created successfully.";
            $conn->query("UPDATE users SET LastActivityTime = CAST('" . $_POST["lastActivityT"] . "' AS DATETIME) WHERE Username = '" . $_POST["loginUsername"] . "'");
        }
        else {
            echo "Error: " . $SUquery . "\n" . $conn->error;
        }
    }
    $conn->close();
?>