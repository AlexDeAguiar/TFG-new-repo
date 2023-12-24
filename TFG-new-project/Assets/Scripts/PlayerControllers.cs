using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControllers : MonoBehaviour {

	public static PlayerControllers Instance { get; private set; } = null;
	public bool KeysEnabled = false;

	private List<IController> controllers;

	private void init(GameObject player) {
		controllers = new List<IController>() {
			new PlayerMovementController(this, player),
			new PlayerInteractionController(this, player)
		};

		Instance = this;
		KeysEnabled = true;
	}

	public PlayerControllers(GameObject player) {
		GameObject.Find("Controllers").AddComponent<PlayerControllers>().init(player);
	}

	void Update() {
		foreach (var controller in controllers) {
			controller.updateController();
		}
	}

	public void requestStartCoroutine(IEnumerator enumerator) {
		StartCoroutine(enumerator);	
	}
}