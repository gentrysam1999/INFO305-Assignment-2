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
    private List<Pose>[] dispValues;
    private Pose[] setUp;
    private Pose currentPose;
    private Pose prevPose;

    public string movement;

    private float startTime;
    private float endTime;
    private bool isReady = false;
    // Start is called before the first frame update
    void Start()
    {
        setUp = new Pose[poseCount];
        prevPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
        startTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > timeStepDuration){
            timer+=Time.deltaTime;
            endTime = timer;
            timeLeft = timer % timeStepDuration;
            currentPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
            Pose tempPose = this.gameObject.GetComponent<BasicInterpolation>().InterpolatePose(prevPose, currentPose, startTime, endTime, timeLeft);
            currentPose = tempPose;

            Pose poseDisp = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, currentPose);
            float totalDisp = Mathf.Sqrt((poseDisp.position.x*poseDisp.position.x) + (poseDisp.position.y*poseDisp.position.y) + (poseDisp.position.z*poseDisp.position.z));
            if (!(totalDisp > 2)){
                this.MoveCalc(poseDisp, poseCount);
            }
        }else{
            timer+=Time.deltaTime;
        }
        prevPose = currentPose; 
    }

    public void MoveCalc(Pose poseDisp, int poseCount)

    {
        if (!isReady)
        {
            if (poseCurrentCount < poseCount)
            {
                setUp[poseCurrentCount-1] = poseDisp;
                poseCurrentCount++;
            }
            else
            {
                setUp[poseCurrentCount - 1] = poseDisp;
                for (int i = 0; i <= poseCount-1; i++){
                    for (int j = 0; j <= i; j++){
                            List<Pose> tempList = new List<Pose>();
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
                    List<Pose> tempList = new List<Pose>();
                    tempList.AddRange(dispValues[i]);
                    tempList.Add(poseDisp);
                    dispValues[i] = tempList;
                }else{
                    movement = this.gameObject.GetComponent<MoveThreshCheck>().findMovement(dispValues[i], poseCount);
                    
                    Debug.Log(movement);
                    dispValues[i].Clear();
                    dispValues[i].Add(poseDisp); 
                }
            }
        }
    //Debug.Log(currentPose +  "," + previousPose + "," + poseCount + "," + timeLeft);
    }
}
