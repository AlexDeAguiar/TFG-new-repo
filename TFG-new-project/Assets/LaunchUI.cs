using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class LaunchUI : MonoBehaviour
{
    [SerializeField]
    private Button hostButton;
    [SerializeField]
    private Button clientButton;

    private void Awake()
    {

        hostButton.onClick.AddListener(() =>
        {
			//todo
			NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
			//todo
			NetworkManager.Singleton.StartClient();
		});

    }
}