using UnityEngine;
using UnityEngine.UIElements;

public class ConnectBoxManager : MonoBehaviour {
	private VisualElement connectBox;
	private Button hostButton;
	private Button clientButton;
	private TextField joinCodeBox;

	private TestRelay testRelay;

	public ConnectBoxManager(VisualElement connectBox) {
		this.connectBox = connectBox;
		hostButton = connectBox.Q<Button>("HostButton");
		var clientBox = connectBox.Q<VisualElement>("ClientBox");
		clientButton = clientBox.Q<Button>("ClientButton");
		joinCodeBox = clientBox.Q<TextField>("JoinCodeBox");

		hostButton.RegisterCallback<ClickEvent>(evt => connectHost(evt));
		clientButton.RegisterCallback<ClickEvent>(evt => connectClient(evt));

		testRelay = new TestRelay();
	}

	public void show() {
		connectBox.RemoveFromClassList("hidden");
	}

	public void hide() {
		connectBox.AddToClassList("hidden");
	}

	private async void connectHost(ClickEvent evt) {
		var joinCode = await testRelay.createRelay();
		joinCodeBox.SetValueWithoutNotify(joinCode);
	}

	private void connectClient(ClickEvent evt) {
		var joinCode = joinCodeBox.text;
		testRelay.joinRelay(joinCode);
	}
}
