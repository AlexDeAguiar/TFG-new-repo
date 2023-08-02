using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour{
    private VisualElement mainOptions;
    private VisualElement SignIn;
    private VisualElement SignUp;
    private VisualElement Settings;

    public const int MAINOPTIONS = 0;
    public const int SIGNIN      = 1;
    public const int SIGNUP      = 2;
    public const int SETTINGS    = 3;

    VisualElement root;
    VisualElement curTab;

    bool loadSignIn;
    bool loadSignUp;

    public AudioSource soundPlayer;
    public AudioSource soundPlayerSelect;
    public AudioSource soundPlayerClose;

    // Start is called before the first frame update
    void Start(){
        loadSignIn = false;
        loadSignUp = false;

        root = GetComponent<UIDocument>().rootVisualElement;
        mainOptions = root.Q<VisualElement>("mainOptions");
        SignIn = root.Q<VisualElement>("SignIn");
        SignUp = root.Q<VisualElement>("SignUp");
        

        //play sound callback:
        mainOptions.Q<Button>("SignIn_Btn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        mainOptions.Q<Button>("SignUp_Btn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        mainOptions.Q<Button>("Settings_Btn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));

        //clicking events of mainOptions tab:
        mainOptions.Q<Button>("SignIn_Btn").RegisterCallback<ClickEvent>(evt => OpenSignIn(evt));
        mainOptions.Q<Button>("SignUp_Btn").RegisterCallback<ClickEvent>(evt => OpenSignUp(evt));
        mainOptions.Q<Button>("Settings_Btn").RegisterCallback<ClickEvent>(evt => OpenSettings(evt));

        setUp1();
        setUp2();
    }

    void setUp1(){
        //clicking events for going back to the mainOptions tab:
        Button signInBackBtn = SignIn.Q<Button>("LoginBack");
        Debug.Log(signInBackBtn);
        signInBackBtn.RegisterCallback<ClickEvent>(evt => CloseTab(evt));
        SignIn.Q<Button>("LoginBtn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
    }

    void setUp2(){
        //clicking events for going back to the mainOptions tab:
        Button signUpBackBtn = SignUp.Q<Button>("SignUpBack");
        signUpBackBtn.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        signUpBackBtn.RegisterCallback<ClickEvent>(evt => CloseTab(evt));
        SignUp.Q<Button>("SignUpBtn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
    }

    void OpenSignIn(ClickEvent evt){
        if(!loadSignIn){
            setUp1();
            loadSignIn = true;
        }

        mainOptions.AddToClassList("hidden");
        soundPlayerSelect.Play();
        SignIn.RemoveFromClassList("hidden");
        curTab = SignIn;
    }

    void OpenSignUp(ClickEvent evt){
        if(!loadSignUp){
            setUp2();
            loadSignUp = true;
        }

        mainOptions.AddToClassList("hidden");
        soundPlayerSelect.Play();
        SignUp.RemoveFromClassList("hidden");
        curTab = SignUp;
    }

    void CloseTab(ClickEvent evt){
        curTab.AddToClassList("hidden");
        soundPlayerClose.Play();
        mainOptions.RemoveFromClassList("hidden");
    }

    void OpenSettings(ClickEvent evt){
        Debug.Log("Settings");
    }

    void PlaySelectSound(MouseEnterEvent evt){
        soundPlayer.Play();
    }
}
