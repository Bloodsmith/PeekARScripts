using UnityEngine;
using System.Collections;
using System;

public class DetailsOverlay {
	
	string assetPath = "Overlay/";
	
	//Styles
	public GUIStyle stringStyle;
	//Textures
	public Texture2D OverlayTex;
	//Fonts
	public Font iPad2Font;
	public Font iPad4Font;
	//Strings
	string currentString = "";
	
	//variables
	bool showGUI = false;
	Furniture currentFurniture = null;
	
	public Rect container;
	Rect buttonRect;
	Rect labelRect;
	
	public event Action CloseInfo;
	
	//Set for showing this element
	public void SetShowGUI(bool show){
		showGUI = show;
	}
	
	// Use this for initialization
	public DetailsOverlay (string baseAssetPath) {
		
		assetPath = baseAssetPath + assetPath;
		
		OverlayTex = (Texture2D) Resources.Load(assetPath + "INFO_overlay_01");
		
		container = ScaledRect.Rect((ScaledRect.FullScreenRect.width / 2) - (OverlayTex.width / 2), (ScaledRect.FullScreenRect.height / 2) - (OverlayTex.height / 2), OverlayTex.width, OverlayTex.height);
		
		buttonRect = ScaledRect.Rect(0,0, 100,100);
		labelRect = ScaledRect.Rect(100,100, OverlayTex.width - 200, OverlayTex.height - 100);
		
		stringStyle = new GUIStyle();
		stringStyle.font = Startup.Font_medium;
		stringStyle.normal.textColor = Color.white;
		stringStyle.wordWrap = true;
		

	
	}
	
	public void Draw () {
		
		
		
		GUI.BeginGroup(container, OverlayTex);
		
			if(GUI.Button(buttonRect, "", stringStyle)){
				CloseInfo();
			}
			GUI.Label(labelRect, currentString, stringStyle);
		
		GUI.EndGroup();
	}
	
	public void SetStrings(Furniture furn){
		string Name = "Name:  " +  furn.GetDisplayName();
		string DesignerName = "Designer Name:  " + furn.GetCategoryString();	
		string Description = "Description:\n" + furn.GetDescription();
		string Dimensions = "Dimensions:\n" + furn.GetDimensions();
		currentString = Name + "\n\n" + DesignerName + "\n\n" + Description + "\n\n" + Dimensions;
		currentFurniture = furn;
		showGUI = true;
	}
}
