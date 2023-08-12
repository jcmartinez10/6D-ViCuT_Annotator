using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour {

	public Camera kin;

	Vector3 forward;

	Vector3 right;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate()
{
        //reading the input:
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
         
        //assuming we only using the single camera:
 
        //camera forward and right vectors:
        forward = kin.transform.forward;
        right = kin.transform.right;
 
        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
		forward.Normalize();
        right.Normalize();
 
        //this is the direction in the world space we want to move:
        Vector3 desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

		desiredMoveDirection.Normalize();
 
        //now we can apply the movement:
        transform.Translate(desiredMoveDirection * 1.1f * Time.deltaTime);
}
}
