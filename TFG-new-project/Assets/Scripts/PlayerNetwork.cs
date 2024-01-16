using UnityEngine;
using UnityEngine.Networking;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Vivox;

public class PlayerNetwork : NetworkBehaviour {
	public static PlayerNetwork MyInstance;	
	private PlayerLocalUpdates localUpdates;

	private NetworkVariable<FixedString32Bytes> username = new NetworkVariable<FixedString32Bytes>();

	public override void OnNetworkDespawn(){ username.OnValueChanged -= OnUpdatePlayerUsernameClientRpc; }

	public override void OnNetworkSpawn() {
		var playerId = transform.GetComponent<NetworkObject>().NetworkObjectId;
		localUpdates = Utility.FindChildByTag(gameObject, "Username").GetComponent<PlayerLocalUpdates>();
		localUpdates.init(gameObject);

		transform.name = generateDefaultPlayerName(playerId);
		username.OnValueChanged += OnUpdatePlayerUsernameClientRpc;

		if (IsOwner) {
			MyInstance = this;
			PlayerControllers.createController(gameObject);
			updateUsernameServerRpc(ConnectBoxManager.usernameStr);

			VivoxService.Instance.Initialize();
			VivoxPlayer.Instance.SignIntoVivox();
		}
		else{ localUpdates.updatePlayerUsername(username.Value.ToString()); }
	}

	[ServerRpc(RequireOwnership = false)] public void updateUsernameServerRpc(string newVal){ username.Value = newVal; }

	[ClientRpc] public void OnUpdatePlayerUsernameClientRpc(FixedString32Bytes previous, FixedString32Bytes current){
		localUpdates.updatePlayerUsername(current.ToString());
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
