using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class InstanceLoader : MonoBehaviour {

	public Text m_Input;
	public Dropdown m_Dropdown;
	public InstanceCreator creador;
	public string corridaActual;
	public GameObject KinematicBox;

	GameObject [] loadedBoxes;

	public string currentInstance;

	string [] csvs;

	int fileN;

	string [] boxOrder;

	string[] descriptors;

	public int targetFrame;

	List<string> m_DropOptions;

	// Use this for initialization
	void Start () {
		DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath +"/Comas");
		FileInfo[] info = dir.GetFiles("*.csv");
		descriptors = File.ReadAllLines(Application.streamingAssetsPath +"/Exogenous/descriptors.csv");

		m_DropOptions = new List<string>();

		csvs=new string[info.Length];

		int fileIndex=0;
		
		foreach (FileInfo f in info) 
		{
			csvs[fileIndex]=f.Name.Replace("corrida","").Replace(".csv","");
			m_DropOptions.Add(csvs[fileIndex]);
			fileIndex++;
		}
		m_Dropdown.ClearOptions();
		m_Dropdown.AddOptions(m_DropOptions);

		// fileN=0;

		//Debug.Log(csvs[fileN]);

		// currentInstance=csvs[fileN];
		// boxOrder = File.ReadAllLines("Assets/Exogenous/names"+currentInstance+".csv");
		// readInsanceFiles(currentInstance,targetFrame);
		// loadRefBoxes(currentInstance);
	}
	
	// Update is called once per frame
	void Update () {

		 if (Input.GetKeyDown("l"))
        {
            readInsanceFiles(currentInstance,targetFrame);
        }
		 if (Input.GetKeyDown("t"))
        {
            SavePositions();
        }
		
	}

	public void SavePositions(){

		string path = Application.streamingAssetsPath +"/Anotaciones/"+currentInstance+"_"+targetFrame+".csv";
		// string path2 =  Application.streamingAssetsPath +"/OldFormat/"+currentInstance+".csv";
		StreamWriter writer = new StreamWriter(path, false);
		// StreamWriter writer2 = new StreamWriter(path2, false);

		int infoIndex = Int32.Parse(currentInstance.Split('-')[0]);

		string instanceDescription = descriptors[infoIndex-1];

		string headerTex;

		GameObject[] cajitas = GameObject.FindGameObjectsWithTag("mocap_box");

		headerTex=cajitas.Length+",finalTime"+currentInstance+","+instanceDescription;

		writer.WriteLine(headerTex);

		//writer.WriteLine("boxID,logTime,r11,r12,r13,px,r21,r22,r23,py,r31,r32,r33,pz");
		//writer2.WriteLine("boxN,0,m11,m12,m13,posX,m21,m22,m23,posZ,m31,m32,m33,posY");

		string posi;

		foreach (string boxPos in boxOrder){
			posi = boxPos.Split('-')[1];
			//Debug.Log("cajita.name");
			//Debug.Log(boxPos);
			//Debug.Log(posi);
			//Debug.Log("cajita.name");
			foreach(GameObject cajita in cajitas){
				//Debug.Log(cajita.name);
				if (cajita.name.Replace("Caja ","")==posi){
					//Debug.Log("compa");
					cajita.transform.parent=null;
					writer.WriteLine(writeBoxData(cajita));
					// writer2.WriteLine(writeBoxDataBackup(cajita));
				}

			}


		}

		writer.Close();
		// writer2.Close();
		Debug.Log("Saved");
		//fileN++;
		//currentInstance=csvs[fileN];
		//readInsanceFiles(currentInstance,targetFrame);
		//loadRefBoxes(currentInstance);
	}

	public void loadInfo(){
		currentInstance=m_Dropdown.options[m_Dropdown.value].text;
		targetFrame=int.Parse(m_Input.text);
		Debug.Log("Target Frame is: "+ targetFrame);
		readInsanceFiles(currentInstance,targetFrame);
		loadRefBoxes(currentInstance);

	}

	void clipTo90(ref GameObject boxObject){

		boxObject.transform.Rotate(0, 90, 0, Space.Self);
		float oldY=boxObject.transform.localEulerAngles[1];
		
		if (oldY>180){
			Debug.Log(boxObject.name+" BIG ANGLE "+oldY);
			boxObject.transform.Rotate(0, -180, 0, Space.Self);
			Debug.Log(boxObject.transform.localEulerAngles[1]);

		} 
		oldY=boxObject.transform.localEulerAngles[1];
		if (oldY>90){
			boxObject.transform.Rotate(0, -180, 0, Space.Self);
			Debug.Log(boxObject.name+" ANGLE "+oldY);
			Debug.Log(boxObject.transform.localEulerAngles[1]);
		}
		// if (oldY<-180){
		// 	boxObject.transform.Rotate(0, 180, 0, Space.Self);
		// 	Debug.Log(boxObject.name+" WHAT");
		// }
		// else if (oldY<-90){
		// 	Debug.Log(boxObject.name+" WHAT");
		// 	boxObject.transform.Rotate(0, 90, 0, Space.Self);
		// }

	}

	string writeBoxData(GameObject boxObject){
		clipTo90(ref boxObject);
		string boxData = "";
		string boxN = boxObject.name.Replace("Caja ","");
		float posX =  boxObject.transform.position.x*1000;
		float posY =  boxObject.transform.position.y*1000;
		float posZ =  boxObject.transform.position.z*1000;
		float scaleX =  boxObject.transform.localScale.x;
		float scaleY =  boxObject.transform.localScale.y;
		float scaleZ =  boxObject.transform.localScale.z;
		//Quaternion theRot = boxObject.transform.rotation;
		Quaternion theRot = Quaternion.Euler(boxObject.transform.eulerAngles.x, boxObject.transform.eulerAngles.y, boxObject.transform.eulerAngles.z);
		Matrix4x4 m = Matrix4x4.TRS(transform.position, theRot, transform.localScale);
		Matrix4x4 flip = Matrix4x4.zero;
		flip[0,0]=1;
		flip[2,1]=-1;
		flip[2,2]=1;
		//m=m*flip;
		float m11 = m[0,0];
		float m12 = m[1,0];
		float m13 = m[2,0];
		float m21 = m[0,1];
		float m22 = m[1,1];
		float m23 = m[2,1];
		float m31 = m[0,2];
		float m32 = m[1,2];
		float m33 = m[2,2];
		float posUp = posY+0.5f*scaleY*1000;
		//boxData=boxN+",0,"+m11+","+m12+","+m13+","+posX+","+m21+","+m22+","+m23+","+posZ+","+m31+","+m32+","+m33+","+posUp;
		boxData=boxN+",0,"+m11+","+m31+","+m12+","+posX+","+m13+","+m33+","+m32+","+posZ+","+m21+","+m23+","+m22+","+posUp;


		return boxData;
	}


	// string writeBoxData(GameObject boxObject){
	// 	//boxObject.transform.Rotate(0, 180, 0, Space.Self);
	// 	clipTo90(ref boxObject);
	// 	string boxData = "";
	// 	string boxN = boxObject.name.Replace("Caja ","");
	// 	float posX =  boxObject.transform.position.x*1000;
	// 	float posY =  boxObject.transform.position.y*1000;
	// 	float posZ =  boxObject.transform.position.z*1000;
	// 	float scaleX =  boxObject.transform.localScale.x;
	// 	float scaleY =  boxObject.transform.localScale.y;
	// 	float scaleZ =  boxObject.transform.localScale.z;
	// 	//Quaternion theRot = boxObject.transform.rotation;
	// 	Quaternion theRot = Quaternion.Euler(boxObject.transform.eulerAngles.x, boxObject.transform.eulerAngles.y, boxObject.transform.eulerAngles.z);
	// 	Matrix4x4 m = Matrix4x4.TRS(transform.position, theRot, transform.localScale);
	// 	Matrix4x4 flip = Matrix4x4.zero;
	// 	flip[0,0]=1;
	// 	flip[2,1]=1;
	// 	flip[1,2]=1;
	// 	flip[3,3]=1;

	// 	Debug.Log("matrices");

	// 	Debug.Log("original");

	// 	Debug.Log(m);

	// 	Debug.Log("Rotación");

	// 	Debug.Log(flip);

	// 	//m=m*flip;

	// 	Debug.Log("Rotada");

	// 	Debug.Log(m);

		
		
	// 	flip = Matrix4x4.zero;
	// 	flip[1,0]=1;
	// 	flip[0,1]=-1;
	// 	flip[2,2]=1;
	// 	flip[3,3]=1;
	// 	Debug.Log("flip 2");
	// 	Debug.Log(flip);

	// 	//m=m*flip;

	// 	Debug.Log("fin matrices");

	// 	float m11 = m[0,0];
	// 	float m12 = m[1,0];
	// 	float m13 = m[2,0];
	// 	float m21 = m[0,1];
	// 	float m22 = m[1,1];
	// 	float m23 = m[2,1];
	// 	float m31 = m[0,2];
	// 	float m32 = m[1,2];
	// 	float m33 = m[2,2];
	// 	float posUp = posY+0.5f*scaleY*1000;
	// 	//boxData=boxN+",0,"+m13+","+m33+","+m32+","+posZ+","+m11+","+m31+","+m12+","+posX+","+m21+","+m23+","+m22+","+posUp;
		
	// 	//boxData=boxN+","+targetFrame+","+m13+","+m33+","+m32+","+posX+","+m11+","+m31+","+m12+","+posZ+","+m21+","+m23+","+m22+","+posUp;
	// 	////boxData=boxN+",0,"+m11+","+m12+","+m13+","+posX+","+m21+","+m22+","+m23+","+posZ+","+m31+","+m32+","+m33+","+posUp;
	// 	//boxData=boxN+",0,"+m11+","+m31+","+m12+","+posX+","+m13+","+m33+","+m32+","+posZ+","+m21+","+m23+","+m22+","+posUp;


	// 	//boxData=boxN+","+targetFrame+","+m11+","+m12+","+m13+","+posX+","+m21+","+m22+","+m23+","+posZ+","+m31+","+m32+","+m33+","+posUp;
	// 	boxData=boxN+","+targetFrame+","+m11+","+m12+","+m13+","+posX+","+m21+","+m22+","+m23+","+posUp+","+m31+","+m32+","+m33+","+posZ;

	// 	return boxData;
	// }

	string writeBoxDataBackup(GameObject boxObject){
		string boxData = "";
		string boxN = boxObject.name;
		float posX =  boxObject.transform.position.x;
		float posY =  boxObject.transform.position.y;
		float posZ =  boxObject.transform.position.z;
		float scaleX =  boxObject.transform.localScale.x;
		float scaleY =  boxObject.transform.localScale.y;
		float scaleZ =  boxObject.transform.localScale.z;
		//Quaternion theRot = boxObject.transform.rotation;
		Quaternion theRot = Quaternion.Euler(boxObject.transform.eulerAngles.x, boxObject.transform.eulerAngles.y, boxObject.transform.eulerAngles.z);
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, theRot, Vector3.one);
		// float m11 = m[0,0];
		// float m12 = m[0,1];
		// float m13 = m[0,2];
		// float m21 = m[1,0];
		// float m22 = m[1,1];
		// float m23 = m[1,2];
		// float m31 = m[2,0];
		// float m32 = m[2,1];
		// float m33 = m[2,2];

		float m11 = m[0,0];
		float m12 = m[1,0];
		float m13 = m[2,0];
		float m21 = m[0,1];
		float m22 = m[1,1];
		float m23 = m[2,1];
		float m31 = m[0,2];
		float m32 = m[1,2];
		float m33 = m[2,2];
		float posUp = posZ+0.5f*scaleZ*1000;
		//boxData=boxN+","+posX+","+posY+","+posZ+","+scaleX+","+scaleY+","+scaleZ+","+m11+","+m12+","+m13+","+m21+","+m22+","+m23+","+m31+","+m32+","+m33;
		boxData=boxN+",0,"+m11+","+m12+","+m13+","+posX+","+m21+","+m22+","+m23+","+posZ+","+m31+","+m32+","+m33+","+posUp;
		


		return boxData;
	}
	void loadRefBoxes(string corrida){
		string guardada="corrida"+corrida.Split('-')[0];

		//creador.numeroDeCorrida=guardada;

		//creador.ReadString();


		//Debug.Log(guardada);
	}

	void readInsanceFiles(string corrida, int frameNumber){


		GameObject[] enemies = GameObject.FindGameObjectsWithTag("container");

   		foreach(GameObject enemy in enemies){
  			 GameObject.Destroy(enemy);
		}

		enemies = GameObject.FindGameObjectsWithTag("mocap_box");

   		foreach(GameObject enemy in enemies){
  			 GameObject.Destroy(enemy);
		}

		string [] dimensionLines = File.ReadAllLines(Application.streamingAssetsPath +"/Exogenous/dims.csv");



		string[] stringSeparators = new string[] { "," };
		string [] corridaLines = File.ReadAllLines(Application.streamingAssetsPath +"/Comas/corrida"+corrida+".csv");
		string [] markerLines = File.ReadAllLines(Application.streamingAssetsPath +"/Exogenous/names"+corrida+".csv");
		string [] markerNames=corridaLines[5].Split( stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		string [] markerValues=corridaLines[8+frameNumber].Split(',');

		Debug.Log(markerLines.Length);
		boxOrder = File.ReadAllLines(Application.streamingAssetsPath +"/Exogenous/orders"+corrida+".csv");

		loadedBoxes = new GameObject[markerLines.Length];

		int n=0;

		foreach (string markerName in markerLines)
		{
			Debug.Log(markerName);
			string [] markerData= markerName.Split(',');
			int marker1	= System.Convert.ToInt16(markerData[0]);
			int marker2	= System.Convert.ToInt16(markerData[1]);
			int boxID = System.Convert.ToInt16(markerData[2]);

			int pos1=-1;
			int pos2=-1;
			for (int i = 1; i < markerNames.Length; i++)
			{
				
				if(System.Convert.ToInt16(markerNames[i].Replace("M",""))==marker1){
					pos1=i-1;
				}
				else if(System.Convert.ToInt16(markerNames[i].Replace("M",""))==marker2){
					pos2=i-1;
				}
				if (pos1>=0 && pos2>=0 ){
					break;
				}
			}
			//Debug.Log("Here "+markerName+" and " + pos1 +" and " + markerValues.Length);
			//Debug.Log(pos1);
			float x1 = System.Convert.ToSingle(markerValues[pos1*3+1]);
			float y1 = System.Convert.ToSingle(markerValues[pos1*3+2]);
			float z1 = System.Convert.ToSingle(markerValues[pos1*3+3]);

			float x2 = System.Convert.ToSingle(markerValues[pos2*3+1]);
			float y2 = System.Convert.ToSingle(markerValues[pos2*3+2]);
			float z2 = System.Convert.ToSingle(markerValues[pos2*3+3]);

			string[] dimensions = dimensionLines[boxID-1].Split(','); 

			float h = System.Convert.ToSingle(dimensions[0])/100f;
			float w = System.Convert.ToSingle(dimensions[1])/100f;
			float d = System.Convert.ToSingle(dimensions[2])/100f;

			Vector3 instancePos= new Vector3(x1,z1,y1);

			loadedBoxes[n] = Instantiate(KinematicBox, instancePos/1000f, Quaternion.identity);

			string str="Marker 1: M"+ marker1+ " x1: "+ x1 + " y1: "+ y1 + " z1: "+ z1;
			//Debug.Log(str);

			loadedBoxes[n].GetComponent<SetUpBox>().BoxName="Caja "+boxID;
			loadedBoxes[n].GetComponent<SetUpBox>().setStartPos(w,h,d,x1/1000,y1/1000,x2/1000,y2/1000,z1/1000);
			


			n++;
		
		}

		


	}


}
