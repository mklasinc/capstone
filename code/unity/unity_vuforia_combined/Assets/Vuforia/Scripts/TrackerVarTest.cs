using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackerVarTest : MonoBehaviour {

	private ExperimentTrackableEventHandler trackHandler;
	// Use this for initialization
	void Start () {
		trackHandler = GameObject.Find ("ImageTarget").gameObject.GetComponent<ExperimentTrackableEventHandler> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
