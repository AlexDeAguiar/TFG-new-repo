using UnityEngine;
using UnityEngine.Video;

public class VideoWithAudio : MonoBehaviour{
    public VideoPlayer videoPlayer; // Asigna el componente VideoPlayer en el Inspector
    private bool is_Playing;

    void Start(){ is_Playing = false; }

    public void Play(){
		videoPlayer.Play(); is_Playing = true;
	}
    public void Pause(){ videoPlayer.Pause(); is_Playing = false; }
	public void Stop() {
		videoPlayer.Stop();
		is_Playing = false;
	}

	public void changeVideo(string path) {
		videoPlayer.Stop();
		is_Playing = false;
		videoPlayer.url = path;

		Debug.Log("Changed the video path of the blackboard to: " + path);

		Debug.Log("Playing first frame");

		//YES
		videoPlayer.Play();
		System.Threading.Thread.Sleep(100);
		videoPlayer.Pause();
	}

	public bool isPlaying(){ return is_Playing; }
}
