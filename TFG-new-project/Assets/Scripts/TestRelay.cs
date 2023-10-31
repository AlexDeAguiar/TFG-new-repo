using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{
	
	public static TestRelay Instance { get; private set; }

	private void Awake() {
		Instance = this;
	}

	private async void Start() {
		Debug.Log("1");
		await UnityServices.InitializeAsync();


		//Add an event to when user signs in. The event is to log it.
		//AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed In"); };
		Debug.Log("2");
		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	public async Task<string> createRelay() {
		try {
			//------------------------------
			Debug.Log("3");
			//Create a room with 1+3 = 4 slots
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

			Debug.Log("4");
			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			Debug.Log("Join code: " + joinCode);

			Debug.Log("5");
			RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
			Debug.Log("6");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			Debug.Log("7");
			NetworkManager.Singleton.StartHost();

			return joinCode;

		} catch (RelayServiceException e) {
			Debug.LogWarning("Error while creating relay. Exception message:\n" + e, this);
			return null;
		}
	}

	public async void joinRelay(string joinCode) {
		try {

			Debug.Log("3");
			Debug.Log("Joining relay with code: " + joinCode);
			JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

			Debug.Log("4");
			RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

			Debug.Log("5");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			Debug.Log("6");
			NetworkManager.Singleton.StartClient();
		} catch (RelayServiceException e) {
			Debug.LogWarning("Error while joining relay. Exception message:\n" + e, this);
		}
	}
}
