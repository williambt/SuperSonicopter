using System.Collections;
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
    public GameObject Barrage;
    public TextAsset WaveFile;

	ObjectPool HeliPool;
    ObjectPool ZeppelinPool;
	ObjectPool KamikazePool;
    ObjectPool BarragePool;

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
        BarragePool = new ObjectPool (Barrage, transform, 30);
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
                    enemy = HeliPool.GetGameObjectFromPool();
                    //resetar o animator
                    DestroyImmediate(enemy.GetComponent<Animator>());
                    Animator animator = enemy.AddComponent<Animator>();
                    animator.runtimeAnimatorController = Helicopter.GetComponent<Animator>().runtimeAnimatorController;
                    animator.Rebind();
                    //resetar o sprite renderer
                    DestroyImmediate(enemy.GetComponent<SpriteRenderer>());
                    SpriteRenderer spriteRenderer = enemy.AddComponent<SpriteRenderer>();
                    spriteRenderer.material = Helicopter.GetComponent<SpriteRenderer>().sharedMaterial;
                    spriteRenderer.sprite = Helicopter.GetComponent<SpriteRenderer>().sprite;
                    spriteRenderer.flipX = true;
                    //-------------------------------------------------------------------------------------------
                    //Setup movement
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
                    //Setup movement
				    StopAndShoot lm = enemy.GetComponent<StopAndShoot>();
                          StopAndShoot waveLm = (StopAndShoot)wave.Movement;
                          lm.MaxSpeed = waveLm.MaxSpeed;
                          lm.Desaceleration = waveLm.Desaceleration;
                          lm.TargetPos = waveLm.TargetPos;
                    enemy.GetComponent<SpriteRenderer>().sprite = Zeppelin.GetComponent<SpriteRenderer>().sprite;
                    break;
                }
		    case ENEMYTYPE.Kamikaze:
			    {
				    enemy = KamikazePool.GetGameObjectFromPool();
				    //Setup movement
				    enemy.GetComponent<KamikazeMovement>().MoveSpeed = wave.Movement.MoveSpeed;
                    enemy.GetComponent<SpriteRenderer>().sprite = Kamikaze.GetComponent<SpriteRenderer>().sprite;
                    break;
			    }
            case ENEMYTYPE.Barrage:
                {
                    enemy = BarragePool.GetGameObjectFromPool();
                    //Setup movement
                    ShootAndGetOut sg = enemy.GetComponent<ShootAndGetOut>();
                    ShootAndGetOut waveSg = (ShootAndGetOut)wave.Movement;
                    sg.MoveSpeed = wave.Movement.MoveSpeed;
                    sg.MaxSpeed = waveSg.MaxSpeed;
                    sg.Desaceleration = waveSg.Desaceleration;
                    sg.TargetPos = waveSg.TargetPos;
                    enemy.GetComponent<SpriteRenderer>().sprite = Kamikaze.GetComponent<SpriteRenderer>().sprite;

                    break;
                }
                default:
                    break;
        }
		enemy.GetComponent<Collider2D>().enabled = true;
		enemy.GetComponent<EnemyShip>().HP = enemy.GetComponent<EnemyShip>().MaxHP;
		if (enemy.GetComponent<EnemyShip> ().stateMachine != null) 
		{
			enemy.GetComponent<EnemyShip> ().stateMachine.Start ();
		}
        enemy.transform.parent = EnemyParent.transform;
        enemy.transform.position = wave.Position;

        if(wave.Score != 0)
        {
            enemy.GetComponent<EnemyShip>().Score = wave.Score;
        }
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
                int currIndex = 0;

                int score = 0;
                if(int.TryParse(els[currIndex], out score))
                {
                    currIndex++;
                }

				switch (els[currIndex++].ToLower()) 
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
                    case "barrage":
                        wave.EnemyType = ENEMYTYPE.Barrage;
                        break;
					default:
						break;
				}
                wave.EnemyCount = int.Parse(els[currIndex++]);
                wave.Position = new Vector2(float.Parse(els[currIndex++]), float.Parse(els[currIndex++]));
                string moveType = els[currIndex++].ToLower();

                switch(moveType)
                {
                    case "parabola":
                        wave.Movement = new ParabolaMovement();
                        ParabolaMovement pm = (ParabolaMovement)wave.Movement;
                        pm.MoveSpeed = float.Parse(els[currIndex++]);
                        pm.Aperture = float.Parse(els[currIndex++]);
                        pm.VertexOffset = float.Parse(els[currIndex++]);
                        pm.xOffset = float.Parse(els[currIndex++]);
                        break;
                    case "linear":
                        wave.Movement = new LinearMovement();
                        LinearMovement lm = (LinearMovement)wave.Movement;
                        lm.MoveSpeed = float.Parse(els[currIndex++]);
                        lm.direction = new Vector2(float.Parse(els[currIndex++]), float.Parse(els[currIndex++]));
                        break;
                    case "stopshoot":
                        wave.Movement = new StopAndShoot();
                        StopAndShoot sm = (StopAndShoot)wave.Movement;
                        sm.TargetPos = new Vector2();
                        sm.TargetPos.x = float.Parse(els[currIndex++]);
                        sm.TargetPos.y = float.Parse(els[currIndex++]);
                        sm.MaxSpeed = float.Parse(els[currIndex++]);
                        sm.Desaceleration = float.Parse(els[currIndex++]);
                        break;
					case "kamikaze":
						wave.Movement = new KamikazeMovement();
						wave.Movement.MoveSpeed = float.Parse(els[currIndex++]);
                        break;
                    case "shootgetout":
                        wave.Movement = new ShootAndGetOut();
                        ShootAndGetOut sg = (ShootAndGetOut)wave.Movement;
                        sg.TargetPos = new Vector2();
                        wave.Movement.MoveSpeed = float.Parse(els[currIndex]);
                        sg.MaxSpeed = float.Parse(els[currIndex++]);
                        sg.TargetPos.x = float.Parse(els[currIndex++]);
                        sg.TargetPos.y = float.Parse(els[currIndex++]);
                        sg.Desaceleration = float.Parse(els[currIndex++]);
                        break;
                    default:
                        break;
                }

                wave.SpawnDelay = float.Parse(els[currIndex++]);
                wave.Interval = float.Parse(els[currIndex++]);

                wave.Score = score;

                Waves.Add(wave);
            }

        } while (line != null);
    }
}
