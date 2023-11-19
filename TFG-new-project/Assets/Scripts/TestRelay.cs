using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
		await UnityServices.InitializeAsync();

		//Add an event to when user signs in. The event is to log it.
		//AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed In"); };
		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	public async Task<string> createRelay() {
		try {
			//------------------------------
			//Create a room with 1+3 = 4 slots
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			Debug.Log("Join code: " + joinCode);

			RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			NetworkManager.Singleton.StartHost();

			return joinCode;

		} catch (RelayServiceException e) {
			Debug.LogWarning("Error while creating relay. Exception message:\n" + e, this);
			return null;
		}
	}

	public async void joinRelay(string joinCode) {
		try {

			//Stop previous connection attempts to be able to retry if it gets stuck the 1st time
			NetworkManager.Singleton.StopAllCoroutines();

			Debug.Log("Joining relay with code: " + joinCode);
			JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

			RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			doSomeAsyncMagic();

			NetworkManager.Singleton.StartClient();
		} catch (RelayServiceException e) {
			Debug.LogWarning("Error while joining relay. Exception message:\n" + e, this);
		}
	}

	private void doSomeAsyncMagic() {
		//TODO: When we don't sleep, there is some race condition between connecting and something else. Investigate this.
		//Don't remove... Seriously
		Thread.Sleep(1000);
	}
}
