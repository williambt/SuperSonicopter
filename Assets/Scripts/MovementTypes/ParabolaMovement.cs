using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

class ParabolaMovement : MovementType
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
        rigidbody2DRef.MovePosition(new Vector2(Aperture * Mathf.Pow(y, 2) + VertexOffset * y + xOffset, y - MoveSpeed * Time.deltaTime));
    }

    private Vector2 GetPosition(Vector3 pos)
    {
        return new Vector2(Aperture * Mathf.Pow(pos.y, 2) + VertexOffset * pos.y + xOffset, pos.y);
    }

//    void OnDrawGizmos()
//    {
//        if (enabled)
//        {
//            float yDelta = 0.25f;
//
//            float xpos = GetPosition(transform.position).x;
//
//            if (transform.position.x != xpos)
//                transform.position = new Vector3(xpos, transform.position.y, transform.position.z);
//
//            if (DrawTrajectory)
//            {
//                Gizmos.color = Color.red;
//
//                float x = 0;
//                for (float y = transform.position.y; y >= Camera.main.transform.position.y - Camera.main.orthographicSize - yDelta; y -= yDelta)
//                {
//                    float tempx = x;
//                    x = GetPosition(new Vector2(0, y)).x;
//                    if (y != transform.position.y)
//                    {
//                        Gizmos.DrawLine(new Vector3(tempx, y + yDelta, 0), new Vector3(x, y, 0));
//                    }
//                }
//            }
//        }
//    }
}

