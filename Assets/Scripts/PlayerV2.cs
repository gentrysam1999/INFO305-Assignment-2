using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class PlayerV2 : MonoBehaviour
{
    //private string fileName = "HeadsetPose1(637547047324757359).csv";
    //private string fileName = "HeadsetPose2(637547047995676525).csv";
    //private string fileName = "HeadsetPose3(637547048653109104).csv";
    //private string fileName = "HeadsetPose4(637547049295046052).csv";
    //private string fileName = "HeadsetPose5(637547049989923940).csv";
    private string fileName = "HeadsetPose1(637551324366594689).csv";

    public List<float[]> dataArrays = new List<float[]>();
    public GameObject lineObj;
    public GameObject parent;
    public GameObject marker;
    private float timer = 0.0f;
    private float timeInstantiate = 0.0f;
    public float instatiateTimeToAdd = 5.0f;
    private int playbackCount = 1;
    private int prevCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.ReadValues();
        
    }

    // Update is called once per frame
    void Update()
    {
        // game object will move along the path using the time intervals    
        // this is just initial stuff, need to add the checker for the difference 
        // between values to help ignore if the headset gets reset to 0
        //Debug.Log(timer);
        float[] a = dataArrays[playbackCount]; //current
        float[] b = dataArrays[playbackCount-1]; //previous
        LineRenderer lineRend = lineObj.GetComponent<LineRenderer>();
        
        if(timer > a[0] && playbackCount < dataArrays.Count-1){
            Pose currentPose = new Pose(new Vector3(a[1], a[2],a[3]), new Quaternion(a[4], a[5], a[6], a[7]));
            Pose prevPose = new Pose(new Vector3(b[1], b[2],b[3]), new Quaternion(b[4], b[5], b[6], b[7]));
            Pose poseDisp = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, currentPose);
            float totalDisp = Mathf.Sqrt((poseDisp.position.x*poseDisp.position.x) + (poseDisp.position.y*poseDisp.position.y) + (poseDisp.position.z*poseDisp.position.z));
            Debug.Log(totalDisp);
            if (!(totalDisp > 2)){
                this.gameObject.transform.localPosition = poseDisp.position;
                this.gameObject.transform.localRotation = poseDisp.rotation;
                Pose tempPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
                parent.transform.position = tempPose.position;
                parent.transform.rotation = tempPose.rotation;
                this.gameObject.transform.localPosition = new Vector3(0,0,0);
                this.gameObject.transform.localRotation = new Quaternion(0,0,0,0);
                lineRend.positionCount = playbackCount;
                lineRend.SetPosition(playbackCount-1, tempPose.position);
                if(timer > timeInstantiate){

                    var obj = Instantiate(marker, this.gameObject.transform.position, this.gameObject.transform.rotation);
                    
                    timeInstantiate += instatiateTimeToAdd;
                    prevCount = playbackCount;
                }
            }
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
