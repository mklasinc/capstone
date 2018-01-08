using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackerVarTest : MonoBehaviour {

	public ExperimentTrackableEventHandler trackHandler;
	// Use this for initialization
	void Start () {
		trackHandler = GameObject.Find ("ImageTarget").gameObject.GetComponent<ExperimentTrackableEventHandler> ();
		Debug.Log ("let's see if the target is visible");
		Debug.Log(trackHandler.targetVisible());
	}
	
	// Update is called once per frame
	void Update () {
				
	}
}	