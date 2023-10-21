using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VideoSelectorBoxManager : MonoBehaviour
{
	private VisualElement videoSelectorBox;
	private TextField videoSelectorTextBox;
	private Button videoSelectorSubmitButton;

	PlayerController playerController;

	public VideoSelectorBoxManager(VisualElement videoSelectorBox) {
		this.videoSelectorBox = videoSelectorBox;
		videoSelectorTextBox = videoSelectorBox.Q<TextField>("FilePathBox");
		videoSelectorSubmitButton = videoSelectorBox.Q<Button>("FilePathSubmitButton");
		videoSelectorSubmitButton.RegisterCallback<ClickEvent>(evt => submitFilePath(evt));
	}

	public void setPlayerController(PlayerController playerController) {
		this.playerController = playerController;
	}

	public void show() {
		videoSelectorBox.RemoveFromClassList("hidden");
	}

	public void hide() {
		videoSelectorBox.AddToClassList("hidden");
	}
	private void submitFilePath(ClickEvent evt)
	{
		if (playerController == null)
		{
			Debug.Log("video Selector can not submit the video pat, because playerController is null. Has the player connected to the world?");
			return;
		}

		string path = videoSelectorTextBox.text;
		Debug.Log("Selected path: " + path);

		playerController.changeVideo(path);
	}

}
