using Unity.Netcode;
using Unity.Services.Vivox;

public class NetworkPlayer : NetworkBehaviour {

	public static NetworkPlayer MyInstance;
	public static bool MyKeysEnabled;

	/// <summary>
	/// <b>OnNetworkSpawn</b> is automatically called when any <b>NetworkPlayer</b> is created in the scene.<br/>
	///	It is in charge of locally initializing all the scripts and properties related to any player.<br/>
	///	The <b>IsOwner</b> section is only run in single machine that owns the player (i.e. On the machine of the player that is now joining).
	/// </summary>
	public override void OnNetworkSpawn() {
		var playerId = transform.GetComponent<NetworkObject>().NetworkObjectId;
		setPlayerName(playerId);

		if (IsOwner) {
			MyInstance = this;
			MyKeysEnabled = true;
			gameObject.AddComponent<PlayerMovementController>();
			gameObject.AddComponent<PlayerInteractionController>();
			gameObject.AddComponent<PlayerNetworkMessagesController>();

			VivoxService.Instance.Initialize();
			VivoxPlayer.Instance.SignIntoVivox();
		}
	}

	private void setPlayerName(ulong myID) {
		if (IsOwnedByServer) {
			transform.name = "Host:" + myID;
		} else {
			transform.name = "Client:" + myID;
		}
	}

	public string getPlayerName() {
		return transform.name;
	}
}