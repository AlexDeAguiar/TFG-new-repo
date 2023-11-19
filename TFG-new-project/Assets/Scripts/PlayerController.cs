using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    public float interactDistance = 2f; // La distancia a la que el jugador puede interactuar con la puerta
	public const KeyCode doorInteractKey = KeyCode.E; // La tecla que el jugador debe presionar para interactuar con la puerta
    public const KeyCode bBoardPlayPauseKey = KeyCode.P; // La tecla que el jugador debe presionar para pausar o reanudar el video de la pizarra
	public const KeyCode boardSelectVideoKey = KeyCode.E; // Tecla para mostrar selector de que video meter en la pizarra
	private SC_TPSController scTpsController;

	private GameObject currentBlackboard;
	private MainGUI mainGui;
	private VideoSelectorBoxManager videoSelectorBoxManager;
	private InfoBoxManager infoBoxManager;
	private bool enabledKeys = true;

	// Start is called before the first frame update
	void Start(){
		mainGui = GameObject.Find("UIDocument").GetComponent<MainGUI>();
		videoSelectorBoxManager = mainGui.getVideoSelectorBoxManager();
		infoBoxManager = mainGui.getInfoBoxManager();
	}
    
    void OnTriggerEnter(Collider other, GameObject door){
        if (other.gameObject == door){
			//TODO: En vez de esto, deberiamos simplemente desactivar el componente collider de la puerta (para que otros usuarios puedan pasar tambien)
            Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update() {

		infoBoxManager.hide();

		if (enabledKeys) {
			// Raycast para detectar la puerta cercana
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
			{
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
		infoBoxManager.showText(doorInteractionTextKey);

		//Detectar teclas y llamar a los metodos apropiados
		if (Input.GetKeyDown(doorInteractKey))
		{
			//TODO: Mover esta logica a la puerta, no tiene porque estar en el controller
			//Ignore collisions with the door
			//OnTriggerEnter(GetComponent<Collider>(), door);

			string doorPath = Main.GetFullPath(door.transform);

			Debug.Log(doorPath);

			NetworkPlayer.MyInstance.ToggleDoorServerRpc(doorPath);
		}
	}

	private const string blackboardInteractionTextKey = "INTERACTION_INFO_BLACKBOARD";
	private void handleBlackBoardBehaviour(GameObject blackboard) {
		infoBoxManager.showText(blackboardInteractionTextKey);

		if (Input.GetKeyDown(bBoardPlayPauseKey))
		{

			if (blackboard.GetComponent<VideoWithAudio>().isPlaying())
			{
				blackboard.GetComponent<VideoWithAudio>().Pause();
			}
			else { blackboard.GetComponent<VideoWithAudio>().Play(); }
		}

		if (Input.GetKeyDown(boardSelectVideoKey))
		{
			//Mostar la UI
			videoSelectorBoxManager.show();

			//Deshabilitar teclas del controller
			enabledKeys = false;
			scTpsController.disableMove();

			//Guardar la referencia de la pizarra
			currentBlackboard = blackboard;
		}
	}

	public void changeVideo(string path) {
		currentBlackboard.GetComponent<VideoWithAudio>().changeVideo(path);
		videoSelectorBoxManager.hide();
		currentBlackboard = null;
		enabledKeys = true;
		scTpsController.enableMove();
	}

	public void setScTpsController(SC_TPSController scTpsController) {
		this.scTpsController = scTpsController;
	}
}
