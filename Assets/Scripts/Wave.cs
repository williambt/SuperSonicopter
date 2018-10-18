using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum ENEMY
{
    Basic, Fighter, Bomber
}
[System.Serializable]
public struct EnemyGroup
{
    public ENEMY Type;
    public int Count;
}
[System.Serializable]
public class Wave 
{
    public EnemyGroup Group;
    public Wave()
    {
        Group = new EnemyGroup();
    }
}
