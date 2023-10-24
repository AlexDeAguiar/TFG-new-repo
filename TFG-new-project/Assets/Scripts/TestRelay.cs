using System.Collections;
using System.Collections.Generic;
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
	private async void Start()
	{
		await UnityServices.InitializeAsync();

		//Add an event to when user signs in. The event is to log it.
		AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed In"); };

		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	private async void createRelay() {
		try {
			//Create a room with 1+3 = 4 slots
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);


			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			Debug.Log("Join code: " + joinCode);

			RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

		} catch (RelayServiceException e) {
			Debug.LogWarning("Error while creating relay. Exception message:\n" + e, this);
		}
	}

	void Update()
	{
		
	}
}
