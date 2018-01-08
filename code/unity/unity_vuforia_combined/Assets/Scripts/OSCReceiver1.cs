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
	public string gameReceiver = "Cube"; //the tag of the object on stage that you want to manipulate
	private Osc handler;

	//VARIABLES YOU WANT TO BE ANIMATED
	private int yRot = 0; //the rotation around the y axis
	private int orig_dim = 1;//initial dimensions
	private int dim = 1;//initial dimensions
	//private var cube = GameObject.Find(gameReceiver);

	//this is for the trackable variable
	public ExperimentTrackableEventHandler trackHandler;

	// Use this for initialization
	void Start ()
	{
		//Initializes on start up to listen for messages
		//make sure this game object has both UDPPackIO and OSC script attached

		UDPPacketIO udp = GetComponent<UDPPacketIO>();
		udp.init(RemoteIP, SendToPort, ListenerPort);
		handler = GetComponent<Osc>();
		handler.init(udp);
		handler.SetAllMessageHandler(AllMessageHandler);

		//check if the target is visible
		trackHandler = GameObject.Find ("ImageTarget").gameObject.GetComponent<ExperimentTrackableEventHandler> ();
		Debug.Log ("let's see if the target is visible, from OSCReceiver1 file");
		Debug.Log(trackHandler.targetVisible());



	}

	void Update () {
		var cube = GameObject.Find(gameReceiver);
		var space = Input.GetKey(KeyCode.Space);

		if(space){
			Debug.Log("space pressed");
			cube.transform.Rotate(0, 0, 0);
		}else{
			cube.transform.Rotate(1, 1, 1);
		}

		cube.transform.localScale = new Vector3(dim,dim,dim);

	}

	//returns true if image target is found, false if it is not
	public string TestCommunication(){
		return "hey there";
	}

	public void AllMessageHandler(OscMessage message){

		Debug.Log("received message");
		//handler.Send("/message received");

		string msgString = Osc.OscMessageToString(message); //the message and value combined
		OscMessage oscM = Osc.StringToOscMessage(msgString);
		handler.Send(oscM);
		Debug.Log(msgString); //log the message and values coming from OSC
		string msgValue = msgString.Split("/"[0])[1];
		//Debug.Log(msgValue);
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
		default:
			Debug.Log("we dont have a match");
			break;
		}
	}

	//FUNCTIONS CALLED BY MATCHING A SPECIFIC MESSAGE IN THE ALLMESSAGEHANDLER FUNCTION
	public void Rotate(string msgValue) //rotate the cube around its axis
	{
		yRot = Int32.Parse(msgValue);
	}

//	public void Scale(string msgValue) //rotate the cube around its axis
//	{
//		//dim = msgValue;
//		//cube.transform.localScale = new Vector3(dim,dim,dim);
//	}

}





//These functions are called when messages are received
////Access values via: oscMessage.Values[0], oscMessage.Values[1], etc






