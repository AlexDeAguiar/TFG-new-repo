using UnityEngine;

public class Main : MonoBehaviour{
	public static Main Instance;
	public Web Web;

	void Start(){
		Instance = this;
		Web = GetComponent<Web>();;
	}

	public static string DateToString(){
		var timeNow = System.DateTime.Now;
		return timeNow.Year.ToString() + "-" + timeNow.Month.ToString()  + "-" + timeNow.Day.ToString() + " " + 
				timeNow.Hour.ToString() + ":" + timeNow.Minute.ToString() + ":" + timeNow.Second.ToString();
	}
}
