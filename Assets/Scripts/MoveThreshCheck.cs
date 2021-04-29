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

    public string findMovement(List<Pose> poses, int poseCount)
    {
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

        if (zPos <= standStill)
        {
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
