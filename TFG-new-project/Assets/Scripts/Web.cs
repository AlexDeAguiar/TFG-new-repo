using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Web : MonoBehaviour{
    public const string SERVER = "http://localhost/UnityBackend/";
    public bool error = false;
    public const string WRONG_CREDENTIALS = "Wrong Credentials";
    public const string INEXISTENT_USER   = "Inexistent Username";
    public const string PREXISTENT_USER   = "Username is already taken";
    public const string NEXT_SCENE        = "InGameNoOffline";
    public const string DATABASE          = "unitybackend";

    public StartMenu sm;

    public IEnumerator GetInfoFromServer(string query, string phpFile, Game_Backend _ref){
        error = false;
        WWWForm form = new WWWForm();
        form.AddField("database", DATABASE);
        form.AddField("query", query);

        UnityWebRequest www = UnityWebRequest.Post(SERVER + phpFile + ".php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
            error = true;
        }
        else{
            if(_ref != null){ _ref.UpdateLocal(ParseList(www.downloadHandler.text)); }
        }
        /*
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
        */
    }

    public static string[] modes = { "Login", "SignUp" };
    public const int LOGIN  = 0;
    public const int SIGNUP = 1;

    public IEnumerator UserAction(Dictionary<string, string> _data, int mode){
        string SignInQuery = "SELECT * FROM users u WHERE u.Username = '" + _data["Username"] + "'";
        string SignUpQuery = "INSERT INTO users (Username, Password, UserType) VALUES ('" +_data["Username"] + "', '" + _data["Password"] + "', " + _data["UserType"] + ")";
        error = false;
        WWWForm form = new WWWForm();
        form.AddField("loginUsername", _data["Username"]);
        form.AddField("loginPassword", _data["Password"]);
        form.AddField("loginUserType", _data["UserType"]);
        form.AddField("lastActivityT", Main.DateToString());
        form.AddField("signInQuery", SignInQuery);
        form.AddField("signUpQuery", SignUpQuery);
        form.AddField("database", DATABASE);

        UnityWebRequest www = UnityWebRequest.Post(SERVER + Web.modes[mode] + ".php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
            error = true;
        }
        else{
            string data = www.downloadHandler.text;
            if((mode == LOGIN  && data != WRONG_CREDENTIALS && data != INEXISTENT_USER)){
                User.Instance = new User(ParseList(data)[0]);
                SceneManager.LoadScene(NEXT_SCENE, LoadSceneMode.Single);
            }
            else if(mode == SIGNUP && data != PREXISTENT_USER){
                User.Instance = new User(_data);
                SceneManager.LoadScene(NEXT_SCENE, LoadSceneMode.Single);
            }
            else{
                sm.showErrorWindow(Translator._INTL(data));
            }
        }
    }


    public List<Dictionary<string, string>> ParseList(string data){
        List<Dictionary<string, string>> resultList = new List<Dictionary<string, string>>();

        Debug.Log(data);
        // Remover los corchetes exteriores
        data = data.Trim('(',')').Trim('\t').Trim('\r');

        // Dividir la cadena en diccionarios individuales
        string[] dictionaryStrings = data.Split(")\nArray");
        foreach (string dictionaryString in dictionaryStrings){
            Dictionary<string, string> dictionary = ParseDictionary(dictionaryString);
            resultList.Add(dictionary);
        }

        // Imprimir la información para verificar
        /*
        foreach (var dictionary in resultList){
            foreach (var pair in dictionary){
                Debug.Log($"{pair.Key}: {pair.Value}");
            }
            Debug.Log("---------");
        }
        */

        return resultList;
    }

    public Dictionary<string, string> ParseDictionary(string data){
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
            else if(keyValue.Length > 0 && keyValue[0] == ")"){ break; }
        }

        return resultDictionary;
    }
}