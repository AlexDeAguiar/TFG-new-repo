using UnityEngine;
using UnityEngine.Networking;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Vivox;

public class PlayerNetwork : NetworkBehaviour {
	public static PlayerNetwork MyInstance;	
	private PlayerLocalUpdates localUpdates;

	private NetworkVariable<FixedString32Bytes> username = new NetworkVariable<FixedString32Bytes>();

	public override void OnNetworkSpawn() {
		localUpdates = Utility.FindChildByTag(gameObject, "Username").GetComponent<PlayerLocalUpdates>();
		localUpdates.init(gameObject);

		username.OnValueChanged += OnUpdatePlayerUsername;

		if (IsOwner) {
			MyInstance = this;
			PlayerControllers.createController(gameObject);
			updateUsernameServerRpc(ConnectBoxManager.usernameStr);

			VivoxService.Instance.Initialize();
			VivoxPlayer.Instance.SignIntoVivox();
		}
		else{ localUpdates.updatePlayerUsername(username.Value.ToString()); }
	}

	public override void OnNetworkDespawn() { username.OnValueChanged -= OnUpdatePlayerUsername; }

	[ServerRpc(RequireOwnership = false)] public void updateUsernameServerRpc(string newVal){ username.Value = newVal; }

	public void OnUpdatePlayerUsername(FixedString32Bytes previous, FixedString32Bytes current){
		localUpdates.updatePlayerUsername(current.ToString());
	}

	[ServerRpc(RequireOwnership = false)]
	public void toggleDoorServerRpc(string doorName) {
		GameObject door = GameObject.Find(doorName);
		DoorInteraction doorController = door.GetComponent<DoorInteraction>();
		doorController.toggleDoor();
	}
}
