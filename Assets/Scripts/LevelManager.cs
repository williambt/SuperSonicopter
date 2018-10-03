using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	public Scenario scenarioRef;
    StateMachine<LevelManager> stateMachine;

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

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Begin)
        {
		    scenarioRef.Scroll ();
        }
        if (Input.GetKey(KeyCode.F))
        {
            Begin = !Begin;
        }

	}
}
