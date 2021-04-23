using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private string fileName = "HeadsetPose1(637547047324757359).csv";
    //private string fileName = "HeadsetPose2(637547047995676525).csv";
    private string fileName = "HeadsetPose3(637547048653109104).csv";
    //private string fileName = "HeadsetPose4(637547049295046052).csv";
    //private string fileName = "HeadsetPose5(637547049989923940).csv";
    public List<float[]> dataArrays = new List<float[]>();
    public GameObject lineObj;
    private float timer = 0.0f;
    private int playbackCount = 1;

    float xLine, yLine, zLine = 0.0f;
    float xPos, yPos, zPos, xRot, yRot, zRot, wRot = 0.0f;
    void Awake()
    {
        this.ReadValues();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Renders a line that shows the path taken
        LineRenderer lineRend = lineObj.GetComponent<LineRenderer>();
        lineRend.positionCount = dataArrays.Count;
        for (int i = 1; i < dataArrays.Count; i++){
            float[] a = dataArrays[i];
            float[] b = dataArrays[i-1];
            float xDisp = a[1] - b[1];
            float yDisp = a[2] - b[2];
            float zDisp = a[3] - b[3];
            float totalDisp = Mathf.Sqrt((xDisp*xDisp) + (yDisp*yDisp) + (zDisp*zDisp));
            xLine += xDisp; 
            yLine += yDisp; 
            zLine += zDisp; 
            //lineRend.SetPosition(i, new Vector3(a[1], a[2], a[3]));
            //if (!(xDisp > 10 || yDisp > 10 || zDisp > 10)){
            if (!(totalDisp > 10)){
                lineRend.SetPosition(i, new Vector3(xLine, yLine, zLine));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // game object will move along the path using the time intervals    
        // this is just initial stuff, need to add the checker for the difference 
        // between values to help ignore if the headset gets reset to 0
        //Debug.Log(timer);
        float[] a = dataArrays[playbackCount];
        float[] b = dataArrays[playbackCount-1];

        if(timer > a[0] && playbackCount < dataArrays.Count-1){
            float xDisp = a[1] - b[1];
            float yDisp = a[2] - b[2];
            float zDisp = a[3] - b[3];
            float xRotDisp = a[4] - b[4];
            float yRotDisp = a[5] - b[5];
            float zRotDisp = a[6] - b[6];
            float wRotDisp = a[7] - b[7];
            float totalDisp = Mathf.Sqrt((xDisp*xDisp) + (yDisp*yDisp) + (zDisp*zDisp));
            xPos += xDisp; 
            yPos += yDisp; 
            zPos += zDisp;
            xRot += xRotDisp;
            yRot += yRotDisp;
            zRot += zRotDisp;
            wRot += wRotDisp;
            if (!(totalDisp > 10)){
                this.gameObject.transform.position = new Vector3(xPos, yPos, zPos);
                //this.gameObject.transform.rotation = new Quaternion(xRot, yRot, zRot, wRot);
                this.gameObject.transform.rotation = new Quaternion(b[4], b[5], b[6], b[7]);
            }
            //this.gameObject.transform.position = new Vector3(b[1], b[2], b[3]);
            //this.gameObject.transform.rotation = new Quaternion(b[4], b[5], b[6], b[7]);
            if(playbackCount < dataArrays.Count-1){
                playbackCount += 1;
            }
            timer+=Time.deltaTime;
        }else{
            timer+=Time.deltaTime;
        }
    }

    public void ReadValues()
    {
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(Application.dataPath + "/Files/" + fileName))
            {
                
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    string[] subs = line.Split(',');
                    float[] tempArray = new float[8];
                    int count = 0; //count set to 0 at start of line read
                    foreach (var sub in subs)
                    {
                        if (sub.Length > 1) //checks that the substring exists
                        {
                            float value = float.Parse(sub);
                            tempArray[count] = value;
                            count += 1;
                        }
                    }
                    dataArrays.Add(tempArray);
                    float[] x = dataArrays[dataArrays.Count - 1];
                    //Debug.Log(x[0] + " " + x[1] + " " + x[2] + " " + x[3] + " " + x[4] + " " + x[5] + " " + x[6] + " " + x[7]);
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}
