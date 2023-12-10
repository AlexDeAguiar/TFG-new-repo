using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public GameObject hitbox;
	public GameObject plane;

	public void Start() {
		hitbox.GetComponent<Renderer>().enabled = false;
		plane.GetComponent<Renderer>().enabled = false;
	}

	public void reset() {
		plane.GetComponent<Renderer>().enabled = true;
		plane.transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
