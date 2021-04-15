using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordMove : MonoBehaviour
{
    public GameObject textObj;

    private float testNum = 0;

    private Vector3 camAngle;
    

    private string camPosX;
    private string camPosY;
    private string camPosZ;
    private string camRotX;
    private string camRotY;
    private string camRotZ;

    private string allCamPosCsv;

    public long myTime;

    private bool testing = false;
    private float timer = 0.0f;
    private float waitTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer <= waitTime & testing == true)
        {
            //get positions and rotations and add to string
            camPosX = this.gameObject.transform.position.x.ToString();
            camPosY = this.gameObject.transform.position.y.ToString();
            camPosZ = this.gameObject.transform.position.z.ToString();
            camAngle = this.gameObject.transform.rotation.eulerAngles;
            camRotX = camAngle.x.ToString();
            camRotY = camAngle.y.ToString();
            camRotZ = camAngle.z.ToString();
            allCamPosCsv += (timer.ToString() + "," + camPosX + "," + camPosY + "," + camPosZ + "," + camRotX + "," + camRotY + "," + camRotZ + ",\n");

            textObj.GetComponent<TextMesh>().text = (timer.ToString());
        }
        else if (timer > waitTime & testing == true)
        {
            
            this.gameObject.GetComponent<RecordData>().WriteData("HeadsetPose" + testNum +"(" + myTime  + ")" + ".csv" , allCamPosCsv);
            allCamPosCsv = " ";

            testing = false;
        }
        else
        {
            timer = 0.0f;
        }
    }

    public void Test()
    {
        //Debug.Log("button clicked");
        testNum += 1;
        testing = true;
        myTime = System.DateTime.Now.Ticks;
    }

}
