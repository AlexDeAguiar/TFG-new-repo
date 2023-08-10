using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public enum LANGUAGES{
    ES,
    //EN,
    //DE,
    //...
}

public class Translator : MonoBehaviour {
    private static Dictionary<LANGUAGES, Dictionary<string, string>> jsons;
    public const  string PATH   = "./Assets/Translations/";
    public static LANGUAGES LAN = LANGUAGES.ES;

    void Start(){}

    public static void InitDicts(){
        jsons = new Dictionary<LANGUAGES, Dictionary<string, string>>();
        foreach (LANGUAGES lan in Enum.GetValues(typeof(LANGUAGES))){
            JObject json = JObject.Parse(File.ReadAllText(PATH + lan + ".json"));
            jsons.Add(lan, json.ToObject<Dictionary<string, string>>());
        }
    }

    public static string _INTL(string text){
        Dictionary<string, string> lan = jsons[LAN];
        return lan.ContainsKey(text) ? lan[text] : text;
    }
}