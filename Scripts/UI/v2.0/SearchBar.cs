using UnityEngine;
using System.Collections;
using System;

public class SearchBar {
	
	
	string assetPath = "";
	
	string searchString;
	string defaultSearchString = "Search";
	
	Texture2D background;
	
	Rect position;
	GUIStyle style;
	GUIStyle textStyle;
	
	public event Action<string> textChange;
	
	public SearchBar(string assetPath, Rect position, GUIStyle style){
	
		this.assetPath = assetPath + this.assetPath;
		this.position = position;
		this.style = style;
		
		textStyle = new GUIStyle();
		Rect offsetRect = ScaledRect.Rect(70, 26, 0, 0);
		textStyle.padding.left = (int)offsetRect.x;
		textStyle.padding.top = (int)offsetRect.y;
		textStyle.alignment = TextAnchor.UpperLeft;
		textStyle.font = Startup.Font_medium;
		textStyle.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
		
		searchString = defaultSearchString;
		
		style.normal.background = (Texture2D) Resources.Load(assetPath + "SEARCH_field");
	}
	
	public void resetToDefault(){
		
		searchString = defaultSearchString;
		
		textChange(searchString);
	}
	
	public void Draw(){
		GUI.Label(position,"", style);
		
		String newSearchString = GUI.TextField(position, searchString, textStyle);
		
		if(newSearchString != searchString && textChange != null){
			textChange(newSearchString);
		}
		
		searchString = newSearchString;
	}
}
