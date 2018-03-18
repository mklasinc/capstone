using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour {
	public Texture2D btnTexture;
	private string status_string = "Trial has no yet begun";
	private GUIStyle style = new GUIStyle();

	void OnGUI() {

		if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
			Debug.Log("Clicked the button with text");

		//status box
		style.fontSize = 40; //change the font size
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.background = btnTexture;
		GUI.Box(new Rect(Screen.width/2 - 400, 10, 800, 60), status_string, style);

	}

	public void NewDisplayMessage(string new_status){
		status_string = new_status;
	}
}