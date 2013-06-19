using UnityEngine;
using System.Collections;

public class TickingRotator : MonoBehaviour {
	
	public Vector3 Axis;
	public float Speed;
	public float Tick;
	
	private float tickCnt;
	private bool slowing = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (tickCnt > Tick) {
			transform.RotateAroundLocal(Axis, Speed);
			slowing = true;
			//tickCnt = 0;
		}
		
		if (slowing && tickCnt > 0) {
			transform.RotateAroundLocal(Axis, tickCnt*Speed*0.1f);
			tickCnt -= 0.5f;
		}
		
		else slowing = false;
		
		tickCnt += 0.1f;
	}
}
