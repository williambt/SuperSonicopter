using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public GameObject bullet;
	AudioSource source;
	public AudioClip explosion;
	public Sprite dead;


    float yStart;
    public float offset = 1f;
    wrmhl device = new wrmhl();

    public string portName = "COM3";
    public int baudRate = 9600;
    public int readTimeout = 20;
    public int queueLength = 1;

    float lastPos = 0;
    public float sensitivity = 3;

    float lerpStartY = 0;
    float lerpTargetY = 0;
    float lerpT = 0.0f;

	// Use this for initialization
	void Start ()
    {
        yStart = transform.position.y;
        device.set(portName, baudRate, readTimeout, queueLength);
        device.connect();
		source = gameObject.AddComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update ()
    {
        if(lerpStartY != lerpTargetY)
        {
            lerpT += 0.15f;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(lerpStartY, lerpTargetY, lerpT));
        }

		string fire = device.readQueue();
		//print (fire);
		if (fire == null) {
			return;
		}
		char[] delim = { '|' };
		string[] a = fire.Split (delim);
		if (a[0] == "fire") 
		{
			Fire ();
		}
		if (a[1] != "!" && a[1] != null)
        {
			float cm = float.Parse(a[1]) / 10.0f;
			if (Mathf.Abs(lastPos - cm) >= sensitivity)
			{
				//print(cm);
				//transform.position = new Vector2(transform.position.x, yStart + (cm / 10.0f - 3.0f) * offset);
				lerpTargetY = yStart + (cm / 10.0f - 3.0f) * offset;
				lerpStartY = transform.position.y;
				lerpT = 0;
				lastPos = cm;
			}
        }
	}

    void OnApplicationQuit()
    {
        device.close();
    }
	void Fire ()
	{
		GameObject.Instantiate (bullet, transform.position,transform.rotation);
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		print (col.gameObject.name);
		source.clip = explosion;
		source.loop = false;
		GetComponent<SpriteRenderer> ().sprite = dead;

		GetComponent<Collider2D> ().enabled = false;
		source.Play ();
	}

}
