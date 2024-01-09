using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamTextureController : MonoBehaviour{
    public RawImage webcamDisplay; // Referencia a la Raw Image para mostrar la vista de la webcam
    private WebCamTexture webcamTexture;

    void Start(){
        // Buscar la referencia a la Raw Image en el Canvas
        webcamDisplay = GameObject.FindObjectOfType<RawImage>();

        // Aseg�rate de que haya al menos una webcam disponible
        if (WebCamTexture.devices.Length == 0){
            Debug.LogError("No se ha encontrado ninguna webcam.");
            return;
        }

        // Obt�n la primera webcam disponible (puedes ajustar esto seg�n tus necesidades)
        webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name);

        // Inicia la vista de la webcam
        webcamTexture.Play();

        // Asigna la vista de la webcam a la Raw Image
        if (webcamDisplay != null){
            webcamDisplay.texture = webcamTexture;
			var playerCameraModelRendered = GameObject.Find("FaceCamNew").GetComponent<Renderer>();
			playerCameraModelRendered.material.SetTexture("RawImage", webcamTexture);
		}
    }

    void OnDestroy(){
        // Det�n la vista de la webcam cuando el script se destruye
        if (webcamTexture != null && webcamTexture.isPlaying){
            webcamTexture.Stop();
        }
    }
}