using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Threading;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;

public class PlayerInteractionController : MonoBehaviour{
    public static readonly string[][] BL_BOARD_FILE_EXTS = {
        new string[] { "Image files", ".jpg", ".jpeg", ".jpe", ".jfif", ".png" },
        new string[] { "Video files", ".mp4" },
        new string[] { "PDF files", ".pdf" }
    };

	public static PlayerInteractionController Instance;
	public float interactDistance = 2f; // La distancia a la que el jugador puede interactuar con la puerta
	public const KeyCode doorInteractKey = KeyCode.E; // La tecla que el jugador debe presionar para interactuar con la puerta
    public const KeyCode bBoardPlayPauseKey = KeyCode.P; // La tecla que el jugador debe presionar para pausar o reanudar el video de la pizarra
	public const KeyCode boardSelectVideoKey = KeyCode.E; // Tecla para mostrar selector de que video meter en la pizarra
	private GameObject currentBlackboard;

	void Start(){ Instance = this; }
    void OnTriggerEnter(Collider other, GameObject door){
        if (other.gameObject == door){
			//TODO: En vez de esto, deberiamos simplemente desactivar el componente collider de la puerta (para que otros usuarios puedan pasar tambien)
            Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update() {
		InfoBoxManager.Instance.hide();
		if (NetworkPlayer.MyKeysEnabled) {
			// Raycast para detectar la puerta cercana
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance)){
				var targetObject = hit.collider.gameObject;

				switch (hit.collider.tag) {
					case "Door":
						handleDoorBehaviour(targetObject);
						break;
					case "BlackBoard":
						handleBlackBoardBehaviour(targetObject);
						break;
				}
			}
		}
	}

	//TODO: Mover este texto tambien a la puerta, por si queremos que mande distintos mensajes si esta abierto o cerrado, o para que cambie el mensaje dependiendo del idioma
	private const string doorInteractionTextKey = "INTERACTION_INFO_DOOR";
	private void handleDoorBehaviour(GameObject door) {
		//Mostrar el mensaje informativo siempre
		InfoBoxManager.Instance.showText(doorInteractionTextKey);

		//Detectar teclas y llamar a los metodos apropiados
		if (Input.GetKeyDown(doorInteractKey)){
			//TODO: Mover esta logica a la puerta, no tiene por que estar en el controller
			//Ignore collisions with the door
			//OnTriggerEnter(GetComponent<Collider>(), door);

			string doorPath = Main.GetFullPath(door.transform);
			PlayerNetworkMessagesController.Instance.toggleDoorServerRpc(doorPath);
		}
	}

	private const string blackboardInteractionTextKey = "INTERACTION_INFO_BLACKBOARD";
	private void handleBlackBoardBehaviour(GameObject blackboard) {
		InfoBoxManager.Instance.showText(blackboardInteractionTextKey);

		if (Input.GetKeyDown(bBoardPlayPauseKey)){
			if (blackboard.GetComponent<VideoWithAudio>().isPlaying()){
				blackboard.GetComponent<VideoWithAudio>().Pause();
			}
			else { blackboard.GetComponent<VideoWithAudio>().Play(); }
		}

		if (Input.GetKeyDown(boardSelectVideoKey)){
			currentBlackboard = blackboard;
			currentBlackboard
				.GetComponent<FileBrowserUpdate>()
				.OpenFileBrowser(DisplayOnBlackboard,BL_BOARD_FILE_EXTS);
		}
	}

	public void StopVideo(){
		if(currentBlackboard != null){ currentBlackboard.GetComponent<VideoWithAudio>().Stop(); }
	}

	public void DisplayOnBlackboard(string path){
		StopVideo();
		string e = Path.GetExtension(path);
		Debug.Log(e);
			 if(e == ".mp4"){ changeVideo(path); }
		else if(e == ".pdf"){ changePDF(path);   }
		else                { changeImg(path);   }
	}

	public void changeVideo(string path) {
		if(currentBlackboard != null){
			currentBlackboard.GetComponent<VideoWithAudio>().changeVideo(path);
			currentBlackboard = null;
		}
	}

	public async void changeImg(string path){ StartCoroutine(InternalChangeImg(path)); }
	private IEnumerator InternalChangeImg(string path){
		using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path)){
			yield return uwr.SendWebRequest();

			if (uwr.isNetworkError || uwr.isHttpError){ Debug.Log(uwr.error); }
			else{
				var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
				updateTexture(uwrTexture);
			}
		}
	}

	public void changePDF(string path){
		//NO FUNCIONA LOL
		Texture2D[] pdfImages = PDFViewer.ConvertPdfToImages(path);

		for(int i = 0; i < pdfImages.Length; i++){
			updateTexture(pdfImages[i]);
			Thread.Sleep(2000);
		}
	}

	public void updateTexture(Texture newTexture){
		if(currentBlackboard != null){
			Renderer renderer = currentBlackboard.GetComponent<Renderer>();
			renderer.material.SetTexture("_MainTex",newTexture);
		}
	}
}
