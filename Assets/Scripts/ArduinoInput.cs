using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoInput : MonoBehaviour
{
    public static ArduinoInput Instance;

    [Header("Arduino Settings")]
    public string portName = "COM3";
    public int baudRate = 9600;
    public int readTimeout = 20;
    public int queueLength = 1;

    [HideInInspector]
    public wrmhlComponent Device;
    public bool KeyboardMode = false;
    public bool Ready = false;

    float USSValue = 0;
    bool Fire = false;
    bool lastFire = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Device = new wrmhlComponent(portName, baudRate, readTimeout, queueLength);
        KeyboardMode = !Device.IsConnected();
    }
	int count = 0;
    void Update ()
    {
        if (!KeyboardMode)
        {

            string fire = Device.Read();
            if (fire == null)
            {
                Ready = false;
                return;
            }
            else
            {
                Ready = true;
            }
            char[] delim = { '|' };
            string[] a = fire.Split(delim);

            lastFire = Fire;
            if (a[0] == "1")
            {
                Fire = true;
            }
            else
            {
                Fire = false;
            }
            if (a[1] != "!" && a[1] != null)
            {
                USSValue = float.Parse(a[1]) / 10.0f;
            }
        }
    }
    public static float GetUSensor()
    {
        return Instance.USSValue;
    }
    public static bool GetFire()
    {
        return Instance.Fire;
    }
    public static bool GetFirePressed()
    {
        return Instance.Fire && !Instance.lastFire;
    }
    public static bool GetFireReleased()
    {
        return !Instance.Fire && Instance.lastFire;
    }
    public static bool IsDeviceReady()
    {
        return Instance.Ready;
    }
    private void OnApplicationQuit()
    {
        Instance.Device.Close();
    }
}
