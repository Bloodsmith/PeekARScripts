using UnityEngine;
using System.Collections;
using System;

public class CameraControls {
	
	string assetPath = "CameraControls/";
	
	GUIStyle style;
	Texture2D background;
	Texture2D freezeLabel;
	Texture2D liveLabel;
	Texture2D trackerLabelRedTex;
	Texture2D trackerLabelYellowTex;
	Texture2D trackerLabelGreenTex;
	
	//Rect for checking GUI Touch
	public Rect checkRect;
	//Rects
	Rect containerRect;
	Rect backgroundRect;
	Rect freezeRect;
	Rect liveRect;
	Rect snapshotRect;
	Rect labelRect;
	Rect trackerLabelRect;
	
	float tryToLockTime;
	float lockTimeout = 3;
	
	int buttonSize = 128;
	
	Button freezeButton;
	Button liveButton;
	Button snapshotButton;
	
	public event Action TakeScreenShot;
	
	bool Locked;
	
	Mode mode;
	enum Mode{
		live, freeze, tryToLock
	};
	
	JeffARManager jman;
	
	public CameraControls(string baseAssetPath){
		
		assetPath = baseAssetPath + assetPath;
		
		style = new GUIStyle();
		background = (Texture2D) Resources.Load(assetPath + "CLUSTER_small");
		
		 jman = Camera.mainCamera.GetComponent<JeffARManager>();
		jman.Locked += () => {Locked = true;};
		jman.Lost += () => {Locked = false;};
		
		GUIStyle buttonStyle = new GUIStyle();
		
		
		GUIStyle snapStyle = new GUIStyle(buttonStyle);
		snapStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_snapshot_avail");
		snapStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_snapshot_active");
		
		snapshotRect = ScaledRect.Rect(0, 0, snapStyle.normal.background.width,snapStyle.normal.background.height);
		snapshotButton = new Button(snapStyle, snapshotRect);
		snapshotButton.click += () => snapClick();
		
		
		GUIStyle freezeStyle = new GUIStyle(buttonStyle);
		freezeStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_freeze_avail");
		freezeStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_freeze_active");
		
		GUIStyle freezeToggleStyle = new GUIStyle(buttonStyle);
		freezeToggleStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_freeze_active");
		freezeToggleStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_freeze_active");
		
		freezeRect = ScaledRect.Rect(0,snapStyle.normal.background.height,freezeStyle.normal.background.width,freezeStyle.normal.background.height);
		
		freezeButton = new Button(freezeStyle, freezeToggleStyle, freezeRect);
		freezeButton.click += () => freezeModeClick();
		
		
		GUIStyle liveStyle = new GUIStyle(buttonStyle);
		liveStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_live_avail");
		liveStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_live_active");
		
		GUIStyle liveToggleStyle = new GUIStyle(buttonStyle);
		liveToggleStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_live_active");
		liveToggleStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_live_active");
		
		liveRect = ScaledRect.Rect(0, snapStyle.normal.background.height + freezeStyle.normal.background.height, liveStyle.normal.background.width,liveStyle.normal.background.height);
		liveButton = new Button(liveStyle, liveToggleStyle, liveRect);
		liveButton.click += () => liveModeClick();
		
		liveButton.toggled = true;
		
		
		
		freezeLabel =  (Texture2D) Resources.Load(assetPath + "STATE_freezeEnabled");
		liveLabel =  (Texture2D) Resources.Load(assetPath + "STATE_liveEnabled");
		
		labelRect = ScaledRect.Rect(35, snapStyle.normal.background.height + freezeStyle.normal.background.height + (liveStyle.normal.background.height), freezeLabel.width, freezeLabel.height);
		//labelRect = ScaledRect.Rect(0, 0, freezeLabel.width, freezeLabel.height);
		
		
		trackerLabelRedTex = (Texture2D) Resources.Load(assetPath + "FEEDBACK_red_text");
		trackerLabelYellowTex = (Texture2D) Resources.Load(assetPath + "FEEDBACK_yellow_text");
		trackerLabelGreenTex = (Texture2D) Resources.Load(assetPath + "FEEDBACK_green_text");
		
		trackerLabelRect = ScaledRect.Rect((ScaledRect.FullScreenRect.width/2) - 275, snapStyle.normal.background.height + freezeStyle.normal.background.height + (liveStyle.normal.background.height), trackerLabelRedTex.width, trackerLabelRedTex.height);
		
		containerRect = ScaledRect.Rect(73, 900, ScaledRect.FullScreenRect.width, snapStyle.normal.background.height + freezeStyle.normal.background.height + liveStyle.normal.background.height + liveLabel.height);
	
		backgroundRect = ScaledRect.Rect(0, 0, background.width, background.height);
		
		checkRect = ScaledRect.Rect(0, 1536 - (liveStyle.normal.background.height * 3), freezeLabel.width, liveStyle.normal.background.height * 3);
	}
	
	void liveModeClick(){
		if(liveButton.toggled)
			SetMode(Mode.live);
		else 
			liveButton.toggled = true;
	}
	
	void freezeModeClick(){
		if(freezeButton.toggled)
			SetMode(Mode.tryToLock);
		else
			freezeButton.toggled = true;
	}
	
	void SetMode(Mode newMode){
		
		switch(newMode){
			
		case Mode.live:
			
			
			Debug.Log("Live");
			freezeButton.toggled = false;
			mode = Mode.live;
			freezeButton.enabled = true;
			liveButton.enabled = true;
			snapshotButton.enabled = true;
			jman.on = true;
			break;
			
		case Mode.tryToLock:
			
			Debug.Log("TryToLock");
			mode = Mode.tryToLock;
			freezeButton.enabled = false;
			liveButton.enabled = false;
			snapshotButton.enabled = false;
			
			tryToLockTime = Time.time;
			break;
			
		case Mode.freeze:
			
			Debug.Log("Freeze");
			freezeButton.enabled = true;
			liveButton.enabled = true;
			snapshotButton.enabled = true;
			mode = Mode.freeze;
			liveButton.toggled = false;
			jman.on = false;
			break;
		}
	}
	
	void snapClick(){
		
		TakeScreenShot();
	}
	
	
	
	public void resetManipulation(){
	
	
		
	}
	
	public void Draw(){
		
		if(mode == Mode.tryToLock){
			if( Time.time - tryToLockTime >= lockTimeout)
				SetMode(Mode.live);
			else if(Locked)
				SetMode(Mode.freeze);
		}
		
		GUI.BeginGroup(containerRect);
		
		if(mode != Mode.live)
			GUI.DrawTexture(backgroundRect, background);
		
		
		if(mode == Mode.freeze || mode == Mode.tryToLock){
		
			GUI.DrawTexture(labelRect, freezeLabel);
			
			snapshotButton.Draw();
			
		} else {
			
			GUI.DrawTexture(labelRect, liveLabel);
		}
	
		freezeButton.Draw();
		
		
		liveButton.Draw();
		
		Texture2D currentTrackerLabel;
		float currentPercent = GameObject.Find("TargeterPlane").GetComponent<Tracker>().percent;

		if(currentPercent == 0f)
			currentTrackerLabel = trackerLabelRedTex;
		else if(currentPercent == 1f)
			currentTrackerLabel = trackerLabelGreenTex;
		else
			currentTrackerLabel = trackerLabelYellowTex;
		
		GUI.DrawTexture(trackerLabelRect, currentTrackerLabel);
		
		
		GUI.EndGroup();
		
		
	}
	
}
