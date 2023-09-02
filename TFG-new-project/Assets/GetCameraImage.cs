using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCameraImage : MonoBehaviour
{
	WebCamTexture webCamTexture;

	// Start is called before the first frame update
	void Start()
	{
		webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		webCamTexture.Play();
		GetComponent<Renderer>().material.mainTexture = webCamTexture;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
