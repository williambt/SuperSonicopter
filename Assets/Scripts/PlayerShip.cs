using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipAudio))]
public class PlayerShip : MonoBehaviour, IShip
{
    [Header("Helicopter Style")]
    public BulletSettings settings;
    public GameObject bullet;
    public Sprite dead;
    [Header("Helicopter Settings")]
    [Range(0, 25)]
    public float displacementSpeed = 10;
    public float sensitivity = 3;
    public int maxBulletsOnScreen = 30;
    public float MaxHP;
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
    public Rigidbody2D RigidbodyRef;

    ShipAudio ShipAudioRef;
    ObjectPool objectPool;

    float HP { get; set; }

    public void Awake()
    {
        device = new wrmhlComponent(portName, baudRate, readTimeout, queueLength);
        keyboardMode = !device.IsConnected();
    }
    // Use this for initialization
    void Start ()
    {
        yStart = transform.position.y;
		ShipAudioRef = gameObject.GetComponent<ShipAudio> ();
        RigidbodyRef = gameObject.GetComponent<Rigidbody2D>();
        objectPool = new ObjectPool(bullet, transform, maxBulletsOnScreen);

        HP = MaxHP;

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
        firedBullet.GetComponent<Bullet>().Initialize(gameObject, settings);
        ShipAudioRef.PlayFireSound();
	}
    public void Move()
    {
        lerpT += displacementSpeed / 100.0f;
        RigidbodyRef.MovePosition( new Vector2(transform.position.x, Mathf.Lerp(lerpStartY, lerpTargetY, lerpT)));
    }
	public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<EnemyShip>())
        {
            HP = 0 ;
        }
    }

    public bool IsDead()
    {
        return HP <= 0;
    }

    public void Explode()
    {
        GetComponent<SpriteRenderer>().sprite = dead;
        GetComponent<Animator>().SetBool("Alive", false);
        ShipAudioRef.PlayExplosionSound();
    }

    public void TakeDamage(float value)
    {
        HP -= value;
    }
    public string ReadInput()
    {
        return device.Read();
    }
}
