/*Class to create a Furniture Object*/
using UnityEngine;
using System.Collections;

public class Furniture{

	//Properties
	string Name;
	string DisplayName;
	string CategoryString;
	string Dimensions;
	string Description;
	bool InScene;
	Texture2D Image;
	
	//Constructor
	public Furniture(string name, string categoryString, string dimensions, string description, 
		bool inScene, Texture2D image){
		Name = name;
		//Display Name uses a string parser on Name
		DisplayName = Name.Replace("_", " ");
		CategoryString = categoryString;
		Dimensions = dimensions;
		Description = description;
		InScene = inScene;
		Image = image;
	}
	
	//Get/Set for Properties
	public string GetName(){ return Name; }
	public string GetDisplayName(){ return DisplayName; }
	public string GetCategoryString(){ return CategoryString; }
	public string GetDescription(){ return Description; }
	public string GetDimensions(){ return Dimensions; }
	public bool GetInScene(){ return InScene; }
	public void SetInScene(bool inScene){ InScene = inScene; }
	public Texture2D GetImage(){ return Image; }
}
