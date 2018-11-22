﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WaveEmitter : MonoBehaviour
{

    List<Wave> Waves;
	public int CurrentIndex = 0;
	float Clock = 0;
    bool Spawning = false;
    int CurrentWaveSpawned = 0;

	public GameObject Helicopter;
	public GameObject Zeppelin;
	public GameObject Kamikaze;
    public TextAsset WaveFile;

	ObjectPool HeliPool;
    ObjectPool ZeppelinPool;
	ObjectPool KamikazePool;


    GameObject EnemyParent = null;

	void Start ()
    {
        if(EnemyParent == null)
        {
            EnemyParent = new GameObject("Enemies");
        }
        Waves = new List<Wave>();
        HeliPool = new ObjectPool (Helicopter, transform, 30);
        ZeppelinPool = new ObjectPool(Zeppelin, transform, 30);
		KamikazePool = new ObjectPool (Kamikaze, transform, 30);

        ParseWavesFile();
        StartSpawning();
	}
	
	void FixedUpdate () 
	{
		Clock += Time.fixedDeltaTime;
        if (Spawning)
        {
            if (CurrentWaveSpawned >= Waves[CurrentIndex].EnemyCount)
            {
                Spawning = false;
                CurrentWaveSpawned = 0;
                Clock = 0;
            }
            else if (Clock >= Waves[CurrentIndex].SpawnDelay)
            {
                Spawn(Waves[CurrentIndex]);
                CurrentWaveSpawned++;
                Clock -= Waves[CurrentIndex].SpawnDelay;
            }
        }
        else
        {
            if (Clock >= Waves[CurrentIndex].Interval)
            {
                if(CurrentIndex + 1 < Waves.Count)
                {
                    CurrentIndex++;
                    StartSpawning();
                }
            }
        }
	}
    public void StartSpawning()
    {
        if (Waves[CurrentIndex].EnemyCount > 0)
        {
            Spawn(Waves[CurrentIndex]);
            Spawning = true;
            Clock = 0;
            CurrentWaveSpawned = 1;
        }
    }

    void Spawn(Wave wave)
    {
        GameObject enemy = null;
        
        switch(wave.EnemyType)
        {
            case ENEMYTYPE.Helicopter:
                {
                    //Setup movement
                    enemy = HeliPool.GetGameObjectFromPool();
					ParabolaMovement pm = enemy.GetComponent<ParabolaMovement>();
                    ParabolaMovement wavePm = (ParabolaMovement)wave.Movement;
                    pm.MoveSpeed = wavePm.MoveSpeed;
                    pm.Aperture = wavePm.Aperture;
                    pm.VertexOffset = wavePm.VertexOffset;
                    pm.xOffset = wavePm.xOffset;
                    break;
                }
            case ENEMYTYPE.Zeppelin:
                {
                    enemy = ZeppelinPool.GetGameObjectFromPool();
//                    //Setup movement
				StopAndShoot lm = enemy.GetComponent<StopAndShoot>();
                      StopAndShoot waveLm = (StopAndShoot)wave.Movement;
                      lm.MaxSpeed = waveLm.MaxSpeed;
                      lm.Desaceleration = waveLm.Desaceleration;
                      lm.TargetPos = waveLm.TargetPos;
                break;
                }
		case ENEMYTYPE.Kamikaze:
			{
				enemy = KamikazePool.GetGameObjectFromPool();
				//Setup movement
				enemy.GetComponent<KamikazeMovement>().MoveSpeed = wave.Movement.MoveSpeed;
				break;
			}

            default:
                break;
        }

		Animator enemyAnimator = enemy.GetComponent<Animator>();
		enemyAnimator.Rebind ();
		enemyAnimator.SetBool ("Alive", true);

		enemy.GetComponent<PolygonCollider2D>().enabled = true;
		enemy.GetComponent<EnemyShip>().HP = enemy.GetComponent<EnemyShip>().MaxHP;
		if (enemy.GetComponent<EnemyShip> ().stateMachine != null) 
		{
			enemy.GetComponent<EnemyShip> ().stateMachine.Start ();
		}
        enemy.transform.parent = EnemyParent.transform;
        enemy.transform.position = wave.Position;
    }

    void ParseWavesFile()
    {
        StringReader reader = new StringReader(WaveFile.text);
        string line = string.Empty;
        do
        {
            line = reader.ReadLine();
            if (line != null)
            {
                if (line == "" || line[0] == '#')
                    continue;
                Wave wave = new Wave();
                List<string> els = new List<string>(line.Split(new char[] { ' ' }));
                els.RemoveAll(item => item == "" || item == " ");

				switch (els[0].ToLower()) 
				{
					case "helicopter":
						wave.EnemyType = ENEMYTYPE.Helicopter;
						break;
					case "zeppelin":
						wave.EnemyType = ENEMYTYPE.Zeppelin;
						break;
					case "kamikaze":
						wave.EnemyType = ENEMYTYPE.Kamikaze;
						break;
					default:
						break;
				}
                wave.EnemyCount = int.Parse(els[1]);
                wave.Position = new Vector2(float.Parse(els[2]), float.Parse(els[3]));
                string moveType = els[4].ToLower();

                switch(moveType)
                {
                    case "parabola":
                        wave.Movement = new ParabolaMovement();
                        ParabolaMovement pm = (ParabolaMovement)wave.Movement;
                        pm.MoveSpeed = float.Parse(els[5]);
                        pm.Aperture = float.Parse(els[6]);
                        pm.VertexOffset = float.Parse(els[7]);
                        pm.xOffset = float.Parse(els[8]);
                        break;
                    case "linear":
                        wave.Movement = new LinearMovement();
                        LinearMovement lm = (LinearMovement)wave.Movement;
                        lm.MoveSpeed = float.Parse(els[5]);
                        lm.direction = new Vector2(float.Parse(els[6]), float.Parse(els[7]));
                        break;
                    case "stopshoot":
                        wave.Movement = new StopAndShoot();
                        StopAndShoot sm = (StopAndShoot)wave.Movement;
                        sm.TargetPos = new Vector2();
                        sm.TargetPos.x = float.Parse(els[6]);
                        sm.TargetPos.y = float.Parse(els[7]);
                        sm.MaxSpeed = float.Parse(els[5]);
                        sm.Desaceleration = float.Parse(els[8]);
                        break;
					case "kamikaze":
						wave.Movement = new KamikazeMovement();
						wave.Movement.MoveSpeed = float.Parse(els[5]);
					break;
                    default:
                        break;
                }

                wave.SpawnDelay = float.Parse(els[els.Count - 2]);
                wave.Interval = float.Parse(els[els.Count - 1]);

                Waves.Add(wave);
            }

        } while (line != null);
    }
}
