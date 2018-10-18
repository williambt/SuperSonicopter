using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour, IShip
{
    [Header("Helicopter Style")]
    public GameObject bullet;
	AudioSource source;
	public AudioClip explosion;
	public Sprite dead;
    [Header("Helicopter Settings")]
    [Range(0, 25)]
    public float displacementSpeed = 10;
    public float sensitivity = 3;
    public int maxBulletsOnScreen = 50;
    [Space(10)]

    [Header("Arduino Settings")]
    public string portName = "COM3";
    public int baudRate = 9600;
    public int readTimeout = 20;
    public int queueLength = 1;

    [HideInInspector]
    public float yStart;
    [HideInInspector]
    public float offset = 1f;
    [HideInInspector]
    public wrmhlComponent device;

    [HideInInspector]
    public float lastPos = 0;
    [HideInInspector]
    public float lerpStartY = 0;
    [HideInInspector]
    public float lerpTargetY = 0;
    [HideInInspector]
    public float lerpT = 0.0f;
    [HideInInspector]
    public bool keyboardMode = false;
    [HideInInspector]
    public StateMachine<PlayerShip> stateMachine;
    Rigidbody2D rigidbody;


    ObjectPool objectPool;

	// Use this for initialization
	void Start ()
    {
        yStart = transform.position.y;
        device = new wrmhlComponent(portName, baudRate, readTimeout, queueLength);
		source = gameObject.AddComponent<AudioSource> ();
        keyboardMode = !device.IsConnected();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        objectPool = new ObjectPool(bullet, transform, maxBulletsOnScreen);

        // inicialização da state machine
        stateMachine = new StateMachine<PlayerShip>(this);
        // 1 - criar estados
        PlayerStates.SShipBegin<PlayerShip> SShipBegin = new PlayerStates.SShipBegin<PlayerShip>(this);
        PlayerStates.SPlayerControlling<PlayerShip> sPlayerControlling = new PlayerStates.SPlayerControlling<PlayerShip>(this);
        PlayerStates.SShipExploding<PlayerShip> SShipExploding = new PlayerStates.SShipExploding<PlayerShip>(this);
        PlayerStates.SShipDead<PlayerShip> SShipDead = new PlayerStates.SShipDead<PlayerShip>(this);
        // 2 - criar transições
        PlayerStates.TLevelStart<PlayerShip> levelStart = new PlayerStates.TLevelStart<PlayerShip>(this);
        PlayerStates.TIsDead<PlayerShip> isDead = new PlayerStates.TIsDead<PlayerShip>(this);
        PlayerStates.TReadyToReset<PlayerShip> readyToReset = new PlayerStates.TReadyToReset<PlayerShip>(this);
        // 3 - definir o distino das transições
        levelStart.TargetState = sPlayerControlling;
        isDead.TargetState = SShipExploding;
        readyToReset.TargetState = SShipDead;
        // 4 - adicionar as transições aos estados
        SShipBegin.AddTransition(levelStart);
        sPlayerControlling.AddTransition(isDead);
        SShipExploding.AddTransition(readyToReset);
        // 5 - adicionar os estados à maquina de estados
        stateMachine.AddState(SShipBegin);
        stateMachine.AddState(sPlayerControlling);
        stateMachine.AddState(SShipExploding);

        stateMachine.InitialState = SShipBegin;

        stateMachine.Start();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        stateMachine.Update();
	}
	public void Fire ()
	{
        GameObject firedBullet = objectPool.GetGameObjectFromPool();
        firedBullet.transform.position += new Vector3(GetComponent<SpriteRenderer>().bounds.extents.x, 0);
	}
    public void Move()
    {
        lerpT += displacementSpeed / 100.0f;
        rigidbody.MovePosition( new Vector2(transform.position.x, Mathf.Lerp(lerpStartY, lerpTargetY, lerpT)));
    }
	public void OnCollisionEnter2D(Collision2D col)
	{
		source.clip = explosion;
		source.loop = false;
		GetComponent<SpriteRenderer> ().sprite = dead;
        GetComponent<Animator>().SetBool("Alive", false);
		//GetComponent<Collider2D> ().enabled = false;
		source.Play ();
	}

    public bool IsDead()
    {
        return !GetComponent<Animator>().GetBool("Alive");
    }
}
