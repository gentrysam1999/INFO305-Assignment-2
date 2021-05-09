using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGridRenderer : Graphic
{
    public GameObject obj;
    public float xPos;
    public float yPos;
    public float zPos;
    public float xRot;
    public float yRot;
    public float zRot;
    public int size = 10;
    public int axis = 0;
    private float[] values = new float[6];
    public float[] histValues;
    public float[] prevValues;
    private float width;
    private float height;
    
    protected override void OnPopulateMesh(VertexHelper vh){
        vh.Clear();

        // width = rectTransform.rect.width;
        // height = rectTransform.rect.height;
        // UIVertex vertex = UIVertex.simpleVert;
        // vertex.color = color;
        // vertex.position = new Vector3(0,0);
        // vh.AddVert(vertex);
        // vertex.position = new Vector3(0, height);
        // vh.AddVert(vertex);
        // vertex.position = new Vector3(width, height);
        // vh.AddVert(vertex);
        // vertex.position = new Vector3(width, 0);
        // vh.AddVert(vertex);
        // vh.AddTriangle(0, 1, 2);
        // vh.AddTriangle(2, 3, 0);
        
        prevValues = new float[size];
    }

    void Start(){

        
    }
    void Update(){
        values[0] = obj.GetComponent<MoveThreshCheck>().xPos;
        values[1] = obj.GetComponent<MoveThreshCheck>().yPos;
        values[2] = obj.GetComponent<MoveThreshCheck>().zPos;
        values[3] = obj.GetComponent<MoveThreshCheck>().xRot;
        values[4] = obj.GetComponent<MoveThreshCheck>().yRot;
        values[5] = obj.GetComponent<MoveThreshCheck>().zRot;
        histValues = new float[size];
        histValues[histValues.Length-1] = values[axis];
        for(int i=1; i<(histValues.Length); i++){
            if(prevValues.Length-i>0){
                histValues[histValues.Length-1-i] = prevValues[prevValues.Length-i];
            }else{
                histValues[histValues.Length-1-i] = 0;
            }
            
        }
        prevValues = histValues;
        int count = 0;
        foreach (float val in histValues){

            VertexHelper vh = new VertexHelper();
            MakeBlock(count, val, vh);
            count++;
        }
        
    }

    void MakeBlock(int blockNum, float height, VertexHelper vh){
        width = rectTransform.rect.width;
        float blockWidth = width / size;
        vh.Clear();
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = new Vector3(blockWidth*blockNum, blockWidth*blockNum);
        vh.AddVert(vertex);
        vertex.position = new Vector3(blockWidth*blockNum, height*10000);
        vh.AddVert(vertex);
        vertex.position = new Vector3(blockWidth*blockNum+blockWidth, height*10000);
        vh.AddVert(vertex);
        vertex.position = new Vector3(blockWidth*blockNum+blockWidth, blockWidth*blockNum);
        vh.AddVert(vertex);
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
        Debug.Log("made block");
    }
}
