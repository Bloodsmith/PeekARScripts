using UnityEngine;
using System.Collections;
using System;

public class Button {
	
	GUIStyle normalStyle;
	GUIStyle toggledStyle;
	
	Rect position;
	bool useLayout = true;
	GUILayoutOption[] options;
	
	public float X {
		get{
			return position.x;
		}
	}
	
	public float Y {
		get{
			return position.y;
		}
	}
	
	public float Width{
		get{
			return position.width;
		}
	}
	
	public float Height{
		get{
			return position.height;
		}
	}
	
	public bool toggled;
	public bool enabled;
	
	public event Action click;
	
	public Button( GUIStyle normalStyle, GUIStyle toggledStyle, Rect position) : this(normalStyle, toggledStyle){
		this.position = position;
		useLayout = false;
	
	}
		
	public Button(GUIStyle normalStyle, GUIStyle toggledStyle, params GUILayoutOption[] options) : this(normalStyle){
		
		this.toggledStyle = toggledStyle;
		this.options = options;
		useLayout &= true;
	
	}
	
	public Button(GUIStyle normalStyle, Rect position) : this(normalStyle){
		
		this.position = position;
		useLayout = false;
	}
	
	public Button(GUIStyle normalStyle, params GUILayoutOption[] options){
		
		this.normalStyle = normalStyle;
		this.options = options;
		enabled = true;
		useLayout &= true;
	}
	
	public void triggerClick(){
		if(toggledStyle != null)
			toggled = !toggled;
			
		click();
	}
	
	public void Draw(){
		
		GUI.enabled = enabled;
		
		GUIStyle currentStyle = toggled ? toggledStyle : normalStyle;
		
		if(useLayout){
			if(GUILayout.Button("", currentStyle, options)){
				if(toggledStyle != null)
					toggled = !toggled;
				
				if(click != null)
					click();
			}
		}
		else {
			if(GUI.Button(position, "", currentStyle)){
				
				if(toggledStyle != null){
					toggled = !toggled;
					Debug.Log ("Toggled: " + toggled);
				}
				if(click != null)
					click();
			}
		}
		
		GUI.enabled = true;
	}
	
	
}
