using UnityEngine;
using System.Collections;

public class TouchPlane {
	
	public static TouchPlane GroundPlane(Camera c) { return new TouchPlane(c, Vector3.zero, Vector3.up); }
	
	public Vector3 origin { get { return _origin; } }
	
	public Vector3 normal { get { return _normal; } }
	
	private Vector3 _origin, _normal;
	private Camera _c;
	
	public TouchPlane(Camera c, GameObject go) : this(c, go.transform.position, go.transform.up) { }
	
	public TouchPlane(Camera c, Vector3 origin, Vector3 normal) {
		_c = c;
		_origin = origin;
		_normal = normal;
	}
	
	private float LinePlaneIntersection(Vector3 lo, Vector3 ld, Vector3 po, Vector3 pd) {
		
		return Vector3.Dot( (po - lo), pd ) / Vector3.Dot( ld, pd );
	}
	
	public Vector3 PointOnPlaneFrom(Ray ray) {
		
		float d = LinePlaneIntersection(ray.origin, ray.direction, _origin, _normal);
		
		if(d > 0)
			return ray.origin + ray.direction * d;
		else 
			return Vector3.zero;
	}
	
	public Vector3 PointOnPlaneFrom(Touch t) {
		 
		return PointOnPlaneFrom(t.position);
	}
	
	public Vector3 PointOnPlaneFrom(Vector3 screenPos) {
		
		Ray r = _c.ScreenPointToRay(screenPos);
		return PointOnPlaneFrom(r);
	}
}
