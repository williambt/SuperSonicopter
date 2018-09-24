using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MovementDirect : MonoBehaviour
{
    public float speed = 0.5f;

    float startY;

    SerialPort port = new SerialPort("COM3", 9600);

	// Use this for initialization
	void Start ()
    {
        startY = transform.position.y;

        port.Open();
        port.ReadTimeout = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(port.IsOpen)
        {
            try
            {
                string input = port.ReadLine();
                if(input != null && input != "!")
                    transform.position = new Vector2(transform.position.x, startY + (float.Parse(input) - 6.0f) * speed);
                print(transform.position.y);
            }
            catch(System.Exception)
            {}
        }

	}
}
