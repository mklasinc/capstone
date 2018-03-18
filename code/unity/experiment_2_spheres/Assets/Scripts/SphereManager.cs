using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour {

	bool is_visible = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTrackingStatus(bool new_tracking_status){
		is_visible = new_tracking_status;
	}

	public bool GetTrackingStatus(){
		return is_visible;
	}

}
