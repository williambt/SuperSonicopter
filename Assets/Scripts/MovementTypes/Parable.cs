using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Parable : MovementType
{
    public override void Move(Rigidbody2D rigidbody2DRef)
    {
        rigidbody2DRef.AddForce(new Vector2(-1, 0));
    }
}

