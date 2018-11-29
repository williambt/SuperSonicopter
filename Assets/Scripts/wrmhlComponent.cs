using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wrmhlComponent
{
    wrmhl Device = new wrmhl(); // wrmhl is the bridge beetwen your computer and hardware.

    [Tooltip("SerialPort of your device.")]
    public string portName = "COM4";

    [Tooltip("Baudrate")]
    public int baudRate = 9600;


    [Tooltip("Timeout")]
    public int readTimeout = 20;

    [Tooltip("QueueLenght")]
    public int queueLenght = 1;

    bool successfullyConnected = false;

    //use this to use the default COM4 and 9600
    public wrmhlComponent()
    {
        Device.set(portName, baudRate, readTimeout, queueLenght); // This method set the communication with the following vars;
                                                                  //                              Serial Port, Baud Rates, Read Timeout and QueueLenght.
        successfullyConnected = Device.connect(); // This method open the Serial communication with the vars previously given.
    }
    public wrmhlComponent(string PortName, int BaudRate, int ReadTimeout, int QueueLenght)
    {
        Device.set(PortName, BaudRate, ReadTimeout, QueueLenght); // This method set the communication with the following vars;
                                                                  //                              Serial Port, Baud Rates, Read Timeout and QueueLenght.
        successfullyConnected = Device.connect(); // This method open the Serial communication with the vars previously given.
    }
    ~wrmhlComponent()
    {
        Device.close();
    }
    public string Read()
    {
        return Device.readQueue();
    }
    public void Write(string dataToSend)
    {
        Device.send(dataToSend);
    }
    public bool IsConnected()
	{
        return successfullyConnected;
    }
	public void Close()
	{
		Device.close ();
	}

}
