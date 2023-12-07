using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class FileBrowserUpdate : MonoBehaviour{
    public void OpenFileBrowser(){
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.mp4) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.mp4";
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
