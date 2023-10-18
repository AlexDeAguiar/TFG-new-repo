using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    public float interactDistance = 2f; // La distancia a la que el jugador puede interactuar con la puerta
	public const KeyCode doorInteractKey = KeyCode.E; // La tecla que el jugador debe presionar para interactuar con la puerta
    public const KeyCode bBoardPlayPauseKey = KeyCode.P; // La tecla que el jugador debe presionar para pausar o reanudar el video de la pizarra
	public const KeyCode boardSelectVideoKey = KeyCode.E; // Tecla para mostrar selector de que video meter en la pizarra
	private GameObject door; // La puerta actual con la que el jugador est� interactuando
    private GameObject blackBoard; // La puerta actual con la que el jugador est� interactuando
	private SC_TPSController scTpsController;

	private GameObject currentBlackboard;
	private MainGUI mainGui;
	private bool enabledKeys = true;

	// Start is called before the first frame update
	void Start(){
		mainGui = GameObject.Find("UIDocument").GetComponent<MainGUI>();
	}
    
    void OnTriggerEnter(Collider other){
        if (other.gameObject == door){
			//TODO: En vez de esto, deberiamos simplemente desactivar el componente collider de la puerta (para que otros usuarios puedan pasar tambien)
            Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update() {
		if (enabledKeys) {

			// Raycast para detectar la puerta cercana
			RaycastHit hit;
			door = null;
			blackBoard = null;

			if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
			{
				if (hit.collider.CompareTag("Door")) { handleDoorBehaviour(hit.collider.gameObject); }
				if (hit.collider.CompareTag("BlackBoard")) { blackBoard = hit.collider.gameObject; }
			}

			// Interactuar con la puerta si el jugador presiona la tecla
			if (door != null && Input.GetKeyDown(doorInteractKey))
			{
				OnTriggerEnter(GetComponent<Collider>());

				if (door.GetComponent<doorController>().isOpen())
				{
					door.GetComponent<doorController>().closeDoor();
				}
				else
				{
					door.GetComponent<doorController>().openDoor();
				}
			}

			if (blackBoard != null)
			{
				if (Input.GetKeyDown(bBoardPlayPauseKey))
				{
					OnTriggerEnter(GetComponent<Collider>());

					if (blackBoard.GetComponent<VideoWithAudio>().isPlaying())
					{
						blackBoard.GetComponent<VideoWithAudio>().Pause();
					}
					else { blackBoard.GetComponent<VideoWithAudio>().Play(); }
				}

				if (Input.GetKeyDown(boardSelectVideoKey))
				{
					OnTriggerEnter(GetComponent<Collider>());

					//Mostar la UI
					mainGui.show();

					//Deshabilitar teclas del controller
					enabledKeys = false;
					scTpsController.disableMove();

					//Guardar la referencia de la pizarra
					currentBlackboard = blackBoard;
				}
			}
		}
	}

	//TODO: Mover este texto tambien a la puerta, por si queremos que mande distintos mensajes si esta abierto o cerrado, o para que cambie el mensaje dependiendo del idioma
	private const string doorInteractionText = "Press E to open/close the door";

	private void handleDoorBehaviour(GameObject door) {
		//Mostrar el mensaje informativo siempre
		//interactionBox.setText(doorInteractionText);
		//interactionBox.makeVisible()

		//Detectar teclas y llamar a los metodos apropiados
		if (Input.GetKeyDown(doorInteractKey))
		{
			//TODO: Mover esta logica a la puerta, no tiene porque estar en el controller
			//Ignore collisions with the door
			OnTriggerEnter(GetComponent<Collider>());

			if (door.GetComponent<doorController>().isOpen())
			{
				door.GetComponent<doorController>().closeDoor();
			}
			else
			{
				door.GetComponent<doorController>().openDoor();
			}
		}
	}

	public void changeVideo(string path) {
		currentBlackboard.GetComponent<VideoWithAudio>().changeVideo(path);
		mainGui.hide();
		currentBlackboard = null;
		enabledKeys = true;
		scTpsController.enableMove();
	}

	public void setScTpsController(SC_TPSController scTpsController) {
		this.scTpsController = scTpsController;
	}
}
