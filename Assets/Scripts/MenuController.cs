using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        GlobalControl.Instance.participantID = "";
    }

    public void RecordID(string arg0)
    {
        GlobalControl.Instance.participantID = arg0;
    }

    public void RecordHand(int arg0)
    {
        GlobalControl.Instance.handOption = arg0;

        switch (arg0)
        {
            case 0:
                GlobalControl.Instance.hand = Hand.RIGHT;
                break;
            case 1:
                GlobalControl.Instance.hand = Hand.LEFT;
                break;
            default:
                GlobalControl.Instance.hand = Hand.RIGHT;
                break;
        }
    }

    public void RecordCondition(int arg0)
    {
        GlobalControl.Instance.conditionOption = arg0;

        switch (arg0)
        {
            case 0:
                GlobalControl.Instance.condition = Condition.REGULAR;
                break;
            case 1:
                GlobalControl.Instance.condition = Condition.SIMPLE;
                break;
            default:
                GlobalControl.Instance.condition = Condition.REGULAR;
                break;
        }
    }


    public void NextScene()
    {
	    
        SceneManager.LoadScene("MainTask");

    }
}