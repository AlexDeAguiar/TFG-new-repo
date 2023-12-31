using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Threading;

public class Blackboard : MonoBehaviour, IInteractable {
    public GameObject hitbox;
	public GameObject plane;
	private Renderer planeRenderer;
    public const KeyCode bBoardPlayPauseKey = KeyCode.P; // La tecla que el jugador debe presionar para pausar o reanudar el video de la pizarra
	public const KeyCode boardSelectVideoKey = KeyCode.E; // Tecla para mostrar selector de que video meter en la pizarra
	public const KeyCode bBoardCloseKey = KeyCode.C; //#
	public const KeyCode bBoardPrevKey = KeyCode.LeftArrow;
	public const KeyCode bBoardNextKey = KeyCode.RightArrow;
	private GameObject currentBlackboard;

	//For changing the file displayed on the blackboard when multiselect
	private static string[] EMPTY_ARRAY = new string[0];
	private string[] paths = EMPTY_ARRAY;
	private int currPathIdx = 0;

	public static readonly string[][] BL_BOARD_FILE_EXTS = {
        new string[] { "Image files", ".jpg", ".jpeg", ".jpe", ".jfif", ".png" },
        new string[] { "Video files", ".mp4" },
        new string[] { "PDF files", ".pdf" }
    };

	public void Start() {
		hitbox.GetComponent<Renderer>().enabled = false;
		planeRenderer = plane.GetComponent<Renderer>();
		planeRenderer.enabled = false;
	}

	public void reset() {
		plane.GetComponent<Renderer>().enabled = true;
		plane.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private const string blackboardInteractionTextKey = "INTERACTION_INFO_BLACKBOARD";
	public void interact(GameObject targetObject) {
		InfoBoxManager.Instance.showText(blackboardInteractionTextKey);
		currentBlackboard = targetObject;

		if (currentBlackboard == null) { return; }

		if (Input.GetKeyDown(boardSelectVideoKey)) {
			currentBlackboard.GetComponent<Blackboard>().plane
				.GetComponent<FileBrowserUpdate>()
				.OpenFileBrowser(DisplayOnBlackboard, BL_BOARD_FILE_EXTS, 3, true);
		}

		if (Input.GetKeyDown(bBoardPlayPauseKey)) {
			if (targetObject.GetComponent<Blackboard>().plane.GetComponent<VideoWithAudio>().isPlaying()) {
				targetObject.GetComponent<Blackboard>().plane.GetComponent<VideoWithAudio>().Pause();
			} else { targetObject.GetComponent<Blackboard>().plane.GetComponent<VideoWithAudio>().Play(); }
		}

		if (Input.GetKeyDown(bBoardCloseKey)) {
			StopAll();
		}

		if (Input.GetKeyDown(bBoardPrevKey)) {
			prevFile();
		}

		if (Input.GetKeyDown(bBoardNextKey)) {
			nextFile();
		}
	}

	public void StopVideo(){
		if(currentBlackboard != null){ currentBlackboard.GetComponent<Blackboard>().plane.GetComponent<VideoWithAudio>().Stop(); }
	}

	public void DisplayOnBlackboard(string[] paths){
		this.paths = paths;
		currPathIdx = 0;
		setCurrentFileInBlackboard();
	}

	private void setCurrentFileInBlackboard() {
		StopVideo();
		string path = paths[currPathIdx];
		if (currentBlackboard != null) {
			string e = Path.GetExtension(path);
			Debug.Log(e);
			if (e == ".mp4") {
				changeVideo(path);
			}
			else if (e == ".pdf") { changePDF(path); }
			else { changeImg(path); }
		}
	}

	public void clearFile() {
		paths = EMPTY_ARRAY;
		currPathIdx = 0;
		StopAll();
	}

	private void StopAll() {
		StopVideo();
		updateTexture(null);
	}

	public void nextFile() {
		if (paths.Length <= 1) { return; }
		currPathIdx = (currPathIdx + 1) % paths.Length;
		setCurrentFileInBlackboard();
	}

	public void prevFile() {
		if (paths.Length <= 1) { return; }
		currPathIdx = (currPathIdx - 1 + paths.Length) % paths.Length;
		setCurrentFileInBlackboard();
	}

	public void changeVideo(string path) {
		if(currentBlackboard != null){
			currentBlackboard.GetComponent<Blackboard>().reset();
			currentBlackboard.GetComponent<Blackboard>().plane.GetComponent<VideoWithAudio>().changeVideo(path);
		}
	}

	public async void changeImg(string path){ PlayerControllers.Instance.requestStartCoroutine(InternalChangeImg(path)); }
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
		if (currentBlackboard != null) {
			var blackboardScript = currentBlackboard.GetComponent<Blackboard>();
			var background = blackboardScript.hitbox;
			var plane = blackboardScript.plane;

			planeRenderer.enabled = (newTexture != null);

			if (newTexture != null) {
				// Obtener la textura y sus proporciones originales
				Vector3 bBoardscale = background.transform.localScale;
				float boardAspectRatio = 1f * bBoardscale.x / bBoardscale.y;
				float imgAspectRatio = 1f * newTexture.width / newTexture.height;
				float deformationRatio = imgAspectRatio / boardAspectRatio;

				float w = 1, h = 1;
				if (deformationRatio < 1f) { w = deformationRatio; } else { h = 1 / deformationRatio; }

				plane.transform.localScale = new Vector3(w, 1f, h);
				//plane.transform.localScale = scale;
				planeRenderer.material.mainTextureScale = new Vector2(1f, 1f);
				planeRenderer.material.mainTextureOffset = new Vector2(0f, 0f);
				//renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
			}

			planeRenderer.material.SetTexture("_MainTex", newTexture);

		}
	}
}
