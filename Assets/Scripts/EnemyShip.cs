using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipAudio))]
public class EnemyShip : MonoBehaviour, IShip
{
	[Header("Style")]
	public GameObject Bullet;
    public BulletSettings Settings;
    public float MaxHP;

    [HideInInspector]
	public Rigidbody2D RigidbodyRef;
	ObjectPool Pool;
    ShipAudio ShipAudioRef;

    [HideInInspector]
	public StateMachine<EnemyShip> stateMachine;

	MovementType MoveType;

    public float Score;

    



    public float HP { get; set; }
    void Start ()
    {
        HP = MaxHP;

        RigidbodyRef = gameObject.GetComponent<Rigidbody2D>();
        ShipAudioRef = gameObject.GetComponent<ShipAudio>();

        Pool = new ObjectPool(Bullet,gameObject.transform, 10);
        // inicialização da state machine
        stateMachine = new StateMachine<EnemyShip>(this);
		// 1 - criar estados
		ShipStates.SShipBegin<EnemyShip> SShipBegin = new ShipStates.SShipBegin<EnemyShip>(this);
		ShipStates.SEnemyControlling sEnemyControlling = new ShipStates.SEnemyControlling(this);
		ShipStates.SShipExploding<EnemyShip> SShipExploding = new ShipStates.SShipExploding<EnemyShip>(this);
		ShipStates.SShipDead<EnemyShip> SShipDead = new ShipStates.SShipDead<EnemyShip>(this);
		// 2 - criar transições
		ShipStates.TLevelStart<EnemyShip> levelStart = new ShipStates.TLevelStart<EnemyShip>(this);
		ShipStates.TIsDead<EnemyShip> isDead = new ShipStates.TIsDead<EnemyShip>(this);
		ShipStates.TReadyToReset<EnemyShip> readyToReset = new ShipStates.TReadyToReset<EnemyShip>(this);
		// 3 - definir o distino das transições
		levelStart.TargetState = sEnemyControlling;
		isDead.TargetState = SShipExploding;
		readyToReset.TargetState = SShipDead;
		// 4 - adicionar as transições aos estados
		SShipBegin.AddTransition(levelStart);
		sEnemyControlling.AddTransition(isDead);
		SShipExploding.AddTransition(readyToReset);
		// 5 - adicionar os estados à maquina de estados
		stateMachine.AddState(SShipBegin);
		stateMachine.AddState(sEnemyControlling);
		stateMachine.AddState(SShipExploding);

		stateMachine.InitialState = SShipBegin;

		stateMachine.Start();
	}
	
	void FixedUpdate ()
    {
		stateMachine.Update ();
	}
    void Update()
    {
        if (ShouldBlink)
        {
            Blink();
        }
    }
    public bool IsDead()
    {
        return HP <= 0;
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if( col.gameObject.GetComponent<PlayerShip>())
        {
            HP = 0;
        }
    }

    public float dropRate = 100;
    public void Explode()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetBool("Alive", false);
        ShipAudioRef.PlayExplosionSound();

        float roll = Random.Range(0.0f, 1.0f);
        if (roll <= dropRate/100 )
        {
            int type = Random.Range(1, 4);
            switch (type)
            {
                case 1:
                    Instantiate(LevelManager.Instance.health_small,transform.position,transform.rotation);
                    break;
                case 2:
                    Instantiate(LevelManager.Instance.health_large, transform.position, transform.rotation);
                    break;
                case 3:
                    Instantiate(LevelManager.Instance.upgrade_small, transform.position, transform.rotation);
                    break;
                case 4:
                    Instantiate(LevelManager.Instance.upgrade_large, transform.position, transform.rotation);
                    break;
                default:
                    break;
            }
        }

        LevelManager.AddScore(Score);
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
    public void Fire()
    {
        GameObject firedBullet = Pool.GetGameObjectFromPool();
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        if (playerRef != null)
        {
            Settings.Dir = playerRef.transform.position - transform.position;
            Settings.Dir.Normalize();
            firedBullet.GetComponent<Bullet>().Initialize(gameObject, Settings);
            ShipAudioRef.PlayFireSound();
        }
    }
	public void Fire(Vector2 dir)
	{
		GameObject firedBullet = Pool.GetGameObjectFromPool();
		GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
		if (playerRef != null)
		{
			Settings.Dir = dir;
			firedBullet.GetComponent<Bullet>().Initialize(gameObject, Settings);
			ShipAudioRef.PlayFireSound();
		}
	}
    private void OnEnable()
    {
    }
}
