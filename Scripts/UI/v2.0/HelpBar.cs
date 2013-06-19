using System;
using System.Collections;
using UnityEngine;

public class HelpBar
{
	Rect container;
	Rect textRect;
		
	Texture2D background;
	Texture2D HELP_centerMarker;
	Texture2D HELP_chooseItem;
	Texture2D HELP_delete;
	Texture2D HELP_drag;
	//Texture2D HELP_lock;
	Texture2D HELP_switchToFreeze;
	
	GUIStyle textStyle;
	
	float barHeight;
	float slideSpeed = 3f;	
	
	bool visible;
	public bool Visible{
		get {
			return visible;
		}
		set{
		
			visible = value;
			
		}
	}
	
	public float Height{
		get{
			return background.height;
		}
	}
	
	MainInterface mi;
	JeffARManager jman;
	bool DeleteMode = false;
	bool Locked = false;
	bool ShowFreezeHelp = false;
	bool NeedFurniture = true;
	int furnCount = 0;
	
	public HelpBar (string assetPath)
	{
		
		background = (Texture2D) Resources.Load(assetPath + "BAR_top_help");
		
		barHeight = background.height;
		container = ScaledRect.Rect(0, 0, ScaledRect.FullScreenRect.width, barHeight);
		textRect = ScaledRect.Rect(0, 0, ScaledRect.FullScreenRect.width, barHeight);
		
		textStyle = new GUIStyle();
		
		HELP_centerMarker = (Texture2D) Resources.Load(assetPath + "HELP_centerMarker");
		HELP_chooseItem = (Texture2D) Resources.Load(assetPath + "HELP_chooseItem");
		HELP_delete = (Texture2D) Resources.Load(assetPath + "HELP_delete");
		HELP_drag = (Texture2D) Resources.Load(assetPath + "HELP_drag");
		//HELP_lock = (Texture2D) Resources.Load(assetPath + "HELP_lock");
		HELP_switchToFreeze = (Texture2D) Resources.Load(assetPath + "HELP_switchToFreeze");
		
		mi = Camera.mainCamera.GetComponent<MainInterface>();
		
		jman = Camera.mainCamera.GetComponent<JeffARManager>();
		jman.Locked += () => {Locked = true;};
		jman.Lost += () => {Locked = false;};
		
		mi.AmountFurniture += (furnCount) => {this.furnCount = furnCount;};
		
		visible = UserOptions.GetPersistantHelpState();
	}
	
	void GetMessage(){
		
	
		if(mi.mode == MainInterface.Mode.Delete)
			textStyle.normal.background = HELP_delete;
		else if(!Locked)
			textStyle.normal.background = HELP_centerMarker;
		else if(jman.on)
			textStyle.normal.background = HELP_switchToFreeze;
		else if(furnCount == 0)
			textStyle.normal.background = HELP_chooseItem;
		else
			textStyle.normal.background = HELP_drag;
			
			
			
		
	}
	
	void DoExpand(){
		
		
		if(visible)
		
			container.y = Mathf.Min( container.y + slideSpeed, 0);
			
		else
			
			container.y = Mathf.Max( container.y - slideSpeed, -50);
			
		UserOptions.SetPersistantHelpState(visible);
	}
	
	public void Draw(){
		
		DoExpand();
		
		GetMessage();
		
		GUI.BeginGroup(container, background);
		
		GUI.Label(textRect, "", textStyle);
		
		GUI.EndGroup();
	}
}


