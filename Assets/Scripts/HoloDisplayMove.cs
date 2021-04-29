using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloDisplayMove : MonoBehaviour
{
    private float timer = 0.0f;
    private float timeLeft;
    public float timeStepDuration;
    public int poseCount;
    public int poseCurrentCount = 1;

    Pose currentPose;
    Pose prevPose;

    // Start is called before the first frame update
    void Start()
    {
        prevPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        currentPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.position);
        Pose poseDisp = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, currentPose);
        float totalDisp = Mathf.Sqrt((poseDisp.position.x*poseDisp.position.x) + (poseDisp.position.y*poseDisp.position.y) + (poseDisp.position.z*poseDisp.position.z));
        
        if (!(totalDisp > 2)){
            
        }

        
        prevPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.position);
        timer+=Time.deltaTime;
    }

    public void MoveCalc(float totalDisp, int poseCount, float timeLeft)

    {
        if (!isReady)
        {
            if (poseCurrentCount < poseCount)
            {
                setUp[poseCurrentCount-1] = totalDisp;
                poseCurrentCount++;
            }
            else
            {
                setUp[poseCurrentCount - 1] = totalDisp;
                for (int i = 0; i <= poseCount-1; i++){
                    for (int j = 0; j <= i; j++){
                            List<float> tempList = new List<float>();
                            if(dispValues[i-j]!=null){
                                tempList.AddRange(dispValues[i-j]);
                            }
                            tempList.Add(setUp[i]);
                            dispValues[i-j] = tempList;    
                    }   
                }
                poseCurrentCount = 0;
                isReady = true;
            }
        }
        else{
            for (int i = 0; i <= poseCount-1; i++){
                if(dispValues[i].Count<=poseCount){
                    List<float> tempList = new List<float>();
                    tempList.AddRange(dispValues[i]);
                    tempList.Add(totalDisp);
                    dispValues[i] = tempList;
                }else{
                    float threshCheck = 0;
                    for (int j = 0; j <= poseCount-1; j++){
                        threshCheck += dispValues[i][j];
                    }
                    threshCheck = (threshCheck/poseCount);
                    Debug.Log(threshCheck);
                    dispValues[i].Clear();
                    dispValues[i].Add(totalDisp); 
                }
            }
        }
    //Debug.Log(currentPose +  "," + previousPose + "," + poseCount + "," + timeLeft);
    }
}
