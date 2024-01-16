using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public class ConnectBoxManager : SuperGUI, ITranslatable {
	private VisualElement connectBox;
	private VisualElement hostBox;
	private VisualElement clientBox;
	private VisualElement usernameBox;
	private VisualElement connectGUI;
	private VisualElement infoBox;
    private VisualElement timeFrame;
	private Button hostButton;
	private Button clientButton;
	private Button acceptButton;
	private TextField joinCodeBox;
	private TextField usernameBoxField;
	private Label SliderLabel;
	private Label ConnectHostLabel;
	private Label ConnectClientLabel;
	private SliderInt Slider;

	private TestRelay testRelay;

	private Label joinCode;
	public string joinCodeStr { get; private set; } = "";
	private Label roomSize;
	private string roomSizeStr;

	private DropdownField Dropdown;
	private bool isHost;
	private Label connectingLabel;
	private Label UsernameLabel;
	private Label errorLabel;
	private Label usernameErrorLabel;

	public static ConnectBoxManager Instance;
	public static string usernameStr;

	public ConnectBoxManager() {}

	public new void Init(VisualElement Root){
		base.Init();
		Instance = this;
		this.Root = Root;
		this.connectGUI = Root.Q<VisualElement>("ConnectGUI");
		this.connectBox = Root.Q<VisualElement>("ConnectBox");
		this.infoBox 	= Root.Q<VisualElement>("InfoLabelsBox");
		this.connectingLabel = Root.Q<Label>("ConnectingLabel");
		this.errorLabel = Root.Q<Label>("ErrorLabel");
		isHost = false;
		joinCodeStr = "";
		roomSizeStr = "";

		hostBox = connectBox.Q<VisualElement>("HostBox");
		hostButton = connectBox.Q<Button>("HostButton");
		hostButton.RegisterCallback<ClickEvent>(evt => connectHost(evt));
		hostButton.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
		SliderLabel = hostBox.Q<Label>("SizeSliderLabel");
		Slider = hostBox.Q<SliderInt>("SizeSlider");
		Slider.RegisterValueChangedCallback(x => SliderLabel.text = x.newValue.ToString());
		SliderLabel.text = Slider.value.ToString();

		ConnectHostLabel = connectBox.Q<Label>("ConnectHostLabel");
		ConnectClientLabel = connectBox.Q<Label>("ConnectClientLabel");
		UsernameLabel = connectBox.Q<Label>("UsernameLabel");

		clientBox = connectBox.Q<VisualElement>("ClientBox");
		clientButton = clientBox.Q<Button>("ClientButton");
		clientButton.RegisterCallback<ClickEvent>(evt => connectClient(evt));
		clientButton.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
		joinCodeBox = clientBox.Q<TextField>("JoinCodeBox");
		joinCodeBox.RegisterValueChangedCallback(x => joinCodeStr = joinCodeBox.value.ToString());

		usernameBox = connectBox.Q<VisualElement>("UsernameBox");
		acceptButton = usernameBox.Q<Button>("AcceptButton");
		acceptButton.RegisterCallback<ClickEvent>(evt => showConnectMenu());
		acceptButton.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
		usernameBoxField = usernameBox.Q<TextField>("UsernameBoxField");
		usernameBoxField.RegisterValueChangedCallback(x => { ConnectBoxManager.usernameStr = usernameBoxField.value.ToString(); });
		usernameErrorLabel = Root.Q<Label>("UsernameErrorLabel");

		testRelay = TestRelay.Instance;

        // Crea una nueva lista de opciones
        List<string> options = Translator.getLanguagesList();
		int idx = Translator.getCurrentLanIdx();

        // Actualiza la lista de opciones del Dropdown
		Dropdown = Root.Q<DropdownField>("LanDropdown");
        registerDropdownCallback(Dropdown, idx, options);

		joinCode = this.infoBox.Q<Label>("JoinCode");
		roomSize = this.infoBox.Q<Label>("RoomSize");

        timeFrame = Root
            .Q<VisualElement>("TimeFrame");
	}

	private void registerDropdownCallback(DropdownField d, int idx, List<string> options){
        d.choices = options;
        d.index = idx;
        d.RegisterValueChangedCallback(evt => OnLanguageChanged(evt, d));
	}

    public void OnLanguageChanged(ChangeEvent<string> evt, DropdownField d){
        Translator.changeLan((LANGUAGES) d.index);
    }

	public new void updateTexts(){
		Debug.Log(joinCodeStr);
		ConnectHostLabel.text   = Translator._INTL("Connect As Host");
		ConnectClientLabel.text = Translator._INTL("Connect As Client");

		hostButton.text   = Translator._INTL("Connect As Host");
		clientButton.text = Translator._INTL("Connect As Client");

		joinCodeBox.label = Translator._INTL("Join Code");
		Slider.label 	  = Translator._INTL("Room Size");

		Dropdown.label = Translator._INTL("Language");

		joinCode.text = Translator._INTL("Join Code") + ": " + joinCodeStr;
		roomSize.text = isHost ? (Translator._INTL("Room Size") + ": " + roomSizeStr) : "";
		
		connectingLabel.text = Translator._INTL("CONNECTING...");

		usernameBoxField.label = Translator._INTL("Username");
		acceptButton.text = Translator._INTL("Accept");
		UsernameLabel.text = Translator._INTL("Enter username");
		usernameErrorLabel.text = Translator._INTL("You need to provide a Username");
	}

	public void show() {
		connectGUI.RemoveFromClassList("hidden");
	}

	public void hide() {
		connectGUI.AddToClassList("hidden");
	}

	private void showConnectMenu(){
		if(ConnectBoxManager.usernameStr != ""){
			usernameBox.AddToClassList("hidden");
			hostBox.RemoveFromClassList("hidden");
			clientBox.RemoveFromClassList("hidden");
		}
		else{
			this.usernameErrorLabel.text = Translator._INTL("You need to provide a Username");
			this.usernameErrorLabel.RemoveFromClassList("NoOpacity");
		}
	}

	private async void connectHost(ClickEvent evt) {
		connectingLabel.RemoveFromClassList("hidden");
		connectBox.AddToClassList("hidden");

		roomSizeStr = SliderLabel.text;
		joinCodeStr = await testRelay.createRelay(int.Parse(SliderLabel.text));
		if(joinCodeStr != null){
			joinCodeBox.SetValueWithoutNotify(this.joinCodeStr);
			this.hide();

			joinCode.text = Translator._INTL("Join Code") + ": " + joinCodeStr;
			roomSize.text = Translator._INTL("Room Size") + ": " + roomSizeStr;
			isHost = true;
			this.infoBox.RemoveFromClassList("hidden");
			this.timeFrame.RemoveFromClassList("hidden");
		}
		else{
			connectBox.RemoveFromClassList("hidden");
			this.errorLabel.text = Translator._INTL("Connection Refused");
			this.errorLabel.RemoveFromClassList("NoOpacity");
		}

		this.connectingLabel.AddToClassList("hidden");
	}

	private async void connectClient(ClickEvent evt) {
		if(this.joinCodeStr != ""){
			connectingLabel.RemoveFromClassList("hidden");
			connectBox.AddToClassList("hidden");
			int code = await testRelay.joinRelay(this.joinCodeStr);
			if(code == 0){
				this.hide();

				joinCode.text = Translator._INTL("Join Code") + ": " + this.joinCodeStr;
				roomSize.text = "";
				this.infoBox.RemoveFromClassList("hidden");
				this.timeFrame.RemoveFromClassList("hidden");
			}
			else{
				connectBox.RemoveFromClassList("hidden");
				this.errorLabel.text = Translator._INTL("Connection Refused");
				this.errorLabel.RemoveFromClassList("NoOpacity");
			}

			this.connectingLabel.AddToClassList("hidden");
		}
		else{
			this.errorLabel.text = Translator._INTL("You need to provide a Join Code");
			this.errorLabel.RemoveFromClassList("NoOpacity");
		}
	}
}
