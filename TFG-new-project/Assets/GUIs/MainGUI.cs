using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using TMPro;

public class MainGUI : MonoBehaviour{
    UIDocument myUI;
    VisualElement root;
    VisualElement userPic;
    Label timeLabel;

    void Update(){
        updateTime();
    }

    void updateTime(){
        if (timeLabel != null){
            timeLabel.text = DateTime.Now.ToShortTimeString();
            print(timeLabel.text);
        }
    }

    void onEnable(){
        myUI = GetComponent<UIDocument>();
        root = myUI.rootVisualElement;
        userPic = root.Q<VisualElement>("UserPic");
        userPic.RegisterCallback<ClickEvent>(ChangeBackgroundSprite);

        timeLabel = root
            .Q<VisualElement>("root")
            .Q<VisualElement>("TopBar")
            .Q<VisualElement>("TimeFrame")
            .Q<Label>("timeLabel");
        updateTime();
        print("holu?");
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

    // Método para cargar un sprite desde un archivo dado.
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
}
