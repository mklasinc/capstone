using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Text.RegularExpressions;

public class OSCReceiver1 : MonoBehaviour {

//	public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
	public string RemoteIP = "192.168.42.57";
	int SendToPort = 9001; //the port you will be sending from
	int ListenerPort = 9002; //the port you will be listening on
	//public var controller : Transform;
	//public string gameReceiver = "sphere"; //the tag of the object on stage that you want to manipulate
	private Osc handler;
	private Osc handler1;

	public string sphere_target = "Sphere";

//	//reference to InterfaceScript.cs allows for the change of the status box upon receving UDP data
//	private InterfaceScript gui_script;
//	private bool trial_is_active = false;
//	private bool target_is_tracked = false;
//	private bool tracking_is_locked = false;

	//this is for the trackable variable
	private ExperimentTrackableEventHandler trackHandler;
	private Computation compute;

//	//VARIABLES YOU WANT TO BE ANIMATED
//	private int yRot = 0; //the rotation around the y axis
//	private float orig_dim = 1.42f;//initial dimensions
//	private float dim = 1.42f;//initial dimensions
	//private var sphere = GameObject.Find(gameReceiver);

	// Use this for initialization
	void Start ()
	{

		// get Interface Script
		//gui_script = GameObject.Find("GUI").gameObject.GetComponent<InterfaceScript>();
		compute = GameObject.Find("computation_script").gameObject.GetComponent<Computation>();

		//Initializes on start up to listen for messages
		//make sure this game object has both UDPPackIO and OSC script attached

		UDPPacketIO udp = GetComponent<UDPPacketIO>();
		udp.init(RemoteIP, SendToPort, ListenerPort);
		handler = GetComponent<Osc>();
		handler.init(udp);
		handler.SetAllMessageHandler(AllMessageHandler);


	}

	void Update () {

	}

	public void SendMessage(string m){
		OscMessage oscM = Osc.StringToOscMessage(m);
		handler.Send (oscM);
	}

	// parse osc string message
	// return substring that is matched for message type in the allMessageHandler function
	private string[] ParseOscMessageForType(string msg){
		// example - trial_status:start
		// example - sphere_sizes
		string[] msg_array = new string[2];
		msg_array[0] = msg.Split(":"[0])[0];
		msg_array[1] = msg.Split(":"[0])[1];
		return msg_array;
	}

	public void AllMessageHandler(OscMessage message){

		Debug.Log ("yessire!");
		//handler.Send("/message received");

		string msgString = Osc.OscMessageToString(message); //the message and value combined
		Debug.Log ("received message:" + msgString);
		msgString = msgString.Split("/"[0])[1];
		string[] msgType = ParseOscMessageForType(msgString);
//		SendMessage ("message_received:ready");

//		OscMessage oscM = Osc.StringToOscMessage("message received");
//		handler1.Send(oscM);

		Debug.Log ("here!");
		//Debug.Log(msgString); //log the message and values coming from OSC
		Debug.Log("message type is:" + msgType[0]);
		Debug.Log("message content is:" + msgType[1]);

		//FUNCTIONS YOU WANT CALLED WHEN A SPECIFIC MESSAGE IS RECEIVED
		switch (msgType[0]){
		case "trial_status":
			compute.ParseTrialStatus(msgType[1]);
			break;
		case "sphere_sizes":
			compute.SetSphereSizes(msgType[1]);
			break;
		default:
			Debug.Log("we dont have a match");
			break;
		}
	}

//	//FUNCTIONS CALLED BY MATCHING A SPECIFIC MESSAGE IN THE ALLMESSAGEHANDLER FUNCTION
//	public void Rotate(string msgValue) //rotate the sphere around its axis
//	{
//		yRot = Int32.Parse(msgValue);
//	}

}





//These functions are called when messages are received
////Access values via: oscMessage.Values[0], oscMessage.Values[1], etc






