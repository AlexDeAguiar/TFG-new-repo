using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour{
    VisualElement mainOptions;
    VisualElement SignIn;
    VisualElement SignUp;
    VisualElement Settings;
    VisualElement ErrorWindow;

    TextField UsernameInput;
    TextField PasswordInput;

    TextField UsernameInput2;
    TextField PasswordInput2;
    TextField PasswordInput2_confirm;
    RadioButtonGroup UserType;

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
        Translator.InitDicts();

        loadSignIn = false;
        loadSignUp = false;

        root = GetComponent<UIDocument>().rootVisualElement;
        
        //LOGIN =================================================================
        SignIn = root.Q<VisualElement>("SignIn");
        SignIn.Q<Button>("LoginBtn").text = Translator._INTL("Login");
        UsernameInput = SignIn.Q<TextField>("LoginUsername");
        UsernameInput.label = Translator._INTL("Username");
        
        PasswordInput = SignIn.Q<TextField>("LoginPassword");
        PasswordInput.label = Translator._INTL("Password");

        PasswordInput = SignIn.Q<TextField>("LoginPassword");

        //SIGNUP ================================================================
        SignUp = root.Q<VisualElement>("SignUp");
        SignUp.Q<Button>("SignUpBtn").text = Translator._INTL("Sign Up");

        UsernameInput2 = SignUp.Q<TextField>("SignUpUsername");
        UsernameInput2.label = Translator._INTL("Username");

        PasswordInput2 = SignUp.Q<TextField>("SignUpPassword");
        PasswordInput2.label = Translator._INTL("Password");

        PasswordInput2_confirm = SignUp.Q<TextField>("SignUpPassword2");
        PasswordInput2_confirm.label = Translator._INTL("Repeat Password");

        UserType = SignUp.Q<RadioButtonGroup>("UserType");
        List<string> aux = (List<string>) UserType.choices;
        aux[0] = Translator._INTL("Student");
        aux[1] = Translator._INTL("Professor");


        //ERROR WINDOW ==========================================================
        ErrorWindow = root.Q<VisualElement>("ErrorWindow");
        ErrorWindow.Q<VisualElement>("ErrorContainer")
                   .Q<VisualElement>("ErrorBtn")
                   .RegisterCallback<ClickEvent>(evt => CloseErrorWindow(evt));

        //MAIN BUTTONS ==========================================================
        mainOptions = root.Q<VisualElement>("mainOptions");
        mainOptions.Q<Button>("SignIn_Btn").text = Translator._INTL("Login");
        mainOptions.Q<Button>("SignUp_Btn").text = Translator._INTL("Sign Up");
        mainOptions.Q<Button>("Settings_Btn").text = Translator._INTL("Settings");

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
        signInBackBtn.RegisterCallback<ClickEvent>(evt => CloseTab(evt));
        SignIn.Q<Button>("LoginBtn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));

        Button signInBtn = SignIn.Q<Button>("LoginBtn");
        signInBtn.RegisterCallback<ClickEvent>(evt => Login(evt));
    }

    void setUp2(){
        //clicking events for going back to the mainOptions tab:
        Button signUpBackBtn = SignUp.Q<Button>("SignUpBack");
        signUpBackBtn.RegisterCallback<ClickEvent>(evt => CloseTab(evt));
        SignUp.Q<Button>("SignUpBtn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        
        Button signUpBtn = SignUp.Q<Button>("SignUpBtn");
        signUpBtn.RegisterCallback<ClickEvent>(evt => Sign_Up(evt));
    }

    void Login(ClickEvent evt){
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Username",UsernameInput.text);
        data.Add("Password",PasswordInput.text);
        data.Add("UserType","0");
        StartCoroutine(Main.Instance.Web.UserAction(data, Web.LOGIN));
    }

    void Sign_Up(ClickEvent evt){
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Username",UsernameInput2.text);
        data.Add("Password",PasswordInput2.text);
        data.Add("UserType",UserType.value.ToString());

        if(UsernameInput2.text != ""){
            if(PasswordInput2.text != ""){
                if(PasswordInput2.text == PasswordInput2_confirm.text){
                    if(UserType.value != -1){
                        StartCoroutine(Main.Instance.Web.UserAction(data, Web.SIGNUP));
                    }
                    else{ showErrorWindow(Translator._INTL("Please select a user Type")); }
                }
                else{ showErrorWindow(Translator._INTL("Passwords do not match")); }
            }
            else{ showErrorWindow(Translator._INTL("Provide a Password")); }
        }
        else{ showErrorWindow(Translator._INTL("Provide a username")); }


    }

    public void showErrorWindow(string text){
        VisualElement ErrorWindow = root.Q<VisualElement>("ErrorWindow");
        ErrorWindow.Q<Label>("ErrorLabel").text = text;
        ErrorWindow.RemoveFromClassList("hidden");
        Debug.Log(text);
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
        UsernameInput.value = "";
        PasswordInput.value = "";
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
        UsernameInput2.value = "";
        PasswordInput2.value = "";
    }

    void CloseTab(ClickEvent evt){
        curTab.AddToClassList("hidden");
        soundPlayerClose.Play();
        mainOptions.RemoveFromClassList("hidden");
    }

    void CloseErrorWindow(ClickEvent evt){
        ErrorWindow.AddToClassList("hidden");
        soundPlayerClose.Play();
    }

    void OpenSettings(ClickEvent evt){
        Debug.Log("Settings");
    }

    void PlaySelectSound(MouseEnterEvent evt){
        soundPlayer.Play();
    }

    public VisualElement getRoot(){ return root; }
    public VisualElement getCurTab(){ return curTab; }
}

//id, username, pwd, type, userPic
