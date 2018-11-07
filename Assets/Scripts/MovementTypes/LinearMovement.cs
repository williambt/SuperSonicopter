using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LinearMovement : MovementType
{
    public Vector2 direction = new Vector2(1, 0);

    public override void Move(Rigidbody2D rigidbody2DRef)
    {
        rigidbody2DRef.MovePosition((Vector2)transform.position + (direction.normalized * MoveSpeed * Time.deltaTime));
    }

    void OnDrawGizmos()
    {
        if (DrawTrajectory)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, (Vector2)transform.position + (direction.normalized * 100));
        }
    }
}

