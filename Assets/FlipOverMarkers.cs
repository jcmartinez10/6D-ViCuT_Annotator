using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipOverMarkers : MonoBehaviour {

	bool shortFlip;
	// Use this for initialization
	void Start () {
		shortFlip=false;
	}
	
	// Update is called once per frame
	void Update () {
		shortFlip=(Input.GetKey(KeyCode.LeftShift));
		
	}
	void OnMouseOver(){
		/*  if (Input.GetMouseButtonDown(0)){
			 gameObject.GetComponentInParent<SetUpBox>().rotateOverMarkers ();
			 Debug.Log("click!");
		 } */

		if (Input.GetMouseButtonDown(0)){
			flipOver();
		}

       // Whatever you want it to do.
    }

	void flipOver(){
		if(shortFlip){
			transform.position=transform.position+transform.parent.localScale.x*transform.right;
		}
		else{
			transform.position=transform.position-transform.parent.localScale.z*transform.forward;
		}
		transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
	}

 
}
