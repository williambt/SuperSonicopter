using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
	public float speed;
	public GameObject Paralax;

	List<GameObject> toParalax;

	Vector3 offset = new Vector3();

	void Start () 
	{
		toParalax = new List<GameObject> ();
		foreach (Transform child in Paralax.transform) 
		{
			toParalax.Add (child.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void Scroll()
	{
		offset.x = speed * Time.deltaTime;
		for (int i = 0; i < toParalax.Count; i++) {
			toParalax [i].transform.position -= offset / (i + 1);
			//print (toParalax [i].GetComponent<SpriteRenderer> ().material.GetTextureOffset ("_MainTex"));
		}
	}
}
