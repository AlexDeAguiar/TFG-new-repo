using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public float openAngle = 90f; // El angulo de apertura de la puerta
    public float closeAngle = 0f; // El angulo de cierre de la puerta
    public float smoothTime = 2f; // El tiempo que tarda la puerta en abrirse o cerrarse
    private bool open = false;
    private Quaternion targetRotation;

    //icono de interaccion:
    private Renderer interactionIcon;
    private Texture2D texture;
    private Vector3 iconOffset = new Vector3(0, 2.5f, 0); // Offset hacia arriba

    public GameObject myPlayer;
    Collider playerCollider;
    Collider doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = myPlayer.GetComponent<Collider>();
        doorCollider = GetComponent<Collider>();
        texture = Resources.Load<Texture2D>("Graphics/iconInteract");
        targetRotation = transform.rotation;

        Material iconMat = new Material(Shader.Find("Standard"));
        iconMat.mainTexture = texture;
        interactionIcon.transform.position = transform.position + iconOffset;
        interactionIcon.GetComponentInChildren<MeshRenderer>().material = iconMat;
    }


    void OnCollisionEnter(Collision collision)
    {
        bool cond = collision.collider.CompareTag("Player");
        //interactionIcon.SetActive(cond);
        if (cond) { Physics.IgnoreCollision(collision.collider, GetComponent<Collider>()); }
    }

    public bool isOpen() { return open; }

    public void openDoor()
    {
        targetRotation = Quaternion.Euler(0, openAngle, 0);
        open = true;
    }

    public void closeDoor()
    {
        targetRotation = Quaternion.Euler(0, closeAngle, 0);
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }
}
