using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Camera cameraRef;
    Renderer rendererRef;
	// Use this for initialization
	void Start ()
    {
        cameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rendererRef = GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector2 (1.0f, 0));
        if (!RendererExtensions.IsVisibleFrom(rendererRef, cameraRef))
        {
            gameObject.SetActive(false);
        }
	}
}
