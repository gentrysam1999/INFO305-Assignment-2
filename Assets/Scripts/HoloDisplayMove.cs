using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoloDisplayMove : MonoBehaviour
{
    public GameObject textObj;
    public GameObject textTimeObj;
    private float timer = 0.0f;
    private float timeLeft;
    public float timeStepDuration;
    public int poseCount;
    public int poseCurrentCount = 1;
    private List<Pose>[] dispValues;
    private Pose[] setUp;
    private Pose currentPose;
    private Pose prevPose;
    private Pose accumPose;

    private float xPos = 0.0f;
    private float yPos = 0.0f;
    private float zPos = 0.0f;
    private Vector3 angle;
    private float xRot = 0.0f;
    private float yRot = 0.0f;
    private float zRot = 0.0f;

    public string movement;

    private float startTime;
    private float endTime;
    private bool isReady = false;
    // Start is called before the first frame update
    void Start()
    {
        setUp = new Pose[poseCount];
        dispValues = new List<Pose>[poseCount];
        prevPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
        startTime = timer;
        
    }

    // Update is called once per frame
    void Update()
    {
        //            currentPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);

        //DO SANITY CHECK (totalDisp < 2)

        //SANITE CHECK SUCCESS
            //IS timer+deltaTime > timeStepDuration
            //NO
            //compute relative, add accumulatePose+=relative (do not forget to accumulate angles as EULER and account for negative angles being returned as >180)
            //set timer +=deltaTime
            //YES
            //interPose = interpolatePose(prevPose, currentPose, 0, deltaTime, timeStepDuration-timer)
            // accumulatePose+=relative (prevPose, interPose) (do not forget to accumulate angles as EULER and account for negative angles being returned as >180)
            //add to accumulatePose; MoveCalc(accumulatePose, poseCount)
            //accumulatePose = relative(interPose, currentPose)
            //timer = timer+deltaTime - timeStepDuration

        //prevPose = currentPose
        if(textTimeObj!=null){
            textTimeObj.GetComponent<TMP_Text>().text = timeStepDuration.ToString("0.00");
        }
        
        currentPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
        Pose poseDisp = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, currentPose);
        float totalDisp = Mathf.Sqrt((poseDisp.position.x*poseDisp.position.x) + (poseDisp.position.y*poseDisp.position.y) + (poseDisp.position.z*poseDisp.position.z));
        if (!(totalDisp > 2)){
            if (timer+Time.deltaTime < timeStepDuration){
                xPos+= poseDisp.position.x;
                yPos+= poseDisp.position.y;
                zPos+= poseDisp.position.z;
                angle = poseDisp.rotation.eulerAngles;
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
                Vector3 accumPos = new Vector3(xPos, yPos, zPos);
                Quaternion accumRot = Quaternion.Euler(xRot, yRot, zRot);
                accumPose  = new Pose(accumPos, accumRot);
                timer += Time.deltaTime;
            }
            else{
                Pose interPose = this.gameObject.GetComponent<BasicInterpolation>().InterpolatePose(prevPose, currentPose, 0, Time.deltaTime, timeStepDuration-timer);
                Pose poseDispFin = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, interPose);
                
                xPos+= poseDispFin.position.x;
                yPos+= poseDispFin.position.y;
                zPos+= poseDispFin.position.z;
                angle = poseDispFin.rotation.eulerAngles;
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
                Vector3 accumPos = new Vector3(xPos, yPos, zPos);
                Quaternion accumRot = Quaternion.Euler(xRot, yRot, zRot);
                accumPose  = new Pose(accumPos, accumRot);
                MoveCalc(accumPose, poseCount);
                accumPose = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(interPose, currentPose);
                xPos= accumPose.position.x;
                yPos= accumPose.position.y;
                zPos= accumPose.position.z;
                angle= accumPose.rotation.eulerAngles;
                if(angle.x > 180){
                    xRot = angle.x - 360;
                }else{
                    xRot = angle.x;
                }
                if(angle.y > 180){
                    yRot = angle.y - 360;
                }else{
                    yRot = angle.y;
                }
                if(angle.z > 180){
                    zRot = angle.z - 360;
                }else{
                    zRot = angle.z;
                }
                timer = timer+Time.deltaTime - timeStepDuration;
            }
        }
        prevPose = currentPose;
    }





    //     if (timer > timeStepDuration){
    //         timer+=Time.deltaTime;
    //         endTime = timer;
    //         timeLeft = timer % timeStepDuration;
    //         currentPose = new Pose(this.gameObject.transform.position, this.gameObject.transform.rotation);
    //         Pose tempPose = this.gameObject.GetComponent<BasicInterpolation>().InterpolatePose(prevPose, currentPose, startTime, endTime, timeLeft);
            
    //         currentPose = tempPose;

    //         Pose poseDisp = this.gameObject.GetComponent<RelativePose>().ComputeRelativePose(prevPose, currentPose);
    //         float totalDisp = Mathf.Sqrt((poseDisp.position.x*poseDisp.position.x) + (poseDisp.position.y*poseDisp.position.y) + (poseDisp.position.z*poseDisp.position.z));
    //         if (!(totalDisp > 2)){
    //             this.MoveCalc(poseDisp, poseCount);
    //         }
    //         timer = 0;
    //     }else{
    //         timer+=Time.deltaTime;
    //     }
    //     prevPose = currentPose; 
    // }

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
                    if(textObj != null)
                    {
                        textObj.GetComponent<TextMesh>().text = (movement);
                    }
                    dispValues[i].Clear();
                    dispValues[i].Add(poseDisp); 
                }
            }
        }
    //Debug.Log(currentPose +  "," + previousPose + "," + poseCount + "," + timeLeft);
    }

    public void IncreaseTimeStep(){
        timeStepDuration += 0.1f;
    }
    public void DecreaseTimeStep(){
        if(timeStepDuration>0.1f){
            timeStepDuration-=0.1f;
        }
    }
    public void IncreasePoseCount(){
        poseCount += 5;
    }
    public void DecreasePoseCount(){
        if(poseCount>5){
            poseCount -= 5;
        }
    }
}
