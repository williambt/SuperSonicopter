using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum ENEMYTYPE
{
    Helicopter, Zeppelin, Kamikaze, Barrage
}
[System.Serializable]
public struct EnemyGroup
{
    public int Count;
}
[System.Serializable]
public class Wave 
{
    public ENEMYTYPE EnemyType;
    public int EnemyCount;
    public Vector2 Position;
    public MovementType Movement;
    public float SpawnDelay;
	public float Interval;
    public Wave()
    {
        //Group = new EnemyGroup();
    }
}
