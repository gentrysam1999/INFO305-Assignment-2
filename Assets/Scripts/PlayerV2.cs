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

    private float timeLeft;

    public float timeStepDuration;


    

    // Start is called before the first frame update
    void Start()
    {
        this.ReadValues();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer);
        float[] a = dataArrays[playbackCount]; //current
        float[] b = dataArrays[playbackCount-1]; //previous
        LineRenderer lineRend = lineObj.GetComponent<LineRenderer>(); //line renderer
        
        if(timer > a[0] && playbackCount < dataArrays.Count-1){
            //put values from a and b arrays into current and previous poses
            Pose currentPose = new Pose(new Vector3(a[1], a[2],a[3]), new Quaternion(a[4], a[5], a[6], a[7]));
            Pose prevPose = new Pose(new Vector3(b[1], b[2],b[3]), new Quaternion(b[4], b[5], b[6], b[7]));
            //get displacement from previous pose to current pose
            Pose poseDisp = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, currentPose);
            //calculate toatal displacement = squareroot(x^2 + y^2 + z^2)
            float totalDisp = Mathf.Sqrt((poseDisp.position.x*poseDisp.position.x) + (poseDisp.position.y*poseDisp.position.y) + (poseDisp.position.z*poseDisp.position.z));
            //Debug.Log(totalDisp);

            timeLeft = timer % timeStepDuration;
            Debug.Log(timeLeft);

            //only run when displacement isn't bigger than 2
            if (!(totalDisp > 2)){
                //set the local position of the gameObject (relative to parent) to the poseDisp(relative pose)
                this.gameObject.transform.localPosition = poseDisp.position;
                this.gameObject.transform.localRotation = poseDisp.rotation;
                //create a tempPose of the gameObject's world position
                Pose tempPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
                //set the parent's position to the tempPose
                parent.transform.position = tempPose.position;
                parent.transform.rotation = tempPose.rotation;
                //set the gameobject back to the center of the parent
                this.gameObject.transform.localPosition = new Vector3(0,0,0);
                this.gameObject.transform.localRotation = new Quaternion(0,0,0,0);
                
                //lineRend.positionCount = playbackCount;
                //lineRend.SetPosition(playbackCount-1, this.gameObject.transform.position);
                

                if(timer > timeInstantiate){
                    //Instantiate an object at the gameobject's world position
                    var obj = Instantiate(marker, this.gameObject.transform.position, this.gameObject.transform.rotation);
                    //increase lineRend array by 1 and add tempPose's position to it
                    lineRend.positionCount = lineRend.positionCount+1;
                    lineRend.SetPosition(lineRend.positionCount-1, tempPose.position);
                    //update timeInstantiate
                    timeInstantiate += instatiateTimeToAdd;
                }
            }
            //increase playBackCount by 1 until there aren't any more values to add from the array
            if(playbackCount < dataArrays.Count-1){
                playbackCount += 1;
            }
            //update time
            timer+=Time.deltaTime;
        }else{
            //update time
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
