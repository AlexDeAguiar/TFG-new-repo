using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Main : MonoBehaviour{
    public static Main Instance;
    public Web Web;
    // Start is called before the first frame update
    void Start(){
        Instance = this;
        Web = GetComponent<Web>();
        Translator.InitDicts();




        var audio = new AudioSource();
        audio.clip = Microphone.Start(null, true, 10, 44100);
        audio.loop = true;
        while(Microphone.GetPosition(null) > 0);
        audio.Play();
    }

    public static string DateToString(){
        var timeNow = System.DateTime.Now;
        return timeNow.Year.ToString() + "-" + timeNow.Month.ToString()  + "-" + timeNow.Day.ToString() + " " + 
               timeNow.Hour.ToString() + ":" + timeNow.Minute.ToString() + ":" + timeNow.Second.ToString();
    }
}
