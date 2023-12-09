using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User {
    public static User Instance;

    public string username;
    public string password;
    public int userType;
    public int userPic;

    public User(Dictionary<string, string> data){
        //campos obligatorios:
        this.username = data["Username"];
        this.password = data["Password"];
        this.userType = int.Parse(data["UserType"]);

        //campos opcionales:
        this.userPic  = data.ContainsKey("UserPic") ? int.Parse(data["UserPic"]) : 0;
    }
}
