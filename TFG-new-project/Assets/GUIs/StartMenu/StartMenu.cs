using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenu : SuperGUI, Translatable {
    VisualElement MainOptions;
    VisualElement SignIn;
    VisualElement SignUp;
    VisualElement Settings;
    VisualElement ErrorWindow;

    TextField UsernameInput;
    TextField PasswordInput;
    TextField UsernameInput2;
    TextField PasswordInput2;
    TextField PasswordInput2_confirm;

    Button SignUpBtn;
    Button SignUpBackBtn;
    Button SignInBtn;
    Button SignInBackBtn;
    RadioButtonGroup UserType;

    VisualElement CurTab;

    // Start is called before the first frame update
    void Start(){
        base.Init();
        
        //LOGIN =================================================================
        SignIn = Root.Q<VisualElement>("SignIn");

        UsernameInput = SignIn.Q<TextField>("LoginUsername");
        PasswordInput = SignIn.Q<TextField>("LoginPassword");

        //clicking events for going back to the MainOptions tab:
        SignInBackBtn = SignIn.Q<Button>("LoginBack");
        SignInBtn     = SignIn.Q<Button>("LoginBtn");

        SignInBtn.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        SignInBtn.RegisterCallback<ClickEvent>(evt => Login(evt));
        SignInBackBtn.RegisterCallback<ClickEvent>(evt => CloseTab(evt));

        //SIGNUP ================================================================
        SignUp = Root.Q<VisualElement>("SignUp");
        UsernameInput2 = SignUp.Q<TextField>("SignUpUsername");
        PasswordInput2 = SignUp.Q<TextField>("SignUpPassword");
        PasswordInput2_confirm = SignUp.Q<TextField>("SignUpPassword2");
        UserType = SignUp.Q<RadioButtonGroup>("UserType");

        //clicking events for going back to the MainOptions tab:
        SignUpBtn     = SignUp.Q<Button>("SignUpBtn");
        SignUpBackBtn = SignUp.Q<Button>("SignUpBack");

        SignUpBtn.RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        SignUpBtn.RegisterCallback<ClickEvent>(evt => Sign_Up(evt));
        SignUpBackBtn.RegisterCallback<ClickEvent>(evt => CloseTab(evt));

        //SETTINGS ==============================================================
        Settings = Root.Q<VisualElement>("Settings");
        Settings.Q<Button>("SettingsBack").RegisterCallback<ClickEvent>(evt => CloseTab(evt));

        //ERROR WINDOW ==========================================================
        ErrorWindow = Root.Q<VisualElement>("ErrorWindow");

        //MAIN BUTTONS ==========================================================
        MainOptions = Root.Q<VisualElement>("mainOptions");

        //play sound callback:
        MainOptions.Q<Button>("SignIn_Btn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        MainOptions.Q<Button>("SignUp_Btn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));
        MainOptions.Q<Button>("Settings_Btn").RegisterCallback<MouseEnterEvent>(evt => PlaySelectSound(evt));

        //clicking events of MainOptions tab:
        MainOptions.Q<Button>("SignIn_Btn").RegisterCallback<ClickEvent>(evt => OpenSignIn(evt));
        MainOptions.Q<Button>("SignUp_Btn").RegisterCallback<ClickEvent>(evt => OpenSignUp(evt));
        MainOptions.Q<Button>("Settings_Btn").RegisterCallback<ClickEvent>(evt => OpenSettings(evt));
    }

    void OpenSignIn(ClickEvent evt){
        MainOptions.AddToClassList("hidden");
        soundPlayerSelect.Play();
        SignIn.RemoveFromClassList("hidden");
        CurTab = SignIn;
        UsernameInput.value = "";
        PasswordInput.value = "";
    }

    void OpenSignUp(ClickEvent evt){
        MainOptions.AddToClassList("hidden");
        soundPlayerSelect.Play();
        SignUp.RemoveFromClassList("hidden");
        CurTab = SignUp;
        UsernameInput2.value = "";
        PasswordInput2.value = "";
    }

    void OpenSettings(ClickEvent evt){
        MainOptions.AddToClassList("hidden");
        soundPlayerSelect.Play();
        Settings.RemoveFromClassList("hidden");
        CurTab = Settings;
        //Translator.changeLan(LANGUAGES.TM);
    }

    public void showErrorWindow(string text){
        ErrorWindow.Q<Label>("ErrorLabel").text = Translator._INTL(text);
        ErrorWindow.RemoveFromClassList("hidden");
        Debug.Log(text);
    }

    void CloseTab(ClickEvent evt){
        CurTab.AddToClassList("hidden");
        soundPlayerClose.Play();
        MainOptions.RemoveFromClassList("hidden");
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
                    else{ showErrorWindow("Please select a user Type"); }
                }
                else{ showErrorWindow("Passwords do not match"); }
            }
            else{ showErrorWindow("Provide a Password"); }
        }
        else{ showErrorWindow("Provide a username"); }
    }

    public new void updateTexts(){
        SignIn.Q<Button>("LoginBtn").text = Translator._INTL("Login");
        SignUpBtn.text = Translator._INTL("Sign Up");

        UsernameInput.label = Translator._INTL("Username");
        PasswordInput.label = Translator._INTL("Password");

        UsernameInput2.label         = Translator._INTL("Username");
        PasswordInput2.label         = Translator._INTL("Password");
        PasswordInput2_confirm.label = Translator._INTL("Repeat Password");

        List<RadioButton> a = UserType.Query<RadioButton>().ToList();
        a[0].text = Translator._INTL("Student");
        a[1].text = Translator._INTL("Professor");

        MainOptions.Q<Button>("SignIn_Btn").text   = Translator._INTL("Login");
        MainOptions.Q<Button>("SignUp_Btn").text   = Translator._INTL("Sign Up");
        MainOptions.Q<Button>("Settings_Btn").text = Translator._INTL("Settings");
    }

    void PlaySelectSound(MouseEnterEvent evt){ soundPlayer.Play(); }
    public VisualElement getRoot(){ return Root; }
    public VisualElement getCurTab(){ return CurTab; }
}

//id, username, pwd, type, userPic
