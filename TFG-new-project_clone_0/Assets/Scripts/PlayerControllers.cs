using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;

public class PlayerControllers : MonoBehaviour {

	public static PlayerControllers Instance;
	public static bool MyKeysEnabled;

	public void Start() {
		Instance = this;
		MyKeysEnabled = true;
		gameObject.AddComponent<PlayerMovementController>();
		gameObject.AddComponent<PlayerInteractionController>();
	}
}