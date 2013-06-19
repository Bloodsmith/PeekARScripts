/* Class for saving PlayerPrefs for the user. Written by Brett Hadley */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserOptions{
	
	//Method to check PlayerPrefs
	public static bool GetPersistantHelpState(){
		//Check if PlayerPrefs contains this key/value pair
		if(!PlayerPrefs.HasKey("Help")){
			//Assign PlayerPrefs for Help
			PlayerPrefs.SetInt("Help", 1);
			//Save PlayerPrefs
			PlayerPrefs.Save();
			return true;
		}else{
			//Check PlayerPrefs
			if(PlayerPrefs.GetInt("Help") == 1){ return true; }
			else{ return false; }
		}
	}
	
	//Method to set the PlayerPrefs
	public static void SetPersistantHelpState(bool newState){
		if(newState){ 
			PlayerPrefs.SetInt("Help", 1);
		}
		else{ 
			PlayerPrefs.SetInt("Help", 0); 
		}
			
		PlayerPrefs.Save() ;
	}
	
	//Sets the device ID
	public static void SetDeviceID(){
		if(!PlayerPrefs.HasKey("DeviceID")){
			PlayerPrefs.SetString("DeviceID", SystemInfo.deviceUniqueIdentifier);
			PlayerPrefs.Save();
			SetDeviceResolution();
		}
		else{
			if(PlayerPrefs.GetString("DeviceID") != SystemInfo.deviceUniqueIdentifier){
				PlayerPrefs.SetString("DeviceID", SystemInfo.deviceUniqueIdentifier);
				PlayerPrefs.Save();
				SetDeviceResolution();
			}
		}
		//Debug.Log (PlayerPrefs.GetString("DeviceID"));
	}
	
	//Checks device resolution
	public static int CheckDeviceResolution(){
		if(PlayerPrefs.GetInt("DeviceResolution") == 2048){
			return 2048;
		}
		else{
			return 1024;
		}
	}
	
	//Sets device resolution
	static void SetDeviceResolution(){
		if(iPhone.generation == iPhoneGeneration.iPad1Gen || iPhone.generation == iPhoneGeneration.iPad2Gen || iPhone.generation == iPhoneGeneration.iPadMini1Gen){
			PlayerPrefs.SetInt("DeviceResolution", 1024);
		}
		else if(iPhone.generation == iPhoneGeneration.iPad3Gen || iPhone.generation == iPhoneGeneration.iPad4Gen){
			PlayerPrefs.SetInt("DeviceResolution", 2048);
		}
	}
}
