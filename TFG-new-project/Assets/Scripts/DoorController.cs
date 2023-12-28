using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable {
	public const KeyCode doorInteractKey = KeyCode.E; // La tecla que el jugador debe presionar para interactuar con la puerta
	public float angleDiff = 90; // El angulo de apertura de la puerta
	public bool isLeftDoor = false;
	public bool isOpen = false;

	private float openAngle;
	private float closeAngle;

	private float animationTime = 1f; // El tiempo que tarda la puerta en abrirse o cerrarse
	private float remainingAnimationTime = 0f;
	private Quaternion targetRotation;

	// Start is called before the first frame update
	void Start() {
		setOpen(isOpen); //Necessary
		if (isOpen) {
			openAngle = transform.rotation.eulerAngles.y;
			closeAngle = openAngle + angleDiff * (isLeftDoor ? -1 : 1);
		} else {
			closeAngle = transform.rotation.eulerAngles.y;
			openAngle = closeAngle + angleDiff * (isLeftDoor ? 1 : -1);
		}
	}

	//TODO: Mover este texto tambien a la puerta, por si queremos que mande distintos mensajes si esta abierto o cerrado, o para que cambie el mensaje dependiendo del idioma
	private const string doorInteractionTextKey = "INTERACTION_INFO_DOOR";
	public void handleBehaviour(GameObject targetObject) {
		//Mostrar el mensaje informativo siempre
		InfoBoxManager.Instance.showText(doorInteractionTextKey);

		//Detectar teclas y llamar a los metodos apropiados
		if (Input.GetKeyDown(doorInteractKey)){
			//TODO: Mover esta logica a la puerta, no tiene por que estar en el controller
			//Ignore collisions with the door
			//OnTriggerEnter(GetComponent<Collider>(), door);

			string doorPath = Main.GetFullPath(targetObject.transform);
			PlayerNetworkMessages.MyInstance.toggleDoorServerRpc(doorPath);
		}
	}

	private void setOpen(bool open) {
		this.isOpen = open;
		GetComponent<Collider>().isTrigger = open;
	}

	public void openDoor() {
		targetRotation = Quaternion.Euler(0, openAngle, 0);
		remainingAnimationTime = animationTime;

		setOpen(true);
	}

	public void closeDoor() {
		targetRotation = Quaternion.Euler(0, closeAngle, 0);
		remainingAnimationTime = animationTime;

		setOpen(false);
	}

	public void toggleDoor() {
		if (isOpen) {
			closeDoor();
		} else {
			openDoor();
		}
	}

	void Update() {
		if (remainingAnimationTime > 0f) {
			float animationDiff = Mathf.Min(Time.deltaTime, remainingAnimationTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, animationDiff / remainingAnimationTime);
			remainingAnimationTime -= animationDiff;
		}
	}
}