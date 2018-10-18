using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEmitter : MonoBehaviour
{
    public List<Wave> Waves;
	public int CurrentIndex = 0;
	float Clock = 0;



	ObjectPool Pool;

	void Start ()
    {
        Waves = new List<Wave>();
		Pool = new ObjectPool ();
	}
	
	void Update () 
	{
		Clock += Time.deltaTime;
		if (Clock >= Waves[CurrentIndex].Interval)
		{
			
		}
	}
	void Spawn()
	{
		
	}
}
