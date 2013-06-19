using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SideMenu
{
	
	string assetPath = "SideMenu/";
	
	SearchBar searchBar;
	
	float topButtonsHeight;
	float scrollWidth;
	float tabWidth;
	float buttonHeight;
	float menuX;
	
	float slideSpeed = 10f;
	
	public Rect container;
	Rect outerScrollRect;
	Rect innerScrollRect;
	Rect furnButtonRect;
	
	Texture2D backgroundMidTex;
	Texture2D backgroundTopTex;
	Texture2D backgroundBotTex;
	Texture2D backgroundBotFrontTex;
	Texture2D backgroundTabTex;
	Texture2D listButtonTex;
	Texture2D furnButtonShadowTex;
	Texture2D furnButtonTex;
	Texture2D furnButtonActiveTex;
	Texture2D furnButtonInfoTex;
	Texture2D furnButtonInfoActiveTex;
	
	Rect backgroundTopRect;
	Rect backgroundMidRect;
	Rect backgroundBotRect;
	Rect backgroundBotFrontRect;
	Rect backgroundTabRect;
	Rect cartTextRect;
	
	Vector2 scrollPosition;
	GUIStyle verticalScrollbarStyle;
	GUIStyle horizontalScrollbarStyle;
	GUIStyle cartTextStyle;
	
	GUIStyle furnButtonStyle;
	GUIStyle furnButtonInfoStyle;
	GUIStyle tabButtonStyle;
	GUILayoutOption[] furnButtonOptions;
	
	Button listButton;
	Button cartButton;
	Button backButton;
	
	ListButton clickedButton;
	
	List<ListButton> furnButtons;
	
	public event Action<Furniture> SpawnFurniture;
	public event Action<Furniture> ShowInfo;
	public event Action CategoryClick;
	
	bool visible;
	public bool Visible{
		get {
			return visible;
		}
		set{
		
			visible = value;
			
		}
	}
	
	public SideMenu (string baseAssetPath)
	{
		assetPath = baseAssetPath + assetPath;
		
		Visible = true;
		
		topButtonsHeight = 75;
		scrollWidth = 100;
		buttonHeight = 100;
		
		tabWidth = 10;
		
		slideSpeed *= Startup.ScaleFactor;
		
		
		GUIStyle listButtonStyle = new GUIStyle();
		listButtonStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_list_avail");
		listButtonStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_list_active");
			
		GUIStyle listButtonToggleStyle = new GUIStyle();
		listButtonToggleStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_list_active");
		listButtonToggleStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_list_active");
		
		Rect listButtonRect = ScaledRect.Rect(0, 0, listButtonStyle.normal.background.width, listButtonStyle.normal.background.height);
		
		listButton = new Button(listButtonStyle, listButtonToggleStyle, listButtonRect);
		listButton.click += () => ListButtonClick ();
		listButton.toggled = true;
		
		
		GUIStyle backButtonToggleStyle = new GUIStyle();
		backButtonToggleStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_list_back");
		backButtonToggleStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_list_back");
		
		Rect backButtonRect = ScaledRect.Rect(0, 0, backButtonToggleStyle.normal.background.width, backButtonToggleStyle.normal.background.height);
		
		backButton = new Button(backButtonToggleStyle, backButtonRect);
		backButton.click += () => BackButtonClick ();
		
		
		
		
		GUIStyle cartButtonStyle = new GUIStyle();
		cartButtonStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_cart_avail");
		cartButtonStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_cart_active");
		
		GUIStyle cartToggleButtonStyle = new GUIStyle();
		cartToggleButtonStyle.normal.background = (Texture2D) Resources.Load(assetPath + "BTN_cart_active");
		cartToggleButtonStyle.active.background = (Texture2D) Resources.Load(assetPath + "BTN_cart_active");
		
		Rect cartButtonRect = ScaledRect.Rect(cartButtonStyle.normal.background.width, 0, cartButtonStyle.normal.background.width, cartButtonStyle.normal.background.height);
		
		cartButton = new Button(cartButtonStyle, cartToggleButtonStyle, cartButtonRect);
		cartButton.click += () => CartButtonClick();
		
		cartTextStyle = new GUIStyle();
		cartTextStyle.alignment = TextAnchor.MiddleCenter;
		cartTextStyle.font = Startup.Font_small;
		cartTextStyle.normal.textColor = Color.white;
		cartTextStyle.active.textColor = Color.white;
		cartTextRect = ScaledRect.Rect(1 + listButtonStyle.normal.background.width + (cartButtonStyle.normal.background.width/2.5197f), 
			cartButtonStyle.normal.background.height/2.3f, 25, 25);
		
		backgroundTopTex = (Texture2D) Resources.Load(assetPath + "MENU_bg_top");
		backgroundMidTex = (Texture2D) Resources.Load(assetPath + "MENU_bg_middle");
		backgroundBotTex = (Texture2D) Resources.Load(assetPath + "MENU_bg_bottom_back");
		backgroundBotFrontTex = (Texture2D) Resources.Load(assetPath + "MENU_bg_bottom_front");
		backgroundTabTex = (Texture2D) Resources.Load(assetPath + "MENU_bg_handle");
		
		container = ScaledRect.Rect(ScaledRect.FullScreenRect.width - backgroundTopTex.width, 
			200,
			backgroundTopTex.width, 
			backgroundTopTex.height + backgroundMidTex.height + backgroundBotTex.height);
		
		backgroundTopRect = ScaledRect.Rect(
			0, 
			0, 
			backgroundTopTex.width, 
			backgroundTopTex.height);
		
		backgroundMidRect = ScaledRect.Rect(
			0, 
			0 + backgroundTopTex.height, 
			backgroundMidTex.width, 
			backgroundMidTex.height);
		
		backgroundBotRect = ScaledRect.Rect(
			0, 
			0 + backgroundTopTex.height + backgroundMidTex.height, 
			backgroundBotTex.width, 
			backgroundBotTex.height);
		
		backgroundBotFrontRect = ScaledRect.Rect(
			0, 
			0 + backgroundTopTex.height + backgroundMidTex.height, 
			backgroundBotFrontTex.width, 
			backgroundBotFrontTex.height);
		
		backgroundTabRect = ScaledRect.Rect(
			0, 
			0 + backgroundTopTex.height + (backgroundMidTex.height / 2) - (backgroundTabTex.height / 2), 
			backgroundTabTex.width, 
			backgroundTabTex.height);
		
		tabButtonStyle = new GUIStyle();
		tabButtonStyle.normal.background = backgroundTabTex;
		tabButtonStyle.font = Startup.Font_small;
		tabButtonStyle.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
		tabButtonStyle.active.textColor = new Color(0.75f, 0.75f, 0.75f);
		
		furnButtonShadowTex =  (Texture2D) Resources.Load(assetPath + "TAB_shadow");
		furnButtonTex =  (Texture2D) Resources.Load(assetPath + "TAB_list_avail");
		furnButtonActiveTex =  (Texture2D) Resources.Load(assetPath + "TAB_list_down");
		furnButtonInfoTex =  (Texture2D) Resources.Load(assetPath + "TAB_cart_avail");
		furnButtonInfoActiveTex =  (Texture2D) Resources.Load(assetPath + "TAB_cart_down");

		listButtonTex = (Texture2D) Resources.Load(assetPath + "BTN_list_avail");
		
		//add 25 so scroll bar is offscreen
		//outerScrollRect = new Rect(backgroundMidTex.width - scrollWidth, 0, scrollWidth + 25, backgroundMidTex.height - topButtonsHeight);
		outerScrollRect = ScaledRect.Rect(5 + backgroundTabTex.width, backgroundTopTex.height - 35, furnButtonShadowTex.width + 50, backgroundMidTex.height + backgroundBotTex.height - 17);
		scrollPosition = new Vector2(0,0);
		
		
		furnButtonStyle = new GUIStyle();
		furnButtonStyle.normal.background = furnButtonTex;
		furnButtonStyle.active.background = furnButtonActiveTex;
		
		furnButtonInfoStyle = new GUIStyle();
		furnButtonInfoStyle.normal.background = furnButtonInfoTex;
		furnButtonInfoStyle.active.background = furnButtonInfoActiveTex;
		
		
		furnButtonRect = ScaledRect.Rect( furnButtonShadowTex.width - furnButtonTex.width, 0, furnButtonTex.width, furnButtonTex.height);
	
		furnButtons = new List<ListButton>();
		FurnitureCollection.Reset();
		updateButtons();
		
		innerScrollRect = new Rect(0, 0, outerScrollRect.width - 100, furnButtons.Count * furnButtonRect.height );

		
		
	}		
	
	void BackButtonClick(){
		FurnitureCollection.Reset();
		updateButtons();
	}
	
	void ListButtonClick() {
	
		//disable interaction while toggled
		if(listButton.toggled){
			FurnitureCollection.SearchMode = true;
			FurnitureCollection.Reset();
			cartButton.toggled = false;
			updateButtons();
		} else 
			listButton.toggled = true;
	}
	
	void CartButtonClick() {
		
		//disable interaction while toggled
		if(cartButton.toggled){
			FurnitureCollection.SearchMode = false;
			listButton.toggled = false;		
			updateButtons();
		} else
			cartButton.toggled = true;
	
		
	}
	
	public void Reset(){
		
		FurnitureCollection.Reset();
		updateButtons();
	}
	
	void OnVisible(){
		Debug.Log("OnVisible");
		
	}
	
	void SearchBarUpdate(string newSearchText){
		
		FurnitureCollection.UpdateSearchString(newSearchText);
		updateButtons();
		
	}
	
	void DoExpand(){
		
		if(visible){
			
			container.x = Mathf.Max( container.x - slideSpeed, Screen.width - container.width);
		}
		
		else{
			
			container.x = Mathf.Min( container.x + slideSpeed, Screen.width - backgroundTabRect.width);
		}
		
	}
	
	
	public void updateButtons(){
	
		
		furnButtons.Clear();
		
		int row = 0;
		
		foreach(object o in FurnitureCollection.GetCurrentList()){
			float topOffset = (furnButtonShadowTex.height - furnButtonTex.height) / 2;
			
			Rect offsetButtonRect = new Rect(furnButtonRect.x, furnButtonRect.y + topOffset + row * (furnButtonRect.height + 2), furnButtonRect.width, furnButtonRect.height);
			
			if(o is string){
			
				furnButtons.Add(new CategoryButton(assetPath, offsetButtonRect, (string) o, furnButtonStyle));
				
				
			} else if(o is Furniture){
				
				furnButtons.Add(new FurnitureButton(assetPath, offsetButtonRect, (Furniture) o, furnButtonStyle, furnButtonInfoStyle));
				
			}
				
			row ++;
		}
		
		foreach(ListButton b in furnButtons){
			
			b.click += (button) => FurnButtonClick(button);	
		}
		
		
		//innerScrollRect = new Rect(0,0, outerScrollRect.width, furnButtons.Count * buttonHeight);
		innerScrollRect = new Rect(0,0, outerScrollRect.width - 2, furnButtons.Count * (furnButtonRect.height + 2) + 20);
	}
	
	public void FurnButtonClick(ListButton button){
		clickedButton = button;
	}
	
	void ButtonClick(){
		
		if(Mathf.Abs(scrollOffset - oldScrollPositionY) > 15){
			clickedButton = null;
			return;
		}
			
		if(clickedButton is CategoryButton){
			FurnitureCollection.CategoryString = ((CategoryButton)clickedButton).label;
			CategoryClick();
		}
		else
		{
			if(!cartButton.toggled && SpawnFurniture != null){
				SpawnFurniture(((FurnitureButton)clickedButton).furniture);
				visible = false;
			}
			
			else if(cartButton.toggled && ShowInfo != null)
				ShowInfo(((FurnitureButton)clickedButton).furniture);
		}
		
		updateButtons();
		
	
		
		clickedButton = null;
	}
	
	float scrollOffset;
	float scrollSpeed;
	float oldScrollPositionY;
	
	void DoScroll(){
		
		if(Input.touchCount > 0){
			
			Touch t1 = Input.GetTouch(0);	
	
			if(t1.phase == TouchPhase.Began)
				scrollOffset = t1.position.y;
			else
				scrollSpeed = -(t1.position.y - oldScrollPositionY);
			
			oldScrollPositionY = t1.position.y;
			
		} 
		
		scrollPosition.y -= scrollSpeed;
			
		scrollSpeed *= 0.95f;
		
		
	}
	
	public void Draw(){
		
		DoExpand();
		
		DoScroll();
		
		GUI.BeginGroup(container);
		
		GUI.DrawTexture(backgroundTopRect, backgroundTopTex);
		GUI.DrawTexture(backgroundMidRect, backgroundMidTex);
		GUI.DrawTexture(backgroundBotRect, backgroundBotTex);
		
		if(GUI.Button(backgroundTabRect, "", tabButtonStyle)){
			Visible = !Visible;
		}
		
		if(string.IsNullOrEmpty(FurnitureCollection.CategoryString))
			listButton.Draw();	
		else
			backButton.Draw();
		
		cartButton.Draw();
		
		GUI.Label(cartTextRect, MainInterface.ActiveFurn.Count.ToString(), cartTextStyle);
		
		horizontalScrollbarStyle = new GUIStyle(GUI.skin.horizontalScrollbar);
		horizontalScrollbarStyle.fixedHeight = horizontalScrollbarStyle.fixedWidth = 0;
	 	verticalScrollbarStyle = new GUIStyle(GUI.skin.verticalScrollbar);
		verticalScrollbarStyle.fixedHeight = verticalScrollbarStyle.fixedWidth = 0;
		
		scrollPosition = GUI.BeginScrollView(outerScrollRect,scrollPosition,innerScrollRect, horizontalScrollbarStyle, verticalScrollbarStyle);
		
		foreach(ListButton button in furnButtons){
			
			GUI.DrawTexture(new Rect(button.position.x - (furnButtonShadowTex.width * Startup.ScaleFactor - button.position.width), 
				button.position.y - (furnButtonShadowTex.height * Startup.ScaleFactor - button.position.height) / 2, 
				furnButtonShadowTex.width * Startup.ScaleFactor, furnButtonShadowTex.height * Startup.ScaleFactor), furnButtonShadowTex);
			
		}
		
		foreach(ListButton button in furnButtons){
		
			button.Draw(cartButton.toggled);
			
		}
		
		if(clickedButton != null)
			ButtonClick ();
		
		GUI.EndScrollView();
		
		GUI.DrawTexture(backgroundBotFrontRect, backgroundBotFrontTex);
		
		GUI.EndGroup();
		
	}
}

