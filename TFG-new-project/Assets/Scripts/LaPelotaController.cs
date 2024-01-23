using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaPelotaController : MonoBehaviour, IInteractable {
    private const string pelotaInteractionTextKey = "PELOTA_INFO_DOOR";
    public const KeyCode pelotaInteractKey = KeyCode.P;
	public void interact(GameObject targetObject) {
		//Mostrar el mensaje informativo siempre
		InfoBoxManager.Instance.showText(pelotaInteractionTextKey);

		//Detectar teclas y llamar a los metodos apropiados
		if (Input.GetKeyDown(pelotaInteractKey)){
			System.Diagnostics.Process.Start("dominoesV2.exe");
		}
	}
}
