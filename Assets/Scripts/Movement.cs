using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update ()
    {
        if(lerpStartY != lerpTargetY)
        {
            lerpT += 0.08f;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(lerpStartY, lerpTargetY, lerpT));
        }

        string rq = device.readQueue();
        if (rq != "!" && rq != null)
        {
            float cm = long.Parse(rq) / 10.0f;
            if (Mathf.Abs(lastPos - cm) >= sensitivity)
            {
                print(cm);
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
}
