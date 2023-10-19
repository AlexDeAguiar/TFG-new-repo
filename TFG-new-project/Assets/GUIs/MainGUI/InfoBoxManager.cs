using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoBoxManager : SuperGUI
{
	VisualElement infoBox;
	Label infoText;

	public InfoBoxManager(VisualElement infoBox) {
		base.Init();
		this.infoBox = infoBox;
		infoText = infoBox.Q<Label>("InfoText");
	}

	public void hide() {
		//TODO
		infoBox.AddToClassList("hidden");
	}

	public void showText(string text) {
		//TODO
		infoText.text = text;
		infoBox.RemoveFromClassList("hidden");
	}
}
