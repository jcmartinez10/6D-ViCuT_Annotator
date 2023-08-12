using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxProperties : MonoBehaviour {

	public bool grounded;
	public int bearing;
	public string myFace;

	public string myInstance;

	public float floorArea;
	
	public int supported;

	float m_MaxDistance;
    bool m_HitDetect;

    Collider m_Collider;
    RaycastHit m_Hit;

	// Use this for initialization
	void Start () {
		 //Choose the distance the Box can reach to
        m_MaxDistance = 1.0f;
        m_Collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

Vector3 MyPos (){
	return this.gameObject.transform.position;

}

public void GetUpRotation(){
	if ((transform.up-Vector3.up).sqrMagnitude<0.01){
		myFace="xz";
		floorArea=transform.localScale.x*transform.localScale.z;
	}
	else if((transform.up-Vector3.forward).sqrMagnitude<0.01){
		myFace="xy";
		floorArea=transform.localScale.x*transform.localScale.y;
	}
	else{
		myFace="yz";
		floorArea=transform.localScale.y*transform.localScale.z;
	}
}

public float MyHeight(){
	float h;
	if(myFace=="xz"){
		h=transform.localScale.y;
	}
	else if (myFace=="yz"){
		h=transform.localScale.x;
	}
	else{
		h=transform.localScale.z;
	}
	m_MaxDistance=h/2+0.02f;
	return h;
}

public float pileArea(){
	float biggestArea=floorArea;


	return biggestArea;

}
public GameObject Pileup(){
	GetUpRotation();
	GameObject other;
	Vector3 newPos;
	 m_HitDetect = Physics.BoxCast(transform.position, 1.01f*transform.localScale, -Vector3.up, out m_Hit, transform.rotation, m_MaxDistance);
        if (m_HitDetect)
        {
			if( m_Hit.collider.tag=="floor"){
				grounded=true;
				return this.gameObject;
			}
			else if (m_Hit.collider.tag=="box"){
				other=m_Hit.collider.gameObject.GetComponent<BoxProperties>().Pileup();
				newPos=new Vector3(other.transform.position.x,transform.position.y,other.transform.position.z);
				if (other.GetComponent<BoxProperties>().supported>=1){
					transform.position=other.transform.position+0.7f*Vector3.forward;
				}
				else if(other.GetComponent<BoxProperties>().floorArea<floorArea){
					transform.position=newPos-Vector3.up*other.GetComponent<BoxProperties>().MyHeight();
					other.transform.position=other.transform.position+Vector3.up*MyHeight();
					supported++;
					return other;
				}
				else{
					transform.position=newPos;
					other.GetComponent<BoxProperties>().supported++;
					if(other.GetComponent<BoxProperties>().grounded){
						grounded=true;
					}
					return this.gameObject;
				}
				}
            //Output the name of the Collider your Box hit
            //Debug.Log("Hit : " + m_Hit.collider.name);
        }
		return this.gameObject;
	}

}
