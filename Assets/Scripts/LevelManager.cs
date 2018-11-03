﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	public Scenario scenarioRef;
    StateMachine<LevelManager> stateMachine;

    AudioSource source;

    public bool Begin { get; set; }

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
        Begin = false;
    }

    // Use this for initialization
    void Start ()
    {
        stateMachine = new StateMachine<LevelManager>(this);
		Begin = true;
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Begin)
        {
		    scenarioRef.Scroll ();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Begin = true;
        }
        if (source.timeSamples > 526912 + 4661888)
        {
            source.timeSamples = 526912;
        }
	}
}
