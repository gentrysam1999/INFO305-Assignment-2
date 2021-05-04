using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThreshCheck : MonoBehaviour
{
    //private float standStill = 0.005f;
    //private float walking = 0.03f;
    //private float jogging = 0.3f;
    
    public float xPos;
    public float yPos;
    public float zPos;
    private Vector3 angle;
    public float xRot;
    public float yRot;
    public float zRot;
    
    private string allThreshValues;

    private int spinCount;

    private bool spinLeft;

    private bool squatDown;
    public int squatCount = 0;

    public string findMovement(List<Pose> poses, int poseCount)
    {
        xPos = 0.0f;
        yPos = 0.0f;
        zPos = 0.0f;
        xRot = 0.0f;
        yRot = 0.0f;
        zRot = 0.0f;
        for (int j = 0; j <= poseCount - 1; j++)
        {
            xPos += poses[j].position.x;
            yPos += poses[j].position.y;
            zPos += poses[j].position.z;
            angle = poses[j].rotation.eulerAngles;
            xRot += angle.x;
            yRot += angle.y;
            zRot += angle.z;

        }

        xPos = (xPos / poseCount);
        yPos = (yPos / poseCount);
        zPos = (zPos / poseCount);
        xRot = (xRot / poseCount);
        yRot = (yRot / poseCount);
        zRot = (zRot / poseCount);

        allThreshValues += (xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "\n");
        if(this.gameObject.GetComponent<RecordData>() != null){
            this.gameObject.GetComponent<RecordData>().WriteData("ThreshValues.csv" , allThreshValues);
        }
        

        if (zPos <= 0.02 && zPos >=-0.02 && yPos <= 0.003 && yPos >=-0.003) //not moving forward, back, up or down
        {
            if(yRot > 0 && yRot < 40){ //rotation threshold for clockwise
                if(spinLeft){
                    spinCount+=1;
                    if(spinCount > 10){ //make sure that it has been within those values for 10 checks (about 1 second).
                        squatCount = 0; 
                        return "Spinning Clockwise";
                    }
                }else{
                    spinLeft = true;
                    spinCount=0;
                }
            }else if(yRot < 360 && yRot > 320){ //rotation threshold for Anti-clockwise
                if(!spinLeft){
                    spinCount+=1;
                    if(spinCount > 10){ //make sure that it has been within those values for 10 checks (about 1 second).
                        squatCount = 0; 
                        return "Spinning AntiClockWise";
                    }
                }else{
                    spinLeft = false;
                    spinCount=0;
                }
            }
            return "Standing Still";

        }
        else if (zPos >= 0.02 && zPos < 0.23) //threshold for walking
        {
            squatCount = 0;  
            if(yPos>0.02){ //if in walking threshold and moving up
                return "Going Up Stairs";
            }else if(yPos<-0.02){ //if in walking threshold and moving down
                return "Going Down Stairs";
            }else{
                return "Walking";
            }
        }
        else if (zPos >= 0.23) //any forward movement faster than walking.
        {
            squatCount = 0;
            return "Jogging";

        }else if (yPos > 0 && zPos < 0 || yPos < 0 && zPos > 0) //if y and z positional movements are opposites and none of the other criteria have been met.
        {
            if (yPos < 0){
                if (squatDown == false){
                    squatCount += 1;
                    squatDown = true;
                }  
            }else{
                squatDown = false;
            }
            return "Squats: " + squatCount;
        }
        else
        {           
            return "Unknown activity";
            
        }
    }
}

