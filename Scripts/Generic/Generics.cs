using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Generics {}

public interface Lerp<T> {
		
	void apply(T a, T b, float t, out T r);
}


public static class Extensions {
	
	public static void Lerp(this Vector3 pos, Vector3 p1, Vector3 p2, float t) {
		float nt = 1-t;
		pos.x = nt*p1.x + t*p2.x;
		pos.y = nt*p1.y + t*p2.y;
		pos.z = nt*p1.z + t*p2.z;
	}
	
	public static void Lerp(this Transform tr, Transform A, Transform B, float t) {
		
		tr.position = Vector3.Lerp(A.position, B.position, t);
		tr.rotation = Quaternion.Slerp(A.rotation, B.rotation, t);
	}
	
	public static void Lerp(this GameObject go, GameObject A, GameObject B, float t) {
		
		go.transform.Lerp (A.transform, B.transform, t);
	}
	
	public static void Lerp(this Camera c, Camera A, Camera B, float t /*, mask: Int*/) {
		
		c.transform.Lerp (A.transform, B.transform, t);
		//c.fieldOfView = Mathf.Lerp(A.fieldOfView, B.fieldOfView, t);
	}
	
	public static float DistanceTo(this GameObject go, GameObject B) {
		
		return Vector3.Distance( go.transform.position, B.transform.position );
	}
	
	public static void RemoveFromScene(this GameObject go) {  }
	
	public static FurnitureItem FurnitureItem(this MonoBehaviour go) { return go.GetComponent<FurnitureItem>(); }
	
}
