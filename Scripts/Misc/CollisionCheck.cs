using UnityEngine;
using System.Collections;
using System;

public class CollisionCheck : MonoBehaviour {
    //Vectors
    Vector3 startVect = Vector3.zero;
    //enum to control movement direction
    enum MovementDirection { Up, Down, Left, Right, None }
    MovementDirection XMovementDirection = MovementDirection.None;
    MovementDirection ZMovementDirection = MovementDirection.None;
    //access Mousescript
    //MouseScript ms;
	MainInterface mi;
    //Variables to control variances and checks
    float movementCheck = 0.5f;
    float collisionOffset = 25f;

    void Start()
    {
        //ms = Camera.mainCamera.GetComponent<MouseScript>();
    }

    //Fired on Collisions
    void OnCollisionEnter(Collision collision)
    {
        //Checks if any movement on this object
        if (XMovementDirection != MovementDirection.None || ZMovementDirection != MovementDirection.None)
        {
			transform.rigidbody.isKinematic = true;
            //Checks which direction it is moving using enum
            if (XMovementDirection == MovementDirection.Right)
            {
                transform.position = new Vector3(transform.position.x - collisionOffset, transform.position.y, transform.position.z);
            }
            if (XMovementDirection == MovementDirection.Left)
            {
                transform.position = new Vector3(transform.position.x + collisionOffset, transform.position.y, transform.position.z);
            }
            if (ZMovementDirection == MovementDirection.Up)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - collisionOffset);
            }
            if (ZMovementDirection == MovementDirection.Down)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + collisionOffset);
            }
        }
    }

    void Update()
    {   
        //Checks if the rigidbody is awake
        if (!transform.rigidbody.IsSleeping() && transform.rigidbody.isKinematic == true)
        {
            transform.rigidbody.isKinematic = false;
            //sets movement vector
            Vector3 movementVect = transform.position - startVect;
            //Debug.Log("Movement Vect: " + movementVect);

            //checks which direction object is moving
            if (movementVect.x > movementCheck)
            {
                XMovementDirection = MovementDirection.Right;
            }
            if (movementVect.x < -movementCheck)
            {
                XMovementDirection = MovementDirection.Left;
            }
            if (movementVect.y > movementCheck)
            {
                ZMovementDirection = MovementDirection.Up;
            }
            if (movementVect.y < -movementCheck)
            {
                ZMovementDirection = MovementDirection.Down;
            }
        }
        else
        {
            //Reset values
            transform.rigidbody.isKinematic = true;
            startVect = transform.position;
            XMovementDirection = MovementDirection.None;
            ZMovementDirection = MovementDirection.None;
        }
    }
}
