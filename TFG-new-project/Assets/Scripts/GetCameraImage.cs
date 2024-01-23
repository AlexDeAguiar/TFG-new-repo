using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GetCameraImage : MonoBehaviour{
	WebCamTexture webCamTexture;
	private Renderer renderer;
	private float currTime = 0f;
	private bool isOwner = false;

	// Start is called before the first frame update
	void Start(){
		renderer = GetComponent<Renderer>();
	}

	public void initForOwner() {
		isOwner = true;
		webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		webCamTexture.requestedWidth = 1;
		webCamTexture.requestedHeight = 1;
		webCamTexture.requestedFPS = 30;
		webCamTexture.Play();
	}

	// Update is called once per frame
	void Update(){
		if (!isOwner) { return; }

		currTime += Time.deltaTime;
		if (currTime < 0.5f) { return; }
		currTime = 0f;

		//Cast webcamTexture -> texture2D
		Color32[] colors = webCamTexture.GetPixels32();
		Texture2D texture = new Texture2D(webCamTexture.width, webCamTexture.height);
		texture.SetPixels32(colors);
		texture.Apply();

		//Cast texture2d -> png bytes
		var bytes = texture.EncodeToJPG();

		string faceCamPath = Main.GetFullPath(gameObject.transform);

		/*
		var dummyBytes = new byte[40000];
		dummyBytes[0] = 0;
		dummyBytes[1] = 100;
		dummyBytes[2] = 200;
		*/

		PlayerNetwork.MyInstance.updateWebcamClientRpc(faceCamPath, bytes, webCamTexture.width, webCamTexture.height);
	}

	public void setWebcamTexture(byte[] bytes, int width, int height) {
		Color32[] colorsFin = bytesToColors(bytes);
		/*
		texture = new Texture2D(width, height);
		texture.SetPixels32(colorsFin);
		texture.Apply();

		renderer.material.mainTexture = texture;
		*/

		//Retrieve the info back
		Texture2D finalTexture = new Texture2D(width, height);
		finalTexture.LoadImage(bytes);
		finalTexture.Apply();

		renderer.material.mainTexture = finalTexture;
	}

	private byte[] colorsToBytes(Color32[] colors) {
		int bLength = colors.Length * 4; //Each color takes up 4 bytes;
		byte[] bytes = new byte[bLength];
		
		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject();
			Marshal.Copy(ptr, bytes, 0, bLength);
		} finally {
			if (handle != default(GCHandle))
				handle.Free();
		}

		return bytes;
	}

	private Color32[] bytesToColors(byte[] bytes) {
		int bLength = bytes.Length; //Each color takes up 4 bytes;
		Color32[] colors = new Color32[bLength / 4];

		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			IntPtr ColorPtr = handle.AddrOfPinnedObject();
			Marshal.Copy(bytes, 0, ColorPtr, bLength);
		} finally {
			if (handle != default(GCHandle))
				handle.Free();
		}

		return colors;
	}
}
