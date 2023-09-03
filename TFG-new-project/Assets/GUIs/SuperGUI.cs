using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SuperGUI : MonoBehaviour, Translatable{
    public static SuperGUI Instance;
    public AudioSource soundPlayer;
    public AudioSource soundPlayerSelect;
    public AudioSource soundPlayerClose;
    protected VisualElement Root;

    public SuperGUI(){ Instance = this; }

    protected void Init(){
        soundPlayer       = SuperGUI.Instance.soundPlayer;
        soundPlayerSelect = SuperGUI.Instance.soundPlayerSelect;
        soundPlayerClose  = SuperGUI.Instance.soundPlayerClose;
        Root = GetComponent<UIDocument>().rootVisualElement;
        Translator.InitDicts();
        Translator.Register(this);
    }

    public void updateTexts(){}

    public void PlaySelectSound(MouseEnterEvent evt){ soundPlayer.Play(); }
}
