using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
//using UnityEditor;
//using UnityEngine.SceneManagement;


public class InstanceCreator : MonoBehaviour {

	private static int resWidth = 3840;
	private static int resHeight = 2160;
	private static GameObject camObj = null;

    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;
    public GameObject box5;
    public GameObject box6;
    public GameObject box7;
    public GameObject box8;
    public GameObject box9;

    public int totalBoxes;
    public int maxPerBox;

    public string numeroDeCorrida;

    private GameObject[] boxPrefabs;
    public GameObject[] allBoxes;

    public Boolean[] allowed;

    public Boolean[] canRotate;

    public float scaleFactor;

	// Use this for initialization
	void Start () {
		boxPrefabs=new GameObject[9]{box1,box2,box3,box4,box5,box6,box7,box8,box9};
        allowed = new Boolean[9]{false, false, false, false, false, false, false, false, false};
        canRotate = new Boolean[9]{false, false, false, false, false, false, false, false, false};
	}
	
	// Update is called once per frame
	void Update () {

          if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            //GetPixelSum ();
            
            SetUpInstances (totalBoxes,maxPerBox);
            //SetUpColumns(5,1);
            //StartCoroutine(PileAll());
        }
        if (Input.GetKeyDown("p"))
        {
            WriteString();
            
            Debug.Log("Written");
        }
		if (Input.GetKeyDown("r"))
        {
            ReadString();
            
            Debug.Log("Read");
        }
	}

    Vector3 randomPos(float boundX, float boundY){
        float a;
        float b;
        float c;
        Vector3 thePos;

        a=UnityEngine.Random.Range(-scaleFactor*boundX+0.1f,scaleFactor*boundX-0.1f);
        b=UnityEngine.Random.Range(0.3f,2.1f);
        c=UnityEngine.Random.Range(-scaleFactor*boundY+0.1f,scaleFactor*boundY-0.1f);

        thePos=new Vector3(a,b,c);
        return thePos;
    }

    Vector3 randomRot(){
        float r;
        Vector3 theRot=new Vector3(0f,0f,0f);
        r=UnityEngine.Random.Range(0,3);
        
        if(r==1){
            theRot=new Vector3(0f,0f,90f);
        }
        else if(r==2){
            theRot=new Vector3(-90f,0f,0f);
        }
        return theRot;
    }

    IEnumerator PileAll(){
        SetUpInstances(20,4);
        foreach (GameObject ob in allBoxes) {
           ob.GetComponent<BoxProperties>().Pileup();
        }
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject ob in allBoxes) {
           ob.GetComponent<BoxProperties>().Pileup();
        }
    }

    void SetUpInstances (int totalInstances, int maxType){
        int[] boxes = new int[9]{ 0, 0, 0, 0, 0, 0, 0, 0, 0};

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("box");
        foreach(GameObject enemy in enemies){
                GameObject.Destroy(enemy);
        }
        
        allBoxes = new GameObject[totalInstances];

        int cycles=(int)totalInstances/maxType;

        Debug.Log("Subgroups: ");
        Debug.Log(cycles);

        for (int i = 0; i < cycles; i++){
            boxes=AssignQnty(boxes, maxType);
        }

        int typeIndex=0;
        float o;

        float xSize=1.5f;
        float ySize=1.25f;
        float occVolume=0.0f;
        for (int i = 0; i < 9; i++){
                for (int j = 0; j < boxes[i]; j++){
                allBoxes[typeIndex]=Instantiate(boxPrefabs[i], randomPos(xSize,ySize), Quaternion.identity);
                if (canRotate[i]){
                    allBoxes[typeIndex].transform.Rotate (randomRot());
                }
                allBoxes[typeIndex].GetComponent<BoxProperties>().GetUpRotation();
                o=UnityEngine.Random.Range(0.0f,180.0f);
                allBoxes[typeIndex].transform.RotateAround(allBoxes[typeIndex].transform.position, Vector3.up, o);
                occVolume+= allBoxes[typeIndex].transform.localScale.x*allBoxes[typeIndex].transform.localScale.y*allBoxes[typeIndex].transform.localScale.z;
                typeIndex++;
            }
        }
        float percUs=100*occVolume/(xSize*ySize*2*4);
        string str="Porcentaje utilizado: "+ percUs+ "%";
         Debug.Log(str);

    }

     void WriteString()
     {
         string path = "Assets/Resources/test.txt";
 
         //Write some text to the test.txt file
         StreamWriter writer = new StreamWriter(path, false);
         string linea="";
         string p="|";
         foreach (GameObject ob in allBoxes) {
             linea=ob.name.Replace("(Clone)","").Replace("Box","")+p+ob.transform.position.x+p+ob.transform.position.y+p+ob.transform.position.z
             +p+ob.transform.eulerAngles.x+p+ob.transform.eulerAngles.y+p+ob.transform.eulerAngles.z;
             writer.WriteLine(linea);
        }
         writer.Close();
     }
 
    public void ReadString(){

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("box");
            foreach(GameObject enemy in enemies){
                 GameObject.Destroy(enemy);
            }

            GameObject boxy;
            Vector3 lePos;
    
            string path = "Assets/Resources/"+numeroDeCorrida+".txt";

    
            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path); 

            

            string line;
            string[] pars;

            while((line = reader.ReadLine()) != null)  
            {  
               pars=line.Split('|');
               lePos=new Vector3(float.Parse(pars[1]),float.Parse(pars[2]),float.Parse(pars[3]));
               boxy=Instantiate(boxPrefabs[int.Parse(pars[0])-1], lePos, Quaternion.identity);
               boxy.transform.localEulerAngles=new Vector3(float.Parse(pars[4]),float.Parse(pars[5]),float.Parse(pars[6]));
            }  
            reader.Close();
        }

    void SetUpColumns (int totalInstances, int maxType){
        int[] boxes = new int[9]{ 0, 0, 0, 0, 0, 0, 0, 0, 0}; 

        foreach (GameObject ob in allBoxes) {
           Destroy(ob);
        }

        allBoxes = new GameObject[totalInstances];

        int cycles=(int)totalInstances/maxType;

        Debug.Log("Subgroups: ");
        Debug.Log(cycles);

        for (int i = 0; i < cycles; i++){
            boxes=AssignQnty(boxes, maxType);
        }

        int typeIndex=0;
        float o;

        float maxProb = 0.2f;
        float columnProb = 0.0f;
        int colIndex=0;
        int maxSupports=1;
        Vector3 basePos;
        GameObject sup;

        float xSize=0.5f;
        float ySize=0.5f;
        for (int i = 5; i-- > 0;){
                for (int j = 0; j < boxes[i]; j++){
                columnProb =UnityEngine.Random.Range(0.0f,1.0f);
                    if(typeIndex>1 && columnProb<maxProb){
                        //while supportrf
                        colIndex=UnityEngine.Random.Range(0,typeIndex-1);
                        allBoxes[typeIndex]=Instantiate(boxPrefabs[i], randomPos(xSize,ySize), Quaternion.identity);
                        sup=allBoxes[colIndex];
                        if (sup.GetComponent<BoxProperties>().floorArea>allBoxes[typeIndex].GetComponent<BoxProperties>().floorArea && sup.GetComponent<BoxProperties>().supported<maxSupports){
                            allBoxes[typeIndex].GetComponent<BoxProperties>().GetUpRotation();
                            float hDiff = allBoxes[colIndex].GetComponent<BoxProperties>().MyHeight()*0.5f+0.5f*allBoxes[typeIndex].GetComponent<BoxProperties>().MyHeight();
                            basePos=allBoxes[colIndex].transform.position;
                            allBoxes[typeIndex].transform.position=basePos+Vector3.up*hDiff;
                            sup.GetComponent<BoxProperties>().supported++;
                        }
                        //Debug.Log("Rendering");
                    }
                    else{
                        allBoxes[typeIndex]=Instantiate(boxPrefabs[i], randomPos(xSize,ySize), Quaternion.identity);
                        allBoxes[typeIndex].transform.Rotate (randomRot());
                        allBoxes[typeIndex].GetComponent<BoxProperties>().GetUpRotation();
                        o=UnityEngine.Random.Range(0.0f,180.0f);
                        allBoxes[typeIndex].transform.RotateAround(allBoxes[typeIndex].transform.position, Vector3.up, o);
                    }
                typeIndex++;
            }
        }


    }

    int[] AssignQnty(int[] assignments, int maxPerType){
         int[] boxA=assignments;
         int numma;
         numma=UnityEngine.Random.Range(0,9);
         if (allowed[numma]){
            if (boxA[numma] >= maxPerType){
                boxA=AssignQnty(boxA, maxPerType);
            }
            else{
                boxA[numma]=boxA[numma]+maxPerType;
            }
         }
         else{
            boxA=AssignQnty(boxA, maxPerType);
         }
         
         

         return boxA;
    }

      float SumPixels (){
         float pixelSum = 0.0f;

         Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
 
         // ofc you probably don't have a class that is called CameraController :P
         Camera activeCamera = Camera.main;
            
            // Initialize and render
         RenderTexture rt = new RenderTexture(256, 256, 24,RenderTextureFormat.ARGB32);

         Debug.Log("Rendering");
         activeCamera.targetTexture = rt;
         activeCamera.Render();
         RenderTexture.active = rt;

         Rect rectReadPicture = new Rect(0, 0, 256, 256);
            
         // Read pixels
         tex.ReadPixels(rectReadPicture, 0, 0);

         Debug.Log("Texture created");

         Color pixelColour;
         

         for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++) //Goes through each pixel
            {
                pixelColour=tex.GetPixel(x, y);
                pixelSum+=pixelColour.grayscale;
            }
        }

        
         Debug.Log(pixelSum);
            
         // Clean up
         activeCamera.targetTexture = null;
         RenderTexture.active = null; // added to avoid errors 
         DestroyImmediate(rt);
         return pixelSum;
    }

    void GetPixelSum (){
         Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
 
         // ofc you probably don't have a class that is called CameraController :P
         Camera activeCamera = Camera.main;
            
            // Initialize and render
         RenderTexture rt = new RenderTexture(256, 256, 24,RenderTextureFormat.ARGB32);

         Debug.Log("Rendering");
         activeCamera.targetTexture = rt;
         activeCamera.Render();
         RenderTexture.active = rt;

         Rect rectReadPicture = new Rect(0, 0, 256, 256);
            
         // Read pixels
         tex.ReadPixels(rectReadPicture, 0, 0);

         Debug.Log("Texture created");

         Color pixelColour;
         float pixelSum =0 ;

         for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++) //Goes through each pixel
            {
                pixelColour=tex.GetPixel(x, y);
                pixelSum+=pixelColour.grayscale;
            }
        }

        
         Debug.Log(pixelSum);
            
         // Clean up
         activeCamera.targetTexture = null;
         RenderTexture.active = null; // added to avoid errors 
         DestroyImmediate(rt);
    }

	void TakeScreenshot (){
		try
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camObj.GetComponent<Camera>().targetTexture = rt;

        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camObj.GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camObj.GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "wow wow wow.png";
        File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        bytes = null;
    }
    catch(Exception e)
    {
        Debug.Log("Error");
    }

	}
}
