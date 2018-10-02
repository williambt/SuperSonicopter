using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Helicopter Style")]
    public GameObject bullet;
	AudioSource source;
	public AudioClip explosion;
	public Sprite dead;
    [Header("Helicopter Settings")]
    [Range(0, 25)]
    public float displacementSpeed = 10;
    public float sensitivity = 3;
    [Space(10)]
    float yStart;
    public float offset = 1f;
    wrmhlComponent device;

    [Header("Arduino Settings")]
    public string portName = "COM3";
    public int baudRate = 9600;
    public int readTimeout = 20;
    public int queueLength = 1;

    float lastPos = 0;

    float lerpStartY = 0;
    float lerpTargetY = 0;
    float lerpT = 0.0f;

    bool keyboardMode = false;

	// Use this for initialization
	void Start ()
    {
        yStart = transform.position.y;
        device = new wrmhlComponent(portName, baudRate, readTimeout, queueLength);
		source = gameObject.AddComponent<AudioSource> ();
        keyboardMode = !device.IsConnected();
    }

    // Update is called once per frame
    void Update ()
    {
        if(lerpStartY != lerpTargetY)
        {
            Move();
        }

        if (keyboardMode)
        {
            bool moved = false;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                lerpTargetY = yStart + 2 * offset;
                moved = true;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                lerpTargetY = yStart + -2 * offset;
                moved = true;
            }
            if (moved)
            {
                lerpStartY = transform.position.y;
                lerpT = 0;
            }
        }
        else
        {
		    string fire = device.Read();
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
				    lerpTargetY = yStart + (cm / 10.0f - 3.0f) * offset;
				    lerpStartY = transform.position.y;
				    lerpT = 0;
				    lastPos = cm;
			    }
            }
        }
	}
	void Fire ()
	{
		GameObject.Instantiate (bullet, transform.position,transform.rotation);
	}
    void Move()
    {
        lerpT += displacementSpeed / 100.0f;
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(lerpStartY, lerpTargetY, lerpT));
    }
	void OnCollisionEnter2D(Collision2D col)
	{
		print (col.gameObject.name);
		source.clip = explosion;
		source.loop = false;
		GetComponent<SpriteRenderer> ().sprite = dead;
        GetComponent<Animator>().SetBool("Alive", false);
		GetComponent<Collider2D> ().enabled = false;
		source.Play ();
	}

}
