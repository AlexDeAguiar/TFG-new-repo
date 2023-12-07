using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;

public class PlayerInteractionController : MonoBehaviour{

	public static PlayerInteractionController Instance;

	public float interactDistance = 2f; // La distancia a la que el jugador puede interactuar con la puerta
	public const KeyCode doorInteractKey = KeyCode.E; // La tecla que el jugador debe presionar para interactuar con la puerta
    public const KeyCode bBoardPlayPauseKey = KeyCode.P; // La tecla que el jugador debe presionar para pausar o reanudar el video de la pizarra
	public const KeyCode boardSelectVideoKey = KeyCode.E; // Tecla para mostrar selector de que video meter en la pizarra

	private GameObject currentBlackboard;

	// Start is called before the first frame update
	void Start(){
		Instance = this;
	}
    
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

			Debug.Log(doorPath);

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
			/*
			//Mostar la UI
			VideoSelectorBoxManager.Instance.show();

			//Deshabilitar teclas de todos los controller
			NetworkPlayer.MyKeysEnabled = false;
			*/

			//Guardar la referencia de la pizarra
			currentBlackboard = blackboard;
			currentBlackboard
				.GetComponent<FileBrowserUpdate>()
				.OpenFileBrowser();
		}
	}

	public void StopVideo(){
		if(currentBlackboard != null){
			currentBlackboard.GetComponent<VideoWithAudio>().Stop();
		}
	}

	public void changeVideo(string path) {
		currentBlackboard.GetComponent<VideoWithAudio>().changeVideo(path);
		VideoSelectorBoxManager.Instance.hide();
		currentBlackboard = null;
		NetworkPlayer.MyKeysEnabled = true;
	}

	public void changeImg(Texture newTexture){
		if(currentBlackboard != null){
			Renderer renderer = currentBlackboard.GetComponent<Renderer>();
			renderer.material.SetTexture("_MainTex",newTexture);
		}
	}
}
