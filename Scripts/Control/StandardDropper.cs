using UnityEngine;
using System.Collections;

public class StandardDropper : MonoBehaviour {

	private float t = 0;
	private FurnitureData fi;
	
	public Vector3 DropLoc;
	
	protected void Start() {
		fi = GetComponent<FurnitureData>();
		
		transform.position = DropLoc;
		//Face Camera
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 
			Camera.mainCamera.transform.rotation.eulerAngles.y + 180);
		fi.Shadowplane.transform.parent = null;
		transform.Translate(0,3,0, Space.World);
		fi.Shadowplane.renderer.material.SetFloat("_AlphaMod", 1);
	}
	
	protected void Update() {
		
		float h = transform.position.y;
		
		t += Time.deltaTime;
		
		fi.Shadowplane.renderer.material.SetFloat("_AlphaMod", h/3);
		
		Vector3 p = transform.position;
		p.y = h/(10*t);
		transform.position = p;
		
		if (h < 0.01) {
			fi.Shadowplane.transform.parent = transform;  //Physics can knock furniture off their shadowplane during drop
			Destroy(this);
		}
	}
}
