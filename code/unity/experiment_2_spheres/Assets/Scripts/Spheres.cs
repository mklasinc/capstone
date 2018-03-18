using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Spheres : MonoBehaviour {

	public GameObject l_sphere;
	public GameObject r_sphere;
	private float orig_dim = 1.42f;
	private float r_dim = 1.42f;// initial dimensions, right side
	private float l_dim = 1.42f;//initial dimensions, left side
	bool visibility = true;
	// Use this for initialization
	void Start () {
		l_sphere = GameObject.Find ("LeftSphere");
		r_sphere = GameObject.Find ("RightSphere");

	}
	
	// Update is called once per frame
	void Update () {
		l_sphere.transform.localScale = new Vector3(l_dim,l_dim,l_dim);
		r_sphere.transform.localScale = new Vector3(r_dim,r_dim,r_dim);
			
	}

	public void UpdateSize(int size_left, int size_right){
			l_dim = orig_dim * size_left;
			r_dim = orig_dim * size_right;
	}

	public void Hide(){
		visibility = false;
//		l_sphere.GetComponent<Renderer>().enabled = false;
//		r_sphere.GetComponent<Renderer>().enabled = false;
		 l_sphere.SetActive(false);
		r_sphere.SetActive(false);
	}

	public void Show(){
		visibility = true;
		l_sphere.GetComponent<Renderer>().enabled = true;
		r_sphere.GetComponent<Renderer>().enabled = true;
//		l_sphere.SetActive(true);
//		r_sphere.SetActive(true);
	}

	public void TrackingFound(string sphere, bool tracking_found){
		Debug.Log ("sphere name is: " + sphere);
		if (sphere == "left") {
//			Debug.Log ("right sphere status change!");
			l_sphere.GetComponent<SphereManager>().SetTrackingStatus (tracking_found);
		} else if (sphere == "right") {
//			Debug.Log ("right sphere status change!");
			r_sphere.GetComponent<SphereManager>().SetTrackingStatus (tracking_found);
		}
	}

	public bool SpheresAreVisible(){
		if (l_sphere.GetComponent<SphereManager> ().GetTrackingStatus () == true && r_sphere.GetComponent<SphereManager> ().GetTrackingStatus () == true) {
			return true;
		} else {
			return false;
		}
	}

	public bool Visibility(){
		return visibility;
	}
}



