using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SuperGUI : MonoBehaviour, Translatable{
    protected AudioSource soundPlayer;
    protected AudioSource soundPlayerSelect;
    protected AudioSource soundPlayerClose;
    protected VisualElement Root;

    protected void Init(){
        soundPlayer       = gameObject.AddComponent<AudioSource>();
        soundPlayerSelect = gameObject.AddComponent<AudioSource>();
        soundPlayerClose  = gameObject.AddComponent<AudioSource>();
        soundPlayer.clip       = Resources.Load<AudioClip>("BW2OpenMenu");
        soundPlayerSelect.clip = Resources.Load<AudioClip>("BW2MenuSelect");
        soundPlayerClose.clip  = Resources.Load<AudioClip>("GUI menu close");
        Root = GetComponent<UIDocument>().rootVisualElement;
        Translator.Register(this);
    }

    public void updateTexts(){}
}
