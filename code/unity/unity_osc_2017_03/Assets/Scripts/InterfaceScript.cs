using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour {
	public Texture btnTexture;
	private string status_string = "Trial has no yet begun";
	void OnGUI() {

		if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
			Debug.Log("Clicked the button with text");

		//status box
		GUI.Box(new Rect(Screen.width/2 - 30, 10, 200, 30), status_string);

	}

	public void ChangeStatus(string new_status){
		status_string = new_status;
	}
}