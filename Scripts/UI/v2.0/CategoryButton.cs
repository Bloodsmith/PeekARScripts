using UnityEngine;
using System.Collections;
using System;

public class CategoryButton : ListButton {
	
	public string label;
	Rect buttonRect;
	
	public CategoryButton(string assetPath, Rect position, string label, GUIStyle style, params GUILayoutOption[] options) 
			: base(assetPath, style, null, position, options){
		
		this.label = label;
		this.position = position;
		style.alignment = TextAnchor.MiddleCenter;
		style.font = Startup.Font_medium;
		style.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
		style.active.textColor = new Color(0.75f, 0.75f, 0.75f);
		
		buttonRect = new Rect(0, 0, position.width, position.height);
	}
	
	public override void Draw(bool showInfoLink){
		
		GUI.BeginGroup(position);
		
		if(GUI.Button(buttonRect, label, style))
			Clicked();
		
		GUI.EndGroup();
		
	}
}
