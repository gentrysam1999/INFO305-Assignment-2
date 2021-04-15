using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    private GameObject cameraObj;
    private string camPos;
    public bool found = false;
    public string allCamPos;
    private string camPosX;
    public string allCamPosX;
    private string camPosY;
    public string allCamPosY;
    private string camPosZ;
    public string allCamPosZ;
    public string allCamPosCsv;

    // Start is called before the first frame update
    void Start()
    {
        cameraObj = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        camPos = cameraObj.transform.position.ToString();
        camPosX = cameraObj.transform.position.x.ToString();
        camPosY = cameraObj.transform.position.y.ToString();
        camPosZ = cameraObj.transform.position.z.ToString();
        allCamPos += (camPos+"\n");
        allCamPosX += (camPosX + "\n");
        allCamPosY += (camPosY + "\n");
        allCamPosZ += (camPosZ + "\n");
        allCamPosCsv += (camPosX + "," + camPosY + "," +camPosZ + ",\n");

        this.gameObject.GetComponent<TextMesh>().text = (camPos + "\nFound = "+ found);

    }

    public void Demo()
    {
        found = true;
    }

    public void ButtonTest()
    {
        //Debug.Log("button clicked");
        this.gameObject.GetComponent<RecordData>().WriteData("test.txt", allCamPos);
        this.gameObject.GetComponent<RecordData>().WriteData("testX.txt", allCamPosX);
        this.gameObject.GetComponent<RecordData>().WriteData("testY.txt", allCamPosY);
        this.gameObject.GetComponent<RecordData>().WriteData("testZ.txt", allCamPosZ);
        this.gameObject.GetComponent<RecordData>().WriteData("test.csv", allCamPosCsv);
     //+ allCamPosZ, + allCamPosZ + "\n");   
    }
}
