using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Scroll()
	{
		transform.Translate (new Vector2 (-speed, 0)* Time.deltaTime );
	}
}
