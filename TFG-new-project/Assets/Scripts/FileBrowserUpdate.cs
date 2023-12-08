using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class FileBrowserUpdate : MonoBehaviour{
    public static readonly string[][] COMPATIBLE_EXTENSIONS = {
        new string[] { "Image files", ".jpg", ".jpeg", ".jpe", ".jfif", ".png" },
        new string[] { "Video files", ".mp4" },
        new string[] { "PDF files", ".pdf" }
    };

    string getFilterString(){
        string filterStr = "";
        for (int i = 0; i < COMPATIBLE_EXTENSIONS.Length; i++){
            filterStr += Translator._INTL(COMPATIBLE_EXTENSIONS[i][0]);

            string part1 = "";
            int length = COMPATIBLE_EXTENSIONS[i].Length;
            for (int j = 1; j < length; j++){
                part1 += "*" + COMPATIBLE_EXTENSIONS[i][j] + ((j + 1 < length) ? ", " : "");
            }

            filterStr += " (" + part1 + ") | " + part1 + ((i + 1 < COMPATIBLE_EXTENSIONS.Length) ? " | " : "");
        }
        Debug.Log(filterStr);
        return filterStr;
    }

    string addCharConditional(string separator, int idx, int length){
        return (idx + 1 < length) ? separator : "";
    }

    public void OpenFileBrowser(){
        var bp = new BrowserProperties(Translator._INTL("Choose a File"));
        //bp.initialDir(path) -> where dialog should be opened initially
        bp.filter = getFilterString();
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path => {
            string extension = Path.GetExtension(path);
            Debug.Log(extension);
            if(extension == ".mp4"){
                PlayerInteractionController.Instance.changeVideo(path);
            }
            else if(extension == ".pdf"){
                PlayerInteractionController.Instance.changePDF(path);
            }
            else{
                PlayerInteractionController.Instance.StopVideo();
                StartCoroutine(LoadImage(path));
            }
        });
    }

    public IEnumerator LoadImage(string path) {
        Debug.Log("Chose an img");
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path)){
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError){
                Debug.Log(uwr.error);
            }
            else{
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                PlayerInteractionController.Instance.changeImg(uwrTexture);
            }
        }
    }
}
