using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour {

	public static NetworkPlayer MyInstance;

	public override void OnNetworkSpawn() {
		var myID = transform.GetComponent<NetworkObject>().NetworkObjectId;
		setName(myID);

		if (IsOwner) {
			MyInstance = this;
			var scTpsController = gameObject.AddComponent<SC_TPSController>();
			var playerControler = gameObject.AddComponent<PlayerController>();

			playerControler.setScTpsController(scTpsController);
			GameObject.Find("UIDocument").GetComponent<MainGUI>().getVideoSelectorBoxManager().setPlayerController(playerControler);
		}
	}

	private void setName(ulong myID) {
		if (IsOwnedByServer) {
			transform.name = "Host:" + myID;
		} else {
			transform.name = "Client:" + myID;
		}
		Debug.Log(transform.name);
	}



	// Update is called once per frame
	void Update() {
		if (!IsOwner) { return; }
		//TODO: Remove this temporary code
		if (Input.GetKeyDown(KeyCode.Q)) {
			PingClientRpc();
		}
	}


	[ServerRpc]
	public void PingServerRpc(ServerRpcParams serverRpcParams = default) {
		Debug.Log("Server pong");
	}

	[ClientRpc]
	public void PingClientRpc(ClientRpcParams clientRpcParams = default) {
		Debug.Log("Client pong");
	}

	[ServerRpc]
	public void ToggleDoorServerRpc(string doorName) {
		ToggleDoorClientRpc(doorName);
	}

	[ClientRpc]
	public void ToggleDoorClientRpc(string doorName) {
		GameObject door = GameObject.Find(doorName);
		DoorController doorController = door.GetComponent<DoorController>();
		doorController.toggleDoor();
	}
}