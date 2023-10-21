using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoBoxManager : MonoBehaviour
{
	VisualElement infoBox;
	Label infoText;

	public InfoBoxManager(VisualElement infoBox) {
		this.infoBox = infoBox;
		infoText = infoBox.Q<Label>("InfoText");
	}

	public void hide() {
		//TODO
		infoBox.AddToClassList("hidden");
	}

	public void showText(string textKey) {
		//TODO
		infoText.text = Translator._INTL(textKey);
		infoBox.RemoveFromClassList("hidden");
	}
}
