using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeMovement : MovementType
{
	Vector2 direction = new Vector2(1, 0);

	void Start()
	{
		GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
		if (playerRef != null)
		{
			direction = playerRef.transform.position - transform.position;
			transform.right = -direction;
			direction.Normalize();
		}
	}
	public override void Move(Rigidbody2D rigidbody2DRef)
	{
		rigidbody2DRef.MovePosition((Vector2)transform.position + (direction * MoveSpeed * Time.deltaTime));
	}
}
