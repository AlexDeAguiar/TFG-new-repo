using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Web : MonoBehaviour{
    public const string SERVER = "http://localhost/UnityBackendTutorial/";
    public bool error = false;
    public const string WRONG_CREDENTIALS = "Wrong Credentials";
    public const string INEXISTENT_USER   = "Inexistent Username";
    public const string PREXISTENT_USER   = "Username is already taken";

    public StartMenu sm;

    public IEnumerator GetInfoFromServer(string phpFile, string ret){
        error = false;
        using (UnityWebRequest www = UnityWebRequest.Get(SERVER + phpFile)){
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
                ret = null;
                error = true;
            }
            else{
                ret = www.downloadHandler.text;
                Debug.Log(ret);
                byte[] results = www.downloadHandler.data;
            }
        }
    }

    public static string[] modes = { "Login", "SignUp" };
    public const int LOGIN  = 0;
    public const int SIGNUP = 1;

    public IEnumerator UserAction(Dictionary<string, string> _data, int mode){
        error = false;
        WWWForm form = new WWWForm();
        form.AddField("loginUsername", _data["Username"]);
        form.AddField("loginPassword", _data["Password"]);
        form.AddField("loginUserType", _data["UserType"]);

        UnityWebRequest www = UnityWebRequest.Post(SERVER + Web.modes[mode] + ".php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
            error = true;
        }
        else{
            string data = www.downloadHandler.text;
            Debug.Log(data);
            if((mode == LOGIN  && data != WRONG_CREDENTIALS && data != INEXISTENT_USER)){
                User.Instance = new User(parseUserData(data));
                SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }
            else if(mode == SIGNUP && data != PREXISTENT_USER){
                User.Instance = new User(_data);
                SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }
            else{
                sm.showErrorWindow(Translator._INTL(data));
            }
        }
    }

    public Dictionary<string, string> parseUserData(string data){
        // Eliminar los paréntesis y dividir la cadena en líneas
        string[] lines = data.Split('\n');

        Dictionary<string, string> resultDictionary = new Dictionary<string, string>();

        foreach (string line in lines){
            // Dividir la línea utilizando '=>' como delimitador
            string[] keyValue = line.Split(" => ");

            if (keyValue.Length == 2){
                // Limpiar claves y valores (eliminar espacios y corchetes)
                string key = keyValue[0].Trim().Trim('[', ']');
                string value = keyValue[1].Trim();

                // Agregar al diccionario resultante
                resultDictionary.Add(key, value);
            }
        }

        foreach (var pair in resultDictionary){
            Debug.Log($"{pair.Key}: {pair.Value}");
        }

        return resultDictionary;
    }
}