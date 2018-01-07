public var RemoteIP : String = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
public var SendToPort : int = 9000; //the port you will be sending from
public var ListenerPort : int = 8050; //the port you will be listening on
public var controller : Transform;
public var gameReceiver = "Cube"; //the tag of the object on stage that you want to manipulate
private var handler : Osc;
public var does_this_work = true;

//VARIABLES YOU WANT TO BE ANIMATED
private var yRot : int = 0; //the rotation around the y axis
private var orig_dim : int = 1;//initial dimensions
private var dim : int = 1;//initial dimensions
//private var cube = GameObject.Find(gameReceiver);

//find trackable status
private var trackHandler;

public function Start ()
{
	//Initializes on start up to listen for messages
	//make sure this game object has both UDPPackIO and OSC script attached

	var udp : UDPPacketIO = GetComponent("UDPPacketIO");
	udp.init(RemoteIP, SendToPort, ListenerPort);
	handler = GetComponent("Osc");
	handler.init(udp);
	handler.SetAllMessageHandler(AllMessageHandler);
	Debug.Log("Logging vuforia variables");
	//trackHandler = GameObject.Find("ImageTarget");
	Debug.Log(trackHandler);

//	var allComponents : Component[];
//	 allComponents = GameObject.Find("ImageTarget").gameObject.GetComponents (Component);
//	 for (var component : Component in allComponents) {
//	     Debug.Log(component.name);
//	 }
	//.gameObject.GetComponent.<ExperimentTrackableEventHandler>();
	//Debug.Log(UnityEngine.DefaultTrackableEventHandler);


}
Debug.Log("Running");

function Update () {
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

//These functions are called when messages are received
//Access values via: oscMessage.Values[0], oscMessage.Values[1], etc

public function AllMessageHandler(oscMessage: OscMessage){

	Debug.Log('received message');
	//handler.Send("/message received");

	var msgString = Osc.OscMessageToString(oscMessage); //the message and value combined
	var oscM = Osc.StringToOscMessage(msgString);
    handler.Send(oscM);
	Debug.Log(msgString); //log the message and values coming from OSC
	var msgValue = msgString.Split("/"[0])[1];
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
			Debug.Log('we dont have a match');
			break;
	}/*

		 switch (name) {
	     case "object":
	         // ...
	         break;
	     case "strawberry":
	         Eat(collision.gameObject);
	         break;
	 }*/
}


//FUNCTIONS CALLED BY MATCHING A SPECIFIC MESSAGE IN THE ALLMESSAGEHANDLER FUNCTION
public function Rotate(msgValue) : void //rotate the cube around its axis
{
	yRot = msgValue;
}

public function Scale(msgValue) : void //rotate the cube around its axis
{
	//dim = msgValue;
	//cube.transform.localScale = new Vector3(dim,dim,dim);
}



