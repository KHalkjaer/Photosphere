using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour {

	public Transform player;

	public GameObject leftSphere;
	public GameObject rightSphere;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			RaycastHit objectHit;

			Vector3 fwd = this.transform.TransformDirection(Vector3.forward);
     		Debug.DrawRay(this.transform.position, fwd * 50, Color.green);

     		if (Physics.Raycast(this.transform.position, fwd, out objectHit, 50)){

			 	Debug.Log(objectHit.transform.name);

			 	// Deactivate Current Spheres
			 	leftSphere.SetActive(false);
			 	rightSphere.SetActive(false);

			 	// Set New Spheres and Activate Them
			 	leftSphere = objectHit.transform.GetChild(0).gameObject;
				objectHit.transform.GetChild(0).gameObject.SetActive(true);

				rightSphere = objectHit.transform.GetChild(1).gameObject;
				objectHit.transform.GetChild(1).gameObject.SetActive(true);
				// ---

				// Move Player
				player.position = objectHit.transform.position;
			}
        }
	}
}
