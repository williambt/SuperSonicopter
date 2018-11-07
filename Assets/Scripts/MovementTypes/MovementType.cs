using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementType : MonoBehaviour
{
    public bool DrawTrajectory = true;

	public float MoveSpeed = 2.5f;

    public virtual void Move(Rigidbody2D rigidbody2DRef)
    {
		
    }
}
