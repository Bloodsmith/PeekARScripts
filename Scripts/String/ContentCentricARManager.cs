
using UnityEngine;
using System;
using System.Collections;

public class ContentCentricARManager : StringCam //MarkerEvents lives in Tracker.cs 
{
	public GameObject[] markerObjects;
	protected Vector4 col = new Vector4(1.13f, 1.13f, 1.1f, 1f);
	public string[] markerNames;
	
	new protected void Start() 
	{
		base.Start();
		
		// Load some image targets
		for (uint i = 0; i < markerObjects.Length; i++)
		{
			//LoadMarkerImage("String Markers/Marker " + (i + 1) + ".png");
			LoadMarkerImage("Norm Li Markers/" + markerNames[i] + ".png");
		}
		
		oldColour = new Vector4(1, 1, 1, 1);
	}
	
	
	private void TargetRest() {
		
		transform.parent = null;
		transform.localPosition = new Vector3(1000000, 0, 0);
		transform.localRotation = Quaternion.identity;
		
	}
	
	
//	protected override void ProcessIfNecessary() {
//		base.ProcessIfNecessary();
//		MarkerCam.projectionMatrix = camera.projectionMatrix;
//	}
	
	/**
	 * Updates markerCam with current marker(s) -- Needs to be enhanced to deal with multiple markers
	 */ 
	private void TargetMarker(StringCam.MarkerInfo markerInfo) {
		
		//Mar 22nd - Need a separate camera to absorb non-locked updates and do screen space calc's
		//!!!! try using parent switching to lock in target for stability and then transform !!!!
		//switch parents and then lerp
		
		if((currentMarkerInfo.rotation.eulerAngles - lastRotation).magnitude < jitterTolerance){
			transform.parent = markerObjects[markerInfo.imageID].transform;

			transform.localRotation = Quaternion.Inverse(markerInfo.rotation);
			transform.localPosition = transform.localRotation * -markerInfo.position;
		}
		
	}
	
	//For JeffARManager Use
	protected uint currentMarkerID;
	protected uint currentMarkerCount;
	
	public float SpotTime = 0.3f;
	public float goodSpotTime = 0;
	float steadyTolerance = 1f;
	float lostTime = 0f;
	float lostTimeTolerance = 1f;
	float jitterTolerance = 3f;
	
	public Vector3 lastPosition;
	public Vector3 lastRotation;
	public bool steady = false;
	
	protected Vector4 oldColour;
	//test
	public static Vector4 oldColourShare;
	
	public event Action Locking;  	//Continuous Event
	public event Action Locked;		//Discrete Event
	public event Action Lost;		//Discrete Event
	
	//update for multi markers
	public StringCam.MarkerInfo currentMarkerInfo;
	private MarkerState state = MarkerState.Lost;
	
	public MarkerState State { get { return state; } }
	
	public float LockPercentage(){ return goodSpotTime/SpotTime; }
	
	public float lostTimePercentage(){ return lostTime / lostTimeTolerance; }
	
	new protected void Update() 
	{
		//Debug.Log("CC AR MAN Update");
		// Update StringCam's internal state
		base.Update();
		
		//Debug.Log ("Running....");

		// Handle detected markers
		uint markerCount = GetDetectedMarkerCount();
		//currentMarkerCount = markerCount;
		for (uint i = 0; i < markerCount; i++)
		{
			//Debug.Log("ContentCentricARManager(31) - Marker Detected");
			// Fetch tracker data for this marker
			currentMarkerInfo = GetDetectedMarkerInfo(i);
			//currentMarkerID = currentMarkerInfo.imageID;
			//Debug.Log (currentMarkerInfo.imageID);
			//oldColour = currentMarkerInfo.color;
			//Test
			//oldColourShare = col;
			//Debug.Log ("Colour Share: R:" + oldColourShare.x + " G:" + oldColourShare.y + " B:" + oldColourShare.z);
			
			col = currentMarkerInfo.color;
			
			if (currentMarkerInfo.imageID < markerObjects.Length)
			{
				
				//GameObject markerObj = markerObjects[currentMarkerInfo.imageID];
				
				TargetMarker (currentMarkerInfo);
				
				//Debug.Log("goodSpotTime " + goodSpotTime);
				
				// Orient the camera according to the marker
				if (goodSpotTime == SpotTime) {
					
					
					
					if (state != MarkerState.Locked && Locked != null) {
						//Debug.Log("CCARman Locked");
						Locked();
						//Add Green Label
//						go.GetComponent<LockingLabel>().CurrentLockState("Locked");
						//GameObject.Find("PeekARGUIObject").GetComponent<LockingLabel>().CurrentLockState("Locked");
						state = MarkerState.Locked;
					}
					
				}
				
				else {
					
					//TargetRest();
					
					//Continuous Event
					if (Locking != null) {
						Locking();
						//GameObject.Find("PeekARGUIObject").GetComponent<LockingLabel>().CurrentLockState("Locking");
						state = MarkerState.Locking;
					}
					
				}
				
				goodSpotTime += Time.deltaTime;
				goodSpotTime = Mathf.Clamp(goodSpotTime, 0, SpotTime);
				
				steady = (currentMarkerInfo.rotation.eulerAngles - lastRotation).magnitude < steadyTolerance && (currentMarkerInfo.position - lastPosition).magnitude < steadyTolerance/100;
				lastPosition = currentMarkerInfo.position;
				lastRotation = currentMarkerInfo.rotation.eulerAngles;
				
				break;
			}
		}
		
		//Lost the marker
		if (markerCount == 0) {  //DOESNT ACCOUNT FOR INDIVIDUAL MARKER LOSS WITH MULTIPLE MARKERS!!!
			
			if (state != MarkerState.Lost && Lost != null) {
				Lost();
				//Add Red Label
				//guiObject.GetComponent<LockingLabel>().CurrentLockState("Lost");
				//GameObject.Find("PeekARGUIObject").GetComponent<LockingLabel>().CurrentLockState("Lost");
				state = MarkerState.Lost;
			}
			goodSpotTime = 0;
		}
		
		if(state != MarkerState.Locked){ lostTime += 0.01f; }
		else{ lostTime = 0; }
		
		// Marker not spotted? Point camera away
		/*if (lastSpottedTime != Time.time)
		{
			//TargetRest();
		}*/
	}
	
}



public enum MarkerState { Locking, Locked, Lost };

public static class MarkerMethods {
	
	public static bool IsLocked(this MarkerState ms) { return ms == MarkerState.Locked; }
	public static bool IsLocking(this MarkerState ms) { return ms == MarkerState.Locking; }
	public static bool IsLost(this MarkerState ms) { return ms == MarkerState.Lost; }
}
