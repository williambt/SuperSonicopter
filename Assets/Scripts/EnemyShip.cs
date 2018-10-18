using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour, IShip
{
	[Header("Style")]
	public GameObject Bullet;
	AudioSource Source;
	public AudioClip Explosion;
	public Sprite Dead;
	public float Speed;
	[HideInInspector]
	public Rigidbody2D rigidbodyRef;
	ObjectPool objectPool;

	[HideInInspector]
	public StateMachine<EnemyShip> stateMachine;

	MovementType moveType; 

    void Start ()
    {
		rigidbodyRef = gameObject.GetComponent<Rigidbody2D>();
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

}
