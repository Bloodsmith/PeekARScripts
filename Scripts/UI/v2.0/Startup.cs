///Checks Generation of iPhone on Awake
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Startup : MonoBehaviour{
	
	static string FolderFonts = "UI/Fonts/";
	static string ThumbnailFolder = "UI/Thumbnails/";
	static string deviceGeneration = "";
	
	
	static GameObject welcomeScreen;
	public static Startup instance;
	
	//Method to retrieve device generation
	public static string GetGeneration(){
		//test
#if UNITY_EDITOR
		return "iPad2";
#endif
#if UNITY_IPHONE
		return deviceGeneration;
#endif
	}
	
	public static float ScaleFactor{
		get{	
		#if UNITY_EDITOR
			return 0.44f;
		#endif
		#if UNITY_IPHONE
			return Screen.width / 2048f;
		#endif
		
			
		}
	}
	
	static Font font_vsmall;
	public static Font Font_vsmall{
		get{
			if(font_vsmall == null){
				if(Startup.ScaleFactor > 0.5f){
					font_vsmall = (Font)Resources.Load(FolderFonts + "Gotham-Medium-iPad4-vSmall", typeof(Font));
				} else {
					font_vsmall = (Font)Resources.Load(FolderFonts + "Gotham-Medium-iPad2-vSmall", typeof(Font));
				}
			}
			return font_vsmall;
		}
		set{
			font_vsmall = value;
		}
	}
	
	
	static Font font_small;
	public static Font Font_small{
		get{
			if(font_small == null){
				if(Startup.ScaleFactor > 0.5f){
					font_small = (Font)Resources.Load(FolderFonts + "Gotham-Medium-iPad4-Small", typeof(Font));
				} else {
					font_small = (Font)Resources.Load(FolderFonts + "Gotham-Medium-iPad2-Small", typeof(Font));
				}
			}
			return font_small;
		}
		set{
			font_small = value;
		}
	}
	
	static Font font_medium;
	public static Font Font_medium{
		get{
			if(font_medium == null){
				if(Startup.ScaleFactor > 0.5f){
					font_medium = (Font)Resources.Load(FolderFonts + "Gotham-Medium-iPad4-Medium", typeof(Font));
				} else {
					font_medium = (Font)Resources.Load(FolderFonts + "Gotham-Medium-iPad2-Medium", typeof(Font));
				}
			}
			return font_medium;
		}
		set{
			font_small = value;
		}
	}
	
	static Font font_large;
	public static Font Font_large{
		get{
			if(font_large == null){
				if(Startup.ScaleFactor > 0.5f){
					font_large = (Font)Resources.Load(FolderFonts + "Gotham-Black-iPad4-Large", typeof(Font));
				} else {
					font_large = (Font)Resources.Load(FolderFonts + "Gotham-Black-iPad2-Large", typeof(Font));
				}
			}
			return font_large;
		}
		set{
			font_small = value;
		}
	}
	
	void Awake(){ 
		instance = this;
		InitializeFurniture(); 
	}
	
	public static void AddWelcomeScreen(){
		welcomeScreen =(GameObject) Instantiate( Resources.Load("Prefabs/WelcomeScreen"));
		welcomeScreen.GetComponent<WelcomeScreen>().InitSlide();
	}
	
	public static void RemoveWelcomeScreen(){
		Component.Destroy(GameObject.Find("PeekARGUIObject").GetComponent<WelcomeScreen>());
		Resources.UnloadUnusedAssets();
	}
	
	//assigns string for iPhone generation
	public static void InitializeFurniture(){
		if(iPhone.generation == iPhoneGeneration.iPad2Gen || 
			iPhone.generation == iPhoneGeneration.iPadMini1Gen){
			deviceGeneration = "iPad2";
		}
		else if(iPhone.generation == iPhoneGeneration.iPad3Gen ||
			iPhone.generation == iPhoneGeneration.iPad4Gen){
			deviceGeneration = "iPad4";
		}
		
		welcomeScreen = (GameObject)Instantiate(Resources.Load("Prefabs/WelcomeScreen"));
		welcomeScreen.GetComponent<WelcomeScreen>().InitCenter();
		
		//Hides iOS keyboard input field
		TouchScreenKeyboard.hideInput = true;
		
		//Creates Furniture Objects
		
		//Test Cabinet
		/*Texture2D CabinetTex = (Texture2D)Resources.Load(ThumbnailFolder + "Aeron_Round_01");
		Furniture Cabinet = new Furniture("Cabinet_Proxy", "Cabinet", false, CabinetTex);
		FurnitureCollection.AddToFurnitureList(Cabinet);*/
			
		#region Designer Furniture
		//Order of load is important for display
		
		//Eames
		Texture2D EamesShellChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Eames_Shell_tn_Round");
		Furniture EamesShellChair = new Furniture("Shell", "Eames", 
			"Fiberglass Side Shell Dimensions: 18W x 1H x 16D\n" +
			"Eiffel Dimensions: 19W x 17H x 17D", 
			"The Case Study Fiberglass Eiffel Chair is a classic icon and its name holds no enigma. " +
			"Pick your shell and choose the wire-frame in either chrome-plated steel or black powder-coated steel " +
			"for a visually satisfying chair.",
			false, EamesShellChairTex);
		FurnitureCollection.AddToFurnitureList(EamesShellChair);
		
		Texture2D EamesChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Eames_LCM_tn_Round");
		Furniture EamesChair = new Furniture("LCM", "Eames", 
			"26.5H x 22H x 24.5D Seat H 15.5", 
			"The iconic LCM (1946) began as an experiment in the Eameses apartment, where they were molding " +
			"plywood in what they called the Kazam! Machine. The machine pressed thin sheets of wood veneer " +
			"against a heated membrane that was inflated by a bicycle pump.", 
			false, EamesChairTex);
		FurnitureCollection.AddToFurnitureList(EamesChair);
		
		Texture2D EamesWireTex = (Texture2D)Resources.Load(ThumbnailFolder + "Eames_Wire_tn_Round");
		Furniture EamesWire = new Furniture("Wire", "Eames", 
			"H 32.75 W 19 D 21.25 Seat H 18.5", 
			"After refining their molded plastic chairs, Charles and Ray Eames began experimenting with a " +
			"chair made of welded wire.", 
			false, EamesWireTex);
		FurnitureCollection.AddToFurnitureList(EamesWire);
		
		Texture2D EamesRockerTex = (Texture2D)Resources.Load(ThumbnailFolder + "Eames_Rocker_tn_Round");
		Furniture EamesRocker = new Furniture("Rocker", "Eames", 
			"Fiberglass Arm Shell Dimensions: 25W x 18H x 18D\n" + "Rocker Dimensions: 15.5W x 14H 27D", 
			"The Case Study Fiberglass Rocking Chair is an essential for the modern home and with so many options, it " +
			"is possible to create your own one-of-a-kind chair.", 
			false, EamesRockerTex);
		FurnitureCollection.AddToFurnitureList(EamesRocker);
		
		Texture2D LaChaiseEamesTex = (Texture2D)Resources.Load(ThumbnailFolder + "Eames_La_Chaise_tn_Round");
		Furniture LaChaiseEames = new Furniture("La_Chaise", "Eames", 
			"H 34.5 W 59 D 35.5 Seat H 15.5", 
			"The voluptuous organic form of the Eames La Chaise was an evolution of plywood chairs developed a " +
			"year earlier in collaboration with architect Eero Saarinen for the Organic Design in Home Furnishings" +
			"competition at the Museum of Modern Art, New York.", 
			false, LaChaiseEamesTex);
		FurnitureCollection.AddToFurnitureList(LaChaiseEames);
		
		Texture2D EamesLoungeChairOttomanTex = (Texture2D)Resources.Load(ThumbnailFolder + "Eames_Lounge_Set_tn_Round");
		Furniture EamesLoungeChairOttoman = new Furniture("Lounge_and_Ottoman", "Eames", 
			"Chair: H 32 W 32.75 D 32.75 Seat H 15\nOttoman: H 17.25 W 26 D 21.5", 
			"Their iconic Eames Lounge Chair (1956) began with a desire to create a chair with the warm, receptive " +
			"look of a well-used first basemans mitt. In continuous production since its introduction in 1956, the " +
			"Eames Lounge Chair is widely considered one of the most significant designs of the 20th century.", 
			false, EamesLoungeChairOttomanTex);
		FurnitureCollection.AddToFurnitureList(EamesLoungeChairOttoman);
		
		//Nelson
		Texture2D NelsonPretzelTex = (Texture2D)Resources.Load(ThumbnailFolder + "Nelson_Pretzel_tn_Round");
		Furniture NelsonPretzel = new Furniture("Pretzel", "Nelson", 
			"733 x 675 x 479 mm", 
			"In 1952, predating the famous Coconut Chair or the Marshmallow Sofa, he designed a chair made " +
			"of moulded plywood originally referred to simply as the Laminated Chair. The bold and elegant curve " +
			"of the seat back and armrest soon earned it the nickname Pretzel Chair. ", 
			false, NelsonPretzelTex);
		FurnitureCollection.AddToFurnitureList(NelsonPretzel);
		
		Texture2D NelsonMarshmellowTex = (Texture2D)Resources.Load(ThumbnailFolder + "Nelson_Marshmellow_tn_Round");
		Furniture NelsonMarshmellow = new Furniture("Marshmellow", "Nelson", 
			"H 31 W 52 D 29",
			"The Marshmallow sofa brightens a room and makes a playful or dramatic statement, depending upon the " +
			"materials and colors you choose. Its whimsical design comprises 18 round, comfortable cushions that seem " +
			"to float on the brushed tubular steel frame. The sofa's unique appearance and eye-catching appeal led the " +
			"way into the pop art style of the 1960s.",
			false, NelsonMarshmellowTex);
		FurnitureCollection.AddToFurnitureList(NelsonMarshmellow);
		
		Texture2D NelsonVitraTrayTex = (Texture2D)Resources.Load(ThumbnailFolder + "Nelson_Vitra_Tray_tn_Round");
		Furniture NelsonVitraTray = new Furniture("Vitra_Tray", "Nelson", 
			"H 19.5 - 27.25 W 15.25 D 15.25", 
			"The Vitra Tray Table from George Nelson is characterized by its simple, classic elegance and intricate individual elements.", 
			false, NelsonVitraTrayTex);
		FurnitureCollection.AddToFurnitureList(NelsonVitraTray);
		
		//Saarinen
		Texture2D SaarinenTulipChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Chair_tn_Round");
		Furniture SaarinenTulipChair = new Furniture("Tulip_Chair", "Saarinen", 
			"H 32 W 20 D 21.25 Seat H 18.5", 
			"Eero Saarinen vowed to address the ugly, confusing, unrestful world he observed underneath chairs and tables " +
			"-- the so-called slum of legs. A five-year design investigation led him to the revolutionary Pedestal Collection, " +
			"including the Tulip Chair, introduced in 1958.", 
			false, SaarinenTulipChairTex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipChair);
		
		Texture2D SaarinenTulipArmchairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_ArmChair_tn_Round");
		Furniture SaarinenTulipArmchair = new Furniture("Tulip_Armchair", "Saarinen", 
			"31 1/2 x 25 1/4 x 23 1/2", 
			"The Tulip Armchair, which resembles the flower but also a stemmed wineglass, is part of Saarinen's last furniture series.", 
			false, SaarinenTulipArmchairTex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipArmchair);
		
		Texture2D SaarinenBallChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Ball_tn_Round");
		Furniture SaarinenBallChair = new Furniture("Ball", "Saarinen", 
			"N/A",
			"Due to its unusual shape Ball Chair sticks in your mind and is one of the most memorable examples of furniture design of the 1960s.",
			false, SaarinenBallChairTex);
		FurnitureCollection.AddToFurnitureList(SaarinenBallChair);
		
		Texture2D SaarinenWombChairStoolTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Womb_Set_tn_Round");
		Furniture SaarinenWombChairStool = new Furniture("Womb_Chair_and_Stool", "Saarinen", 
			"Chair: H 35.5 W 40 D 34 Seat H 16\nStool: H 16 W 25.5 D 20", 
			"Eero Saarinen designed the groundbreaking Womb Chair at Florence Knoll request for a chair that was like a " +
			"basket full of pillows - something I could really curl up in. The fiberglass design supported multiple sitting " +
			"postures and provided a comforting sense of security - hence the name. ", 
			false, SaarinenWombChairStoolTex);
		FurnitureCollection.AddToFurnitureList(SaarinenWombChairStool);
		
		Texture2D SaarinenTulipTableATex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Table_A_tn_Round");
		Furniture SaarinenTulipTableA = new Furniture("Tulip_Table_A", "Saarinen", 
			"H 29.5 W 80.5 L 52.3", 
			"N/A", 
			false, SaarinenTulipTableATex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipTableA);
		
		Texture2D SaarinenTulipTableFTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Table_F_tn_Round");
		Furniture SaarinenTulipTableF = new Furniture("Tulip_Table_F", "Saarinen", 
			"H 29.5 W 71 L 71", 
			"N/A",
			false, SaarinenTulipTableFTex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipTableF);
		
		Texture2D SaarinenTulipTableCTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Table_C_tn_Round");
		Furniture SaarinenTulipTableC = new Furniture("Tulip_Table_C", "Saarinen", 
			"H 20 W 17 L 17", 
			"N/A",
			false, SaarinenTulipTableCTex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipTableC);
		
		Texture2D SaarinenTulipTableETex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Table_E_tn_Round");
		Furniture SaarinenTulipTableE = new Furniture("Tulip_Table_E", "Saarinen", 
			"H 20 W 20.5 L 20.5", 
			"N/A",
			false, SaarinenTulipTableETex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipTableE);
		
		Texture2D SaarinenTulipTableBTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Table_B_tn_Round");
		Furniture SaarinenTulipTableB = new Furniture("Tulip_Table_B", "Saarinen", 
			"H 20 W 23 L 16", 
			"N/A",
			false, SaarinenTulipTableBTex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipTableB);
		
		Texture2D SaarinenTulipTableDTex = (Texture2D)Resources.Load(ThumbnailFolder + "Saarinen_Tulip_Table_D_tn_Round");
		Furniture SaarinenTulipTableD = new Furniture("Tulip_Table_D", "Saarinen", 
			"H 15.5 W 23 L 23", 
			"N/A",
			false, SaarinenTulipTableDTex);
		FurnitureCollection.AddToFurnitureList(SaarinenTulipTableD);
		
		
		//Wegner - Main Category
		Texture2D PPMobler62Tex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_62_tn_Round");
		Furniture PPMobler62 = new Furniture("PP62", "Wegner", 
			"H 66 W 58 D 48 Seat H 43",
			"pp62 is a versatile chair with many interesting solutions. The back draws attention due to its " +
			"organic idiom, creating an interesting geometrical structure. As always with Wegner, the point of " +
			"departure is pragmatic. He thus combines solid and laminated wood into the top rail.",
			false, PPMobler62Tex);
		FurnitureCollection.AddToFurnitureList(PPMobler62);
		
		Texture2D PPMobler68Tex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_68_tn_Round");
		Furniture PPMobler68 = new Furniture("PP68", "Wegner", 
			"H 71 W 58 D 48",
			"In 1987, 73 years old, Hans J. Wegner designed the pp68 as his final basic chair. Genuine in all aspects " +
			"- comfortable, practical, strong, durable and affordable. ",
			false, PPMobler68Tex);
		FurnitureCollection.AddToFurnitureList(PPMobler68);
		
		Texture2D PPMoblerCowHornChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_Cow_Horn_tn_Round");
		Furniture PPMoblerCowHornChair = new Furniture("Cow_Horn", "Wegner", 
			"H 71 W 59 D 45 Seat H 42",
			"Designed in 1952 the Cow Horn Chair is the immediate follow up on the breakthrough of Wegner's carrier, " +
			"the Round Chair, and the continuity in shape and philosophy is obvious. But serving a different purpose " +
			"Wegner created a chair that was to play an important role in his following line of works.",
			false, PPMoblerCowHornChairTex);
		FurnitureCollection.AddToFurnitureList(PPMoblerCowHornChair);
		
		Texture2D WegnerValetChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Wegner_Valet_tn_Round");
		Furniture WegnerValetChair = new Furniture("Valet", "Wegner", 
			"H 95 W 51 D 50 Seat H 45",
			"Fun often plays a major part in Wegner's work. And this is perhaps most obvious in The Valet Chair. " +
			"The idea for the chair was conceived in 1951 after a long talk with Professor of Architecture Steen Eiler " +
			"Rasmussen and designer Bo Bojesen about the problems in folding clothes in the most practical manner at bedtime.",
			false, WegnerValetChairTex);
		FurnitureCollection.AddToFurnitureList(WegnerValetChair);
		
		Texture2D CarlHansen33Tex = (Texture2D)Resources.Load(ThumbnailFolder + "Carl_Hansen_33_tn_Round");
		Furniture CarlHansen33 = new Furniture("CH33", "Wegner", 
			"H 74 W 55 D 48 Seat H 44",
			"The CH33 chair has been designed by Hans J. Wegner in 1957. Carl Hansen proposed a contemporary twist on the original design.",
			false, CarlHansen33Tex);
		FurnitureCollection.AddToFurnitureList(CarlHansen33);
		
		Texture2D CarlHansenShellChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Carl_Hansen_Shell_tn_Round");
		Furniture CarlHansenShellChair = new Furniture("CH07", "Wegner", 
			"H 83 W 92 D 83 Seat H 35",
			"Shell chair is one of the most original chairs designed by Hans J. Wegner. ",
			false, CarlHansenShellChairTex);
		FurnitureCollection.AddToFurnitureList(CarlHansenShellChair);
	
		Texture2D CarlHansen25Tex = (Texture2D)Resources.Load(ThumbnailFolder + "Carl_Hansen_25_tn_Round");
		Furniture CarlHansen25 = new Furniture("CH25", "Wegner", 
			"H 73 W 71 D 73 Seat H 35",
			"Hans J. Wegners oblique back legs give a very characteristic look to the CH25, both calm and dynamic.",
			false, CarlHansen25Tex);
		FurnitureCollection.AddToFurnitureList(CarlHansen25);
		
		Texture2D PPMoblerCircleChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_Circle_tn_Round");
		Furniture PPMoblerCircleChair = new Furniture("Circle", "Wegner", 
			"H 97 W 112 D 94 Seat H 42",
			"In 1986 PP Mobler initiated the production of this remarkable chair, which Wegner designed for the workshop. " +
			"Like the Flag Halyard Chair, the Circle Chair's design appears unconnected with both historical predecessors " +
			"and Wegner's remaining production.",
			false, PPMoblerCircleChairTex);
		FurnitureCollection.AddToFurnitureList(PPMoblerCircleChair);
		
		Texture2D CarlHansenWingChairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Carl_Hansen_Wing_tn_Round");
		Furniture CarlHansenWingChair = new Furniture("Wing", "Wegner", 
			"H 103 W 90 D 90 Seat H 60",
			"Hans J. Wegner designed the wing chair in 1960.",
			false, CarlHansenWingChairTex);
		FurnitureCollection.AddToFurnitureList(CarlHansenWingChair);
		
		Texture2D ErikJoergensenOxchairTex = (Texture2D)Resources.Load(ThumbnailFolder + "Erik_Jeorgensen_Oxchair_tn_Round");
		Furniture ErikJoergensenOxchair = new Furniture("Oxchair", "Wegner", 
			"H 35.5 W 39 D 39 Seat H 14",
			"Wegner's fascination with Picasso inspired him to do this sculptural and powerful chair.",
			false, ErikJoergensenOxchairTex);
		FurnitureCollection.AddToFurnitureList(ErikJoergensenOxchair);
		
		Texture2D PPMoblerTeddyChairStoolTex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_Teddy_Bear_Set_tn_Round");
		Furniture PPMoblerTeddyChairStool = new Furniture("Teddy_Chair_and_Footstool", "Wegner", 
			"Chair: H 101 W 90 D 95 Seat H 42\nStool: H 43 W 65 D 40",
			"Be embraced by the great bear paws of this all time maximum comfort easy chair. " +
			"Consider it an investment for life, and along comes at least two weeks of thorough work of Danish craftsmen. ",
			false, PPMoblerTeddyChairStoolTex);
		FurnitureCollection.AddToFurnitureList(PPMoblerTeddyChairStool);
		
		Texture2D CarlHansen103Tex = (Texture2D)Resources.Load(ThumbnailFolder + "Carl_Hansen_103_tn_Round");
		Furniture CarlHansen103 = new Furniture("CH103", "Wegner", 
			"H 28 W 85.5 D 30 Seat H 16.5",
			"Hans J. Wegner designed this series in the early 70s with the Danish firm Johannes Hansen, " +
			"producing a limited number at that time.",
			false, CarlHansen103Tex);
		FurnitureCollection.AddToFurnitureList(CarlHansen103);
		
		Texture2D PPMobler75Tex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_75_tn_Round");
		Furniture PPMobler75 = new Furniture("PP75", "Wegner", 
			"H 70 W 140 D 140",
			"The table was designed by Hans J. Wegner in 1982 for PP Mobler. pp75 is an exiting table from Wegner's hand. " +
			"Especially noticeable is the frame, where we see Wegner's preoccupation with triangles.",
			false, PPMobler75Tex);
		FurnitureCollection.AddToFurnitureList(PPMobler75);
		
		Texture2D PPMoblerTondernTable01Tex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_Tondern_Table_01_tn_Round");
		Furniture PPMoblerTeddyTondernTable01 = new Furniture("Tondern_Table_01", "Wegner", 
			"H 71 W 180 D 86",
			"N/A",
			false, PPMoblerTondernTable01Tex);
		FurnitureCollection.AddToFurnitureList(PPMoblerTeddyTondernTable01);
		
		Texture2D PPMoblerTondernTable02Tex = (Texture2D)Resources.Load(ThumbnailFolder + "PP_Mobler_Tondern_Table_02_tn_Round");
		Furniture PPMoblerTeddyTondernTable02 = new Furniture("Tondern_Table_02", "Wegner", 
			"H 71 W 160 D 86",
			"N/A",
			false, PPMoblerTondernTable02Tex);
		FurnitureCollection.AddToFurnitureList(PPMoblerTeddyTondernTable02);
		#endregion Designer Furniture
	}
}
