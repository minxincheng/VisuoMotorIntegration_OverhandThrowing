using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessTrigger : MonoBehaviour
{

    public int success = 0;

    public void OnTriggerEnter(Collider other){

        if(other.tag == "Ball"){
            success = 1;
        }

    }

    public void OnTriggerExit(){

        success = 0;

    }

}
