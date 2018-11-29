using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ShootAndGetOut : MovementType
{
    EnemyShip shipRef;
    public Vector2 TargetPos;
    public float MaxSpeed;
    public float Desaceleration;
    public Vector3 ExitPoint;
    void Start()
    {
        shipRef = GetComponent<EnemyShip>();
        //TargetPos = GetComponentsInChildren<Transform>()[1].transform.position;
    }
    float Clock = 0;
    float ShootInterval = 0.5f;
    bool HasStopped = false;
    bool HasShot = false;
    public override void Move(Rigidbody2D rigidbody2DRef)
    {
        if (!HasStopped)
        {
            Vector2 force = Steerings.Arrive(gameObject, TargetPos);
            Vector2 Result = new Vector2();
            Vector2 ToTarget = TargetPos - new Vector2(transform.position.x, transform.position.y);
            if (ToTarget.magnitude > 0)
            {
                float speed = ToTarget.magnitude / Desaceleration;
                speed = Mathf.Clamp(speed, 0, MaxSpeed);
                Result = ToTarget * speed / ToTarget.magnitude;
            }
            rigidbody2DRef.AddForce(Result - GetComponent<Rigidbody2D>().velocity);
            if (force.magnitude < 0.05f)
            {
                HasStopped = true;
            }
        }
        else if(!HasShot)
        {
            rigidbody2DRef.velocity = Vector2.zero;
            Clock += Time.deltaTime;
            if (Clock >= ShootInterval)
            {
                Clock = 0;
                shipRef.Fire();
                HasShot = true;
                ExitPoint = transform.position - GameObject.FindGameObjectWithTag("Bounds").GetComponent<Collider2D>().bounds.center;
                ExitPoint.Normalize();
            }
        }
        else
        {
            rigidbody2DRef.AddForce(ExitPoint * MaxSpeed);
        }

    }
	void OnEnable()
	{
		HasStopped = false;
		HasShot = false;
	}

}

