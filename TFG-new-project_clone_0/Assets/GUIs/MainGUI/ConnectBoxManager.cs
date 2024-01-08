using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public class ConnectBoxManager : SuperGUI, Translatable {
	private VisualElement connectBox;
	private VisualElement connectGUI;
	private VisualElement infoBox;
	private Button hostButton;
	private Button clientButton;
	private TextField joinCodeBox;
	private Label SliderLabel;
	private Label ConnectHostLabel;
	private Label ConnectClientLabel;
	private SliderInt Slider;

	private TestRelay testRelay;

	private Label joinCode;
	private string joinCodeStr;
	private Label roomSize;
	private string roomSizeStr;

	private DropdownField Dropdown;
	private bool isHost;

	public ConnectBoxManager() {}

	public new void Init(VisualElement Root){
		base.Init();
		this.Root = Root;
		this.connectGUI = Root.Q<VisualElement>("ConnectGUI");
		this.connectBox = Root.Q<VisualElement>("ConnectBox");
		this.infoBox 	= Root.Q<VisualElement>("InfoLabelsBox");
		isHost = false;
		joinCodeStr = "";
		roomSizeStr = "";

		var hostBox = connectBox.Q<VisualElement>("HostBox");
		hostButton = connectBox.Q<Button>("HostButton");
		hostButton.RegisterCallback<ClickEvent>(evt => connectHost(evt));
		hostButton.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
		SliderLabel = hostBox.Q<Label>("SizeSliderLabel");
		Slider = hostBox.Q<SliderInt>("SizeSlider");
		Slider.RegisterValueChangedCallback(x => SliderLabel.text = x.newValue.ToString());
		SliderLabel.text = Slider.value.ToString();

		ConnectHostLabel = connectBox.Q<Label>("ConnectHostLabel");
		ConnectClientLabel = connectBox.Q<Label>("ConnectClientLabel");

		var clientBox = connectBox.Q<VisualElement>("ClientBox");
		clientButton = clientBox.Q<Button>("ClientButton");
		clientButton.RegisterCallback<ClickEvent>(evt => connectClient(evt));
		clientButton.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
		joinCodeBox = clientBox.Q<TextField>("JoinCodeBox");
		joinCodeBox.RegisterValueChangedCallback(x => joinCodeStr = joinCodeBox.value.ToString());

		testRelay = TestRelay.Instance;

        // Crea una nueva lista de opciones
        List<string> options = Translator.getLanguagesList();
		int idx = Translator.getCurrentLanIdx();

        // Actualiza la lista de opciones del Dropdown
		Dropdown = Root.Q<DropdownField>("LanDropdown");
        registerDropdownCallback(Dropdown, idx, options);

		joinCode = this.infoBox.Q<Label>("JoinCode");
		roomSize = this.infoBox.Q<Label>("RoomSize");
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
	}

	public void show() {
		connectGUI.RemoveFromClassList("hidden");
	}

	public void hide() {
		connectGUI.AddToClassList("hidden");
	}

	private async void connectHost(ClickEvent evt) {
		roomSizeStr = SliderLabel.text;
		joinCodeStr = await testRelay.createRelay(int.Parse(SliderLabel.text));
		joinCodeBox.SetValueWithoutNotify(this.joinCodeStr);
		this.hide();

		joinCode.text = Translator._INTL("Join Code") + ": " + joinCodeStr;
		roomSize.text = Translator._INTL("Room Size") + ": " + roomSizeStr;
		isHost = true;
		this.infoBox.RemoveFromClassList("hidden");
	}

	private void connectClient(ClickEvent evt) {
		testRelay.joinRelay(this.joinCodeStr);
		this.hide();

		joinCode.text = Translator._INTL("Join Code") + ": " + this.joinCodeStr;
		roomSize.text = "";
		this.infoBox.RemoveFromClassList("hidden");
	}
}
