using UnityEngine;
using System.Collections;
using System;

public class FurnitureButton : ListButton {
	
	
	
	public Furniture furniture;
	
	GUIStyle rowStyle;
	GUIStyle shadowStyle;
	GUIStyle textStyle;
	GUIStyle counterTextStyle;
	GUIStyle infoStyle;
	
	Rect mainRect;
	Rect buttonRect;
	Rect imageRect;
	Rect textRect;
	
	public FurnitureButton(string assetPath, Rect position, Furniture furniture, GUIStyle style,  GUIStyle infoStyle, params GUILayoutOption[] options) 
				: base(assetPath, style, infoStyle, position, options){
		
		this.furniture = furniture;
		this.infoStyle = infoStyle;
			
		textRect = ScaledRect.Rect(120, 4, 166, position.height);
		imageRect = ScaledRect.Rect(6, 6, furniture.GetImage().height, furniture.GetImage().height);
		buttonRect = new Rect(0,0, position.width, position.height);
		
		
		shadowStyle = new GUIStyle();
		shadowStyle.normal.background = (Texture2D)Resources.Load(baseDir + "TAB_Shadow_04");
		shadowStyle.border = new RectOffset(-10,-10,-10,-10);
		
		
		//textStyle = new GUIStyle(optionsSkin.label);
		textStyle = new GUIStyle();
		textStyle.alignment = TextAnchor.UpperLeft;
		textStyle.font = Startup.Font_vsmall;
		textStyle.wordWrap = true;
		textStyle.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
		textStyle.active.textColor = new Color(0.75f, 0.75f, 0.75f);
		textStyle.padding.top = 5;
		
		
		
		counterTextStyle = new GUIStyle(style);
		counterTextStyle.alignment = TextAnchor.MiddleRight;
		counterTextStyle.padding.right = 10;
	}
	
	public override void Draw(bool showInfoLink){
		
		GUIStyle currentStyle = showInfoLink ? infoStyle : style;
		
		GUI.BeginGroup(position);
			
		
		//Drop shadow
		GUI.Label(buttonRect, "", shadowStyle);
		
		
		//Draw Button without an image
		if(GUI.Button(buttonRect, "", currentStyle))
			Clicked();
		
		//Picture
		GUI.DrawTexture(imageRect, furniture.GetImage());
				
		
		
		//Draws a text for the button
		GUI.Label(textRect, furniture.GetDisplayName(), textStyle);
			
		
//		GUI.TextArea(new Rect(0,0,position.width, position.height), "(" + Camera.mainCamera.GetComponent<MainInterface>().GetNumActiveFurniture(furniture.GetName()) + ")", counterTextStyle);
		
		
		GUI.EndGroup();
	}
}
