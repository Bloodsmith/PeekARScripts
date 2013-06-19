using UnityEngine;
using System.Collections;

public class MarkerScaleButton : MonoBehaviour {
	MainInterface mi;
	GUIStyle buttonStyle;
	GUIStyle stringStyle;
	//located below side menu
	float PositionX = 1800f;
	float PositionY = 1450;
	float PositionWidth = 100f;
	float PositionHeight = 80f;
	
	float PositionStringX = 1940f;
	float PositionStringY = 1450;
	float PositionStringWidth = 100f;
	float PositionStringHeight = 80f;
	string ShowScale = "Large Marker";
	
	Rect ButtonRect;
	Rect StringRect;
	bool SetSmall = false;
	float Timer = 0f;
	float Timed = 1f;
	
	// Use this for initialization
	void Start () {
		mi = Camera.mainCamera.GetComponent<MainInterface>();
		buttonStyle = new GUIStyle();
		stringStyle = new GUIStyle();
		buttonStyle.normal.background = null;
		buttonStyle.active.background = null;
		buttonStyle.hover.background = null;
		stringStyle.alignment = TextAnchor.MiddleCenter;
		stringStyle.wordWrap = true;
	}
	
	//PROGRAMMING!!
	void OnGUI(){
		ButtonRect = ScaledRect.Rect(PositionX, PositionY, PositionWidth, PositionHeight);
		StringRect = ScaledRect.Rect (PositionStringX, PositionStringY, PositionStringWidth, PositionStringHeight);
		if(GUI.Button(ButtonRect, "", buttonStyle)){
			if(SetSmall){
				mi.SetMarkerScale(1.086f, 1.086f, 1.086f, 1.086f);
				SetSmall = false;
				ShowScale = "Large Marker";
			}
			else{
				mi.SetMarkerScale(3f, 3f, 3f, 3f);
				SetSmall = true;
				ShowScale = "Small Marker";
			}
			Timer = Time.time;
		}
		if(Timer != 0f){
			if(Timer + Timed > Time.time){
				GUI.Label(StringRect, ShowScale);
			}
			else{
				Timer = 0f;
			}
		}
	}
}
