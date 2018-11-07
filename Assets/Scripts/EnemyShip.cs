using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipAudio))]
public class EnemyShip : MonoBehaviour, IShip
{
	[Header("Style")]
	public GameObject Bullet;
    public Sprite dead;
    public float MaxHP;

    [HideInInspector]
	public Rigidbody2D RigidbodyRef;
	ObjectPool ObjectPool;
    ShipAudio ShipAudioRef;

    [HideInInspector]
	public StateMachine<EnemyShip> stateMachine;

	MovementType MoveType;

    float HP { get; set; }
    void Start ()
    {
        HP = MaxHP;
        
        RigidbodyRef = gameObject.GetComponent<Rigidbody2D>();
        ShipAudioRef = gameObject.GetComponent<ShipAudio>();
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
        if(GetComponent<MovementType>().GetType() == typeof(LinearMovement))
        {
            print(GetComponent<SpriteRenderer>().sprite);
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
}
