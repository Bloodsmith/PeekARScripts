using UnityEngine;
using System.Collections;

public class PrefabInit : MonoBehaviour {
	
	public GameObject prefab;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Created: " + prefab);
		//Instantiate(prefab);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
