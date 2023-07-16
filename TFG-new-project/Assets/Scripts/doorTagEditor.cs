using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObject))]
public class doorTagEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.CompareTag("Door"))
        {
            // Verifica si el componente ya existe antes de agregarlo
            if (Selection.activeGameObject.GetComponent<doorController>() == null)
            {
                Selection.activeGameObject.AddComponent<doorController>();
            }

            // Verifica si el collider ya existe antes de agregarlo
            if (Selection.activeGameObject.GetComponent<MeshCollider>() == null)
            {
                Selection.activeGameObject.AddComponent<MeshCollider>();
            }
        }
    }
}
