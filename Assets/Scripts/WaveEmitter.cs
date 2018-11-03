using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEmitter : MonoBehaviour
{
    public List<Wave> Waves;
	public int CurrentIndex = 0;
	float Clock = 0;

	public GameObject Helicopter;
	public GameObject Zeppelin;

	ObjectPool Pool;

	void Start ()
    {
        Waves = new List<Wave>();
		Pool = new ObjectPool (Helicopter, gameObject.transform,30);
		// passar classe de movimento pro inimigo na criação.
	}
	
	void Update () 
	{
		Clock += Time.deltaTime;
		//if (Clock >= Waves[CurrentIndex].Interval)
		{
			
		}
	}
	void Spawn()
	{
		
	}
}
