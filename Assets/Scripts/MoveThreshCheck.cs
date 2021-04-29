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
    public float xRot;
    public float yRot;
    public float zRot;

    public string findMovement(List<Pose> poses, int poseCount)
    {
        for (int j = 0; j <= poseCount - 1; j++)
        {
            xPos += poses[j].position.x;
            yPos += poses[j].position.y;
            zPos += poses[j].position.z;
            xRot += poses[j].position.x.eulerAngles;
            yRot += poses[j].position.y.eulerAngles;
            zRot += poses[j].position.z.eulerAngles;

        }

        xPos = (xPos / poseCount);
        yPos = (yPos / poseCount);
        zPos = (zPos / poseCount);
        xRot = (xRot / poseCount);
        yRot = (yRot / poseCount);
        zRot = (zRot / poseCount);

        if (xPos <= standStill)
        {
            return "Standing Still";

        }
        else if (xPos >= walking && xPos < jogging)

        {
            return "Walking";

        }
        else if (xPos >= jogging)

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
