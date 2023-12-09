using UnityEngine;

public class DoorController : MonoBehaviour {
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