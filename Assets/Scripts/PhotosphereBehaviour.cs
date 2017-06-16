using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotosphereBehaviour : MonoBehaviour {
	
	[Header("Variables")]
	public int sphereCount;

	[Header("Paths")]
	public string imagePath;
	public string floorplanPath;
	public string leftSphere;
	public string rightSphere;
	public List<string> leftSphereTextures;
	public List<string> rightSphereTextures;

	[Header("State Machine")]
	public string state;

	[Header("Objects")]
	public Transform sphere;
	public Transform newestSphere;
	[DontSaveMember]public GameObject quad;
	
	[Header("Style Settings")]
	[DontSaveMember]public GUIStyle style;
	
	void Start () {
		// Nothing happening here at the moment 
	}

	// Switch floorplan material when the path is changed
	IEnumerator LoadFloorTexture(){
		WWW www = new WWW("file://" + imagePath + floorplanPath);
        yield return www;

        Renderer renderer = quad.GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
	}	
	
	// Update is called once per frame
    void Update()
    {
    	if (state == "placeSphere"){
	        if (Input.GetMouseButtonDown(0)) {

	        	// Raycast from camera to mouseposition
	            RaycastHit hit;
	            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

	            if (Physics.Raycast(ray, out hit)){
	            	// Create sphere at raycast hit location
	                Transform newSphere = Instantiate(sphere,hit.point,Quaternion.identity);
	                newestSphere = newSphere;
	                newestSphere.GetComponent<SphereLoadMaterial>().number = sphereCount;
					
        			// Increment Count and go back to Main State
        			sphereCount++;
	                state = "main";
	            }
	        }
    	}
    }

    void OnGUI() {
    	// Debug
    	GUI.Label(new Rect(Screen.width-100, 0, 100, 20), state, style);

    	// Main Screen
    	if(state == "main"){
    		// Main screen button
    		if (GUI.Button(new Rect(Screen.width-1.1f*Screen.width/7, Screen.height-3.75f*Screen.height/9, Screen.width/7, Screen.height/9), "Set Image Path")){
	        	state = "setImagePath";
	        }

	        // Main screen button
	        if (GUI.Button(new Rect(Screen.width-1.1f*Screen.width/7, Screen.height-2.5f*Screen.height/9, Screen.width/7, Screen.height/9), "Load Floorplan")){
	        	state = "loadFloor";
	        }

	        // Main screen button
	       	if (GUI.Button(new Rect(Screen.width-1.1f*Screen.width/7, Screen.height-1.25f*Screen.height/9, Screen.width/7, Screen.height/9), "Place Photosphere")){
	        	state = "defineSphere";
	        }
    	}

    	// Change Imagepath
    	if(state == "setImagePath"){

    		// Set variable based on text field
    		GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-20, 100, 20), "Image Path:", style);
        	imagePath = GUI.TextField(new Rect(Screen.width/2-Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/18), imagePath, 50);
	       	
	       	if (GUI.Button(new Rect(Screen.width/2-Screen.width/14,  Screen.height/2+Screen.height/14, Screen.width/7, Screen.height/9), "Set")){
	        	state = "main";
	        } 
    	}

    	// Load Floorplan
    	if(state == "loadFloor"){

    		if(!quad){
    			quad = GameObject.FindWithTag("Floorplan");
    		}

    		// Set variable based on text field
    		GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-20, 100, 20), "Floorplan Image Name:", style);
        	floorplanPath = GUI.TextField(new Rect(Screen.width/2-Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/18), floorplanPath, 50);
	       	
	       	if (GUI.Button(new Rect(Screen.width/2-Screen.width/14,  Screen.height/2+Screen.height/14, Screen.width/7, Screen.height/9), "Load")){
	        	StartCoroutine(LoadFloorTexture());
	        	state = "main";
	        } 
    	}

    	// Settings for new photosphere
    	if(state == "defineSphere"){
    		// Left Eye
    		// Set variable based on text field
       		GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-Screen.height/9-20, 100, 20), "Left Sphere Name:", style);
        	leftSphere = GUI.TextField(new Rect(Screen.width/2-Screen.width/4, Screen.height/2-Screen.height/9, Screen.width/2, Screen.height/18), leftSphere, 50);

        	// Right Eye
        	// Set variable based on text field
        	GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-20, 100, 20), "Right Sphere Name:", style);
        	rightSphere = GUI.TextField(new Rect(Screen.width/2-Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/18), rightSphere, 50);

        	if (GUI.Button(new Rect(Screen.width/2-Screen.width/7-Screen.width/21,  Screen.height/2+Screen.height/10, Screen.width/7, Screen.height/9), "Load")){
        		leftSphereTextures.Add(leftSphere);
        		rightSphereTextures.Add(rightSphere);

	        	state = "placeSphere";
	        }

			if (GUI.Button(new Rect(Screen.width/2+Screen.width/21,  Screen.height/2+Screen.height/10, Screen.width/7, Screen.height/9), "Cancel")){
	        	state = "main";
	        }
    	}
    }
}
