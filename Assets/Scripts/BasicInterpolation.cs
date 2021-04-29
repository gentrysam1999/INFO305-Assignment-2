using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BasicInterpolation : MonoBehaviour
{
    public Pose InterpolatePose(Pose startPose, Pose endPose, float startTime, float endTime, float interpolateTime)
    {
        if (interpolateTime > endTime)
        {
            return endPose;
        }

        Pose timestampPose = new Pose();
        timestampPose.rotation = Quaternion.Lerp(startPose.rotation, endPose.rotation, (interpolateTime - startTime) / (endTime-startTime));
        timestampPose.position = Vector3.Lerp(startPose.position, endPose.position, (interpolateTime - startTime) / (endTime - startTime));
        return timestampPose;
    }
}
