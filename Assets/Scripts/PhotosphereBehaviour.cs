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
	
	// Use this for initialization
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
    		if (GUI.Button(new Rect(Screen.width-130, Screen.height-50, 120, 40), "Set Image Path")){
	        	state = "setImagePath";
	        }

	        if (GUI.Button(new Rect(Screen.width-130, Screen.height-100, 120, 40), "Load Floorplan")){
	        	state = "loadFloor";
	        }

	       	if (GUI.Button(new Rect(Screen.width-130, Screen.height-150, 120, 40), "Place Photosphere")){
	        	state = "defineSphere";
	        }
    	}

    	// Change Imagepath
    	if(state == "setImagePath"){

    		GUI.Label(new Rect(Screen.width/2-250, Screen.height/2-20, 100, 20), "Image Path:", style);
        	imagePath = GUI.TextField(new Rect(Screen.width/2-250, Screen.height/2, 500, 20), imagePath, 50);
	       	
	       	if (GUI.Button(new Rect(Screen.width/2-25,  Screen.height/2+30, 50, 40), "Set")){
	        	state = "main";
	        } 
    	}

    	// Load Floorplan
    	if(state == "loadFloor"){

    		if(!quad){
    			quad = GameObject.FindWithTag("Floorplan");
    		}

    		GUI.Label(new Rect(Screen.width/2-250, Screen.height/2-20, 100, 20), "Floorplan Image Name:", style);
        	floorplanPath = GUI.TextField(new Rect(Screen.width/2-250, Screen.height/2, 500, 20), floorplanPath, 50);
	       	
	       	if (GUI.Button(new Rect(Screen.width/2-25,  Screen.height/2+30, 50, 40), "Load")){
	        	StartCoroutine(LoadFloorTexture());
	        	state = "main";
	        } 
    	}

    	// Settings for new photosphere
    	if(state == "defineSphere"){
    		// Left Eye
       		GUI.Label(new Rect(Screen.width/2-250, Screen.height/2-70, 100, 20), "Left Sphere Name:", style);
        	leftSphere = GUI.TextField(new Rect(Screen.width/2-250, Screen.height/2-50, 500, 20), leftSphere, 50);

        	// Right Eye
        	GUI.Label(new Rect(Screen.width/2-250, Screen.height/2-20, 100, 20), "Right Sphere Name:", style);
        	rightSphere = GUI.TextField(new Rect(Screen.width/2-250, Screen.height/2, 500, 20), rightSphere, 50);

        	if (GUI.Button(new Rect(Screen.width/2-75,  Screen.height/2+30, 50, 40), "Load")){
        		leftSphereTextures.Add(leftSphere);
        		rightSphereTextures.Add(rightSphere);

	        	state = "placeSphere";
	        }

			if (GUI.Button(new Rect(Screen.width/2+25,  Screen.height/2+30, 50, 40), "Cancel")){
	        	state = "main";
	        }
    	}
    }
}
