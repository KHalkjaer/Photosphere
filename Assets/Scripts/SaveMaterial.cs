﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaterial : MonoBehaviour {

	void Start(){
		StartCoroutine(WaitForLoad());
	}

	IEnumerator WaitForLoad(){
		// Wait for Behaviour to spawn
		yield return new WaitForSeconds(0.1f);
		// Load in .5 seconds after the final sphere
		yield return new WaitForSeconds(0.15f+0.25f*GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().sphereCount);

		if(GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().floorplanPath != ""){
			StartCoroutine(LoadFloorTexture());
		}
	}

	[InspectorButton("OnButtonClicked")]
	public bool loadFloor;

	private void OnButtonClicked()
	{
	   StartCoroutine(LoadFloorTexture());
	}

	IEnumerator LoadFloorTexture(){
		if(GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().webImages){
			WWW www = new WWW(GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().floorplanPath);
	        yield return www;

	        Renderer renderer = gameObject.GetComponent<Renderer>();
	        renderer.material.mainTexture = www.texture;
		}

		if(!GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().webImages){
			WWW www2 = new WWW("file://" + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().imagePath + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().floorplanPath);
	        yield return www2;

	        Renderer renderer = gameObject.GetComponent<Renderer>();
	        renderer.material.mainTexture = www2.texture;
		}
		
	}	
}
