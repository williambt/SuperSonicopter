using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipAudio))]
public class PlayerShip : MonoBehaviour, IShip
{
    [Header("Helicopter Style")]
    public GameObject bullet;
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
	[HideInInspector]
    public Rigidbody2D rigidbody;

    ShipAudio ShipAudioRef;
    ObjectPool objectPool;

	// Use this for initialization
	void Start ()
    {
        yStart = transform.position.y;
        device = new wrmhlComponent(portName, baudRate, readTimeout, queueLength);
		ShipAudioRef = gameObject.AddComponent<ShipAudio> ();
        keyboardMode = !device.IsConnected();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        objectPool = new ObjectPool(bullet, transform, maxBulletsOnScreen);

        // inicialização da state machine
        stateMachine = new StateMachine<PlayerShip>(this);
        // 1 - criar estados
        ShipStates.SShipBegin<PlayerShip> SShipBegin = new ShipStates.SShipBegin<PlayerShip>(this);
        ShipStates.SPlayerControlling<PlayerShip> sPlayerControlling = new ShipStates.SPlayerControlling<PlayerShip>(this);
        ShipStates.SShipExploding<PlayerShip> SShipExploding = new ShipStates.SShipExploding<PlayerShip>(this);
        ShipStates.SShipDead<PlayerShip> SShipDead = new ShipStates.SShipDead<PlayerShip>(this);
        // 2 - criar transições
        ShipStates.TLevelStart<PlayerShip> levelStart = new ShipStates.TLevelStart<PlayerShip>(this);
        ShipStates.TIsDead<PlayerShip> isDead = new ShipStates.TIsDead<PlayerShip>(this);
        ShipStates.TReadyToReset<PlayerShip> readyToReset = new ShipStates.TReadyToReset<PlayerShip>(this);
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
        ShipAudioRef.PlayFireSound();
	}
    public void Move()
    {
        lerpT += displacementSpeed / 100.0f;
        rigidbody.MovePosition( new Vector2(transform.position.x, Mathf.Lerp(lerpStartY, lerpTargetY, lerpT)));
    }
	public void OnCollisionEnter2D(Collision2D col)
	{
        Explode();
	}

    public bool IsDead()
    {
        return !GetComponent<Animator>().GetBool("Alive");
    }

    public void Explode()
    {
        GetComponent<SpriteRenderer>().sprite = dead;
        GetComponent<Animator>().SetBool("Alive", false);
        ShipAudioRef.PlayExplosionSound();
    }
}
