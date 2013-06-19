using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class NativeCalls : MonoBehaviour {
	
	[DllImport("__Internal")]
	public static extern void _FireAlert(string title, string msg);
	
	//Moves photo to gallery in native code
	[DllImport("__Internal")]
	public static extern void _ImageSaver(string imageName);
	
	//Method sent to iOS to confirm social posting
	[DllImport("__Internal")]
	public static extern void _ConfirmAlert(string socialName);
	
	//Methid called from iOS to confirm social posting
	public void ReturnConfirm(string confirm){

		Debug.Log ("Passed from iOS: " + confirm);
		
		if(confirm == "Yes"){
		
			Application.OpenURL("http://peek-ar.com");
		
		}

	}
}
