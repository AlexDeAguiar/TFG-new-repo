using UnityEngine;
using UnityEngine.Video;

public class VideoWithAudio : MonoBehaviour{
    public VideoPlayer videoPlayer; // Asigna el componente VideoPlayer en el Inspector
    private bool is_Playing;

    void Start(){ is_Playing = false; }

    public void Play(){ videoPlayer.Play(); is_Playing = true; }
    public void Pause(){ videoPlayer.Pause(); is_Playing = false; }

    public bool isPlaying(){ return is_Playing; }
}
