using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

class ParabolaMovement : MovementType
{
	EnemyShip shipRef;
    public float Aperture = 0.5f;
    public float VertexOffset = 0;
    public float xOffset = 0;
	bool canFire = false;
	void Start()
	{
		shipRef = GetComponent<EnemyShip>();
		//TargetPos = GetComponentsInChildren<Transform>()[1].transform.position;
	}
	float Clock = 0;
	float ShootInterval = 1.0f;
    public override void Move(Rigidbody2D rigidbody2DRef)
    {
        float y = transform.position.y;
        rigidbody2DRef.MovePosition(new Vector2(Aperture * Mathf.Pow(y, 2) + VertexOffset * y + xOffset, y - MoveSpeed * Time.deltaTime));
		if (canFire) {
			Clock += Time.deltaTime;
			if (Clock >= ShootInterval)
			{
				Clock = 0;
				GameObject playerref = GameObject.FindGameObjectWithTag ("Player");
				float dot = Vector2.Dot (playerref.transform.right, (playerref.transform.position - transform.position).normalized);
				if (dot < -0.5f ) {
					shipRef.Fire();
				}
			}
		}
    }

    private Vector2 GetPosition(Vector3 pos)
    {
        return new Vector2(Aperture * Mathf.Pow(pos.y, 2) + VertexOffset * pos.y + xOffset, pos.y);
    }
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Bounds")
		{
			canFire = false;
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Bounds")
		{
			canFire = true;
		}
	}
}

