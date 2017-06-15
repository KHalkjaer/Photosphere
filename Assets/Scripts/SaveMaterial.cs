using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMaterial : MonoBehaviour {

	void Start(){
		StartCoroutine(WaitForLoad());
	}

	IEnumerator WaitForLoad(){
		yield return new WaitForSeconds(0.1f);

		if(GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().floorplanPath != ""){
			StartCoroutine(LoadFloorTexture());
		}
	}

	IEnumerator LoadFloorTexture(){
		WWW www = new WWW("file://" + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().imagePath + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().floorplanPath);
        yield return www;

        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
	}	
}
