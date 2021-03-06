﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipAudio))]
public class PlayerShip : MonoBehaviour, IShip
{
    [Header("Helicopter Style")]
    public BulletSettings settings;
    public GameObject bullet;
    [Header("Helicopter Settings")]
    [Range(0, 25)]
    public float displacementSpeed = 10;
    public float sensitivity = 3;
    public int maxBulletsOnScreen = 30;
    public float MaxHP;
    [Space(10)]

    [HideInInspector]
    public float yStart;
    [HideInInspector]
    public float offset = 1f;
    
	public GameObject porta;

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

    public float HP { get; set; }

    [HideInInspector]
    public float MaxSensorValue;
    [HideInInspector]
    public float UpperLimit;
    [HideInInspector]
    public float LowerLimit;

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
		ShipStates.SShipPlayerDead SShipDead = new ShipStates.SShipPlayerDead(this);
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
	void Update()
	{
		if (ShouldBlink)
		{
			Blink();
		}
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
           TakeDamage(MaxHP/3);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        PowerUp powerUp = collision.gameObject.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            PowerUp(powerUp);
            Destroy(powerUp.gameObject);
        }
    }
    public bool IsDead()
    {
        return HP <= 0;
    }

    public void Explode()
    {
		GameObject.Instantiate(porta, transform.position, transform.rotation);
        GetComponent<Animator>().SetBool("Alive", false);
        ShipAudioRef.PlayExplosionSound();
        LevelManager.Instance.GameOver = true;
    }

	float TakeDamageClock = 0;
	bool ShouldBlink = false;
	float TakeDamageBlinkLimit = 0.05f;

	public void TakeDamage(float value)
	{
		HP -= value;
		ShouldBlink = true;
		GetComponent<SpriteRenderer>().material.SetFloat("_ShouldBlink", 1);

	}
	void Blink()
	{
		TakeDamageClock += Time.deltaTime;
		if (TakeDamageClock >= TakeDamageBlinkLimit)
		{
			ShouldBlink = false;
			TakeDamageClock = 0;
			GetComponent<SpriteRenderer>().material.SetFloat("_ShouldBlink", 0);
		}
	}
    void PowerUp(PowerUp powerUp)
    {
        if (powerUp.IsHealthBooster)
        {
            HP += powerUp.HPRecover;
            if (HP > MaxHP)
            {
                HP = MaxHP;
            }
        }
        else
        {
            settings = powerUp.BulletPowerUp;
        }
    }
	
}
