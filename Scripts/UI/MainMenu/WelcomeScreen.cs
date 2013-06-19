/*Welcome, Tips, About screens. */
using UnityEngine;
using System.Collections;

/* About Screen
 * <image width="405" height="267" id="Stringlogo" xlink:href="65170846D81AC18B.png"  transform="matrix(1 0 0 1 295 294)">
</image>
<image width="226" height="38" id="madeby" xlink:href="65170846D81AC189.png"  transform="matrix(1 0 0 1 839 363)">
</image>
<image width="623" height="229" id="NormLilogo" xlink:href="65170846D81AC18F.png"  transform="matrix(1 0 0 1 1196 290)">
</image>
<image width="451" height="286" id="Peeklogo" xlink:href="65170846D81AC188.png"  transform="matrix(1 0 0 1 831 719)">
</image>
*/

public class WelcomeScreen : MonoBehaviour {
	
	//string FolderBackgrounds = "Textures/Welcome Screen Textures/Backgrounds/";
	//string FolderButtons = "Textures/Welcome Screen Textures/Buttons/";
	string FolderBackgrounds = "UI/WelcomeScreenTextures/Backgrounds/";
	string FolderButtons = "UI/WelcomeScreenTextures/Buttons/";
	
	//Styles
	GUIStyle MainButtonStyle;
	GUIStyle AboutButtonStyle;
	GUIStyle TipsButtonStyle;
	GUIStyle TextStyle;
	
	//Textures
	
	
	//Variables
	bool About = false;
	bool Tips = false;
	bool SlideLeft = false;
	float SlideSpeed;
	bool loaded;
	
	//background
	Rect BackgroundRect;
	Rect GridRect;
	
	Texture2D BackgroundMainTex;
	Texture2D BackgroundAboutTex;
	Texture2D BackgroundGridTex;
	Texture2D BackgroundTex;
	
	//about screen
	Rect StringLogoRect;
	Rect MadeByRect;
	Rect NormLiLogoRect;
	Rect PeekLogoRect;
	
	Texture2D StringLogoTex;
	Texture2D MadeByTex;
	Texture2D NormLiLogoTex;
	Texture2D PeekLogoTex;
	
	
	//tips screen
	Rect TipsLogoRect;
	Rect TipsTextRect;
	
	Texture2D TipsTextTex;
	Texture2D TipsLogoTex;
	
	//main screen
	Rect MainLogoRect;
	Texture2D MainLogoTex;
	Texture2D backgroundGradient;
	
	//buttons
	Rect StartButtonRect;
	Rect TipsButtonRect;
	Rect AboutButtonRect;
	
	Texture2D AboutAvailableButtonTex;
	Texture2D AboutActiveButtonTex;
	Texture2D MainAvailableButtonTex;
	Texture2D TipsAvailableButtonTex;
	Texture2D TipsActiveButtonTex;
	Texture2D currentAboutButtonTex;
	Texture2D currentTipsButtonTex;
	
	Texture2D StartButtonTextTex;
	Texture2D TipsButtonTextTex;
	Texture2D AboutButtonTextTex;
	 
	
	
	AssetBundle bundle;
	
	void Start(){
		
		TextStyle = new GUIStyle();
		TextStyle.normal.textColor = new Color(166f/255f, 166f/255f, 166f/255f, 255f/255f);
	}
	
	//Start Up Positions
	public void InitCenter(){
		
		loadTextures();
		
		BackgroundRect.x = 0;
		SlideLeft = false;
		
	}
	
	//Back Button trigger Positions
	public void InitSlide(){
		
		loadTextures();
		
		BackgroundRect.x = -BackgroundRect.width;
		
		SlideLeft = false;
		
	}

	
	void loadTextures(){
		loaded = false;
		
		backgroundGradient = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_about_Gradient_XSMALL");
		BackgroundGridTex = (Texture2D)Resources.Load(FolderBackgrounds + "GRID_1024");
		BackgroundTex = (Texture2D)Resources.Load(FolderBackgrounds + "Background_White");
		MainAvailableButtonTex = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_start_avail");
		AboutAvailableButtonTex = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_about_avail");
		AboutActiveButtonTex = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_about_active");
		TipsAvailableButtonTex = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_tips_avail");
		TipsActiveButtonTex = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_tips_active");
		
		MainButtonStyle = new GUIStyle();
		MainButtonStyle.normal.background = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_start_avail");
		MainButtonStyle.active.background = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_start_active");
		AboutButtonStyle = new GUIStyle();
		AboutButtonStyle.normal.background = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_about_avail");
		AboutButtonStyle.active.background = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_about_active");
		TipsButtonStyle = new GUIStyle();
		TipsButtonStyle.normal.background  = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_tips_avail");
		TipsButtonStyle.active.background  = (Texture2D)Resources.Load(FolderButtons + "BTN_splash_tips_active");
	 	
		StartButtonTextTex = (Texture2D)Resources.Load(FolderButtons + "SPLASH_button_start_text");
	 	TipsButtonTextTex = (Texture2D)Resources.Load(FolderButtons + "SPLASH_button_tips_text");
	 	AboutButtonTextTex = (Texture2D)Resources.Load(FolderButtons + "SPLASH_button_about_text");
		
		StringLogoRect = ScaledRect.Rect(295, 294, 405, 267);
		MadeByRect = ScaledRect.Rect(839,363,226,38);
		NormLiLogoRect = ScaledRect.Rect(1196, 290, 623, 229);
		
		BackgroundRect = ScaledRect.Rect(0,0,ScaledRect.FullScreenRect.width,ScaledRect.FullScreenRect.height);
		//GridRect = ScaledRect.Rect(289, 349, 1024, 619, 1.4834f);
		GridRect = ScaledRect.Rect(538, 415, 1024, 619);
		
		StartButtonRect = ScaledRect.Rect(1276,1130,MainAvailableButtonTex.width,MainAvailableButtonTex.height);
		TipsButtonRect = ScaledRect.Rect(926,1130,TipsAvailableButtonTex.width,TipsAvailableButtonTex.height);
		AboutButtonRect = ScaledRect.Rect(572,1130,AboutAvailableButtonTex.width,AboutAvailableButtonTex.height);
		
		TipsTextRect = ScaledRect.Rect(222, 127, 1603, 1005);
		
		SlideSpeed = 128f * Startup.ScaleFactor;
		
		TextStyle = new GUIStyle();
		TextStyle.font = Startup.Font_medium;
	
		loaded = true;
		
		
	}
	
	
	// Update is called once per frame
	void OnGUI () {
		
		
		 if(loaded){
			Slide ();
			GUI.depth = 1;
			
			GUI.BeginGroup(BackgroundRect);
			
			GUI.DrawTexture(ScaledRect.FullScreenRect, BackgroundTex,  ScaleMode.StretchToFill);
			GUI.DrawTexture(ScaledRect.FullScreenRect, backgroundGradient,  ScaleMode.StretchToFill);
			GUI.DrawTexture(GridRect, BackgroundGridTex, ScaleMode.StretchToFill);
			//GUI.DrawTexture(ScaledRect.FullScreenRect, currentBackground, ScaleMode.StretchToFill);
			
			//Main screen
			if(!Tips && !About){ 
				if(MainLogoTex == null){
					MainLogoTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_about_logos_PEEKAR");
					MainLogoRect = ScaledRect.Rect(ScaledRect.FullScreenRect.width / 1.9f - MainLogoTex.width / 2, 500, MainLogoTex.width, MainLogoTex.height);
						
				}
				
				GUI.DrawTexture(MainLogoRect, MainLogoTex);
				
			}
			//About screen
			else if(About){ 
				if(StringLogoTex == null){
					
					StringLogoTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_about_logos_STRING");
					MadeByTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_about_logos_MADEBY");
					NormLiLogoTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_about_logos_NORMLI");
					PeekLogoTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_about_logos_PEEKAR");
					
					PeekLogoRect = ScaledRect.Rect(831, 719, PeekLogoTex.width, PeekLogoTex.height);
					
				
				}
				GUI.DrawTexture( StringLogoRect, StringLogoTex);
				GUI.DrawTexture( MadeByRect, MadeByTex);
				GUI.DrawTexture( NormLiLogoRect, NormLiLogoTex);
				GUI.DrawTexture( PeekLogoRect, PeekLogoTex);
			}
			//Tips screen
			else if(Tips){ 
				if(TipsTextTex == null){
					TipsTextTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_tips_tips");
					TipsLogoTex = (Texture2D)Resources.Load(FolderBackgrounds + "SPLASH_tips_logo");
					TipsLogoRect = ScaledRect.Rect(1595, 19, TipsLogoTex.width, TipsLogoTex.height);
				}
				
				GUI.DrawTexture(TipsLogoRect, TipsLogoTex);
				GUI.DrawTexture(TipsTextRect, TipsTextTex);
				
			}
			
			//currentAboutButtonTex = About ? AboutActiveButtonTex : AboutAvailableButtonTex;
			
			//currentTipsButtonTex = Tips ? TipsActiveButtonTex : TipsAvailableButtonTex;

			
			//Buttons - Starts the app
			if(GUI.Button(StartButtonRect, "", MainButtonStyle)){
				SlideLeft = true;
				About = false;
				Tips = false;
			}
			if(About)
				GUI.DrawTexture(AboutButtonRect, AboutActiveButtonTex);
			//Shows about screen
			else{
				if(GUI.Button(AboutButtonRect, "", AboutButtonStyle)){
					About = true;
					Tips = false;
				}
			}
			
			if(Tips){
				GUI.DrawTexture(TipsButtonRect, TipsActiveButtonTex);
			}
			else{
				//Shows Tips screen
				if(GUI.Button(TipsButtonRect , "", TipsButtonStyle)){
					Tips = true;
					About = false;
				}
			}
			
			GUI.EndGroup();	
			GUI.depth = 0;
		}
	}
	
	//Update
	void Slide(){
		
		//Slide Welcome Screen off screen
		if(SlideLeft){

			BackgroundRect.x -= SlideSpeed;
			
			if(BackgroundRect.x <= -BackgroundRect.width){ 
				Component.Destroy(gameObject);
				Resources.UnloadUnusedAssets();
			}
		
		}
		//Slide Welcome Screen on screen
		else {
		
			BackgroundRect.x = Mathf.Min (BackgroundRect.x + SlideSpeed, 0);
			
		}
	}
}
