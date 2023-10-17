using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_TPSController : MonoBehaviour{
	private GameObject player;
	private GameObject head;
	//private GameObject body;
	//private GameObject faceCam;

    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public GameObject playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

	public void enableMove() {
		canMove = true;
	}

	public void disableMove()
	{
		canMove = false;
	}

	void Start() {
		player = gameObject;
		head = Utility.FindChildByTag(player, "Head");
		//body = Utility.FindChildByTag(player, "Body");


		characterController = GetComponent<CharacterController>();
		playerCamera = GameObject.Find("MainCamera");

		playerCamera.transform.SetPositionAndRotation(head.transform.position, head.transform.rotation);

		rotation.y = transform.eulerAngles.y;
    }

    void Update() {
        if (characterController.isGrounded) {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove){
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove) {
			float currRotationX = head.transform.rotation.eulerAngles.x; // De 0 a 360 grados (90 es abajo, 270 es arriba)
            float rotationChangeY = Input.GetAxis("Mouse X") * lookSpeed;
			float rotationChangeX = -Input.GetAxis("Mouse Y") * lookSpeed;
			if (currRotationX <= 180 )
			{ //Looking down
				float maxDownChange = lookXLimit - currRotationX;
				if (rotationChangeX > maxDownChange) { currRotationX = maxDownChange; }
			}
			else
			{
				float maxUpChange = -(lookXLimit - currRotationX);
				if (rotationChangeX < maxUpChange) { currRotationX = 360 + maxUpChange; }
			}

			player.transform.Rotate(new Vector3(0, rotationChangeY, 0));
			head.transform.Rotate(new Vector3(rotationChangeX, 0, 0));
		}

		playerCamera.transform.SetPositionAndRotation(head.transform.position, head.transform.rotation);
	}
}