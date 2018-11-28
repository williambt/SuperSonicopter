using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BulletSettings
{
    public float DamageValue;
    public float Speed; 
    public Vector2 Dir;
    public Sprite BulletSprite;
    public BulletSettings(float damageValue, float speed, Vector2 dir, Sprite bulletSprite)
    {
        this.DamageValue = damageValue;
        this.Speed = speed;
        this.Dir = dir;
        this.BulletSprite = bulletSprite;
    }
}


public class Bullet : MonoBehaviour
{
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
        Vector2 ShootOffset = GetComponent<SpriteRenderer>().bounds.extents;
		transform.position = owner.transform.position;
        transform.up = settings.Dir;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = settings.BulletSprite;
        this.Settings = settings;
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        IShip shipcolider = col.gameObject.GetComponent(typeof(IShip)) as IShip;
        if (shipcolider != null)
        {
            shipcolider.TakeDamage(Settings.DamageValue);
            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bounds")
        {
            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
