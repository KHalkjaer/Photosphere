using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereLoadMaterial : MonoBehaviour {

	public Transform leftChild;
	public Transform rightChild;
	public int number;

	void Start(){
		StartCoroutine(WaitForLoad());
	}

	IEnumerator WaitForLoad(){
		// This can't spawn at the same time as behaviour, because it needs information from there, so we wait to make sure behaviour is loaded in
		yield return new WaitForSeconds(0.1f);

		if(GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().imagePath != ""){
			leftChild = this.gameObject.transform.GetChild(0);
			rightChild = this.gameObject.transform.GetChild(1);

			StartCoroutine(LoadLeftSphereTextures());
			StartCoroutine(LoadRightSphereTextures());
		}
	}

	IEnumerator LoadLeftSphereTextures(){
		WWW www = new WWW("file://" + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().imagePath + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().leftSphereTextures[number]);
        yield return www;

        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;

        renderer = leftChild.GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
	}	

	IEnumerator LoadRightSphereTextures(){
		WWW www = new WWW("file://" + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().imagePath + GameObject.FindWithTag("Behaviour").GetComponent<PhotosphereBehaviour>().rightSphereTextures[number]);
        yield return www;

        Renderer renderer = leftChild.GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
	}	
}
