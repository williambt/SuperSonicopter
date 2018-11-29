using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	public Scenario scenarioRef;
    StateMachine<LevelManager> stateMachine;
    public PlayerShip player;
    public Animator title;
    public WaveEmitter emitter;
    AudioSource source;

    public bool Calibrating { get; set; }

    public bool Begin { get; set; }
    public bool Connected { get; set; }
    public static LevelManager Instance;

    [HideInInspector]
    public bool GameOver = false;

    [HideInInspector]
    public float Score = 0;

    public GameObject GameOverUI;

    float ButtonPressTimer = 0f;
    float CalibratePressLength = 1f;


    public GameObject health_small;
    public GameObject health_large;
    public GameObject upgrade_small;
    public GameObject upgrade_large;

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

        player.UpperLimit = transform.GetChild(0).position.y;
        player.LowerLimit = transform.GetChild(1).position.y;

        try
        {
            BinaryReader file = new BinaryReader(new FileStream(Application.dataPath + "/calibration.bin", FileMode.Open));
            player.MaxSensorValue = file.ReadByte();
            file.Close();
        }
        catch(IOException e)
        {
            print(e.Message);
            Calibrating = true;
            title.SetBool("Calibrating", true);
        }
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
            else if (Calibrating)
            {
                if (ArduinoInput.GetFirePressed())
                {
                    float MaxSensorValue = ArduinoInput.GetUSensor();
                    player.MaxSensorValue = MaxSensorValue;
                    try
                    {
                        BinaryWriter file = new BinaryWriter(new FileStream(Application.dataPath + "/calibration.bin", FileMode.Create));
                        file.Write(MaxSensorValue);
                        file.Close();
                    }
                    catch(IOException e)
                    {
                        print(e.Message);
                    }
                    print("MaxValue: " + MaxSensorValue);
                    title.SetBool("Calibrating", false);
                    Calibrating = false;
                }
            }
            else if (!Begin)
            {
				if (ArduinoInput.GetFire())
				{
                    ButtonPressTimer += Time.deltaTime;
                    title.SetFloat("EnterCalibrationBar", ButtonPressTimer / CalibratePressLength);
                    if (ButtonPressTimer >= CalibratePressLength)
                    {
                        ButtonPressTimer = 0;
                        title.SetFloat("EnterCalibrationBar", 0);
                        Calibrating = true;
                        title.SetBool("Calibrating", true);
                    }
                }
                else if (ButtonPressTimer > 0)
                {
                    PressedBegin();
                }
            }
            else if (GameOver)
            {
                if(ArduinoInput.GetFirePressed())
                    SceneManager.LoadScene(0);
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
        if(GameOver)
        {
            GameOverUI.SetActive(true);
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

        ButtonPressTimer = 0;
        title.SetFloat("EnterCalibrationBar", 0);
    }
    public static void AddScore(float deltaScore)
    {
        LevelManager.Instance.Score += deltaScore;
    }
}
