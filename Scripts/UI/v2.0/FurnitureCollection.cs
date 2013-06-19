/*Collection for Furniture*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FurnitureCollection{
	static string defaultSearchString = "Search";
	static string SearchString = "";
	static string categoryString = "";
	public static string CategoryString{
		get{
			return categoryString;
		}
		set {
			categoryString = value; 
			UpdateSearch();
		}
	}
	static bool searchMode = true;
	public static bool SearchMode{
		get{
			return searchMode;
		}
		set{
			searchMode = value; 
			UpdateSearch();
		}
	}
	//List
	static List<Furniture> FurnitureList;
	static List<string> CategoryList;
	static List<object> currentList = new List<object>();
	
	public static void Reset(){
		SearchString = defaultSearchString;
		CategoryString = "";
		UpdateSearch();
	}
	
	
	//Sets for Lists
	public static void AddToFurnitureList(Furniture furn){
		//Initialize the list
		if(FurnitureList == null){ FurnitureList = new List<Furniture>(); }
		if(CategoryList == null){ CategoryList = new List<string>(); }
		//Add Furniture Objects
		FurnitureList.Add(furn);
		
		if(!CategoryList.Contains(furn.GetCategoryString())){ 
			CategoryList.Add(furn.GetCategoryString()); 
			CategoryList.Sort();
		}
	}

	//Set for Strings
	public static void UpdateSearchString(string search){ 
		SearchString = search;
		UpdateSearch();
	}
	//Get for current list & Furniture List
	public static List<object> GetCurrentList(){ return currentList; }
	//public static List<Furniture> GetFurnitureList(){ return FurnitureList; }
	
	
	//Search update
	public static void UpdateSearch(){
		currentList.Clear();
		if(SearchMode){
			//Shows Furniture in a Category
			if(CategoryString != "" && (SearchString == "" || SearchString == defaultSearchString)){
				foreach(Furniture f in FurnitureList){
					if(f.GetCategoryString() == CategoryString){
						currentList.Add(f);
					}
				}
			}
			//Shows Furniture from the user search string
			else if(CategoryString == "" &&  !(SearchString == "" || SearchString == defaultSearchString)){
				foreach(string s in CategoryList){
					if(s.ToLower().Contains(SearchString.ToLower())){
						currentList.Add(s);
					}
				}
			}
			//Shows Furniture in a Category with the user search string
			else if(CategoryString != "" && !(SearchString == "" || SearchString == defaultSearchString)){
				foreach(Furniture f in FurnitureList){
					if(f.GetCategoryString() == CategoryString){
						if(f.GetDisplayName().ToLower().Contains(SearchString.ToLower())){
							currentList.Add(f);
						}
					}
				}
			}
			//no category and no search
			else{
				foreach(string s in CategoryList){
					currentList.Add(s);
				}
			}
		}
		//User Cart
		else{
			SearchString = "";
			categoryString = "";
			foreach(Furniture f in FurnitureList){
				foreach(GameObject g in MainInterface.ActiveFurn){
					if (g != null && g.name.Contains (f.GetName())) {
						currentList.Add(f);
					}
				}
			}
		}
	}
}
