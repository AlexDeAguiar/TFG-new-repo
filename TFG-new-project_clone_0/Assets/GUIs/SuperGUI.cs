using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SuperGUI : MonoBehaviour, Translatable{
    public static SuperGUI _Instance;
    public AudioSource soundPlayer;
    public AudioSource soundPlayerSelect;
    public AudioSource soundPlayerClose;
    protected VisualElement Root;

    public SuperGUI(){}

    public static SuperGUI Instance(){
        if(_Instance == null){ _Instance = new SuperGUI(); }
        return _Instance;
    }

    protected void Init(){
        Root = GetComponent<UIDocument>()?.rootVisualElement;
        Translator.InitDicts();
        Translator.Register(this);
    }

    public void updateTexts(){}

    public void PlaySelectSound(MouseEnterEvent evt){ soundPlayerSelect.Play(); }
}
