using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosphereBehaviour : MonoBehaviour {
	
	[Header("Variables")]
	public int sphereCount;
	public bool legacyUI;
	public SaveLoadUtility slu;

	[Header("Paths")]
	public string imagePath;
	public string floorplanPath;
	public string leftSphere;
	public string rightSphere;
	public List<string> leftSphereTextures;
	public List<string> rightSphereTextures;
	public string savePath;

	[Header("State Machine")]
	public string state;

	[Header("Objects")]
	public GameObject UI;
	public Transform sphere;
	public Transform newestSphere;
	[DontSaveMember]public GameObject quad;
	
	[Header("Style Settings")]
	[DontSaveMember]public GUIStyle style;
	
	// Initialization
	void Start () {
		UI = GameObject.FindWithTag("UI");
		quad = GameObject.FindWithTag("Floorplan");
	
		if(slu == null) {
			slu = GetComponent<SaveLoadUtility>();
			if(slu == null) {
				Debug.Log("[SaveLoadMenu] Start(): Warning! SaveLoadUtility not assigned!");
			}
		}

		// Debug
		Debug.Log(Application.dataPath);
	}

	// Switch floorplan material when the path is changed
	IEnumerator LoadFloorTexture(){
		WWW www = new WWW("file://" + imagePath + floorplanPath);
        yield return www;

        Renderer renderer = quad.GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
	}	
	
	// Update is called once per frame
    void Update() {
    	if (state == "placeSphere"){
    		// Placing Spheres on Desktop
    		if (SystemInfo.deviceType == DeviceType.Desktop){
		        if (Input.GetButtonDown("Fire1")) {

		        	// Raycast from camera to mouse position
		            RaycastHit hit;
		            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		            if (Physics.Raycast(ray, out hit)){
		            	// Create sphere at raycast hit location
		                Transform newSphere = Instantiate(sphere,hit.point,Quaternion.identity);
		                newestSphere = newSphere;
		                newestSphere.GetComponent<SphereLoadMaterial>().number = sphereCount;
						
	        			// Increment Count and go back to Main State
	        			sphereCount++;
		                ChangeState("main");
		            }
		        }
    		}

    		// Placing Spheres on Mobile
    		if (SystemInfo.deviceType == DeviceType.Handheld){
    			// Do things
    		}
    	}
    } 

    public void ChangeState(string newState){
    	state = newState;

    	// If 
    	if(newState != "main"){
    		UI.transform.GetChild(0).gameObject.SetActive(false);

    		// Image Path
    		if(newState == "setImagePath"){
    			UI.transform.GetChild(1).gameObject.SetActive(true);
    		}
    		if(newState == "pathSet"){
    			imagePath = UI.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<InputField>().text;
    			ChangeState("main");
    		}

    		// Floorplan
    		if(newState == "loadFloor"){
    			UI.transform.GetChild(2).gameObject.SetActive(true);
    		}
    		if(newState == "floorSet"){
    			floorplanPath = UI.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<InputField>().text;
    			StartCoroutine(LoadFloorTexture());
    			ChangeState("main");
    		}

    		// Define Sphere
    		if(newState == "defineSphere"){
    			UI.transform.GetChild(3).gameObject.SetActive(true);
    		}

    		// Place Sphere
    		if(newState == "placeSphere"){
    			// Disable all ui elements
    			UI.transform.GetChild(0).gameObject.SetActive(false);
	    		UI.transform.GetChild(1).gameObject.SetActive(false);
	    		UI.transform.GetChild(2).gameObject.SetActive(false);
	    		UI.transform.GetChild(3).gameObject.SetActive(false);
	    		//

    			leftSphereTextures.Add(UI.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<InputField>().text);
	        	rightSphereTextures.Add(UI.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<InputField>().text);
    		}

    		if(newState == "saveGame"){
    			slu.SaveGame(UI.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<InputField>().text);
    			Debug.Log(UI.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<InputField>().text);
    			ChangeState("Main");
    		}    		

    		if(newState == "loadGame"){
    			//slu.SaveGame(UI.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<InputField>().text);
    			slu.LoadGame(UI.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<InputField>().text);
    			ChangeState("Main");
    		}
    	}

    	// Activate the main menu elements
    	else{
    		UI.transform.GetChild(0).gameObject.SetActive(true);
    		UI.transform.GetChild(1).gameObject.SetActive(false);
    		UI.transform.GetChild(2).gameObject.SetActive(false);
    		UI.transform.GetChild(3).gameObject.SetActive(false);
    	}
    }

    void OnGUI() {
    	// Current State Debug
    	GUI.Label(new Rect(Screen.width-100, 0, 100, 20), state, style);

    	if(legacyUI){
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
}
