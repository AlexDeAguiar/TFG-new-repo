using UnityEngine;
using UnityEngine.UIElements;

public class VideoSelector : SuperGUI, Translatable
{
	PlayerController playerController = null;
	VisualElement fileSelector;
	TextField filePathTextBox;
	Button filePathSubmitButton;

	void Start()
	{
		base.Init();
		fileSelector = Root.Q<VisualElement>("FileSelector");
		filePathTextBox = fileSelector.Q<TextField>("FilePathBox");
		filePathSubmitButton = fileSelector.Q<Button>("FilePathSubmitButton");
		filePathSubmitButton.RegisterCallback<ClickEvent>(evt => submitFilePath(evt));
	}

	public void setPlayerController(PlayerController playerController) {
		this.playerController = playerController;
	}

	void submitFilePath(ClickEvent evt) {
		if (playerController == null) {
			Debug.Log("video Selector can not submit the video pat, because playerController is null. Has the player connected to the world?");
			return;
		}

		string path = filePathTextBox.text;
		Debug.Log("Selected path: " + path);

		playerController.changeVideo(path);
	}
}
