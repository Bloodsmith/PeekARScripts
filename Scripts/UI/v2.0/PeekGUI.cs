using UnityEngine;
using System.Collections;
using System;

public class PeekGUI : MonoBehaviour {

	
	string baseDir = "UI/";
	
	public CameraControls cameraControls;
	public UpperMenu upperMenu;
	public SideMenu sideMenu;
	public DetailsOverlay overlay;
	
	Texture2D SSLogoTex;
	
	public event Action DeleteBtn;
	public event Action CancelDelete;
	public event Action FurnitureClick;
	public event Action InfoClick;
	public event Action ClearBtn;
	
	bool showOverlay;
	bool showGUI = true;
	
	public bool interactable = true;
	
	//Checks if GUI elements contain mouse/touch position for disabling interaction
	void CheckGUITouch(){
		if(sideMenu.container.Contains(Event.current.mousePosition) || 
			upperMenu.container.Contains(Event.current.mousePosition) ||
			(overlay.container.Contains(Event.current.mousePosition) && showOverlay) || 
			cameraControls.checkRect.Contains(Event.current.mousePosition))
		{
			//Debug.Log ("Mouse Position is in GUI!");
			interactable = false;
		}
		else{
			//Debug.Log ("Mouse Position is not in GUI!");
			interactable = true;
		}
	}
	
	void Start(){
		
		overlay = new DetailsOverlay(baseDir);
		overlay.CloseInfo += () => {showOverlay = false;};
		
		cameraControls = new CameraControls(baseDir);
		cameraControls.TakeScreenShot += () => TakeScreenShot();
		
		upperMenu = new UpperMenu(baseDir);
		upperMenu.CancelDelete += () => CancelDeleteFn();
		upperMenu.DeleteBtn += () => DeleteButtonFn();
		upperMenu.searchClick += () => searchClickHandler();
		upperMenu.ClearBtn += () => {ClearBtn(); ResetManipulation();};
		upperMenu.InfoClick += () => InfoClick();
		upperMenu.textChange += (newText) => setSearchText(newText);
		
		
		sideMenu = new SideMenu(baseDir);
		sideMenu.SpawnFurniture += (furniture) => spawnFurniture(furniture);
		sideMenu.ShowInfo += (furniture) => showInfo(furniture);
		sideMenu.CategoryClick += () => {upperMenu.resetSearchBar();};
		
		SSLogoTex = (Texture2D)Resources.Load(baseDir + "Screenshot/SNAPSHOT_cornerlogo_02");
	}
	void DeleteButtonFn(){
		if(DeleteBtn != null) 
			DeleteBtn();
		FurnitureCollection.UpdateSearch();
		sideMenu.updateButtons();
		Debug.Log("PeekGUI DeleteButtonFn");
	}
	
	void CancelDeleteFn(){
		if(CancelDelete != null) 
			CancelDelete();
	}
	
	void setSearchText(string newText){
		
		FurnitureCollection.UpdateSearchString(newText);
		sideMenu.updateButtons();
		
	}
	
	void searchClickHandler(){
		
	}
	
	void showInfo(Furniture furniture){
		overlay.SetStrings(furniture);
		showOverlay = true;
	}
	
	void spawnFurniture(Furniture furniture){
		
		Camera.mainCamera.GetComponent<MainInterface>().PickFurniture(furniture);
		sideMenu.Reset();
		upperMenu.resetSearchBar();
	}
	
	public void ResetManipulation(){ 
		cameraControls.resetManipulation();	
		FurnitureCollection.UpdateSearch();
		sideMenu.Reset();
		sideMenu.updateButtons();
		upperMenu.ToggleDeleteButton(false);
	}
	
	public void TakeScreenShot(){
		
		StartCoroutine("DoScreenShot");
	}
	
	void OnGUI(){
		if(showGUI){
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
				showOverlay = false;
			
			GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
			
				cameraControls.Draw();
				upperMenu.Draw();
				sideMenu.Draw();
				
			
				if(showOverlay)
					overlay.Draw();
			
			GUI.EndGroup();
		}
		else{
			GUI.Label(ScaledRect.Rect(ScaledRect.FullScreenRect.width - (SSLogoTex.width+25), 
				ScaledRect.FullScreenRect.height - (SSLogoTex.height+25), SSLogoTex.width, SSLogoTex.height), SSLogoTex);
		}
		CheckGUITouch();
	}
	
	IEnumerator DoScreenShot(){
		showGUI = false;
		string ImageFile = "PEEK_AR_" + Time.time;
		yield return new WaitForEndOfFrame();
		Application.CaptureScreenshot(ImageFile);
		
		yield return new WaitForSeconds(1.5f);
		showGUI = true;
		NativeCalls._ImageSaver(ImageFile);
				
		NativeCalls._FireAlert("Screenshot Taken!", "Check your devices photo gallery to view and share your PEEK AR custom photo!");
		
	}
}
