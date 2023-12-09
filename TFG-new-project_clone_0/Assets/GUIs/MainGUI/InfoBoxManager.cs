using UnityEngine;
using UnityEngine.UIElements;

public class InfoBoxManager : MonoBehaviour {
	public static InfoBoxManager Instance;

	VisualElement infoBox;
	Label infoText;

	public InfoBoxManager(VisualElement infoBox) {
		Instance = this;

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
