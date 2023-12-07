using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class FileBrowserUpdate : MonoBehaviour{
    public const string[][] COMPATIBLE_EXTENSIONS = {
        ["Image files", ".jpg", ".jpeg", ".jpe", ".jfif", ".png"],
        ["Video files", ".mp4"]
    };

    string getFilterString(){
        string filterStr = ""
        for (int i = 0; i < COMPATIBLE_EXTENSIONS.length; i++){
            filterStr += Translator._INTL(COMPATIBLE_EXTENSIONS[i][0]);

            string part1 = "";
            string part2 = "";
            int length = COMPATIBLE_EXTENSIONS[i].length;
            for (int j = 1; j < length; j++){
                part1 += "*" + COMPATIBLE_EXTENSIONS[i][j] + ((j + 1 < length) ? ", " : "");
              //part2 += "*" + COMPATIBLE_EXTENSIONS[i][j] + ((j + 1 < length) ? "; " : "");
            }

            filterStr += "(" + part1 + ") | " + part1 + ((i + 1 < COMPATIBLE_EXTENSIONS.length) ? " | " : "")
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
        //string filter = 
            //Translator._INTL("Image files") + " (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; |" +
            //Translator._INTL("Video files") + " (*.mp4) | *.mp4";
        bp.filter = getFilterString();
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path => {
            //Load image from local path with UWR
            string extension = Path.GetExtension(path);
            if(extension == ".mp4"){
                Debug.Log(extension);
                PlayerInteractionController.Instance.changeVideo(path);
            }
            else{
                PlayerInteractionController.Instance.StopVideo();
                StartCoroutine(LoadImage(path));
            }
        });
    }

    IEnumerator LoadImage(string path) {
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
