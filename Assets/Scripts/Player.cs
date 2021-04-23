using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string fileName = "walk test 1.csv";
    //private string fileName = "HeadsetPose1(637547047324757359).csv";
    public List<float[]> dataArrays = new List<float[]>();
    public GameObject lineObj;
    private float timer = 0.0f;
    private int playbackCount = 0;

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
        for (int i = 0; i < dataArrays.Count; i++){
            float[] x = dataArrays[i];
            lineRend.SetPosition(i, new Vector3(x[1], x[2], x[3]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // game object will move along the path using the time intervals    
        // this is just initial stuff, need to add the checker for the difference 
        // between values to help ignore if the headset gets reset to 0
        //Debug.Log(timer);
        float[] x = dataArrays[playbackCount];
        if(timer > x[0] && playbackCount < dataArrays.Count){
            this.gameObject.transform.position = new Vector3(x[1], x[2], x[3]);
            //this.gameObject.transform.rotation = new Quarternion(x[4], x[5], x[6], x[7]);
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
                    float[] tempArray = new float[7];
                    int count = 0; //count set to 0 at start of line read
                    foreach (var sub in subs)
                    {
                        if (sub.Length > 1) //checks that the substring exists
                        {
                            float value = float.Parse(sub);
                            tempArray[count] = value;
                            count += 1;
                            Debug.Log(sub);
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
