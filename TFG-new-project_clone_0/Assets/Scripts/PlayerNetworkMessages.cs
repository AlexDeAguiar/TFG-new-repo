using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkMessages : NetworkBehaviour {
	public static PlayerNetworkMessages MyInstance;

	public override void OnNetworkSpawn() {
		if (!IsOwner) { return; }
		MyInstance = this;
	}

	[ServerRpc(RequireOwnership = false)]
	public void toggleDoorServerRpc(string doorName) {
		GameObject door = GameObject.Find(doorName);
		DoorController doorController = door.GetComponent<DoorController>();
		doorController.toggleDoor();
	}
}
