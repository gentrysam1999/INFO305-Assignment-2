using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThreshCheck : MonoBehaviour
{
    public float standStill;
    public float walking;
    public float jogging;
    public float sitUps;
    
    public float xPos;
    public float yPos;
    public float zPos;
    private Vector3 angle;
    public float xRot;
    public float yRot;
    public float zRot;
    
    private string allThreshValues;

    private int moveCount;

    private bool spinLeft;

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
        this.gameObject.GetComponent<RecordData>().WriteData("ThreshValues.csv" , allThreshValues);

        if (zPos <= standStill && yPos <= standStill) //not moving forward or up
        {
            if(yRot > 0 && yRot < 40){
                if(spinLeft){
                    moveCount+=1;
                    if(moveCount > 20){
                        return "Spinning Clockwise";
                    }
                }else{
                    spinLeft = true;
                    moveCount=0;
                }
            }else if(yRot < 360 && yRot > 320){
                if(!spinLeft){
                    moveCount+=1;
                    if(moveCount > 10){
                        return "Spinning AntiClockWise";
                    }
                }else{
                    spinLeft = false;
                    moveCount=0;
                }
            }
            return "Standing Still";

        }
        else if (zPos >= walking && zPos < jogging)
        {
            return "Walking";

        }
        else if (zPos >= jogging)

        {
            return "Jogging";

        } else if (yPos > sitUps)

        {
            return "Sit ups";
        }

        else

        {
            return "Unknown activity";
        }



    }
}
