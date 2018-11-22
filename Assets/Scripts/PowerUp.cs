using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float HPRecover = 0;
    public BulletSettings BulletPowerUp;
    public bool IsHealthBooster;

    private void Update()
    {
        Vector3 move = new Vector3(transform.position.x, transform.position.y, transform.position.z) + (new Vector3(-1, 0, 0.0f) * 1 * Time.deltaTime);
        transform.position = move;
    }
}
