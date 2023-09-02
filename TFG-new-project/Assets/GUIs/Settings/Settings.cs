using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Settings : SuperGUI, Translatable {
    DropdownField Dropdown;

    // Start is called before the first frame update
    void Start(){
        base.Init();

        // Obtén una referencia al componente Dropdown
        Dropdown = Root.Q<DropdownField>();

        // Crea una nueva lista de opciones
        List<string> nuevasOpciones = new List<string>();
        foreach (LANGUAGES lan in Enum.GetValues(typeof(LANGUAGES))){ nuevasOpciones.Add(Translator.LANGUAGE_NAMES[lan]); }
        // Agrega más opciones según sea necesario

        // Actualiza la lista de opciones del Dropdown
        Dropdown.choices = nuevasOpciones;
        Dropdown.index = 0;
        Dropdown.RegisterValueChangedCallback(evt => OnLanguageChanged(evt));
    }

    public new void updateTexts(){
        Dropdown.label = Translator._INTL("Language");
    }

    public void OnLanguageChanged(ChangeEvent<string> evt){
        Translator.changeLan((LANGUAGES) Dropdown.index);
    }
}

//id, username, pwd, type, userPic
