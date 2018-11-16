using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BulletSettings
{
    public float DamageValue;
    public float Speed; 
    public Vector2 Dir;

    public BulletSettings(float damageValue, float speed, Vector2 dir)
    {
        this.DamageValue = damageValue;
        this.Speed = speed;
        this.Dir = dir;
    }
}


public class Bullet : MonoBehaviour
{
    Camera CameraRef;
    Renderer RendererRef;
    Rigidbody2D RigidbodyRef;

    public BulletSettings Settings;

    void Start()
    {
        
    }

    void Update()
    {
    }
    private void FixedUpdate()
    {
        Vector3 move = new Vector3(transform.position.x,transform.position.y, transform.position.z) + (new Vector3(Settings.Dir.x, Settings.Dir.y,0.0f) * Settings.Speed * Time.deltaTime);
        transform.position = move;
    }
    public void Initialize(GameObject owner, BulletSettings settings)
    {
        CameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        RendererRef = GetComponent<Renderer>();
        RigidbodyRef = GetComponent<Rigidbody2D>();
        Vector2 ShootOffset = GetComponent<SpriteRenderer>().bounds.extents;

        transform.position = (owner.transform.position) + (new Vector3(settings.Dir.x * ShootOffset.x, settings.Dir.y * ShootOffset.y ,0.0f) * 20);
        transform.up = settings.Dir;
        GetComponent<CircleCollider2D>().enabled = true;
        this.Settings = settings;
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        IShip shipcolider = col.gameObject.GetComponent(typeof(IShip)) as IShip;
        if (shipcolider != null)
        {
            shipcolider.TakeDamage(Settings.DamageValue);
            GetComponent<CircleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bounds")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
