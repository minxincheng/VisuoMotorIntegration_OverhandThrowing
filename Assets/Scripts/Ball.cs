using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{

    public GameObject gameController;

    public int success = 0;

    //public ParticleSystem successFX;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
        {
            gameController.GetComponent<GameController>().success = 1;
            //successFX.Play();
        }
        else
        {
            gameController.GetComponent<GameController>().success = 0;
        }
        /*
        if(other.tag == "Hand")
        {
            gameController.GetComponent<GameController>().grasping = 1;
        }
        else
        {
            gameController.GetComponent<GameController>().grasping = 0;
        }
        */

        //ballHitGround = other.tag == "Ground" ? true : false;

    }

    private void OnTrigerExit(Collider other)
    {

        gameController.GetComponent<GameController>().success = 0;
        gameController.GetComponent<GameController>().grasping = 0;
        //ballHitGround = false;

    }
    
}

