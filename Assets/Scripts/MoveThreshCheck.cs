using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveThreshCheck : MonoBehaviour
{
    public GameObject dispValTxtObj;
    public GameObject histRend;
    public float xPos;
    public float yPos;
    public float zPos;
    private Vector3 angle;
    public float xRot;
    public float yRot;
    public float zRot;
    
    private string allThreshValues;
    

    private string walkCheck, stillCheck, runCheck, squatCheck;
    private string threshTruthCheck;

    private int spinCount;

    private bool spinLeft;

    private bool squatDown;
    public int squatCount = 0;

    private string moveString;

    private int moveCount = 0;

    
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
            if(angle.x > 180){
                xRot += angle.x - 360;
            }else{
                xRot += angle.x;
            }
            if(angle.y > 180){
                yRot += angle.y - 360;
            }else{
                yRot += angle.y;
            }
            if(angle.z > 180){
                zRot += angle.z - 360;
            }else{
                zRot += angle.z;
            }
        }

        xPos = (xPos / poseCount);
        yPos = (yPos / poseCount);
        zPos = (zPos / poseCount);
        xRot = (xRot / poseCount);
        yRot = (yRot / poseCount);
        zRot = (zRot / poseCount);
        string dispVals = ("xPos: "+xPos + "\nyPos: " + yPos + "\nzPos: " + zPos + "\nxRot: " + xRot + "\nyRot: " + yRot + "\nzRot: " + zRot);
        if(histRend!=null){
            histRend.GetComponent<HistRenderer>().RendHistFrame(xPos, yPos, zPos, xRot, yRot, zRot);
        }
        if(dispValTxtObj!=null){
            dispValTxtObj.GetComponent<TMP_Text>().text = dispVals;
        }

        allThreshValues += (xPos + "," + yPos + "," + zPos + "," + xRot + "," + yRot + "," + zRot + "\n");
        if(this.gameObject.GetComponent<RecordData>() != null){
            this.gameObject.GetComponent<RecordData>().WriteData("ThreshValues.csv" , allThreshValues);
            this.gameObject.GetComponent<RecordData>().WriteData("ThreshTruthCheck.csv" , threshTruthCheck);
            this.gameObject.GetComponent<RecordData>().WriteData("StillCheck.csv" , stillCheck);
            this.gameObject.GetComponent<RecordData>().WriteData("WalkCheck.csv" , walkCheck);
            this.gameObject.GetComponent<RecordData>().WriteData("RunCheck.csv" , runCheck);
            this.gameObject.GetComponent<RecordData>().WriteData("SquatCheck.csv" , squatCheck);

        }
        

        if (zPos <= 0.0008 && zPos >=-0.0008 && yPos <= 0.0006 && yPos >=-0.0006) //not moving forward, back, up or down
        {
            /*if(yRot > 0 && yRot < 40){ //rotation threshold for clockwise
                if(spinLeft){
                    spinCount+=1;
                    if(spinCount > 10){ //make sure that it has been within those values for 10 checks (about 1 second).
                        squatCount = 0; 
                        moveString = "Spinning Clockwise";
                        moveCount = 0;
                        return moveString;
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
                        moveString = "Spinning AntiClockWise";
                        moveCount = 0;
                        return moveString;
                    }
                }else{
                    spinLeft = false;
                    spinCount=0;
                }
            }*/
            if(moveCount<5){
                moveCount+=1;
                return moveString;
            }else{
                moveString = "Standing Still";
                threshTruthCheck +=("1, 0, 0, 0\n");
                moveCount = 0;
                return moveString;  
            }
        }
        else if (zPos >= 0.075) //any forward movement faster than walking.
        {
            squatCount = 0;
            moveString = "Jogging";
            threshTruthCheck +=("0, 0, 1, 0\n");
            moveCount = 0;
            return moveString;
        }
        //Running and stairs seem to mess up
        // else if(zPos >= 0.03 && yPos > 0.023){ //threshold for going up stairs
        //     squatCount = 0; 
        //     return "Going Up Stairs";
        // }else if(zPos >= 0.03 && yPos < -0.023){ //threshold for going down stairs
        //     squatCount = 0; 
        //     return "Going Down Stairs";
        // }
        else if (zPos >= 0.0008 && zPos < 0.075) //threshold for walking
        {
            squatCount = 0;   
            moveString = "Walking";
            threshTruthCheck +=("0, 1, 0, 0\n");
            return moveString; 
        }
        else if (yPos > 0 && zPos < 0 || yPos < 0 && zPos > 0) //if y and z positional movements are opposites and none of the other criteria have been met.
        {
            if (yPos < 0){
                if (squatDown == false){
                    squatCount += 1;
                    squatDown = true;
                }  
            }else{
                squatDown = false;
            }
            moveString = "Squats: " + squatCount;
            threshTruthCheck +=("0, 0, 0, 1\n");
            moveCount = 0;
            return moveString;
        }
        else
        {   
            if(moveCount<10){
                moveCount+=1;
                return moveString;
            }else{
                moveString = "Unknown Activity";
                threshTruthCheck +=("0, 0, 0, 0\n");
                stillCheck += "0\n";
                walkCheck += "0\n";
                runCheck += "0\n";
                squatCheck += "0\n";
                moveCount = 0;
                return moveString;  
            }  
                
            
        }
    }
}

