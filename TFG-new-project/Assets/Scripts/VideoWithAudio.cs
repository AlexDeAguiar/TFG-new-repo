using UnityEngine;
using UnityEngine.Video;

public class VideoWithAudio : MonoBehaviour{
    public VideoPlayer videoPlayer; // Asigna el componente VideoPlayer en el Inspector
    private bool is_Playing;
    private bool videoLoaded;

    void Start(){ is_Playing = false; videoLoaded = false; }

    public void Play(){
		if(videoLoaded){
			videoPlayer.Play(); 
			is_Playing = true;
		}
	}
    public void Pause(){
		if(videoLoaded){
			videoPlayer.Pause(); 
			is_Playing = false;
		}
	}
	public void Stop() {
		videoPlayer.Stop();
		is_Playing = false;
		videoLoaded = false;
	}

	public void changeVideo(string path) {
		videoPlayer.Stop();
		is_Playing = false;
		videoLoaded = true;
		videoPlayer.url = "File://" + path;

		Debug.Log("Changed the video path of the blackboard to: " + path);

		Debug.Log("Playing first frame");

		//YES
		videoPlayer.Play();
		System.Threading.Thread.Sleep(100);
		videoPlayer.Pause();
	}

	public bool isPlaying(){ return is_Playing; }
}
