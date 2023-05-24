using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Condition { REGULAR, SIMPLE };
public enum Hand { RIGHT, LEFT };

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public Condition condition;

    public Hand hand;

    public int handOption = 0;

    public int conditionOption = 0;

    public string participantID = "";

    public bool recordingData = true;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}