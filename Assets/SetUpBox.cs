using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SetUpBox : MonoBehaviour {


	public bool flipped;
	public string BoxName;
	bool shortSide;

	Renderer m_Renderer;

	public bool shortflip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	/* 	if (Input.GetKeyDown(KeyCode.LeftShift)){
			 shortflip=true;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift)){
			 shortflip=false;
		}
		
	}
	void OnMouseOver(){
		 if (Input.GetMouseButtonDown(0)){
			 rotateOverMarkers ();
			 Debug.Log(BoxName);
		 } */
       // Whatever you want it to do.
    }
 

	public void setStartPos(float x, float y, float z,float p1x, float p1y, float p2x, float p2y, float pz){

		GameObject laCaja = transform.GetChild (0).gameObject; 
		laCaja.name=BoxName;

		float dsquared = (p2x-p1x)*(p2x-p1x)+(p2y-p1y)*(p2y-p1y);
		float theta = Mathf.Atan2((p2x-p1x),(p2y-p1y));

		string msg= "X: " + x + " Y: " + z + "Distancia: " + Mathf.Sqrt(dsquared);
		//Debug.Log(msg);

		float alpha = 0.0f;

		transform.localScale=new Vector3(x,y,z);

		/* if (dsquared < y*y){
			transform.localScale=new Vector3(z,y,x);
			flipped=true;
		} */

		transform.localEulerAngles=new Vector3(0f,theta*Mathf.Rad2Deg+alpha,0f);
		transform.position = new Vector3(p1x,pz,p1y);

		var texture = Resources.Load<Texture2D>("Textures/"+BoxName);
		m_Renderer = GetComponentInChildren<MeshRenderer> ();
		m_Renderer.material.SetTexture("_MainTex", texture);



	}


	public void setStartPos1(float x, float y, float z,float p1x, float p1y, float p2x, float p2y, float pz){

		float dsquared = (p2x-p1x)*(p2x-p1x)+(p2y-p1y)*(p2y-p1y);
		float theta = Mathf.Atan2((p2x-p1x),(p2y-p1y));


		float alpha = 0.0f;

		transform.localScale=new Vector3(x,y,z);

		if (dsquared < y*y){
			alpha=90.0f;
			shortSide=true;
		}
		else{
			
		}

		transform.localEulerAngles=new Vector3(0f,theta*Mathf.Rad2Deg+alpha,0f);
		transform.position = new Vector3(p1x,pz,p1y);



	}


	public void rotateOverMarkers(){
		//transform.position=transform.position+transform.localScale.z*transform.forward;
		//transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
		//transform.position=transform.position+transform.localScale.x*transform.right;
		if(shortflip){
			transform.localScale=new Vector3(transform.localScale.x,transform.localScale.y,-transform.localScale.z);
		}
		else{
			transform.localScale=new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}

	
	}
	public void rotateOverMarkersA (){
		float theta1 = 0.0f;
		theta1=transform.localEulerAngles.y* Mathf.Deg2Rad;

		if (shortSide){
			transform.position=new Vector3(transform.position.x+transform.localScale.z*Mathf.Sin(theta1),transform.position.y,transform.position.z+transform.localScale.z*Mathf.Cos(theta1));
		}
		else {
			transform.position=new Vector3(transform.position.x+transform.localScale.z*Mathf.Sin(theta1),transform.position.y,transform.position.z+transform.localScale.z*Mathf.Cos(theta1));
		}
		

	
	}
 }
