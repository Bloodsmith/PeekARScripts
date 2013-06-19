using UnityEngine;
using System.Collections;
using System;

public class UpperMenu {

	
	string baseDir = "UpperMenu/";
	
	HelpBar helpBar;
	SearchBar searchBar;
	
	GUIStyle style;
	GUIStyle searchStyle;
	Texture2D backgroundTex;
	Texture2D deviderTex;
	Texture2D logoTex;
	Texture2D searchTex;
	
	Rect buttonRect;
	public Rect container;
	Rect logoRect;
	Rect searchRect;
	Rect helpBarRect;
	
	Button helpButton;
	Button infoButton;
	Button deleteButton;
	Button clearButton;
	
	
	public event Action CancelDelete;
	public event Action DeleteBtn;
	public event Action ClearBtn;
	public event Action InfoClick;
	
	
	GUILayoutOption[] deviderOptions;
	
	public event Action searchClick;
	public event Action<string> textChange;
	
	public UpperMenu (string assetPath) {
		
		baseDir = assetPath + baseDir;
		
		
		backgroundTex = (Texture2D) Resources.Load(baseDir + "BAR_top");
		container = ScaledRect.Rect(0, 0,  ScaledRect.FullScreenRect.width, backgroundTex.height);
		
		logoTex = (Texture2D) Resources.Load(baseDir + "LOGO_peek");
		logoRect = ScaledRect.Rect(10, 0, logoTex.width, logoTex.height);
		
		searchTex = (Texture2D) Resources.Load(baseDir + "SEARCH_field");
		searchStyle = new GUIStyle();
		searchStyle.normal.background = searchTex;
		searchRect = ScaledRect.Rect(ScaledRect.FullScreenRect.width - searchTex.width, 0, searchTex.width, searchTex.height);
		searchBar = new SearchBar(baseDir, searchRect, searchStyle);
		searchBar.textChange += (newText) => textChange(newText);
		
		buttonRect =  ScaledRect.Rect(300, 0, ScaledRect.FullScreenRect.width, backgroundTex.height);
		
		
		deviderTex = (Texture2D) Resources.Load(baseDir + "DIVIDER");
		deviderOptions = new GUILayoutOption[2];
		Rect dividerSize = ScaledRect.Rect(0,0, deviderTex.width, deviderTex.height);
		deviderOptions [0] = GUILayout.Width(dividerSize.width);
		deviderOptions [1] = GUILayout.Height(dividerSize.height);
	
		GUIStyle buttonStyle = new GUIStyle();
		GUILayoutOption[] buttonOptions = new GUILayoutOption[2];
		Rect buttonSize = ScaledRect.Rect(0,0, 206, 123);
		buttonOptions [0] = GUILayout.Width(buttonSize.width);
		buttonOptions [1] = GUILayout.Height(buttonSize.height);
		
		
		
		GUIStyle infoStyle = new GUIStyle(buttonStyle);
		infoStyle.normal.background = (Texture2D) Resources.Load(baseDir + "BTN_info_avail");
		infoStyle.active.background = (Texture2D) Resources.Load(baseDir + "BTN_info_active");
		
		infoButton = new Button(infoStyle,buttonOptions);
		infoButton.click += () => infoClick();
		
		
		GUIStyle helpStyle = new GUIStyle(buttonStyle);
		helpStyle.normal.background = (Texture2D) Resources.Load(baseDir + "BTN_help_avail");
		helpStyle.active.background = (Texture2D) Resources.Load(baseDir + "BTN_help_active");
		
		GUIStyle helpToggleStyle = new GUIStyle(buttonStyle);
		helpToggleStyle.normal.background = (Texture2D) Resources.Load(baseDir + "BTN_help_active");
		helpToggleStyle.active.background = (Texture2D) Resources.Load(baseDir + "BTN_help_active");
		
		helpButton = new Button(helpStyle,helpToggleStyle,buttonOptions);
		helpButton.click += () => helpClick();
		helpButton.toggled = UserOptions.GetPersistantHelpState();
		
		
		GUIStyle deleteStyle = new GUIStyle(buttonStyle);
		deleteStyle.normal.background = (Texture2D) Resources.Load(baseDir + "BTN_delete_avail");
		deleteStyle.active.background = (Texture2D) Resources.Load(baseDir + "BTN_delete_active");
		
		GUIStyle deleteToggleStyle = new GUIStyle(buttonStyle);
		deleteToggleStyle.normal.background = (Texture2D) Resources.Load(baseDir + "BTN_delete_active");
		deleteToggleStyle.active.background = (Texture2D) Resources.Load(baseDir + "BTN_delete_active");
		
		deleteButton = new Button(deleteStyle,deleteToggleStyle,buttonOptions);
		deleteButton.click += () => deleteClick();
		deleteButton.enabled = false;
		
		GUIStyle clearStyle = new GUIStyle(buttonStyle);
		clearStyle.normal.background = (Texture2D) Resources.Load(baseDir + "BTN_clear_avail");
		clearStyle.active.background = (Texture2D) Resources.Load(baseDir + "BTN_clear_active");
		
		clearButton = new Button(clearStyle,buttonOptions);
		clearButton.click += () => clearClick();
		clearButton.enabled = false;
		
	
		MainInterface mi = Camera.mainCamera.GetComponent<MainInterface>();
		mi.AmountFurniture += (furnCount) => {
			if(furnCount == 0){
				deleteButton.toggled = false;
			}
			deleteButton.enabled = furnCount != 0;
			clearButton.enabled = furnCount != 0;
		};
		
		helpBar = new HelpBar(baseDir);
		helpBarRect = ScaledRect.Rect(0, 0, ScaledRect.FullScreenRect.width, helpBar.Height);
	}
	
	public void resetSearchBar(){
		
		searchBar.resetToDefault();	
	}
	
	public void ToggleDeleteButton(bool toggle){
		deleteButton.toggled = toggle;
	}
	
	void helpClick(){
		helpBar.Visible = !helpBar.Visible;
	}
	
	void infoClick(){
		InfoClick();
	}
	
	void clearClick(){
		deleteButton.toggled = false;
		ClearBtn();
	}
	
	void deleteClick(){
		if(deleteButton.toggled){
			if(DeleteBtn != null){
				DeleteBtn();
				Debug.Log ("Upper Menu DeleteClick");
			}
		}
		else {
			if(CancelDelete != null){
				CancelDelete();
				Debug.Log ("Upper Menu Cancel Click");
			}
		}
	}
	
	void searchClickHandler(){
		searchClick();
	}
	
	
	public void Draw(){
		
		
		GUI.BeginGroup(helpBarRect);
				
		helpBar.Draw();
				
		GUI.EndGroup();
		
		GUI.BeginGroup(container, backgroundTex);
		
		GUI.DrawTexture(logoRect, logoTex);
		
		GUILayout.BeginArea(buttonRect);

		GUILayout.BeginHorizontal("");//buttonRect);
		  
			infoButton.Draw();
			
			helpButton.Draw();
		
			GUILayout.Label(deviderTex,deviderOptions);
			
			deleteButton.Draw();
			
			clearButton.Draw();
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
		
		searchBar.Draw();
		
		GUI.EndGroup();
		
	}
}
