using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ErrorWindow : SuperGUI {
    VisualElement ErrWindow;
    Button ErrorBtn;

    // Start is called before the first frame update
    void Start(){
        base.Init();
        Translator.Register(this);

        //ERROR WINDOW ==========================================================
        ErrWindow = Root.Q<VisualElement>("ErrorWindow");
        ErrorBtn  = ErrWindow.Q<VisualElement>("ErrorContainer").Q<Button>("ErrorBtn");
        ErrorBtn.RegisterCallback<ClickEvent>(evt => CloseErrorWindow(evt));
    }

    public new void updateTexts(){
        ErrorBtn.text = Translator._INTL("Accept");
    }

    void CloseErrorWindow(ClickEvent evt){
        ErrWindow.AddToClassList("hidden");
        soundPlayerClose.Play();
    }
}

//id, username, pwd, type, userPic
