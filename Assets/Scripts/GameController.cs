using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using Manus.Interaction;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    GlobalControl globalControl;
    DataHandler dataHandler;
    
    private Condition condition;
    private Hand hand;

    // gameobjects in the scene
    public GameObject ballPerfab;
    private GameObject ball;
    private GameObject newBall;
    public GameObject basket, container, successTrigger;
    public GameObject hmd;
    private GameObject environmentObj;
    public GameObject environmentRegular, environmentSimple;
    public List<GameObject> leftHand = new List<GameObject>();
    public List<GameObject> rightHand = new List<GameObject>();
    public List<float> distances = new List<float>();
    public List<float> distancesAdjust = new List<float>();

    // positions for reference
    public Vector3 hmdPos, basketPos;

    // bool to control ball spawn
    public bool inRespawnMode = false;

    public int trialNum = 0;
    public int grasping, ballHitGround, success;
    public float distance;
    public Vector3 handPos;
    public int currDisIdx = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        globalControl = GlobalControl.Instance;
        dataHandler = GetComponent<DataHandler>();

        // gain information from global settings
        dataHandler.dataWritten = false;

        condition = globalControl.condition;
        hand = globalControl.hand;

        // make sure the correct environment is loaded
        if(condition == Condition.REGULAR){
            environmentSimple.SetActive(false);
            environmentRegular.SetActive(true);
            environmentObj = environmentRegular;
        }
        else{
            environmentSimple.SetActive(true);
            environmentRegular.SetActive(false);
            environmentObj = environmentSimple;
        }

        // create the distance list
        distances.Add(0.5f);
        distances.Add(0.8f);
        distances.Add(1.1f);

        distancesAdjust.Add(-0.6f);
        distancesAdjust.Add(-0.8f);
        distancesAdjust.Add(-1.1f);

        // make sure everything starts at a correct status
        inRespawnMode = true;
        trialNum = 0;
        grasping = 0;
        ballHitGround = 0;
        success = 0;

    }

    public void Initialize()
    {

        // display basket and container
        basket.SetActive(true);
        container.SetActive(true);
        // start recording data
        if (globalControl.recordingData)
        {
            GatherContinuousData();
        }

        Debug.Log("Initialized");

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){

            if(trialNum == 0){

                hmdPos = hmd.gameObject.transform.position;
                environmentObj.transform.position = new Vector3(hmdPos.x - 1.73f, hmdPos.y + 0.2f, hmdPos.z + 2.1f);

                // set container position
                if (hand == Hand.RIGHT)
                {
                    container.gameObject.transform.position = new Vector3(hmdPos.x  + 0.2f, hmdPos.y * 0.8f, hmdPos.z + 0.3f);
                    foreach (GameObject lh in leftHand)
                    {
                        lh.SetActive(false);
                    }

                }
                else
                {
                    container.gameObject.transform.position = new Vector3(hmdPos.x  - 0.5f, hmdPos.y * 0.8f, hmdPos.z + 0.3f);
                    foreach (GameObject rh in rightHand)
                    {
                        rh.SetActive(false);
                    }

                }

                Initialize();

            }

            SpawnBall();

        }

        
        GatherContinuousData();

        ball = GameObject.FindWithTag("Ball");


    }

    
    public void SpawnBall()
    {

        Debug.Log("spawn");

        if (ball != null && ball.transform.position.y <=  0.55f)
        {

            ballHitGround = 1;
            inRespawnMode = true;
            ball.gameObject.transform.tag = "Null";

        }

        if (ball == null)
        {
            trialNum++;
            WhatsNext();

            if (hand == Hand.RIGHT)
            {
                newBall = Instantiate(ballPerfab, new Vector3(container.transform.position.x, container.transform.position.y + 0.1f, container.transform.position.z), Quaternion.identity);
            }
            else
            {
                newBall = Instantiate(ballPerfab, new Vector3(container.transform.position.x, container.transform.position.y + 0.1f, container.transform.position.z), Quaternion.identity);
            }
            ball = newBall;
            ball.SetActive(true);
            newBall.gameObject.tag = "Ball";
            inRespawnMode = false;
            
            ballHitGround = 0;

            /*
            var colliders = new ClothSphereColliderPair[1];
            colliders[trialNum - 1] = ball.gameObject.GetComponent<SphereCollider>();
            basket.GetComponent<Cloth>().SphereColliders = colliders;
            */
        }
    }

    public void WhatsNext()
    {

        if (trialNum == 1 || trialNum == 16 || trialNum == 31)
        {

            MoveBasket();

        }

        if (trialNum == 46)
        {

            QuitTask();

        }

    }

    public void MoveBasket()
    {

        basket.gameObject.transform.position = new Vector3(distancesAdjust[currDisIdx], hmdPos.y - 0.2f, distances[currDisIdx]);
        distance = distances[currDisIdx];

        currDisIdx++;

    }

    public void GatherContinuousData()
    {

        // ADD EYE TRACKING DATA HERE //
        int isValid = gameObject.GetComponent<EyeTracker>().isValid;
        Vector3 gazePos = gameObject.GetComponent<EyeTracker>().gaze;
        string gazedObject = gameObject.GetComponent<EyeTracker>().gazedObj;
        //

        if (hand == Hand.RIGHT)
        {
            handPos = rightHand[0].transform.position;
            grasping = rightHand[0].transform.Find("Interaction").gameObject.GetComponent<HandGrabInteraction>().m_Grabbing ? 1 : 0;            
        }
        else
        {
            handPos = leftHand[0].transform.position;
            grasping = leftHand[0].GetComponentInChildren<HandGrabInteraction>().m_Grabbing ? 1 : 0;
        }

        if (globalControl.recordingData && ball != null)
        {

            Vector3 ballPos = ball.transform.position;

            Vector3 basketPos = basket.transform.position;

            int success = successTrigger.GetComponent<SuccessTrigger>().success;

            dataHandler.recordContinuous(condition.ToString(), Time.time, grasping, ballHitGround, success, trialNum, distance, basketPos, handPos, ballPos, isValid, gazePos, gazedObject);

        }

    }

    private void OnApplicationQuit()
    {

        QuitTask();

    }

    public void QuitTask()
    {

        dataHandler.WriteDataToFiles();
        SceneManager.LoadScene("Menu");
        //Application.Quit();

    }

}