using System;
using UnityEngine;

public class ScaledRect
{
	static iPhoneGeneration generation;
	static float scaleFactor = -1;
	
	static void Init(){
		

			
		generation = iPhoneGeneration.iPhone; 
		scaleFactor = Startup.ScaleFactor;
	}
	
	public static Rect FullScreenRect{
		get{
			return new UnityEngine.Rect (0,0,2048,1536);
		}
	}
	
	
	public static Rect Rect (float x, float y, float width, float height, float sizeScale){
		return Rect(x, y, width * sizeScale, height * sizeScale);
	}
	
	public static Rect Rect (float x, float y, float width, float height)
	{
		if(scaleFactor == -1)
		Init ();
		
		Rect r = new Rect(x,y,width,height);
		
		r.x *= scaleFactor;
		r.y *= scaleFactor;
		r.width *= scaleFactor;
		r.height *= scaleFactor;
		
		return r;
	}
}


