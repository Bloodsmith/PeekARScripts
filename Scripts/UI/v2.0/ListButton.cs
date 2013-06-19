using System;
using UnityEngine;

public abstract class ListButton
{
	
	protected string baseDir = "";
	
	protected static Texture2D shadowBackground;
	
	
	protected GUIStyle style;
	public Rect position;
	protected GUILayoutOption[] options;
	
	public event Action<ListButton> click;
	
	public ListButton(string assetPath, GUIStyle style, GUIStyle infoStyle, Rect position, params GUILayoutOption[] options){
		
		baseDir = assetPath;
		this.style = style;
		this.position = position;
		this.options = options;
		
		shadowBackground  = (Texture2D)Resources.Load(assetPath + "TAB_shadow");
	}
	
	protected void Clicked(){
		if (click != null)
			click(this);
	}
	
	public abstract void Draw(bool showInfoLink);
}


