/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class ExperimentTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;

		private bool TrackableStatus = false;

		private OSCReceiver1 osc;

		private Computation compute;

		private Spheres spheres;

		private bool osc_ready = false;

		private bool computation_script_ready = false;

		#endregion // PRIVATE_MEMBER_VARIABLES

		public bool is_seen_bool = false;

		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}

			//set up osc script, do it here and use a 'ready' boolean to avoid runtime errors
			osc = GameObject.Find ("osc_script").gameObject.GetComponent<OSCReceiver1> ();
			spheres = GameObject.Find ("spheres_script").gameObject.GetComponent<Spheres> ();
			osc_ready = true;

			//set up connection with computation script
			compute = GameObject.Find ("computation_script").gameObject.GetComponent<Computation> ();
			Debug.Log ("let's see if the communication is possible");
			//Debug.Log(osc.TestCommunication());
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				spheres = GameObject.Find ("spheres_script").gameObject.GetComponent<Spheres> ();
				TrackableStatus = true;
				OnTrackingFound();
				//send message to osc script
//				if(osc_ready){
//					osc.TargetTracking (true);	
//				}
				// relay tracking data to computation script
//				if(computation_script_ready){
//					compute.TargetTrackingState (true);	
//				}

			}
			else
			{
				spheres = GameObject.Find ("spheres_script").gameObject.GetComponent<Spheres> ();
				TrackableStatus = false;
				OnTrackingLost();
				//send message to osc script
//				if(osc_ready){
//					osc.TargetTracking (false);	
//				}

				// relay tracking data to computation script
//				if(computation_script_ready){
//					compute.TargetTrackingState (true);	
//				}
			}
		}

		//returns true if image target is found, false if it is not
		public bool targetVisible(){
			return TrackableStatus;
		}
		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		private void OnTrackingFound()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			// methods used in the experiment
		
			spheres.TrackingFound (mTrackableBehaviour.TrackableName,true);
			//			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

		}


		private void OnTrackingLost()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			// custom methods

			spheres.TrackingFound (mTrackableBehaviour.TrackableName,false);
//			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}