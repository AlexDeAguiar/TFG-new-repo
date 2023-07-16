using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//no funciona

public class TagDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Encuentra todos los objetos en la escena con componente Transform
        GameObject[] objects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject obj in objects)
        {
            // Verifica si el nombre del objeto contiene la palabra "Puerta"
            if (obj.name.Contains("Puerta"))
            {
                // Asigna la etiqueta "Door" al objeto
                obj.tag = "Door";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
