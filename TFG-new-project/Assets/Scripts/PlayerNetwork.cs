using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour {

	public static PlayerNetwork MyInstance;

	public override void OnNetworkSpawn() {
		var playerId = transform.GetComponent<NetworkObject>().NetworkObjectId;
		transform.name = generateDefaultPlayerName(playerId);

		if (IsOwner) {
			MyInstance = this;
			PlayerControllers.createController(gameObject);

			VivoxService.Instance.Initialize();
			VivoxPlayer.Instance.SignIntoVivox();
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void toggleDoorServerRpc(string doorName) {
		GameObject door = GameObject.Find(doorName);
		DoorInteraction doorController = door.GetComponent<DoorInteraction>();
		doorController.toggleDoor();
	}

	private string generateDefaultPlayerName(ulong myID) {
		if (IsOwnedByServer) {
			return "Host:" + myID;
		} else {
			return "Client:" + myID;
		}
	}
}
