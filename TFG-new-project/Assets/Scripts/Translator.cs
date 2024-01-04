using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum LANGUAGES{ ES, EN, DE, FR, JP, TM, /*...*/ }
public interface Translatable{ public void updateTexts(); }

public class Translator : MonoBehaviour {
    public const  string PATH   = "./Assets/Translations/";
    private static List<Translatable> translatables = new List<Translatable>();

    public static Dictionary<LANGUAGES, Dictionary<string, string>> jsons;
    public static LANGUAGES LAN = LANGUAGES.ES;

    private static bool DICTS_LOADED = false;
    public static void InitDicts(){
        if(!DICTS_LOADED){
            jsons = new Dictionary<LANGUAGES, Dictionary<string, string>>();
            foreach (LANGUAGES lan in Enum.GetValues(typeof(LANGUAGES))){
                string rutaArchivo = PATH + lan + ".json";

                if (File.Exists(rutaArchivo)){
                    JObject json = JObject.Parse(File.ReadAllText(rutaArchivo));
                    jsons.Add(lan, json.ToObject<Dictionary<string, string>>());
                }
                else{
                    Debug.Log("File '" + lan + ".json' not found. Please locate the file at '" + rutaArchivo + "'.");
                }
            }

            DICTS_LOADED = true;
        }
    }

    public static string _INTL(string text){ return _INTL(text, LAN); }

    public static string _INTL(string text, LANGUAGES lang){
        Dictionary<string, string> lan = jsons[lang];
		if (lan.ContainsKey(text)) {
			return lan[text];
		}
		else {
			Debug.LogWarning(
				String.Format("Could not find a tranlation for key: %s and language %s", text, lang));
			return text;
		}
    }

    public static void changeLan(LANGUAGES newLan){
        LAN = newLan;
        foreach (Translatable tr in translatables) { tr.updateTexts(); }
    }

    public static int getCurrentLanIdx(){ return (int) LAN; }

    public static List<string> getLanguagesList(){
        List<string> options = new List<string>();
        foreach (LANGUAGES lan in Enum.GetValues(typeof(LANGUAGES))){ options.Add(Translator._INTL("LAN_NAME", lan)); }
		return options;
    }

    public static void Register(Translatable tr){ translatables.Add(tr); }
}