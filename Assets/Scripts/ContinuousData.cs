using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ContinuousData : MonoBehaviour
{
    
    public readonly string environment;
    public readonly float time;
    public readonly int grasping;
    public readonly int ballHitGround;
    public readonly int success;
    public readonly int trialNum;
    public readonly float distance;
    public readonly Vector3 basketPos;
    public readonly Vector3 handPos;
    public readonly Vector3 ballPos;
    public readonly int isValid;
    public readonly Vector3 gazePos;
    public readonly string gazedObject;
    
    public ContinuousData(string environment, float time, int grasping, int ballHitGround, int success, int trialNum, float distance, Vector3 basketPos, Vector3 handPos, Vector3 ballPos, int isValid, Vector3 gazePos, string gazedObject){
        
        this.environment = environment;
        this.time = time;
        this.grasping = grasping;
        this.ballHitGround = ballHitGround;
        this.success = success;
        this.trialNum = trialNum;
        this.distance = distance;
        this.basketPos = basketPos;
        this.handPos = handPos;
        this.ballPos = ballPos;
        this.isValid = isValid;
        this.gazePos = gazePos;
        this.gazedObject = gazedObject;
    }

}