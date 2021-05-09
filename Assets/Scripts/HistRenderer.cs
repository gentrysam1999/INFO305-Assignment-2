using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistRenderer : MonoBehaviour
{
    public GameObject obj;
    public GameObject blockPref;

    public int size = 10;
    public int axis = 0;
    private float[] values = new float[6];
    // public List<float> histValues;
    // public List<float> prevValues;

    public float[] histValues;
    public float[] prevValues;
    private float width= 10.0f;
    private float height = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        prevValues = new float[size];
        //prevValues = new List<float>();
        // for(int i=0; i<size-1; i++){
        //     prevValues.Add(0.0f);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
        values[0] = obj.GetComponent<MoveThreshCheck>().xPos;
        values[1] = obj.GetComponent<MoveThreshCheck>().yPos;
        values[2] = obj.GetComponent<MoveThreshCheck>().zPos;
        values[3] = obj.GetComponent<MoveThreshCheck>().xRot;
        values[4] = obj.GetComponent<MoveThreshCheck>().yRot;
        values[5] = obj.GetComponent<MoveThreshCheck>().zRot;
        //histValues = new List<float>();
        histValues = new float[size];
        histValues[histValues.Length-1] = (values[axis]);
        
        for (int i = 1; i < size; i++)
        {   
            //Debug.Log(prevValues.Length-i);
            Debug.Log(prevValues[prevValues.Length-i-1]);
            if(prevValues.Length-i>0){
                histValues[histValues.Length-1-i] = prevValues[prevValues.Length-i];
            }else{
                histValues[histValues.Length-1-i] = 0;
            }

            
            
            
            //histValues.Add(prevValues[prevValues.Length-1-i]);
        }
        prevValues = histValues;
        int count = 0;
        foreach (float val in histValues){
            float height = prevValues[prevValues.Length-1-count]*100;
            if(this.gameObject.transform.Find("block"+count)==null){
                GameObject block =  Instantiate(blockPref, new Vector3(300+count * 2.0F, height / 2.0F, 0), Quaternion.identity);
                block.name = ("block"+count);
                block.transform.parent = gameObject.transform;
                block.transform.localScale = new Vector3(1, height, 1);
            
            }else{
                GameObject block = GameObject.Find("block"+count);
                block.transform.localScale = new Vector3(1, height, 1);
                block.transform.position = new Vector3(300+count * 2.0F, height / 2.0F, 0);
            }
            count++;
        }

        //prevValues = histValues;
    }
}
