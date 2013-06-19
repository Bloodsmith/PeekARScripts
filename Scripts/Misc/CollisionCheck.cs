using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	 void OnCollisionEnter(Collision collision) {
		Debug.Log ("CC Added");
        foreach (ContactPoint contact in collision.contacts) {
            Debug.Log ("Contact: " + contact.thisCollider + " Contacted: " + contact.otherCollider + " Where: " + contact.point);
        } 
    }
}
