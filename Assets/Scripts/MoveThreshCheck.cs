using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThreshCheck
{
    public float standStill;
    public float walking;
    public float jogging;
    public float sitUps;
    public float xPos;
    public float yPos;
    public float zPos;

    public string FindMovement(float[] poses, int poseCount)
    {
        for (int j = 0; j <= poseCount - 1; j++)
        {
            xPos += poses[j].position.x;
            yPos += poses[j].position.y;
            zPos += poses[j].position.z

        }

        xPos = (xPos / poseCount);
        yPos = (yPos / poseCount);
        zPos = (zPos / poseCount);

        if(xPos <= standStill)
        {
            return "Standing Still";

        }else if( xPos >= walking && xPos < jogging)

        {
            return "Walking";

        }else if(xPos >= jogging)

        {
            return "Jogging"
        }


 
    }
}
