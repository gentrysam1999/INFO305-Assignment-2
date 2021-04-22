using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TextAsset testFile;
    public string fileName = "walk test 1 copy.csv";
    //public List<string> testValues = new List<string>();

    public bool read = false;

    public List<float[]> dataArrays = new List<float[]>();

    //private float time, xPos, yPos, zPos, xRot, yRot, zRot;

    // Start is called before the first frame update
    void Start()
    {
        this.ReadValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadValues()
    {
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(Application.dataPath + "/" + fileName))
            {
                string line;
                //read = true;

                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null) //not reading each line of code
                {

                    //Debug.Log(line);
                    string[] subs = line.Split(',');
                    float[] tempArray = new float[7];
                    int count = 0;
                    foreach (var sub in subs)
                    {
                        if (sub.Length > 1)
                        {
                            float value = float.Parse(sub);
                            Debug.Log(value);
                            tempArray[count] = value;
                            count+=1;
                        }else{
                            dataArrays.Add(tempArray);
                        }

                        //Debug.Log(sub);

                    }
                    
                    
                    //testValues.Add(line);
                    //Console.WriteLine(line);
                    
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
