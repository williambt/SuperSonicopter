using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StopAndShoot : MovementType
{
    EnemyShip shipRef;
    Vector3 TargetPos;

    void Start()
    {
        shipRef = GetComponent<EnemyShip>();
        TargetPos = GetComponentsInChildren<Transform>()[1].transform.position;
    }
    float Clock = 0;
    float ShootInterval = 0.5f;
    Vector2 force = Vector2.zero;
    public override void Move(Rigidbody2D rigidbody2DRef)
    {
        if (force.magnitude < 0.01f)
        {
            force = Steerings.Arrive(gameObject, TargetPos);
            rigidbody2DRef.AddForce(force);
        }
        else
        {
            rigidbody2DRef.velocity = Vector2.zero;
            Clock += Time.deltaTime;
            if (Clock >= ShootInterval)
            {
                Clock = 0;
                shipRef.Fire();
            }
        }
    }
}

