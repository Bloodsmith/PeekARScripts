using UnityEngine;
using System;
using System.Collections;

/**
 * Tracks one object with another from the point of view of camera 'Cam'
 */ 
public class Tracker : MonoBehaviour {
	
	public float ResetTime;
	
	public ContentCentricARManager CamMan;
	public GameObject child;
	
	private float depth;
	private JeffARManager Jman;
	//float iPadAdjustment = 0.27f;
	float iPadAdjustment = 0.15f;
	public float percent = 0f;
	// Use this for initialization
	
	Texture2D green;
	Texture2D yellow;
	Texture2D red;
	
	void Start () {
		
		origin = transform.localPosition;
		if(iPhone.generation == iPhoneGeneration.iPad2Gen || iPhone.generation == iPhoneGeneration.iPadMini1Gen || Application.isEditor){
			transform.localScale *= 0.75f;
		}
		depth = Camera.main.WorldToViewportPoint(transform.position).z;
		
		Jman = Camera.mainCamera.GetComponent<JeffARManager>();
		
		green = (Texture2D) Resources.Load("UI/Tracker/FEEDBACK_green");
		yellow = (Texture2D) Resources.Load("UI/Tracker/FEEDBACK_yellow");
		red = (Texture2D) Resources.Load("UI/Tracker/FEEDBACK_red");
	}
	
	private Vector3 tgt;
	private Vector3 origin;
	
	
	private float t = 0;
	private float resetTimer = 0;
	private float near = 0f;
	float d = 10;
	
	// Update is called once per frame
	void Update () {
		renderer.enabled = Jman.on;
		child.renderer.enabled = Jman.on;
		
		near = Camera.main.nearClipPlane + 0.5f;
		
		if(Jman.goodSpotTime / Jman.SpotTime >= percent)
			percent = Jman.goodSpotTime / Jman.SpotTime;
		else
			percent = Mathf.Clamp(percent - 0.03f,0f,1);
		
		
		StringCam.MarkerInfo mi = Jman.currentMarkerInfo;
	
		GameObject markerObj = Jman.markerObjects[mi.imageID];
		
		
		Vector3 marker = Camera.main.WorldToViewportPoint(markerObj.transform.position);
		
		
		Vector3 newPosition = new Vector3(
			0.5f + (marker.x - 0.5f) * percent,
			iPadAdjustment + (marker.y - iPadAdjustment) * percent, 
			near + (marker.z - near) * percent/2f);
		
		transform.position = Camera.main.ViewportToWorldPoint(newPosition);
		
		if(percent == 1){
			renderer.material.mainTexture = green;
		}
		else if(percent == 0) {
			renderer.material.mainTexture = red;
		} 
		else {
			renderer.material.mainTexture = yellow;
			
		}
		
	}
	
}


public interface MarkerEvents {
	
	event Action<Camera> Locking;  	//Continuous Event
	event Action<Camera> Locked;	//Discrete Event
	event Action Lost;				//Discrete Event
}