using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadWriteCSV;
using System.IO;

public class DataHandler : MonoBehaviour
{
    GlobalControl globalControl;

    List<ContinuousData> continuousDatas = new List<ContinuousData>();

    private string pid;

    private string hand;

    public bool dataWritten = false;

    public void Start(){

	    globalControl = GlobalControl.Instance;

    }

    void OnDisable()
    {
        WriteContinuousFile();
    }

    public void WriteDataToFiles()
    {
        if (dataWritten)
        {
            Debug.Log("Data already saved");
            return;
        }

        dataWritten = true;

        System.DateTime now = System.DateTime.Now;
	    hand = globalControl.hand.ToString();

        pid = GlobalControl.Instance.participantID + "_" +  "Task2_VR" + GlobalControl.Instance.participantID + "_" + "Taks1" + "_" + now.Month.ToString() + "_" + now.Day.ToString() + "_" + now.Hour + "-" + now.Minute;

        if (GlobalControl.Instance.recordingData)
        {
            WriteContinuousFile();
        }

    }

    public void recordContinuous(string environment, float time, int grasping, int ballHitGround, int success, int trialNum, float distance, Vector3 basketPos, Vector3 handPos, Vector3 ballPos, int isValid, Vector3 gazePos, string gazedObject){
        
        continuousDatas.Add(new ContinuousData(environment, time, grasping, ballHitGround, success, trialNum, distance, basketPos, handPos, ballPos, isValid, gazePos, gazedObject));
        
    }

    private void WriteContinuousFile()
    {
        //string directory = Application.persistentDataPath + "VRFG_" + GlobalControl.Instance.participantID;
        string directory = "Data/" + GlobalControl.Instance.participantID + "_" + "Taks2_VR";
        Directory.CreateDirectory(@directory);
        
        using(CsvFileWriter writer = new CsvFileWriter(@directory + "/" + pid + ".csv")){
                Debug.Log("Writing continuous data to file");
                
                //WriteHeaderInfo(writer);
                
                CsvRow header = new CsvRow();
                header.Add("ID");
                header.Add("Hand");
                header.Add("Condition");
                header.Add("Timestamp");
                header.Add("Grasping");
                header.Add("BallHitGround");
                header.Add("Success");
                header.Add("Trial");
                header.Add("Distance");
                header.Add("BasketPosX");
                header.Add("BasketPosY");
                header.Add("BasketPosZ");
                header.Add("HandPosX");
                header.Add("HandPosY");
                header.Add("HandPosZ");               
                header.Add("BallPosX");
                header.Add("BallPosY");
                header.Add("BallPosZ");
                header.Add("isValid");
                header.Add("GazePosX");
                header.Add("GazePosY");
                header.Add("GazePosZ");
                header.Add("GazedObject");
                
                writer.WriteRow(header);
                
                // write each line of data
                foreach(ContinuousData c in continuousDatas){
                    CsvRow row = new CsvRow();
                    
                    row.Add(GlobalControl.Instance.participantID);
                    row.Add(hand);
                    row.Add(c.environment.ToString());
                    row.Add(c.time.ToString());
                    row.Add(c.grasping.ToString());
                    row.Add(c.ballHitGround.ToString());
                    row.Add(c.success.ToString());
                    row.Add(c.trialNum.ToString());
                    row.Add(c.distance.ToString());
                    row.Add(c.basketPos.x.ToString());
                    row.Add(c.basketPos.y.ToString());
                    row.Add(c.basketPos.z.ToString());                   
                    row.Add(c.handPos.x.ToString());
                    row.Add(c.handPos.y.ToString());
                    row.Add(c.handPos.z.ToString());
                    row.Add(c.ballPos.x.ToString());
                    row.Add(c.ballPos.y.ToString());
                    row.Add(c.ballPos.z.ToString());
                    row.Add(c.isValid.ToString());
                    row.Add(c.gazePos.x.ToString());
                    row.Add(c.gazePos.y.ToString());
                    row.Add(c.gazePos.z.ToString());
                    row.Add(c.gazedObject.ToString());
                    
                    writer.WriteRow(row);
                    
                }
            }
        }
}