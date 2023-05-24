using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;

public class EyeTracker : MonoBehaviour
{
    public Vector3 origin;
    public Vector3 direction;
    public Vector3 gaze;
    public string gazedObj;
    public int isValid;

    GlobalControl globalControl;

    DataHandler dataHandler;

    //public GameObject gameController;
    //public GameObject startPos, eyeCenter, target, targetComplex;

    void Start()
    {
        globalControl = GlobalControl.Instance;
        dataHandler = gameObject.GetComponent<DataHandler>();
    }

    void Update()
    {
        var recordEye = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
        var gazeRay = recordEye.GazeRay;
        if (gazeRay.IsValid)
        {
            isValid = 1;
        }
        else
        {
            isValid = 0;
        }

        origin = gazeRay.Origin;
        direction = gazeRay.Direction;

        RaycastHit hit;
        Debug.DrawRay(gazeRay.Origin, gazeRay.Direction * 1000, Color.green);
        if (Physics.Raycast(gazeRay.Origin, gazeRay.Direction, out hit))
        {
            gaze = hit.point;
            gazedObj = hit.collider.gameObject.tag.ToString();

            /*
            if (hit.collider.gameObject.tag == "Ball")
            {
                gazedObj = 1;
            }
            */

        }

    }
}