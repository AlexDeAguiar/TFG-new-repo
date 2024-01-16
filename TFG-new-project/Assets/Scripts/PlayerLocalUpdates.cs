using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLocalUpdates : MonoBehaviour{
	public GameObject playerCamera;
	private TextMeshPro UsernameTag;


    public void init(GameObject player){
		playerCamera = GameObject.Find("MainCamera");
		UsernameTag  = Utility.FindChildByTag(player, "Username").GetComponent<TextMeshPro>(); //Utility.FindChildByTag(player, "Username").
    }

	public void updatePlayerUsername(string current){
	   UsernameTag.text = current;
	}

	void Update(){ updateUsernameTagPos(); }

	void updateUsernameTagPos(){
		if (UsernameTag != null){
			Quaternion invertedRotation = Quaternion.Inverse(playerCamera.transform.rotation);
			UsernameTag.transform.LookAt(playerCamera.transform.position);
		}
	}
}
