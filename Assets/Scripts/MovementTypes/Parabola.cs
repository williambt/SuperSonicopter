using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Parabola : MovementType
{
	public float Aperture = 0.5f;
	public float VertexOffset = 0;
	public float xOffset = 0;

	void Start()
	{
	}

    public override void Move(Rigidbody2D rigidbody2DRef)
    {
		float y = transform.position.y;
		rigidbody2DRef.MovePosition (new Vector2(Aperture * Mathf.Pow (y, 2) + VertexOffset * y + xOffset, y - MoveSpeed * Time.deltaTime));
    }
}

