using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;


public class MainInterface : MonoBehaviour {
	private JeffARManager camManager;	//this should be at top of script
	int markerID = 0;
	//use 1.086f for large markers (poster size), 3f for small markers (letter size) based on current furniture scale
	float Marker1Scale = 1.086f;
	float Marker2Scale = 1.086f;
	float Marker3Scale = 1.086f;
	float Marker4Scale = 1.086f;
	float[] markerScale;
	
	//Touch Restrictions
	int minX = 25;
	int maxX = 320;
	int minY = 575;
	int maxY = 1050;
	int MaxHeightCheck = 1536;
	
	int MaxFurniture = 6;
	
	PeekGUI menu;
	
	public enum Mode { Delete, Manip };
	
	public Mode mode = Mode.Manip;
	
	//Action for active furniture changes
	public event Action<int> AmountFurniture;
	
	public void SetMarkerScale(float one, float two, float three, float four){
		Marker1Scale = one;
		Marker2Scale = two;
		Marker3Scale = three;
		Marker4Scale = four;
		markerScale = new float[]{Marker1Scale, Marker2Scale, Marker3Scale, Marker4Scale};
	}
	
	// Use this for initialization
	void Start () {
		markerScale = new float[]{Marker1Scale, Marker2Scale, Marker3Scale, Marker4Scale};
		if(Startup.GetGeneration() == "iPad2"){
			minX /= 2;
			maxX /= 2;
			minY /= 2;
			maxY /= 2;
			MaxHeightCheck /= 2;
			MaxFurniture = 3;
		}
		menu = GameObject.Find ("PeekARGUIObject").GetComponent<PeekGUI>();
		menu.DeleteBtn += () => {
			mode = Mode.Delete;};
		menu.CancelDelete += () => {mode = Mode.Manip;};
		menu.InfoClick += () => Startup.AddWelcomeScreen();
		menu.ClearBtn += () => DeleteAll();
		
		camManager = this.GetComponent<JeffARManager>();
		
		tplane = TouchPlane.GroundPlane(camera);
		
		camManager.Locked 	+= () => {
			
			camState = MarkerState.Locked;
		
		};
		camManager.Lost 	+= () => {
			
			camState = MarkerState.Lost;
		};
		
	}
	
	private MarkerState camState = MarkerState.Lost;
	
    void FixedUpdate () {
		
		AmountFurniture(ActiveFurn.Count);
		//Sanitize list from nulls
		ActiveFurn.RemoveAll( (obj) => obj == null );
		if (mode == Mode.Delete)
			doDeleteMode();
			
		else if (mode == Mode.Manip)
			doManipMode();
		
	}
	
	public TouchPlane tplane;
	
	private Vector3 moveOffset;
	private GameObject grabObj;
	private GameObject rotateObj;
	private Vector3 screenStartTouchPoint;
	private Vector3 objStartPosition;
	float scale;
	
	
	public static void DeleteAll() {
		Debug.Log ("MainInterface DeleteAll");
		foreach (GameObject go in ActiveFurn){
			go.AddComponent<Deleter>();
		}
			
		ActiveFurn.Clear();
		Camera.mainCamera.GetComponent<MainInterface>().mode = Mode.Manip;
	}
	
	//Public object to destroy
	static GameObject go;
	//Method to destroy public object and release memory
	public static void ReleaseMemory(){
		Destroy(go);
		Resources.UnloadUnusedAssets();
	}
	
	void doDeleteMode() {
		if (Input.touchCount > 0) {
			
			Touch f0 = Input.GetTouch(0);
			if(!(f0.position.x > minX && f0.position.x < maxX && f0.position.y < maxY && f0.position.y > minY)){
				go = FurnitureRayCast(f0);
				if (go != null) {
					go.AddComponent<Deleter>();
					ActiveFurn.Remove(go);
					mode = Mode.Manip;
					menu.ResetManipulation();
				}
			}
			
		}
	}
	
	void doManipMode() {
		if(GUIUtility.hotControl == 0){
			if (Input.touchCount == 0) {
				
				if (grabObj != null) {
					
					grabObj.rigidbody.isKinematic = true;
				}
				grabObj = null;
			}
			
			else if (Input.touchCount == 1) {
				Debug.Log ("DoMove()");
				DoMove();
			}
			
			else if (Input.touchCount == 2) {
				Debug.Log ("DoRotate()");
				DoRotate ();
			}
		}
	}
	
	private void DoRotate() {
		
		//Save current move object for rotation 
		//and then disable moving object by setting null
		if (grabObj != null) {
			rotateObj = grabObj;
			grabObj = null; //Stop any moving
		}
		
		
		//If either finger touches an object, rotate that object,
		//otherwise rotate last moved object
		Touch f0 = Input.GetTouch(0);
		Touch f1 = Input.GetTouch(1);
		
		GameObject o1 = FurnitureRayCast(f0);
		GameObject o2 = FurnitureRayCast(f0);
		
		if (o1 != null)
			rotateObj = o1;
		
		else if (o2 != null)
			rotateObj = o2;
		
		
		//Average the movement amount of both fingers for rotation
		Vector3 avgDeltaPos = (f0.deltaPosition + f1.deltaPosition)/2;
		
		if (rotateObj != null)
			rotateObj.transform.Rotate(0, -avgDeltaPos.x * 0.1f, 0, Space.World);
	}
	
	private void DoMove() {
		
		Touch f0 = Input.GetTouch(0);
		
		/*
		 * Grabbing
		 * 1 finger touchdown will grab object under finger
		 * 2 finger touchany will cancel move 
		 */
		if (f0.phase == TouchPhase.Began) {
			
			grabObj = FurnitureRayCast(f0);
			screenStartTouchPoint = f0.position;
			objStartPosition = (grabObj != null) ? grabObj.transform.position : Vector3.zero;  //needs guard because RayCast() returns null if no object hit
			
			if(grabObj == null)
				return;
			
			Vector3 offsetPoint = tplane.PointOnPlaneFrom(screenStartTouchPoint);
			if(offsetPoint == Vector3.zero)
				return;
			
			Vector3 pop = tplane.PointOnPlaneFrom(f0);
	
			if(pop == Vector3.zero)
				return;
				
			scale = (camera.transform.position - grabObj.transform.position).magnitude / (camera.transform.position - pop).magnitude;
			
		
		}
			//Need to save touch pos for offset calc
		
		if (grabObj != null) {
			
			grabObj.rigidbody.isKinematic = false;
			
			Vector3 pop = tplane.PointOnPlaneFrom(f0);
			Debug.Log("pop " + pop);
			if(pop == Vector3.zero)
				return;
			
			Vector3 offsetPoint = tplane.PointOnPlaneFrom(screenStartTouchPoint);
			if(offsetPoint == Vector3.zero)
				return;
			
			
			Vector3 moveOffset = offsetPoint - pop;
			
			moveOffset *= scale;
			
			grabObj.transform.position = objStartPosition - moveOffset;
			float localMarkerScale = 25f;
			
			markerID = (int)camManager.currentMarkerInfo.imageID;
			if(markerID < 1){
				if(grabObj.transform.position.magnitude > localMarkerScale){
					grabObj.transform.position = (grabObj.transform.position / grabObj.transform.position.magnitude) * 25f;
				}
			}
			else{
				localMarkerScale /= 3f;
				if(grabObj.transform.position.magnitude > localMarkerScale){
					grabObj.transform.position = (grabObj.transform.position / grabObj.transform.position.magnitude) * localMarkerScale;
				}
			}
		}
	}
	
	/**
	 * Prioritizes Placemat object over others when casting
	 */ 
	public GameObject PlacerRayCast(Touch t) {
		
		Ray ray = camera.ScreenPointToRay(t.position);
		
		RaycastHit hitInfo;
			
		int pmask = LayerMask.NameToLayer("Placemat");
		bool rayDidHit = Physics.Raycast(ray, out hitInfo, Camera.main.farClipPlane, 1 << pmask);
		
		if (rayDidHit)
			return hitInfo.collider.gameObject;
		
		else
			return null;
	}
	
	public GameObject FurnitureRayCast(Touch t) {
		
		Ray ray = camera.ScreenPointToRay(t.position);
		ray.origin -= ray.direction * 1f;
		
		RaycastHit hitInfo;
			
		int mask = LayerMask.NameToLayer("Furniture");
		bool rayDidHit = Physics.Raycast(ray, out hitInfo, Camera.main.farClipPlane, 1 << mask);
		
		if (rayDidHit)
			return hitInfo.collider.gameObject;
		
		else
			return null;
	}
	
	/*
	void OnGUI() {
		
		//Needs to guard against null objects if deleter self-destructs gameobjects
		foreach (GameObject go in activeFurn) {
			Vector3 pos = go.transform.position;
			Vector3 s_pos = camera.WorldToScreenPoint(pos);
			GUI.Label(new Rect(s_pos.x, Screen.height - s_pos.y, 200, 75), go.name + ": " + go.transform.position);
			//Debug.Log("MainInterface(101) - ScreenPos of Object: " + s_pos);
		}
	}
	*/
	
	
	
	public static List<GameObject> ActiveFurn = new List<GameObject>();
	
	
	
	
	
	/**
	 * Hook from interface.
	 * 
	 * Adds furniture from Resources folder to marker origin.
	 * 
	 * Tracks furniture in active list.
	 * 
	 */ 
	public void PickFurniture(Furniture furniture) {
		
		if (ActiveFurn.Count > MaxFurniture - 1) return;
		
		GameObject furnPrefab = Resources.Load ("Prefab Models/" + furniture.GetName()) as GameObject;
		
		GameObject furn = null;
		
		if (furnPrefab != null) {
			furn = Instantiate(furnPrefab) as GameObject;
			int furnitureLayer = 11; //FOR TESTING
			//furn.layer = LayerMask.NameToLayer("Furniture");
			furn.layer = furnitureLayer;
		}
		
		if (furn != null) {
			ActiveFurn.Add (furn);
			markerID = (int)camManager.currentMarkerInfo.imageID;
			furn.transform.localScale *= markerScale[markerID];
			furn.transform.localScale = furn.transform.localScale * 1.2f;  //Scale correction
			furn.SetActiveRecursively(true);
			
			furn.GetComponent<FurnitureData>().furniture = furniture;
			
			
			furn.rigidbody.isKinematic = true;
			
			furn.AddComponent<EmptySpotDropper>();
			//furn.AddComponent<CollisionCheck>();
			
			AmountFurniture(ActiveFurn.Count);
			
		} else Debug.LogError("Received invalid furniture name: "+name);
		
	}
	
	public int GetNumActiveCategory(string category){
	
		int count = 0;
		
		foreach(GameObject furniture in ActiveFurn){
			
			if(furniture.GetComponent<FurnitureData>().furniture.GetCategoryString() == category)
				count ++;
			
		}
		
		return count;
	}
	
	public int GetNumActiveFurniture(string name){
	
		int count = 0;
		
		foreach(GameObject furniture in ActiveFurn){
			FurnitureData fd = furniture.GetComponent<FurnitureData>();
			if(fd.furniture.GetName() == name)
				count ++;
			
		}
		
		return count;
	}
	
	
}


public class PeekInput {
	
	public static event Action<Touch> On1Touch;
	public static event Action<Touch, Touch> On2Touch;
	
	public static event Action<Touch> OnTouchBegan;
	
	public static void UpdateInput() {
		
		if (Input.touchCount == 1) {
			
			if (On1Touch != null)
				On1Touch(Input.GetTouch(0));
			
			
		}
		
		else if (Input.touchCount == 2)
			On2Touch(Input.GetTouch(0), Input.GetTouch(1));
	}
}






