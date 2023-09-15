using UnityEngine;
using UnityEngine.Video;

public class VideoWithAudio : MonoBehaviour{
    public VideoPlayer videoPlayer; // Asigna el componente VideoPlayer en el Inspector
    public Renderer boardRenderer; // Asigna el material de la pizarra en el Inspector

    void Start(){
        videoPlayer.Play(); // Inicia la reproducción del video
        
        // Configura la textura del material de la pizarra con la textura de video del reproductor
        Material material    = boardRenderer.material;
        material.mainTexture = videoPlayer.texture;
    }
}
