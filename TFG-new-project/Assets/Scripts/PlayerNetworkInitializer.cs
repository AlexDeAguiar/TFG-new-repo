using Unity.Netcode;
using Unity.Services.Vivox;

public class PlayerNetworkInitializer : NetworkBehaviour {

	public override void OnNetworkSpawn() {
		var playerId = transform.GetComponent<NetworkObject>().NetworkObjectId;
		setPlayerName(playerId);

		if (IsOwner) {
			new PlayerControllers(gameObject);

			VivoxService.Instance.Initialize();
			VivoxPlayer.Instance.SignIntoVivox();
		}
	}

	//TODO: Change player name to a network variable
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
