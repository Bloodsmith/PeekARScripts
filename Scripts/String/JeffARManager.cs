using UnityEngine;
using System.Collections;

public class JeffARManager : ContentCentricARManager
{
	
	MainInterface mi;
	new void Start()
	{
		mi = Camera.mainCamera.GetComponent<MainInterface>();
		base.Start ();
	}
	
	public bool on = true;
	
	float RValue = 1.1f;
	float GValue = 1.1f;
	float BValue = 1f;
	Vector4 ncol;
	
	new void Update() 
	{
		ncol = new Vector4(col.x, col.y, col.z, col.w);
	
		if(mi.mode == MainInterface.Mode.Delete){ 
			//Debug.Log("Red mode");
			ncol = new Vector4(1, col.y/8, col.z/8, col.w);
		}
		else{ 
			if(ncol.x > ncol.y && ncol.x > ncol.z){
				RValue = ncol.x;
				GValue = (ncol.x - ncol.y)*(2f/3f) + ncol.y;
				BValue = (ncol.x - ncol.z)*(2f/3f) + ncol.z;
			}
			else if(ncol.y > ncol.x && ncol.y > ncol.z){
				RValue = (ncol.y - ncol.x)*(2f/3f) + ncol.x;
				GValue = ncol.y;
				BValue = (ncol.y - ncol.z)*(2f/3f) + ncol.z;
			}
			else if(ncol.z > ncol.x && ncol.z > ncol.y){
				RValue = (ncol.z - ncol.x)*(2f/3f) + ncol.x;
				GValue = (ncol.z - ncol.y)*(2f/3f) + ncol.y;
				BValue = ncol.z;
			}
			//Multiply modifier with Raw marker colour
			//ncol.x *= yellowR;
			//ncol.y *= yellowG;
			//ncol.z *= yellowB;
			//Multiply by itself to emphasize tint
			//float colX = Mathf.Pow(ncol.x, 1.5f);
			//float colY = Mathf.Pow(ncol.y, 1.5f);
			//float colZ = Mathf.Pow(ncol.z, 1.5f);
			//ncol = new Vector4(colX, colY, colZ, 1f);
			ncol = new Vector4(RValue, GValue, BValue, 1f);
			ncol.Normalize();
			//To account for darkening by normalization
			ncol *= 1.7f;
			//Debug.Log ("Normalize Col: " + col);
		}
		Shader.SetGlobalColor("_MarkerHue", ncol);
		//Debug.Log("Jman Update");
		if (on){
			//Debug.Log ("On is On");
			base.Update();
		}
	}
	
	/*void OnGUI(){
		GUI.Label(new Rect(400, 25, 200, 100), "R Calc: " + ncol.x);
		GUI.Label(new Rect(550, 25, 200, 100), "G Calc: " + ncol.y);
		GUI.Label(new Rect(700, 25, 200, 100), "B Calc: " + ncol.z);

	}*/
}
