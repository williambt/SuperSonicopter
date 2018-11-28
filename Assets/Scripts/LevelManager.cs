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
        if (!ArduinoInput.Instance.KeyboardMode)
        {
            if (!Connected)
            {
                if (ArduinoInput.IsDeviceReady())
                {
                    SuccesfullyConnected();
                }
            }
            else
            {
				if (ArduinoInput.GetFire())
				{
					PressedBegin();
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
