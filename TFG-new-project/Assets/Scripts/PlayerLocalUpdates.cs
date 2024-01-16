using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLocalUpdates : MonoBehaviour{
	public GameObject playerCamera;
	private TextMeshPro UsernameTag;
	private GameObject player;


    public void init(GameObject player){
		this.player = player;
		playerCamera = GameObject.Find("MainCamera");
		UsernameTag  = Utility.FindChildByTag(player, "Username").GetComponent<TextMeshPro>(); //Utility.FindChildByTag(player, "Username").
    }

	public void updatePlayerUsername(string current){
		UsernameTag.text = current;
		player.transform.name = "Player: " + current;
	}

	void Update(){ updateUsernameTagPos(); }

	void updateUsernameTagPos(){
		if (UsernameTag != null){
			Quaternion invertedRotation = Quaternion.Inverse(playerCamera.transform.rotation);
			UsernameTag.transform.LookAt(playerCamera.transform.position);
		}
	}
}
