using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deleter : MonoBehaviour {
		
	private float t = 0;
	private FurnitureData fi;
	
	void Start() {
		//moveObj = GetComponent<FurnitureItem>().Item;
		//Shader.SetGlobalFloat("_AlphaMod", 0);
		fi = GetComponent<FurnitureData>();
		fi.Shadowplane.transform.parent = null;
		transform.collider.enabled = false;
	}
	
	void Update() {
		
		float h = transform.position.y;
		
		t += Time.deltaTime;
		
		float m = 2*t*t*t;
		
		transform.Translate (0,m,0, Space.World);
		//fi.transform.Translate(0,-m,0);
		
		fi.Shadowplane.renderer.material.SetFloat("_AlphaMod", h/3);
		
		
		if (h > 3) {
			fi.Shadowplane.transform.parent = transform;
			Debug.Log (gameObject);
			//Call to release memory
			MainInterface.ReleaseMemory();
			Destroy(gameObject);
		}
	}
}
