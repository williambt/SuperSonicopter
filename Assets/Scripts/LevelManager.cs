using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	public Scenario scenarioRef;
    StateMachine<LevelManager> stateMachine;
    public PlayerShip player;
    public Animator title;
    public WaveEmitter emitter;
    AudioSource source;

    public bool Begin { get; set; }
    public bool Connected { get; set; }
    public static LevelManager Instance;


    private void Awake()
    {
        Instance = this;
        Begin = false;
        Connected = false;
    }

    // Use this for initialization
    void Start ()
    {
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!player.keyboardMode)
        {
            if (!Connected)
            {
                if (player.ReadInput() != null)
                {
                    SuccesfullyConnected();
                }
            }
            else
            {
                string input = player.ReadInput();
				if (input != null) 
				{
					char[] delim = { '|' };
					string[] a = input.Split(delim);
					if (a[0] == "fire")
					{
						PressedBegin();
					}
				}
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.C))
            {
                SuccesfullyConnected();
            }
            if(Input.GetKeyDown(KeyCode.B) && Connected)
            {
                PressedBegin();
            }
        }
		if (Begin) {
			scenarioRef.Scroll ();
		}
        if (source.timeSamples > 526912 + 4661888)
        {
            source.timeSamples = 526912;
        }
	}
    public void SuccesfullyConnected()
    {
        Connected = true;
        title.SetBool("Connected", true);
    }
    public void PressedBegin()
    {
        Begin = true;
        title.SetBool("Begin", true);
        emitter.enabled = true;
    }
}
