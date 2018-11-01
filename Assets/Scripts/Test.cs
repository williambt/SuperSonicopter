using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Test : MonoBehaviour 
{
	SerialPort port;

	// Use this for initialization
	void Start ()
	{
		port = new SerialPort ("COM7", 9600);
		port.Open ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (port.IsOpen) {
			try {
				string d = port.ReadLine ();
				print (d);
			} catch (System.Exception) {
				print ("NOPE");
			}
		}
		else 
		{
			print ("NAH!");
		}
	}
}
