using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class OSCReceiver1 : MonoBehaviour {

	public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
	public int SendToPort = 9000; //the port you will be sending from
	public int ListenerPort = 8000; //the port you will be listening on
	//public var controller : Transform;
	//public string gameReceiver = "sphere"; //the tag of the object on stage that you want to manipulate
	private Osc handler;

	public string sphere_target = "Sphere";

	//reference to InterfaceScript.cs allows for the change of the status box upon receving UDP data
	private InterfaceScript gui_script;
	private bool trial_is_active = false;
	private bool target_is_tracked = false;
	private bool tracking_is_locked = false;

	//this is for the trackable variable
	public ExperimentTrackableEventHandler trackHandler;

	//VARIABLES YOU WANT TO BE ANIMATED
	private int yRot = 0; //the rotation around the y axis
	private float orig_dim = 1.42f;//initial dimensions
	private float dim = 1.42f;//initial dimensions
	//private var sphere = GameObject.Find(gameReceiver);

	// Use this for initialization
	void Start ()
	{

		// get Interface Script
		gui_script = GameObject.Find("GUI").gameObject.GetComponent<InterfaceScript>();

		//Initializes on start up to listen for messages
		//make sure this game object has both UDPPackIO and OSC script attached

		UDPPacketIO udp = GetComponent<UDPPacketIO>();
		udp.init(RemoteIP, SendToPort, ListenerPort);
		handler = GetComponent<Osc>();
		handler.init(udp);
		handler.SetAllMessageHandler(AllMessageHandler);



	}

	void Update () {
		var sphere = GameObject.Find(sphere_target);
		var space = Input.GetKey(KeyCode.Space);

		if(space){
			Debug.Log("space pressed");
			sphere.transform.Rotate(0, 0, 0);
		}else{
			sphere.transform.Rotate(1, 1, 1);
		}

		sphere.transform.localScale = new Vector3(dim,dim,dim);

	}

	//returns true if image target is found, false if it is not
	public string TestCommunication(){
		return "hey there";
	}

	public void TargetTracking(bool tracking){
		target_is_tracked = tracking;
		CheckTrialValidity ();
	}

	public void CheckTrialValidity(){
		if (!tracking_is_locked && target_is_tracked) {
			// lock in the tracking
			tracking_is_locked = true;
		} else if (tracking_is_locked && !target_is_tracked) {
			//trial is not valid, abort mission
			gui_script.ChangeStatus("Trial is not valid! Shut down the glasses!");
			SendMessage ("trial is not valid");
			ResetTrialParameters ();
		}
	}

	private void ResetTrialParameters(){
		trial_is_active = false;
		target_is_tracked = false;
		tracking_is_locked = false;
	}

	private void SendMessage(string m){
		OscMessage oscM = Osc.StringToOscMessage(m);
		handler.Send (oscM);
	}
		

	public void AllMessageHandler(OscMessage message){

		Debug.Log("received message");
		//handler.Send("/message received");

		string msgString = Osc.OscMessageToString(message); //the message and value combined

//		OscMessage oscM = Osc.StringToOscMessage(msgString);
//		handler.Send(oscM);

		//Debug.Log(msgString); //log the message and values coming from OSC

		string msgValue = msgString.Split("/"[0])[1];
		Debug.Log("message value is:" + msgValue.Substring(3, 2));
		//FUNCTIONS YOU WANT CALLED WHEN A SPECIFIC MESSAGE IS RECEIVED
		switch (msgValue){
		case "1":
			//Debug.Log('we have a match');
			dim = orig_dim;
			break;
		case "2":
			//Debug.Log('we have a match');
			dim = orig_dim;
			dim *= 2;
			break;
		case "3":
			//Debug.Log('we have a match');
			dim = orig_dim;
			dim *= 3;
			break;
		case "trial_start":
			//Debug.Log('we have a match');
			gui_script.ChangeStatus ("Trial has begun");
			trial_is_active = true;
			break;
		case "trial_end":
			//Debug.Log('we have a match');
			gui_script.ChangeStatus ("Trial is done");
			ResetTrialParameters ();
			SendMessage ("trial is valid");
			break;
		default:
			Debug.Log("we dont have a match");
			break;
		}
	}

	//FUNCTIONS CALLED BY MATCHING A SPECIFIC MESSAGE IN THE ALLMESSAGEHANDLER FUNCTION
	public void Rotate(string msgValue) //rotate the sphere around its axis
	{
		yRot = Int32.Parse(msgValue);
	}

//	public static bool IsTrialActive(){
//		return trial_is_active;
//	}

//	public void Scale(string msgValue) //rotate the sphere around its axis
//	{
//		//dim = msgValue;
//		//sphere.transform.localScale = new Vector3(dim,dim,dim);
//	}

}





//These functions are called when messages are received
////Access values via: oscMessage.Values[0], oscMessage.Values[1], etc






