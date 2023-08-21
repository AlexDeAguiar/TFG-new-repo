using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Backend : MonoBehaviour{
    int lastT = 0;
    public const int UPDATE_INTERVAL = 5;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        int newT = System.DateTime.Now.Millisecond;
        if(newT - lastT > UPDATE_INTERVAL){
            newT = lastT;


        }
    }

    public void UpdateLocal(List<Dictionary<string, string>> data){

    }
}
