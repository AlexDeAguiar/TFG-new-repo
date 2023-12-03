using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkMessagesController : MonoBehaviour {

	public static PlayerNetworkMessagesController Instance;

	void Start() {
		Instance = this;
	}

	void Update() {
		if (!NetworkPlayer.MyKeysEnabled) { return; }

		if (Input.GetKeyDown(KeyCode.Q)) {
			pingServerRpc("some-message");
		}
	}

	/// <summary>
	/// <b>pingServerRpc</b> is an example of how we can use <b>ServerRpc</b> to communicate accross the network.<br/>
	/// The request is <b>initiated on the client side</b> (by just calling this method), and is <b>sent to the host</b> accross the network.<br/>
	/// Then, <b>on each host machine</b>, the code inside this method will run (after some network delay).<br/>
	/// You can obtain the id of the caller inside serverRpcParams
	/// <see>For documentation: https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/ </see><br/>
	/// <seealso>See also: pingClientRpc()</seealso>
	/// </summary>
	[ServerRpc]
	public void pingServerRpc(string message, ServerRpcParams serverRpcParams = default) {
		Debug.Log("A client has sent to the host the following message: " + message);
	}

	/// <summary>
	/// <b>pingClientRpc</b> is an example of how we can use <b>ClientRpc</b> to communicate accross the network.<br/>
	/// The request is <b>initiated on the host side</b> (by just calling this method), and is <b>broadcasted to all players</b> (host included) accross the network.<br/>
	/// Then, <b>on each player machine</b>, the code inside this method will run (after some network delay).<br/>
	/// Optionally, you can change clientRpcParams to send only broadcast to some specific players
	/// <see>For documentation: https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/clientrpc/ </see><br/>
	/// <seealso>See also: pingServerRpc()</seealso>
	/// </summary>
	[ClientRpc]
	public void pingClientRpc(string message, ClientRpcParams clientRpcParams = default) {
		Debug.Log("Host is broadcasting. " + NetworkPlayer.MyInstance.getPlayerName() + " has received the following message:" + message);
	}

	[ServerRpc]
	public void toggleDoorServerRpc(string doorName) {
		toggleDoorClientRpc(doorName);
	}

	[ClientRpc]
	public void toggleDoorClientRpc(string doorName) {
		Debug.Log(doorName);
		GameObject door = GameObject.Find(doorName);
		Debug.Log(door);
		DoorController doorController = door.GetComponent<DoorController>();
		Debug.Log(doorController);
		doorController.toggleDoor();
	}
}
