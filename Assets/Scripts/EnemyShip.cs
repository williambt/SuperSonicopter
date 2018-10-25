using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShipAudio))]
public class EnemyShip : MonoBehaviour, IShip
{
	[Header("Style")]
	public GameObject Bullet;
    public Sprite dead;

    [HideInInspector]
	public Rigidbody2D RigidbodyRef;
	ObjectPool ObjectPool;
    ShipAudio ShipAudioRef;

    [HideInInspector]
	public StateMachine<EnemyShip> stateMachine;

	MovementType MoveType; 

    void Start ()
    {
		RigidbodyRef = gameObject.GetComponent<Rigidbody2D>();
        ShipAudioRef = gameObject.AddComponent<ShipAudio>();
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
	
	void Update ()
    {
		stateMachine.Update ();
	}
    public bool IsDead()
    {
        return false;
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        Explode();
    }
    public void Explode()
    {
        GetComponent<SpriteRenderer>().sprite = dead;
        GetComponent<Animator>().SetBool("Alive", false);
        ShipAudioRef.PlayExplosionSound();
    }
}
