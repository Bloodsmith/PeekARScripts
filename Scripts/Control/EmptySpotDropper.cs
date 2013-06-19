using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptySpotDropper : StandardDropper {
	
	private GameObject testCube;
	
	//radians
	private float Angle = 0;
	private float Radius = 0;
	
	new void Start() {
		
		//transform.position = origin;
		transform.Translate(100000,0,0);
		
		DropLoc = Vector3.zero;
		SpiralTest();
		
		base.Start();
	}
	
	
	private void SpiralTest() {
		Radius = 1f;
		
		while (Radius < 100) {
			
			//calculate X
			float X = Mathf.Cos(Angle)*Radius;
			//assign Y based on origin Y and adding to it so the ray is cast from above
			float Y = 10f;
			//Calculate Z
			float Z = Mathf.Sin(Angle)*Radius;
			
			RaycastHit hit;
			//Create a vector 3 from the instantiated objects extents
			Vector3 extents = collider.bounds.extents;
			//Cast a ray and assign bool
			bool rayhit = Physics.SphereCast(new Vector3(X, Y, Z), Mathf.Max(extents.x, extents.z), new Vector3(0, -1, 0), out hit);
			
			 bool viewable = Camera.mainCamera.WorldToViewportPoint(new Vector3(X,0,Z)).x < 0.7f &&
                             Camera.mainCamera.WorldToViewportPoint(new Vector3(X,0,Z)).x > 0.3f &&
                             Camera.mainCamera.WorldToViewportPoint(new Vector3(X,0,Z)).y < 0.85f &&
                             Camera.mainCamera.WorldToViewportPoint(new Vector3(X,0,Z)).y > 0f;
			//if bool is false
			if (!rayhit && viewable) {
				//Change the position of the instantiated game object
				DropLoc = new Vector3(X,0,Z);
				break;
			}
			//Calculate new Angle and Radius
			Angle -= (Mathf.PI * 2f)/100f;
			Radius += .02f;
		}
	}
}
