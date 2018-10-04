using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool
{
    List<GameObject> Pool;
    GameObject toInstantiate;
    Transform Owner;
    int LimitToPool { get; set; }


    public ObjectPool(GameObject toPool, Transform owner, int limit)
    {
        Pool = new List<GameObject>();
        toInstantiate = toPool;
        Owner = owner;
        LimitToPool = limit;
    }
    /// <summary>
    /// returns defined object 
    /// - owner position and rotation
    /// - active
    /// </summary> 
    public GameObject GetGameObjectFromPool()
    {
        if (Pool.Count < LimitToPool)
        {
            GameObject obj = GameObject.Instantiate(toInstantiate, Owner.position, Owner.rotation);
            Pool.Add(obj);
            return obj;
        }
        else
        {
            foreach (GameObject item in Pool)
            {
                if (!item.activeSelf)
                {
                    item.transform.position = Owner.position;
                    item.transform.rotation = Owner.rotation;
                    item.SetActive(true);
                    return item;
                }
            }
            LimitToPool++;
            return GetGameObjectFromPool();
        }
    }
    public int GetActiveCount()
    {
        int active = 0;
        foreach (GameObject item in Pool)
        {
            if (item.activeSelf)
            {
                active++;
            }
        }
        return active;
    }
}