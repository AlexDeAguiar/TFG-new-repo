using UnityEngine;
using System.Collections;

public class PlayerInteractionController : IController {

	public static PlayerInteractionController Instance { get; private set; } = null;
	private bool InteractionKeysEnabled = false;

	private PlayerControllers playerControllers;
	private GameObject player;

	
	public float interactDistance = 2f; // La distancia a la que el jugador puede interactuar con la puerta

	public PlayerInteractionController(PlayerControllers playerControllers, GameObject player) {
		this.playerControllers = playerControllers;
		this.player = player;

		Instance = this;
		InteractionKeysEnabled = true;
	}

    public void update() {
		InfoBoxManager.Instance.hide();
		if (canInteract()) {
			// Raycast para detectar la puerta cercana
			RaycastHit hit;

			if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, interactDistance)){
				var targetObject = hit.collider.gameObject;
				IInteractable interactable = targetObject.GetComponent<IInteractable>();
				
				if(interactable != null){
					interactable.interact(targetObject);
				}

/*//ALTERNATIVE
				if(hit.collider.tag == "Interactable"){
					targetObject.GetComponent<IInteractable>().handleBehaviour(targetObject);
				}
*/

/*//OLD CODE
				switch (hit.collider.tag) {
					case "Door":
						handleDoorBehaviour(targetObject);
						break;
					case "BlackBoard":
						handleBlackBoardBehaviour(targetObject);
						break;
				}
//*/
			}
		}
	}

	private bool canInteract() {
		return playerControllers.KeysEnabled && InteractionKeysEnabled;
	}
}
