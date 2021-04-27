using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePose : MonoBehaviour
{

    public Pose ComputeRelativePose(Pose A, Pose B)
    {
        Pose temp = new Pose();
        temp.rotation = Quaternion.Inverse(A.rotation) * B.rotation;
        temp.position = Quaternion.Inverse(A.rotation) * (B.position - A.position);
        return temp;
    }
}
