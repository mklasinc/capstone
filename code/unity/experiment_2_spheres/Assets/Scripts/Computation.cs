using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Text.RegularExpressions;

public class Computation : MonoBehaviour {

	//references to other scripts
	private InterfaceScript gui_script;
	private OSCReceiver1 osc;
	private Spheres spheres;
	public ExperimentTrackableEventHandler trackHandler;

	// target status references
	private bool tracking_is_locked = false;

	// trial status references
	private bool trial_is_staged = false;
	private bool trial_is_underway = false;
	private bool trial_is_done = false;

	// pre trial boolean
	private bool pretrial_message_switch = false;



	// Use this for initialization

	void Start () {

		// get Interface Script
		gui_script = GameObject.Find("GUI").gameObject.GetComponent<InterfaceScript>();
		osc = GameObject.Find ("osc_script").gameObject.GetComponent<OSCReceiver1> ();
		spheres = GameObject.Find ("spheres_script").gameObject.GetComponent<Spheres> ();
		
	}
	
	// Update is called once per frame

	void Update () {

		if (trial_is_staged) {
			PreTrialManager ();
		} else if (trial_is_underway) {
			TrialManager ();
		}

	}

	// PRE TRIAL 

	private void PreTrialManager(){
		if (!pretrial_message_switch == true && spheres.SpheresAreVisible () == true) {
			// send message to Unity saying that you are ready
			pretrial_message_switch = true;
			osc.SendMessage ("sphere_tracking:found");

		} else if (pretrial_message_switch == true && !spheres.SpheresAreVisible () == true) {
			// send message to Matlab that the spheres are not ready
			pretrial_message_switch = false;
			osc.SendMessage ("sphere_tracking:lost");
		}
	}

	// DURING TRIAL

	private void TrialManager(){
		CheckTrialValidity ();
	}

	public void CheckTrialValidity(){


		if (!tracking_is_locked && spheres.SpheresAreVisible ()) {
			// if target is tracked and the tracking is not locked, then lock in the tracking
			tracking_is_locked = true;
		} else if (tracking_is_locked == true && spheres.SpheresAreVisible () == false) {
			// if tracking is locked in and the target is not tracked, trial is unvalid
			gui_script.NewDisplayMessage("Trial is not valid! Shut down the glasses!");
			osc.SendMessage ("trial_status:invalid");
			ResetTrialParameters ();
		}

	}

	// this functions resets all trial parameters

	private void ResetTrialParameters(){
		
		// trial parameters
		trial_is_done = true;
		trial_is_staged = false;
		trial_is_underway = false;

		// tracking parameter
		tracking_is_locked = false;

	}

	// this method parses incoming sphere size messages
		
	public void SetSphereSizes(string msg){
		//split the message, remember, the message should be in the following format: "L2_R1"
		string l = msg.Split("_"[0])[0];
		string r  = msg.Split("_"[0])[1];
		//left sphere value
		int left_sphere_size = Int32.Parse(l.Substring(1,1));
		//right sphere value
		int right_sphere_size = Int32.Parse(r.Substring(1,1));
		//set sphere sizes
		spheres.UpdateSize (left_sphere_size,right_sphere_size);

	}

	// this method parses incoming trial status messages

	public void ParseTrialStatus(string msg){
		Debug.Log ("new trial status is: " + msg);

		string pre_trial_string = "staged";
		string trial_string = "ongoing";
		string post_trial_string = "done";

		bool pre_trial_bool = new Regex(pre_trial_string, RegexOptions.IgnoreCase).Match(msg).Success;
		bool trial_bool = new Regex(trial_string, RegexOptions.IgnoreCase).Match(msg).Success;
		bool post_trial_bool = new Regex(post_trial_string, RegexOptions.IgnoreCase).Match(msg).Success;

		if (pre_trial_bool) {
			gui_script.NewDisplayMessage ("We are in the pre-trial stage");
			ResetTrialParameters ();
			trial_is_done = false;
			trial_is_staged = true; // trial is not active yet
		} else if (trial_bool) {
			gui_script.NewDisplayMessage ("We are recording data!");
			ResetTrialParameters ();
			trial_is_staged = false;
			trial_is_underway = true;
		} else if (post_trial_bool) {
			gui_script.NewDisplayMessage ("The trial is done!");
			ResetTrialParameters ();
			trial_is_underway = false;
			trial_is_done = true;
		} else {
			gui_script.NewDisplayMessage ("ERROR INVALID TRIAL STATUS!");
		}

	}



}
