using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFlying : MonoBehaviour 
{
	Vector2 dir;
	float speed = 10.0f;
	float zoomSpeed = 100.0f;

	public Rigidbody2D traseira;
	public Rigidbody2D cabine;

	// Use this for initialization
	void Start () 
	{	
		dir = new Vector2 (Random.Range (1, 100), Random.Range (1, 100)).normalized;
		traseira.AddForce (new Vector2 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f)) * 20, ForceMode2D.Impulse);
		cabine.AddForce (new Vector2 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f)) * 20, ForceMode2D.Impulse);

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (new Vector3(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, -0.1f));
		transform.localScale += new Vector3 (zoomSpeed * Time.deltaTime, zoomSpeed * Time.deltaTime, 0);
	}
}
