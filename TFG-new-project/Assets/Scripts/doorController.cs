using UnityEngine;

public class DoorController : MonoBehaviour{
    private const float OPENANGLE = 90; // El angulo de apertura de la puerta
    public float smoothTime = 2f; // El tiempo que tarda la puerta en abrirse o cerrarse
    private bool open = false;
    private float initialAngle;
    private Quaternion targetRotation;
    public bool isLeftDoor = false;
	
	//icono de interaccion:
	private Renderer interactionIcon;
    private Texture2D texture;
    private Vector3 iconOffset = new Vector3(0, 2.5f, 0); // Offset hacia arriba

    // Start is called before the first frame update
    void Start(){
        initialAngle = transform.rotation.eulerAngles.y;
	}

	public bool isOpen() { return open; }

	public void openDoor() {
		//x, y ,z

		targetRotation = Quaternion.Euler(0, initialAngle + (OPENANGLE * (isLeftDoor ? -1 : 1)), 0);

		open = true;

		GetComponent<Collider>().isTrigger = false;

	}

	public void closeDoor() {
		targetRotation = Quaternion.Euler(0, initialAngle, 0);

		open = false;

		GetComponent<Collider>().isTrigger = true;

	}

	public void toggleDoor() {
		if (isOpen()) {
			closeDoor();
		} else {
			openDoor();
		}
	}

	void Update(){
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }
}
