using UnityEngine;
using UnityEngine.UIElements;

public class VideoSelectorBoxManager : MonoBehaviour {
	public static VideoSelectorBoxManager Instance;

	private VisualElement videoSelectorBox;
	private TextField videoSelectorTextBox;
	private Button videoSelectorSubmitButton;

	public VideoSelectorBoxManager(VisualElement videoSelectorBox) {
		Instance = this;

		this.videoSelectorBox = videoSelectorBox;
		videoSelectorTextBox = videoSelectorBox.Q<TextField>("FilePathBox");
		videoSelectorSubmitButton = videoSelectorBox.Q<Button>("FilePathSubmitButton");
		videoSelectorSubmitButton.RegisterCallback<ClickEvent>(evt => submitFilePath(evt));
	}

	public void show() {
		videoSelectorBox.RemoveFromClassList("hidden");
	}

	public void hide() {
		videoSelectorBox.AddToClassList("hidden");
	}
	private void submitFilePath(ClickEvent evt){
		if (PlayerInteractionController.Instance == null){
			Debug.Log("video Selector can not submit the video path, because PlayerInteractionController is null. Has the player connected to the world?");
			return;
		}

		string path = videoSelectorTextBox.text;
		Debug.Log("Selected path: " + path);

		PlayerInteractionController.Instance.changeVideo(path);
	}
}
