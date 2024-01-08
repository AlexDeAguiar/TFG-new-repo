using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using System; 

public class MainGUI : SuperGUI {
	public static new MainGUI Instance; //Note: Using keyword "new" because SuperGUI is also a singleton

    UIDocument myUI;
    VisualElement userPic;
    Label timeLabel;

    Boolean showRight;
    Boolean showLeft;
    VisualElement rightTab;
    VisualElement leftTab;
    Label rightTabLabel;
    Label leftTabLabel;

	VideoSelectorBoxManager videoSelectorBoxManager;
	InfoBoxManager infoBoxManager;
	public ConnectBoxManager connectBoxManager;

	void Start(){
        base.Init();

		Instance = this;

		showRight = false;
        showLeft  = false;

        userPic = Root.Q<VisualElement>("UserPic");
        userPic.RegisterCallback<ClickEvent>(evt => ChangeBackgroundSprite(evt));

        rightTab = Root.Q<VisualElement>("RightTab");
        rightTab.Q<VisualElement>("RightArrowBtn").RegisterCallback<ClickEvent>(toggleRightTab);
        rightTabLabel = rightTab.Q<VisualElement>("RightArrowBtn").Q<Label>();

        leftTab = Root.Q<VisualElement>("LeftTab");
        leftTab.Q<VisualElement>("LeftArrowBtn").RegisterCallback<ClickEvent>(toggleLeftTab);
        leftTabLabel = leftTab.Q<VisualElement>("LeftArrowBtn").Q<Label>();

		VisualElement videoSelectorBox = Root.Q<VisualElement>("FileSelector");
		videoSelectorBoxManager = new VideoSelectorBoxManager(videoSelectorBox);

		VisualElement infoBox = Root.Q<VisualElement>("InfoBox");
		infoBoxManager = new InfoBoxManager(infoBox);

		connectBoxManager.Init(Root);

		timeLabel = Root
            .Q<VisualElement>("TopBar")
            .Q<VisualElement>("TimeFrame")
            .Q<Label>("timeLabel");
        
		/*	
		updateTime();

        updateUserData();
        if(User.Instance.userType == 0){
            NetworkManager.Singleton.StartClient();
        }
        else{
            NetworkManager.Singleton.StartHost();
        }
		*/
        Translator.changeLan(Translator.LAN);
	}

    void Update(){
        updateTime();
    }

    void updateTime(){
        if (timeLabel != null){
            timeLabel.text = DateTime.Now.ToShortTimeString();
        }
    }

    void updateUserData(){
        User myUser = User.Instance;
        Root.Q<Label>("Username").text = myUser.username;
        NetworkManager.Singleton.StartHost();
    }

    void toggleRightTab(ClickEvent evt){        
        showRight = !showRight;
        if(showRight){
            rightTab.AddToClassList("showRightContainer");
            rightTab.RemoveFromClassList("hideRightContainer");
            rightTabLabel.AddToClassList("showRightLabel");
            rightTabLabel.RemoveFromClassList("hideRightLabel");
        }
        else{
            rightTab.AddToClassList("hideRightContainer");
            rightTab.RemoveFromClassList("showRightContainer");
            rightTabLabel.AddToClassList("hideRightLabel");
            rightTabLabel.RemoveFromClassList("showRightLabel");
        }
    }

    void toggleLeftTab(ClickEvent evt){
        showLeft = !showLeft;
        if(showLeft){
            leftTab.AddToClassList("showLeftContainer");
            leftTab.RemoveFromClassList("hideLeftContainer");
            leftTabLabel.AddToClassList("showLeftLabel");
            leftTabLabel.RemoveFromClassList("hideLeftLabel");
        }
        else{
            leftTab.AddToClassList("hideLeftContainer");
            leftTab.RemoveFromClassList("showLeftContainer");
            leftTabLabel.AddToClassList("hideLeftLabel");
            leftTabLabel.RemoveFromClassList("showLeftLabel");
        }
    }


    // Llamamos a este método para cambiar el fondo del UI Element.
    public void ChangeBackgroundSprite(ClickEvent evt){
        //TO DO:
/*
        // Abrir la ventana emergente del explorador de archivos y obtener la ruta del archivo seleccionado.
        string path = EditorUtility.OpenFilePanel("Seleccionar imagen", "", "png,jpg,jpeg");

        // Verificar si se ha seleccionado un archivo.
        if (!string.IsNullOrEmpty(path)){
            // Cargar el sprite desde la ruta del archivo.
            Sprite newSprite = LoadSpriteFromFile(path);

            // Verificar si se pudo cargar el sprite correctamente.
            if (newSprite != null){
                // Asignar el nuevo sprite al componente Image.
                userPic.GetComponent<Image>().sprite = newSprite;
            }
            else{
                Debug.LogWarning("No se pudo cargar el sprite desde el archivo seleccionado.");
            }
        }
*/
    }

    // Método para cargar un sprite desde un archivo dado. no se usa ahora mismo
    private Sprite LoadSpriteFromFile(string path){
        // Cargar los bytes del archivo seleccionado.
        byte[] bytes = System.IO.File.ReadAllBytes(path);

        // Crear una nueva textura y cargar los bytes en ella.
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes)){
            // Crear un nuevo sprite utilizando la textura cargada.
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        return null;
    }

	public VideoSelectorBoxManager getVideoSelectorBoxManager() {
		return videoSelectorBoxManager;
	}

	public InfoBoxManager getInfoBoxManager() {
		return infoBoxManager;
	}
}
