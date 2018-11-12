using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Steerings
{
    
    public static Vector2 Seek(GameObject agent, Vector2 Target)
    {
        Vector2 Result = new Vector2();
        Result = Target - new Vector2(new Vector2(agent.transform.position.x, agent.transform.position.y).x, new Vector2(agent.transform.position.x, agent.transform.position.y).y) ;
        Result = Result.normalized * 10;
        Result = Vector2.ClampMagnitude(Result, 15);
        return Result - agent.GetComponent<Rigidbody2D>().velocity;
    }
    public static Vector2 Flee(GameObject agent, Vector2 target)
    {
        if (Vector2.Distance(new Vector2(agent.transform.position.x, agent.transform.position.y), target) > 5)
        {
            return new Vector2(0, 0);
        }
        Vector2 desired = (new Vector2(agent.transform.position.x, agent.transform.position.y) - target).normalized * 10;
        return desired - agent.GetComponent<Rigidbody2D>().velocity;
    }
    public static Vector2 Arrive(GameObject agent, Vector2 Target)
    {
        Vector2 Result = new Vector2();
        Vector2 ToTarget = Target - new Vector2(agent.transform.position.x, agent.transform.position.y);
        float desaceleration = 0.5f;
        if (ToTarget.magnitude > 0)
        {
            float speed = ToTarget.magnitude / desaceleration;
            speed = Mathf.Clamp(speed, 0, 50);
            Result = ToTarget * speed / ToTarget.magnitude;
        }
        return Result - agent.GetComponent<Rigidbody2D>().velocity;
    }
    /// <summary>
    /// Projects vector from start to point onto path vector
    /// </summary>
    public static Vector2 GetNormalPoint(Vector2 PathStart, Vector2 PathEnd, Vector2 Point)
    {
        Vector2 NormalPoint = new Vector2();
        Vector2 A = Point - PathStart;
        Vector2 B = PathEnd - PathStart;
        B.Normalize();
        NormalPoint = (B * Vector2.Dot(A, B)) + PathStart;
        return NormalPoint;
    }
}

